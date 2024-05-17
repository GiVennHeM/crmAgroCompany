namespace Client.ViewModel
{
    public class LoginWindowViewModel : ViewModelBase
    {
        public OpenTabViewModel OpenTabViewModel { get; }
        public LoginPageViewModel LoginPageViewModel { get; }
        public SignUpPageViewModel SignUpPageViewModel { get; }

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICommand BackToOpenTabCommand { get; }

        public event EventHandler RequestCloseWindow;
        public event EventHandler<string> RequestNavigate;

        public LoginWindowViewModel()
        {
            OpenTabViewModel = new OpenTabViewModel();
            LoginPageViewModel = new LoginPageViewModel();
            SignUpPageViewModel = new SignUpPageViewModel();

            LoginPageViewModel.CloseThisWindow += (s, e) => OnRequestCloseWindow();
            SignUpPageViewModel.s_BackToOpenPage += (s, e) => OnRequestNavigate("OpenTab");
            LoginPageViewModel.BackToOpenPage += (s, e) => OnRequestNavigate("OpenTab");

            OpenTabViewModel.LoginButtonClicked += (s, e) => OnRequestNavigate("LoginPage");
            OpenTabViewModel.SignUpButtonClicked += (s, e) => OnRequestNavigate("SignUpPage");

            NavigateToLoginCommand = new RelayCommand(_ => OnRequestNavigate("LoginPage"));
            NavigateToSignUpCommand = new RelayCommand(_ => OnRequestNavigate("SignUpPage"));
            CloseWindowCommand = new RelayCommand(_ => OnRequestCloseWindow());
            BackToOpenTabCommand = new RelayCommand(_ => OnRequestNavigate("OpenTab"));
        }

        private void OnRequestCloseWindow()
        {
            RequestCloseWindow?.Invoke(this, EventArgs.Empty);
        }

        private void OnRequestNavigate(string page)
        {
            RequestNavigate?.Invoke(this, page);
        }
    }
}
