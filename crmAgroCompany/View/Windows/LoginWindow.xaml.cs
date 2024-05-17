using Client.View.Pages.Tabs.LoginTabs;
using Client.ViewModel;

namespace Client.View.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly LoginWindowViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = new LoginWindowViewModel();
            _viewModel.RequestCloseWindow += ViewModel_RequestCloseWindow;
            _viewModel.RequestNavigate += ViewModel_RequestNavigate;
            DataContext = _viewModel;

            NavigateToPage("OpenTab");
        }

        private void ViewModel_RequestCloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewModel_RequestNavigate(object sender, string pageName)
        {
            NavigateToPage(pageName);
        }

        private void NavigateToPage(string pageName)
        {
            Page page = pageName switch
            {
                "LoginPage" => new LoginPage { DataContext = _viewModel.LoginPageViewModel },
                "SignUpPage" => new SignUpPage { DataContext = _viewModel.SignUpPageViewModel },
                _ => new OpenTab { DataContext = _viewModel.OpenTabViewModel }
            };

            logFrame.Navigate(page);
        }
    }
}

