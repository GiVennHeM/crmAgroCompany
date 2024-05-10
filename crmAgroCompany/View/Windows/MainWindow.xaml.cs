using Client.View.Pages.Tabs;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace Client.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private User _user;
        private readonly string _token;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public MainWindow(string token)
        {
            InitializeComponent();
            homeBorder.Visibility = Visibility.Visible;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
            staffDataGrid.IsReadOnly = true;
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


                meButton.Content = $"Helo, {user.UserName}";
            }
            else
            {
                MessageBox.Show("Failed to use token.");
            }

        }


        private async Task LoadStaff()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7280/api/Users");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var customers = JsonConvert.DeserializeObject<List<User>>(json);

                        staffDataGrid.ItemsSource = customers;

                        customers.Sort((x, y) => y.QuantityOfСreated.CompareTo(x.QuantityOfСreated));
                        var top10 = customers.Take(10);

                        // Load chart data
                        SeriesCollection = new SeriesCollection();
                        Labels = new string[10];
                        ChartValues<int> values = new ChartValues<int>();

                        int index = 0;
                        foreach (var user in top10)
                        {
                            Labels[index] = user.UserName;
                            values.Add(user.QuantityOfСreated);
                            index++;
                        }

                        SeriesCollection.Add(new ColumnSeries
                        {
                            Title = "Quantity of created contacts",
                            Values = values
                        });

                        SeriesCollection.Add(new ColumnSeries
                        {
                            Title = "Quantity of created contacts",
                            Values = values,
                            Fill = values.Count > 30 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22231E")) :
values.Count > 20 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E885B")) :
                                values.Count < 20 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8F71")) :
                                  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8F71"))
                        });


                        Formatter = value => value.ToString("N");

                        DataContext = this;
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve customer data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading customers: " + ex.Message);
            }
        }

        private async void StaffButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Visible;
            settingsBorder.Visibility = Visibility.Collapsed;
            await LoadStaff(); // Ensure that LoadStaff is awaited
        }

        private void staffDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName != "UserName" &&
            e.PropertyName != "PhoneNumber" &&
            e.PropertyName != "Surname" &&
            e.PropertyName != "Role" &&
            e.PropertyName != "Email" &&
            e.PropertyName != "QuantityOfСreated" &&
            e.PropertyName != "UserId")
            {
                e.Cancel = true;
            }
            if (e.PropertyName == "UserId")
                e.Column.Header = "User Id";
            if (e.PropertyName == "UserName")
                e.Column.Header = "Name of employee";
            if (e.PropertyName == "PhoneNumber")
                e.Column.Header = "Phone number";
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Visible;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }



        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Visible;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Hidden;
            clientsBorder.Visibility = Visibility.Hidden;
            productBorder.Visibility = Visibility.Hidden;
            dealBorder.Visibility = Visibility.Visible;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Visible;
            dealBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
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
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
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
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Collapsed;

            settingsBorder.Visibility = Visibility.Visible;
        }



        private async void NewLeadContactCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }

}