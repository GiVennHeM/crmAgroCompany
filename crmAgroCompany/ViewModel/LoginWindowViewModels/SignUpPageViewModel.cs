using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class SignUpPageViewModel : ViewModelBase
    {
        private string _profileImageSource;
        private string _login;
        private string _name;
        private string _surname;
        private string _numberOfPhone;
        private SecureString _password;
        private string _role;
        private string _email;

        public string ProfileImageSource
        {
            get => _profileImageSource;
            set => SetProperty(ref _profileImageSource, value);
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        public string NumberOfPhone
        {
            get => _numberOfPhone;
            set => SetProperty(ref _numberOfPhone, value);
        }

        public SecureString Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

       

        public string Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public ICommand SignUpCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand UploadImageCommand { get; }

        public event EventHandler<RoutedEventArgs> s_BackToOpenPage;

        public SignUpPageViewModel()
        {
            SignUpCommand = new AsyncRelayCommand(SignUpAsync);
            ResetCommand = new RelayCommand(ResetFields);
            UploadImageCommand = new RelayCommand(UploadImage);

            Password = new SecureString();
        }

        private async Task SignUpAsync(object parameter)
        {
            if (Password == null)
            {
                MessageBox.Show("Password cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
          

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname) ||
                string.IsNullOrEmpty(NumberOfPhone) || string.IsNullOrEmpty(Login) ||
                Password == null || string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsPhoneNumberValid(NumberOfPhone))
            {
                MessageBox.Show("Invalid phone number format. Please enter numbers only.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var password = new System.Net.NetworkCredential(string.Empty, Password).Password;
                var user = new Client.Models.User
                {
                    Avatar = ProfileImageSource,
                    UserName = Name,
                    Surname = Surname,
                    PhoneNumber = NumberOfPhone,
                    Login = Login,
                    Password = password,
                    Role = Role,
                    Email = Email,
                    QuantityOfСreated = 0
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
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        MessageBox.Show("User with this email or login already exists. Please enter other data.", "User already in System", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show($"Error adding user: {response}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ResetFields(object parameter = null)
        {
            ProfileImageSource = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            NumberOfPhone = string.Empty;
            Login = string.Empty;
            Password = new SecureString();
            Role = string.Empty;
            Email = string.Empty;

            s_BackToOpenPage?.Invoke(this, new RoutedEventArgs());
        }

        private void UploadImage(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Images (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImageSource = openFileDialog.FileName;
            }
        }
    }
}

