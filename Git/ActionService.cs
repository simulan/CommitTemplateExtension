using CommitTemplateExtension.EnumAndConstants;
using CommitTemplateExtension.Utils;
using Community.VisualStudio.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitTemplateExtension.Git
{
    /// <summary>
    /// This service provides (re)actions once the application has an error or notification
    /// e.g. if no git repository is found the user can click on the notification to create a git repository
    /// 
    /// Uses the error or notification message constant to link to the correct action
    /// </summary>
    public class ActionService
    {
        private GitService gitService;

        public ActionService(GitService gitService)
        {
            this.gitService = gitService;
        }

        public bool ReactToNotification(string notificationActionConstant)
        {
            switch(notificationActionConstant)
            {
                case NotificationMessages.Actions.REPOSITORY_NONEXISTANT_ACTION:
                    var solution = VS.Solutions.GetCurrentSolutionAsync().Result;
                    return gitService.OpenOrInitRepository(System.IO.Path.GetDirectoryName(solution.FullPath));
                case NotificationMessages.Actions.SOLUTION_NONEXISTANT_ACTION:
                    string dir = FileUtils.GetDirFromUser();
                    if (dir == null)
                        return false;
                    gitService = GitService.GetInstance(dir);
                    return gitService.Initialized;
                default:
                    return false;
            }
        }
    }
}
