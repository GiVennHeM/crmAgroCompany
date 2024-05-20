using Client.View.Pages.Tabs;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace Client.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private User _user;
        private readonly string _token;

        public MainWindow(string token)
        {
            InitializeComponent();
            mainFrame.Navigate(new HomePageView());
            DataContext = this;
            _token = token;
            var validator = new JwtTokenValidator("MHcCAQEEIIUBvAynpoWdGca1SFW28uk1k7Ax9kz/MB+TLqzlOIbuoAoGCCqGSM49\r\nAwEHoUQDQgAEUXNryWuRslXtLFe6QXcniexfiWo35crDg7MHMZvHECqpQunKD6Mv\r\njqVlpQydLKV+kcXMPa4J8AN4L+A55zB4ww==");
            var principal = validator.GetPrincipal(_token);

            User user = new User
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
            _user = user;

            if (principal != null)
            {
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    try
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(user.Avatar);
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        ProfileImageSource.Source = bitmapImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Image path not found in claims.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                meButton.Content = $"Hello, {user.UserName}";
            }
            else
            {
                MessageBox.Show("Failed to use token.");
            }
            SearchContactCard.DataContext = this;
        }
        private async void StaffButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new StaffPageView());

        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new HomePageView());
        }
        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new DealsPageView());
        }
        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ProductsPageView());
        }
        public class JwtTokenValidator
        {
            private readonly string _secretKey;

            public JwtTokenValidator(string secretKey)
            {
                _secretKey = secretKey;
            }

            public ClaimsPrincipal GetPrincipal(string token)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = GetValidationParameters();

                    SecurityToken validatedToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                    return principal;
                }
                catch
                {
                    return null;
                }
            }

            private TokenValidationParameters GetValidationParameters()
            {
                return new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "crm_server",
                    ValidAudience = "crm_client",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
                };
            }
        }

        private void ContactsButton_Click_1(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ContactsPageView(_token));
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPageView(_token));
        }

        private void meButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPageView(_token));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search.TextChanged += Search_TextChanged1;
        }

        private async void Search_TextChanged1(object sender, TextChangedEventArgs e)
        {
            var name = Search.Text;

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"https://localhost:7280/api/SearchContacts?Name={name}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                            SearchedContacts = contacts;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading user contacts: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private List<Contact> _SearchedContacts;
        public List<Contact> SearchedContacts
        {
            get => _SearchedContacts;
            set
            {
                _SearchedContacts = value;
                OnPropertyChanged(nameof(SearchedContacts));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}