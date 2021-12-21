using CommitTemplateExtension.ViewModels;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows;
using System.Windows.Controls;

namespace CommitTemplateExtension.Views
{
    public partial class NotificationControl : UserControl
    {
        public AddRemoteVM ViewModel;

        public NotificationControl(AddRemoteVM data)
        {
            InitializeComponent();
            ViewModel = data;
        }

        public void BtnCloseNotification_Click(object sender, RoutedEventArgs args)
        {

        }

        public void LnkAction_Click(object sender, RoutedEventArgs args)
        {

        }

    }
}
