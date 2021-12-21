using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CommitTemplateExtension.views.navigation
{
    public static class NavigationManager
    {
        private static List<UserControl> _previousControls = new List<UserControl>();
        private const int STACK_LIMIT = 2;
        public static void Navigate(this CommitTemplateToolWindowControl toolWindow, UserControl control)
        {
            if(toolWindow.pnlContainer.Children.Count>0)
                _previousControls.Add(toolWindow.pnlContainer.Children[0] as UserControl);
            toolWindow.pnlContainer.Children.Clear();
            toolWindow.pnlContainer.Children.Add(control);

            if(_previousControls.Count > STACK_LIMIT)
                _previousControls.RemoveAt(0);
        }

        public static void NavigateBack(this CommitTemplateToolWindowControl toolWindow)
        {
            toolWindow.pnlContainer.Children.Clear();
            UserControl latestControl = _previousControls.Last();
            toolWindow.pnlContainer.Children.Add(latestControl);
            _previousControls.Remove(latestControl);
        }
    }
}
