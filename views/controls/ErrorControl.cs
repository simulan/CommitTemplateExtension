using CommitTemplateExtension.ViewModels;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows;
using System.Windows.Controls;

namespace CommitTemplateExtension.Views
{
    public partial class ErrorControl : UserControl
    {
        public AddRemoteVM ViewModel;

        public ErrorControl(AddRemoteVM data)
        {
            InitializeComponent();
            ViewModel = data;
        }

        public void BtnCloseError_Click(object sender, RoutedEventArgs args)
        {

        }
    }
}
