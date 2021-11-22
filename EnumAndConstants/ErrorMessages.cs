using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitTemplateExtension.EnumAndConstants
{
    public static class ErrorMessages
    {
        public const string COMMIT_FAILED = "An error has occured, the commit has not been succesfull";
        public const string PUSH_FAILED = "An error has occured, the push has not been succesfull";
        public const string STAGE_FAILED = "An error has occured, the staging has not been succesfull";
        public const string UNSTAGE_FAILED = "An error has occured, the unstaging has not been succesfull";
        public const string INITIALIZE_FAILED = "An error has occured, no git repository was found or it cannot be read";

    }
}
