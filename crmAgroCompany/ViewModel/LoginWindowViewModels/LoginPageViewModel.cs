using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client.View.Windows;
using Newtonsoft.Json;

namespace Client.ViewModel
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string _login;
        private SecureString _password;

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public SecureString Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand OpenInstagramCommand { get; }
        public ICommand OpenGitHubCommand { get; }

        public event EventHandler<RoutedEventArgs> CloseThisWindow;
        public event EventHandler<RoutedEventArgs> BackToOpenPage;
        public LoginPageViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            BackCommand = new RelayCommand(ResetFields);
            OpenInstagramCommand = new RelayCommand(OpenInstagram);
            OpenGitHubCommand = new RelayCommand(OpenGitHub);

            Password = new SecureString();
            foreach (char c in "sashenkapaseka")
                Password.AppendChar(c);
            Login = "paseka_alex";
        }

        private async Task LoginAsync(object parameter)
        {
            try
            {
                if (Password == null)
                {
                    MessageBox.Show("Password cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var password = new System.Net.NetworkCredential(string.Empty, Password).Password;
                LoginModel model = new LoginModel()
                {
                    Login = Login,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://localhost:7280/api/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Authentication successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetFields();
                        MainWindow mainWindow = new MainWindow(jsonResponse);
                        mainWindow.Show();
                        CloseThisWindow?.Invoke(this, new RoutedEventArgs());
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error logging in: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFields(object parameter = null)
        {
            Login = "";
            Password = new SecureString();
            BackToOpenPage?.Invoke(this, new RoutedEventArgs());
        }

        private void OpenInstagram(object parameter)
        {
            var url = "https://www.instagram.com/bogggddan.kr";

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenGitHub(object parameter)
        {
            var url = "https://github.com/nationalistUAStormtrooper/crmAgroCompany";

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class LoginModel
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }


}
