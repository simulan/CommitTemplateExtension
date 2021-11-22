using CommitTemplateExtension.EnumAndConstants;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommitTemplateExtension.ViewModels
{
    /// <summary>
    /// Temporary viewmodel for initial stage, only allow commits in the style of 
    /// https://www.conventionalcommits.org/en/v1.0.0/
    /// 
    /// Goal is to compose viewmodels based on textfiles by the user detailing custom templates
    /// </summary>
    public class ConventionalCommitVM : INotifyPropertyChanged
    {
        private const string REPO_LABEL = "Repo: ";
        private string commitMessage, description, scope, body, footer, error, notification, notificationAction, repoDirectory;
        private bool breakingChanges, includeUntracked;
        private int type;
        private ObservableCollection<FileChangeVM> indexFiles, workspaceFiles;

        public ConventionalCommitVM()
        {
            IndexFiles = new ObservableCollection<FileChangeVM>();
            workspaceFiles = new ObservableCollection<FileChangeVM>();
        }

        public bool BreakingChanges
        {
            get
            {
                return breakingChanges;
            }
            set
            {
                if (value != breakingChanges    )
                {
                    breakingChanges = value;
                    AssembleCommitMessage();
                    NotifyPropertyChanged();
                }
            }
        }

        public string RepoDirectoryMessage
            => REPO_LABEL + repoDirectory;

        public string RepoDirectory
        {
            get
            {
                return repoDirectory;
            }
            set
            {
                if (value != repoDirectory)
                {
                    repoDirectory = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IncludeUntracked
        {
            get
            {
                return includeUntracked;
            }
            set
            {
                if (value != includeUntracked)
                {
                    includeUntracked = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                if (value != error)
                {
                    error = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string NotificationAction
        {
            get
            {
                return notificationAction;
            }
            set
            {
                if (value != notificationAction)
                {
                    notificationAction = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Notification
        {
            get
            {
                return notification;
            }
            set
            {
                if (value != notification)
                {
                    notification = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /// <summary>
        /// The entire commit message to be passed to the versioning system
        /// </summary>
        public string CommitMessage {
            get
            {
                return commitMessage;
            }
            set
            {
                if (value != commitMessage)
                {
                    commitMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// The type of commit, see enum
        /// </summary>
        public int Type {
            get
            {
                return type;
            }
            set
            {
                if (value != type)
                {
                    type = value;
                    AssembleCommitMessage();
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// A short description of the changes
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    AssembleCommitMessage();
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Filled in if the changes happen only to a specific part of the project
        /// </summary>
        public string Scope {
            get
            {
                return scope;
            }
            set
            {
                if (value != scope)
                {
                    scope = value;
                    AssembleCommitMessage();
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// A larger multiline description of the changes
        /// </summary>
        public string Body {
            get
            {
                return body;
            }
            set
            {
                if (value != body)
                {
                    body = value;
                    AssembleCommitMessage();
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// A footer
        /// </summary>
        public string Footer {
            get
            {
                return footer;
            }
            set
            {
                if (value != footer)
                {
                    footer = value;
                    AssembleCommitMessage();
                    NotifyPropertyChanged();
                }
            }
        }


        /// <summary>
        /// The git files in the index, ready to commit
        /// </summary>
        public ObservableCollection<FileChangeVM> IndexFiles
        {
            get
            {
                return indexFiles;
            }
            set
            {
                if (value != indexFiles)
                {
                    indexFiles = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The git files in the workspace, waiting to be added to the index
        /// </summary>
        public ObservableCollection<FileChangeVM> WorkSpaceFiles
        {
            get
            {
                return workspaceFiles;
            }
            set
            {
                if (value != workspaceFiles)
                {
                    workspaceFiles = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Applies the actual format through a string.format with all the form parts as inputs
        /// </summary>
        public void AssembleCommitMessage()
        {
            string commitType = Enum.GetName(typeof(CommitTypeEnum), (CommitTypeEnum) this.type).ToLower();
            string breakingChangeIndicator = string.Empty;
            if (BreakingChanges)
                breakingChangeIndicator = "!";
            
            if (!string.IsNullOrEmpty(this.scope))
                CommitMessage = string.Format("{0}({1}){2}: {3}\r\n\r\n{4}\r\n\r\n{5}", commitType, scope, breakingChangeIndicator, description, body, footer);
            else
                CommitMessage = string.Format("{0}: {1}\r\n\r\n{2}\r\n\r\n{3}", commitType + breakingChangeIndicator, description, body, footer);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
