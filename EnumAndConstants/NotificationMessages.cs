using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitTemplateExtension.EnumAndConstants
{
    public static class NotificationMessages
    {

        public const string REPOSITORY_NONEXISTANT = "There is no git repository yet for the current solution.";
        public const string SOLUTION_NONEXISTANT = "There is no solution loaded at the moment.";

        public static class Actions
        {
            public const string REPOSITORY_NONEXISTANT_ACTION = "Initiate this directory as repository.";
            public const string SOLUTION_NONEXISTANT_ACTION = "Choose a directory and initiate it.";
        }


        public static readonly Dictionary<string, string> NotificationByAction = new Dictionary<string, string>()
        {
            { REPOSITORY_NONEXISTANT,  Actions.REPOSITORY_NONEXISTANT_ACTION},
            { SOLUTION_NONEXISTANT,  Actions.SOLUTION_NONEXISTANT_ACTION}
        };

    }
}
