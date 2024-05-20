using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Client.View.Windows.MainWindow;

namespace Client.View.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for SettingsPageView.xaml
    /// </summary>
    public partial class SettingsPageView : Page, INotifyPropertyChanged
    {
        private User _user;
        private readonly string _token;

        public SettingsPageView(string token)
        {
            InitializeComponent();
            _token = token;
            InitializeUser();
            SetTextBoxesEnabled(false);
        }

        private void InitializeUser()
        {
            var validator = new JwtTokenValidator("MHcCAQEEIIUBvAynpoWdGca1SFW28uk1k7Ax9kz/MB+TLqzlOIbuoAoGCCqGSM49\r\nAwEHoUQDQgAEUXNryWuRslXtLFe6QXcniexfiWo35crDg7MHMZvHECqpQunKD6Mv\r\njqVlpQydLKV+kcXMPa4J8AN4L+A55zB4ww==");
            var principal = validator.GetPrincipal(_token);

            _user = new User
            {
                UserId = Convert.ToInt16(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Role = principal.FindFirst(ClaimTypes.Role)?.Value,
                PhoneNumber = principal.FindFirst(ClaimTypes.MobilePhone)?.Value,
                Surname = principal.FindFirst(ClaimTypes.Surname)?.Value,
                Email = principal.FindFirst(ClaimTypes.Email)?.Value,
                QuantityOfСreated = Convert.ToInt16(principal.FindFirst(ClaimTypes.UserData)?.Value),
                UserName = principal.FindFirst(ClaimTypes.Name)?.Value,
                Avatar = principal.FindFirst(ClaimTypes.Uri)?.Value
            };

            LoginTextBox.Text = _user.UserName;
            RoleTextBox.Text = _user.Role;
            NameTextBox.Text = _user.UserName;
            SurnameTextBox.Text = _user.Surname;
            PhoneNumberTextBox.Text = _user.PhoneNumber;
            EmailTextBox.Text = _user.Email;
        }

        private void SetTextBoxesEnabled(bool isEnabled)
        {
            LoginTextBox.IsEnabled = isEnabled;
            RoleTextBox.IsEnabled = isEnabled;
            NameTextBox.IsEnabled = isEnabled;
            SurnameTextBox.IsEnabled = isEnabled;
            PhoneNumberTextBox.IsEnabled = isEnabled;
            EmailTextBox.IsEnabled = isEnabled;
        }

        private async void EditSaveButton_Click56(object sender, RoutedEventArgs e)
        {
            if (EditSaveButton.Content.ToString() == "Edit")
            {
                SetTextBoxesEnabled(true);
                EditSaveButton.Content = "Save";
            }
            else
            {
                SetTextBoxesEnabled(false);
                EditSaveButton.Content = "Edit";
                await SaveUserData();
            }
        }

        private JArray CreatePatchDocument()
        {
            var patchArray = new JArray();

            if (LoginTextBox.Text != _user.UserName)
            {
                patchArray.Add(new JObject
                {
                    { "op", "replace" },
                    { "path", "/UserName" },
                    { "value", LoginTextBox.Text }
                });
            }

            if (NameTextBox.Text != _user.UserName)
            {
                patchArray.Add(new JObject
                {
                    { "op", "replace" },
                    { "path", "/Name" },
                    { "value", NameTextBox.Text }
                });
            }

            if (SurnameTextBox.Text != _user.Surname)
            {
                patchArray.Add(new JObject
                {
                    { "op", "replace" },
                    { "path", "/Surname" },
                    { "value", SurnameTextBox.Text }
                });
            }

            if (PhoneNumberTextBox.Text != _user.PhoneNumber)
            {
                patchArray.Add(new JObject
                {
                    { "op", "replace" },
                    { "path", "/PhoneNumber" },
                    { "value", PhoneNumberTextBox.Text }
                });
            }

            if (EmailTextBox.Text != _user.Email)
            {
                patchArray.Add(new JObject
                {
                    { "op", "replace" },
                    { "path", "/Email" },
                    { "value", EmailTextBox.Text }
                });
            }

            return patchArray;
        }

        private async Task SaveUserData()
        {
            try
            {
                var patchDoc = CreatePatchDocument();

                using (var client = new HttpClient())
                {
                    var json = patchDoc.ToString();
                    var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

                    var response = await client.PatchAsync($"https://localhost:7280/api/User/{_user.UserId}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Failed to update user data. Status code: " + response.StatusCode, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving user data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

