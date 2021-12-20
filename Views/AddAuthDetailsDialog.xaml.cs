using CommitTemplateExtension.ViewModels;
using Microsoft.VisualStudio.PlatformUI;

namespace CommitTemplateExtension.Views.ToolWindows
{
    public partial class AddAuthDetailsDialog : DialogWindow
    {
        public AddAuthDetailsVM ViewModel;
        public AddAuthDetailsDialog(AddAuthDetailsVM data)
        {
            InitializeComponent();
            ViewModel = data;
        }

        private void BtnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Name = txtName.Text;
            ViewModel.TokenOrPassword = txtRemote.Text;
            Close();
        }
        private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel = null;
            Close();
        }
    }
}
