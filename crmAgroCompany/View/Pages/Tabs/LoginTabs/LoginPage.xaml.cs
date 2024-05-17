using Client.View.Windows;
using Client.ViewModel;
using System.Diagnostics;

namespace Client.View.Pages.Tabs.LoginTabs
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    ///
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            var viewModel = (LoginPageViewModel)DataContext;
            viewModel.CloseThisWindow += (s, e) => NavigationService?.GoBack();
        }


    }
}