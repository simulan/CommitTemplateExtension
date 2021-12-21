using CommitTemplateExtension.ViewModels;
using CommitTemplateExtension.views.navigation;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows.Controls;

namespace CommitTemplateExtension.Views
{
    public partial class AddRemoteControl : UserControl
    {
        public AddRemoteVM ViewModel;
        private INavigationParent navigationParent;
        public AddRemoteControl(INavigationParent navigationParent, AddRemoteVM data)
        {
            InitializeComponent();
            ViewModel = data;
            this.navigationParent = navigationParent;
        }

        private void BtnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Name = txtName.Text;
            ViewModel.Remote = txtRemote.Text;
            navigationParent.NavigateBack();
        }

        private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel = null;
            navigationParent.NavigateBack();
        }
    }
}
