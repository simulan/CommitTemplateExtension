using CommitTemplateExtension.EnumAndConstants;
using CommitTemplateExtension.Utils;
using CommitTemplateExtension.Git;
using CommitTemplateExtension.ViewModels;
using CommitTemplateExtension.Views;
using Microsoft.VisualStudio.Imaging;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static CommitTemplateExtension.Git.GitService;
using MessageBox = Community.VisualStudio.Toolkit.MessageBox;
using CommitTemplateExtension.views.navigation;

namespace CommitTemplateExtension
{
    /// <summary>
    /// Interaction logic for CommitTemplateToolWindowControl.
    /// </summary>
    public partial class CommitTemplateToolWindowControl : UserControl, INavigationParent
    {
        private GitService gitService;
        private ActionService actionService;
        public ConventionalCommitVM ViewModel
        {
            get { return DataContext as ConventionalCommitVM; }
            set { DataContext = value; }
        }

        public CommitTemplateToolWindowControl()
        {
            InitializeComponent();
            Loaded += CommitTemplateToolWindowControl_Loaded;
        }

        private async void CommitTemplateToolWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            var configControl = new ConfigurationControl(null);
            configControl.Width = 280;
            configControl.Height = 400;

            pnlContainer.Children.Add(configControl);

            ViewModel = new ConventionalCommitVM();
            imgIcon.Source = KnownMonikers.ContextMenu.ToBitMapSource(25,25);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            string baseDirectory = await FileUtils.GetBaseDir();
            actionService = new ActionService(gitService);
            if (baseDirectory!=null)
            {
                ViewModel.RepoDirectory = baseDirectory;
                gitService = GitService.GetInstance(baseDirectory);
                if (!gitService.Initialized)
                    SetNotification(NotificationMessages.REPOSITORY_NONEXISTANT);

                ViewModel.RepoDirectory = gitService.Directory;
                // should be in an event listener "on successfull git init"
                RefreshFileChangesDisplay();
                ViewModel.AssembleCommitMessage();
            }
            else
                SetNotification(NotificationMessages.SOLUTION_NONEXISTANT);
        }

        private void RefreshFileChangesDisplay()
        {
            var workSpaceFiles = gitService.GetWorkspaceFiles(ViewModel.IncludeUntracked);
            var indexFiles = gitService.GetIndexFiles();
            ViewModel.WorkSpaceFiles = new ObservableCollection<FileChangeVM>(workSpaceFiles);
            ViewModel.IndexFiles = new ObservableCollection<FileChangeVM>(indexFiles);
        }

        private void btnCommit_Click(object sender, RoutedEventArgs e)
        => Commit();

        private void mnuPush_Click(object sender, RoutedEventArgs e)
         => Push();

        private void btnCommitPush_Click(object sender, RoutedEventArgs e)
            => CommitAndPush();

        private bool Commit()
        {
            bool success = gitService.Commit(ViewModel.CommitMessage);
            if (!success)
                ViewModel.Error = ErrorMessages.COMMIT_FAILED;
            return success;
        }

        private bool Push()
        {
            bool success = false;
            PushResult result = gitService.Push();
            if (result == PushResult.REQUIRES_REMOTE)
                PromptUserToSpecifyRemote();
            else if (result == PushResult.FAIL)
                ViewModel.Error = ErrorMessages.PUSH_FAILED;
            else
                success = true;
            return success;
        }

        private void CommitAndPush()
        {
            if (Commit())
                Push();
        }

        private void PromptUserToSpecifyRemote()
        {
            AddRemoteDialog dlgRemote = new AddRemoteDialog(new AddRemoteVM());
            dlgRemote.Show();
            dlgRemote.Closing += CreateRemoteAndPush;
        }

        private void CreateRemoteAndPush(object sender, System.ComponentModel.CancelEventArgs e)
        {
           if(sender is AddRemoteDialog dlgRemote)
            {
                AddRemoteVM data = dlgRemote.ViewModel;
                gitService.CreateRemote(data.Name, data.Remote);
            }
        }

        private void btnStageToggle_Click(object sender, RoutedEventArgs e)
        {
            FileChangeVM fileChange = (sender as Button).DataContext as FileChangeVM;
            if (ViewModel.WorkSpaceFiles.Contains(fileChange))
            {
                if (gitService.Stage(fileChange.Name))
                    RefreshFileChangesDisplay();
                else
                    ViewModel.Error = ErrorMessages.STAGE_FAILED;
            }
            else
            {
                if (gitService.Unstage(fileChange.Name))
                    RefreshFileChangesDisplay();
                else
                    ViewModel.Error = ErrorMessages.UNSTAGE_FAILED;
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Error))
            {
                if (string.IsNullOrWhiteSpace(ViewModel.Error))
                    brdError.Visibility = Visibility.Collapsed;
                else
                    brdError.Visibility = Visibility.Visible;
            }
            if (e.PropertyName == nameof(ViewModel.Notification))
            {
                if (string.IsNullOrWhiteSpace(ViewModel.Notification))
                    brdNotification.Visibility = Visibility.Collapsed;
                else
                    brdNotification.Visibility = Visibility.Visible;
            }
            if(e.PropertyName == nameof(ViewModel.IncludeUntracked))
                RefreshFileChangesDisplay();
        }

        private void SetNotification(string notification)
        {
            ViewModel.Notification = notification;
            ViewModel.NotificationAction = NotificationMessages.NotificationByAction[notification];
        }

        private void BtnCloseError_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Error = string.Empty;
            brdError.Visibility = Visibility.Collapsed;
        }

        private void BtnCloseNotification_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Notification = null;
            ViewModel.NotificationAction = null;
            brdNotification.Visibility = Visibility.Collapsed;
        }
        private void LnkAction_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Notification))
                return;

            if (gitService == null)
                gitService = GitService.GetInstance(null);

            bool success = actionService.ReactToNotification(ViewModel.NotificationAction);
            if (success)
            {
                if (ViewModel.NotificationAction == NotificationMessages.Actions.SOLUTION_NONEXISTANT_ACTION)
                    ViewModel.RepoDirectory = gitService.Directory;
                BtnCloseNotification_Click(null, null);
            }
        }

        public void Back()
        {
        }
    }
}