using System.Diagnostics;
namespace Client.View.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly string _connectionString;
        public LoginWindow()
        {
            InitializeComponent();
            LoginObjectsGrid.Visibility = Visibility.Collapsed;
            SignupObjectsGrid.Visibility = Visibility.Collapsed;
            gridOpen.Visibility = Visibility.Visible;
            loginGridTextbox.Text = "admin";
            passwordGridTextbox.Password = "1233211233213";
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            gridOpen.Visibility = Visibility.Collapsed;
            LoginObjectsGrid.Visibility = Visibility.Visible;
        }
        private void SignUpButton_Click(Object sender, RoutedEventArgs e)
        {
            gridOpen.Visibility = Visibility.Collapsed;
            SignupObjectsGrid.Visibility = Visibility.Visible;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var profileimagesource = Convert.ToString(ProfileImageSource.Source);
            var login = signupLoginTextBox.Text;
            var name = signupNameTextBox.Text;
            var surname = signupSurnameTextBox.Text;
            var numberofphone = signupNumberofphoneTextBox.Text;
            var password = signupPasswordBox.Password;
            var role = TypeofAccountCombobox.Text;
            var email = emailTextBox.Text;
            if (signupPasswordBox.Password == PasswordBoxSignupCheker.Password)
            {
                password = signupPasswordBox.Password;
            }
            else
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) ||
                string.IsNullOrEmpty(numberofphone) ||
                string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            if (!IsPhoneNumberValid(numberofphone))
            {
                MessageBox.Show("Invalid phone number format. Please enter numbers only.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            try
            {
                Client.Models.User user = new Client.Models.User()
                {
                    Avatar = profileimagesource,
                    UserName = name,
                    Surname = surname,
                    PhoneNumber = numberofphone,
                    Login = login,
                    Password = password,
                    Role = role,
                    Email = email,
                    QuantityOfСreated = 5
                };

                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://localhost:7280/api/signup", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("User was successfully added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetFields();
                    }
                    else
                    {
                        MessageBox.Show($"Error adding user:{response}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit);
        }


        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                var imagePath = openFileDialog.FileName;
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));


            }
        }


        private void ResetFields()
        {
            signupNameTextBox.Text = "";
            signupSurnameTextBox.Text = "";
            signupNumberofphoneTextBox.Text = "";
            signupLoginTextBox.Text = "";
            signupPasswordBox.Password = "";
            loginGridTextbox.Text = "";
            passwordGridTextbox.Password = "";
            gridOpen.Visibility = Visibility.Visible;
            LoginObjectsGrid.Visibility = Visibility.Collapsed;
            SignupObjectsGrid.Visibility = Visibility.Collapsed;
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = loginGridTextbox.Text;
                var password = passwordGridTextbox.Password;

                LoginModel model = new LoginModel()
                {
                    Login = login,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(model);

                // Create StringContent with JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send HTTP POST request to API endpoint
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://localhost:7280/api/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        MessageBox.Show("Authentication successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        loginGridTextbox.Text = "";
                        passwordGridTextbox.Password = "";
                        ResetFields();

                        MainWindow mainWindow = new MainWindow(jsonResponse);
                        mainWindow.Show();
                        this.Close();
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

        public class LoginModel
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
    }
}

