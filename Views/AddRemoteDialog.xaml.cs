using CommitTemplateExtension.ViewModels;
using Microsoft.VisualStudio.PlatformUI;

namespace CommitTemplateExtension.Views
{
    public partial class AddRemoteDialog : DialogWindow
    {
        public AddRemoteVM ViewModel;

        public AddRemoteDialog(AddRemoteVM data)
        {
            InitializeComponent();
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
