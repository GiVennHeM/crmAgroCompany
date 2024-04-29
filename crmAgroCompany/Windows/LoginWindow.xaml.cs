using crmAgroCompany.Classes;
namespace crmAgroCompany
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
            var password = "";
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
                string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsPhoneNumberValid(numberofphone))
            {
                MessageBox.Show("Invalid phone number format. Please enter numbers only.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsPasswordValid(password))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain numbers and symbols.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            try
            {
                LoggerUser loggerUser = new LoggerUser()
                {
                    ProfilePicture = profileimagesource,
                    UserName = name,
                    Surname = surname,
                    PhoneNumber = numberofphone,
                    Login = login,
                    Password = password
                };

                var jsonSettings = new JsonSerializerSettings();
                var json = JsonConvert.SerializeObject(loggerUser, jsonSettings);

                // Create StringContent with JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send HTTP POST request to API endpoint
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://localhost:7280/api/Customer/loggerUsers", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("User was successfully added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetFields();
                        
                    }
                    else
                    {
                        MessageBox.Show("Error adding user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            private bool IsPasswordValid(string password)
            {
                // Check if password is at least 8 characters long
                if (password.Length < 8)
                {
                    return false;
                }

                // Check if password contains at least one digit
                if (!password.Any(char.IsDigit))
                {
                    return false;
                }
                return true;                    
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

                openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif|Все файлы (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    var imagePath = openFileDialog.FileName;
                    BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                    ProfileImageSource.Source = bitmapImage;
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
                var login = loginGridTextbox.Text; 
                var password = passwordGridTextbox.Password;

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:7280/api/Customer/loggerUser");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var loggers = JsonConvert.DeserializeObject<List<Customer>>(json);

                        foreach(var c in loggers)
                        {
                            
                        }
                        MessageBox.Show("Login successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        loginGridTextbox.Text = "";
                        passwordGridTextbox.Password = "";
                        ResetFields();
                        Hide();
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
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
        }
    }
    
