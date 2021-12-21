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
        public CommitTemplateToolWindowControl()
        {
            InitializeComponent();
            Loaded += CommitTemplateToolWindowControl_Loaded;
        }

        private async void CommitTemplateToolWindowControl_Loaded(object sender, RoutedEventArgs e)
        {
            imgIcon.Source = KnownMonikers.ContextMenu.ToBitMapSource(25, 25);
            Navigate(new CommitControl(this));
        }

        public void NavigateBack()
            => NavigationManager.NavigateBack(this);

        public void Navigate(UserControl userControl)
            => NavigationManager.Navigate(this, userControl);
    }
}