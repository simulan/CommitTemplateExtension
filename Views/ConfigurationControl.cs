using CommitTemplateExtension.ViewModels;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows.Controls;

namespace CommitTemplateExtension.Views
{
    public partial class ConfigurationControl : UserControl
    {
        public AddRemoteVM ViewModel;

        public ConfigurationControl(AddRemoteVM data)
        {
            ViewModel = data;
        }

        private void BtnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Name = txtName.Text;
            ViewModel.Remote = txtRemote.Text;
            Close();
        }

        private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel = null;
            Close();
        }
    }
}
