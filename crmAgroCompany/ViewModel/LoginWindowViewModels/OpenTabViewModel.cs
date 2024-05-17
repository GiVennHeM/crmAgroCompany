namespace Client.ViewModel
{
    public class OpenTabViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand SignUpCommand { get; }

        public event EventHandler<RoutedEventArgs> LoginButtonClicked;
        public event EventHandler<RoutedEventArgs> SignUpButtonClicked;
        public OpenTabViewModel()
        {
            LoginCommand = new RelayCommand(OnLoginButtonClicked);
            SignUpCommand = new RelayCommand(OnSignUpButtonClicked);
        }

        private void OnLoginButtonClicked(object parameter)
        {
            LoginButtonClicked?.Invoke(this, new RoutedEventArgs());
        }

        private void OnSignUpButtonClicked(object parameter)
        {
            SignUpButtonClicked?.Invoke(this, new RoutedEventArgs());
        }

    }

}
