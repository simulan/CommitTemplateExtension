using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CommitTemplateExtension.views.navigation
{
    public interface INavigationParent
    {
        /// <summary>
        /// Navigates to a new control, temporarily moving the current control to the navigation history
        /// </summary>
        /// <param name="userControl"></param>
        void Navigate(UserControl userControl);

        /// <summary>
        /// Navigates back to the previous control
        /// </summary>
        void NavigateBack();
    }
}
