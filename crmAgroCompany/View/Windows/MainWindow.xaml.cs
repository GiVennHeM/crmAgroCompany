using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private List<Contact> _contacts;
        public List<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }

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
            contactsBorder.Visibility = Visibility.Collapsed;
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
            contactsBorder.Visibility = Visibility.Collapsed;
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
            contactsBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void addContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewContactBorder.Visibility == Visibility.Collapsed)
            {
                // Show NewContactBorder and hide contactsDataGrid and BorderEditContact
                NewContactBorder.Visibility = Visibility.Visible;
                contactsDataGrid.Visibility = Visibility.Collapsed;
                BorderCardsContact.Visibility = Visibility.Collapsed;

            }
            else if (NewContactBorder.Visibility == Visibility.Visible)
            {
                // Hide NewContactBorder and show contactsDataGrid and BorderEditContact
                NewContactBorder.Visibility = Visibility.Collapsed;
                contactsDataGrid.Visibility = Visibility.Visible;
                BorderCardsContact.Visibility = Visibility.Collapsed;
            }

        }

        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Visible;
            contactsBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Hidden;
            clientsBorder.Visibility = Visibility.Hidden;
            productBorder.Visibility = Visibility.Hidden;
            dealBorder.Visibility = Visibility.Visible;
            contactsBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Visible;
            dealBorder.Visibility = Visibility.Collapsed;
            contactsBorder.Visibility = Visibility.Collapsed;
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
            contactsBorder.Visibility = Visibility.Visible;
            settingsBorder.Visibility = Visibility.Collapsed;
            LoadContacts();
        }
        private async void LoadContacts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7280/api/Contacts");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);

                        var filteredContacts = Contacts.Select(c => new
                        {
                            c.ContactId,
                            c.Name,
                            c.Surname,
                            c.PhoneNumber,
                            c.Email,
                            c.Address,
                            c.City,
                            c.Region,
                            c.PostalCode,
                            c.Country,
                            c.Age,
                            c.CreatedDate,
                            Creator = c.CreatorUserId
                        }).ToList();

                        contactsDataGrid.ItemsSource = filteredContacts;
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve contact data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading contacts: " + ex.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void contactsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            contactsBorder.Visibility = Visibility.Collapsed;
            contactsDataGrid.Visibility = Visibility.Collapsed;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            homeBorder.Visibility = Visibility.Collapsed;
            clientsBorder.Visibility = Visibility.Collapsed;
            productBorder.Visibility = Visibility.Collapsed;
            dealBorder.Visibility = Visibility.Collapsed;
            contactsBorder.Visibility = Visibility.Collapsed;

            settingsBorder.Visibility = Visibility.Visible;
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = string.IsNullOrEmpty(addContactName.Text) ? null : addContactName.Text;
                var surname = string.IsNullOrEmpty(addContactSurname.Text) ? null : addContactSurname.Text;
                var numberphone = string.IsNullOrEmpty(addContactNumberofphoneTextBox.Text) ? null : addContactNumberofphoneTextBox.Text;
                var email = string.IsNullOrEmpty(addContactEmail.Text) ? null : addContactEmail.Text;
                var address = string.IsNullOrEmpty(addContactAddress.Text) ? null : addContactAddress.Text;
                var city = string.IsNullOrEmpty(addContactCity.Text) ? null : addContactCity.Text;
                var region = string.IsNullOrEmpty(addContactRegion.Text) ? null : addContactRegion.Text;
                var country = string.IsNullOrEmpty(addContactCountry.Text) ? null : addContactCountry.Text;
                var postalcode = string.IsNullOrEmpty(addContactPostalCode.Text) ? null : addContactPostalCode.Text;
                var ageText = string.IsNullOrEmpty(addContactAge.Text) ? null : addContactAge.Text;
                var description = string.IsNullOrEmpty(addContactDescription.Text) ? null : addContactDescription.Text;
                var lead = false;
                if (addContactlead.IsChecked == true)
                {
                    lead = true;
                }
                else lead = false;
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(numberphone))
                {
                    MessageBox.Show("Please fill in all required fields (Name, surname, and number of phone).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int? age = null;
                if (int.TryParse(ageText, out int parsedAge))
                {
                    age = parsedAge;
                }

                Contact contact = new Contact()
                {
                    Name = name,
                    Surname = surname,
                    PhoneNumber = numberphone,
                    Email = email,
                    Address = address,
                    City = city,
                    Region = region,
                    Country = country,
                    PostalCode = postalcode,
                    Age = age,
                    Description = description,
                    CreatedDate = DateTime.Now,
                    CreatorUserId = _user.UserId,
                    Lead = lead
                };
                if (contact.Lead)
                {
                    contact.LeadStatus = LeadStatus.New;
                    contact.LastContactDate = DateTime.Now;
                    contact.LastContactedBy = $"{_user.UserName} {_user.Surname}";
                    if (addContactLeadSource.SelectedIndex == 0)
                    {
                        contact.LeadSource = LeadSource.Personal;
                    }
                    if (addContactLeadSource.SelectedIndex == 1)
                    {
                        contact.LeadSource = LeadSource.International;
                    }
                    if (addContactLeadSource.SelectedIndex == 2)
                    {
                        contact.LeadSource = LeadSource.Client;
                    }
                }

                string jsonContact = JsonConvert.SerializeObject(contact);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonContact, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://localhost:7280/api/Contact", content);

                    if (response.IsSuccessStatusCode)
                    {
                        _user.QuantityOfСreated++;

                        addContactName.Text = "";
                        addContactSurname.Text = "";
                        addContactNumberofphoneTextBox.Text = "";
                        addContactEmail.Text = "";
                        addContactAddress.Text = "";
                        addContactCity.Text = "";
                        addContactRegion.Text = "";
                        addContactCountry.Text = "";
                        addContactPostalCode.Text = "";
                        addContactAge.Text = "";
                        addContactDescription.Text = "";

                        LoadContacts();
                        NewContactBorder.Visibility = Visibility.Collapsed;
                        contactsDataGrid.Visibility = Visibility.Visible;
                        MessageBox.Show("Successful added new contact " + contact.Name, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve contact data. Status code: " + response.StatusCode, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading contacts: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void RadioButtonGroupChoiceChipPrimaryOutline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RadioButtonGroupChoiceChipPrimaryOutline.SelectedItem != null)
            {
                var selectedListItem = (RadioButtonGroupChoiceChipPrimaryOutline.SelectedItem as ListBoxItem).Content.ToString();

                if (selectedListItem == "Cards")
                {
                    contactsDataGrid.Visibility = Visibility.Collapsed;
                    BorderCardsContact.Visibility = Visibility.Visible;
                    NewContactBorder.Visibility = Visibility.Collapsed;
                }
                else if (selectedListItem == "Table")
                {
                    contactsDataGrid.Visibility = Visibility.Visible;
                    BorderCardsContact.Visibility = Visibility.Collapsed;
                    NewContactBorder.Visibility = Visibility.Collapsed;
                }
            }
        }
        public class BoolToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool isChecked && isChecked)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }

}