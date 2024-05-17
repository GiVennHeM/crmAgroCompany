using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Client.View.Windows.MainWindow;
using Client.View.Cards;
namespace Client.View.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for ContactsPageView.xaml
    /// </summary>
    public partial class ContactsPageView : Page, INotifyPropertyChanged
    {
        private Contact _selectedContact;
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
        public ContactsPageView(string token)
        {
            InitializeComponent();
            LoadContacts();
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

            NotLeadContactCard.DataContext = this;
            NewLeadContactCard.DataContext = this;
        }
        private void OnContactSelected(object sender, ContactEventArgs e)
        {
            _selectedContact = e.Contact;
            InfoContact.Navigate(new ContactInfoPageView(_selectedContact));
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = addContactName.Text;
                var surname = addContactSurname.Text;
                var numberphone = addContactNumberofphoneTextBox.Text;
                var email = addContactEmail.Text;
                var address = addContactAddress.Text;
                var city = addContactCity.Text;
                var region = addContactRegion.Text;
                var country = addContactCountry.Text;
                var postalcode = addContactPostalCode.Text;
                var ageText = addContactAge.Text;
                var description = addContactDescription.Text;
                var lead = addContactlead.IsChecked ?? false;

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(numberphone) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) ||
                string.IsNullOrEmpty(region) || string.IsNullOrEmpty(country) || string.IsNullOrEmpty(postalcode) ||
                string.IsNullOrEmpty(ageText) || string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int? age = null;
                if (!string.IsNullOrEmpty(ageText) && int.TryParse(ageText, out int parsedAge))
                {
                    age = parsedAge;
                }

                var contact = new Contact
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

                if (lead == true)
                {
                    contact.LeadStatus = 0;
                    contact.LastContactDate = DateTime.Now;
                    contact.LastContactedBy = $"{_user.UserName} {_user.Surname}";

                    if (addContactLeadSource.SelectedIndex >= 0)
                    {
                        switch (addContactLeadSource.SelectedIndex)
                        {
                            case 0:
                                contact.LeadSource = LeadSource.Personal;
                                break;
                            case 1:
                                contact.LeadSource = LeadSource.International;
                                break;
                            case 2:
                                contact.LeadSource = LeadSource.Client;
                                break;
                            default:
                                contact.LeadSource = LeadSource.Personal;
                                break;
                        }

                    }

                    if (addContactLeadPriority.SelectedIndex >= 0)
                    {
                        switch (addContactLeadPriority.SelectedIndex)
                        {
                            case 0:
                                contact.Priority = LeadPriority.Low;
                                break;
                            case 1:
                                contact.Priority = LeadPriority.Medium;
                                break;
                            case 2:
                                contact.Priority = LeadPriority.High;
                                break;
                            default:
                                contact.Priority = LeadPriority.Low;
                                break;
                        }
                    }
                }
                else
                {
                    contact.LeadStatus = null; contact.LeadSource = null; contact.Priority = null; contact.LastContactDate = null; contact.LastContactedBy = null;
                }

                var jsonContact = JsonConvert.SerializeObject(contact);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonContact, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://localhost:7280/api/Contact", content);

                    if (response.IsSuccessStatusCode)
                    {
                        _user.QuantityOfСreated++;
                        ClearContactFields();
                        LoadContacts();
                        NewContactBorder.Visibility = Visibility.Collapsed;
                        contactsDataGrid.Visibility = Visibility.Visible;
                        MessageBox.Show("Successfully added new contact " + contact.Name, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add contact. Status code: " + response.StatusCode, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearContactFields()
        {
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
            addContactlead.IsChecked = false;
            addContactLeadSource.SelectedIndex = -1;
            addContactLeadPriority.SelectedIndex = -1;
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

                        NoLeadContacts = Contacts.Where(c => c.Lead == false).ToList();
                        Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                        NewLeadContacts = Contacts.Where(c => c.LeadStatus == LeadStatus.New).OrderBy(c => c.LeadStatus).ToList();
                        Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                        ContactedLeadContacts = Contacts.Where(c => c.LeadStatus == LeadStatus.Contacted).OrderBy(c => c.LeadStatus).ToList();
                        Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                        QualifiedLeadContacts = Contacts.Where(c => c.LeadStatus == LeadStatus.Qualified).OrderBy(c => c.LeadStatus).ToList();
                        Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                        LostLeadContacts = Contacts.Where(c => c.LeadStatus == LeadStatus.Lost).OrderBy(c => c.LeadStatus).ToList();
                        Contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                        ConvertedLeadContacts = Contacts.Where(c => c.LeadStatus == LeadStatus.Converted).OrderBy(c => c.LeadStatus).ToList();
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            contactsBorder.Visibility = Visibility.Collapsed;
            contactsDataGrid.Visibility = Visibility.Collapsed;
        }
        private List<Contact> _noLeadContacts;
        public List<Contact> NoLeadContacts
        {
            get { return _noLeadContacts; }
            set
            {
                _noLeadContacts = value;
                OnPropertyChanged(nameof(NoLeadContacts));
            }
        }
        private List<Contact> _newLeadContacts;
        public List<Contact> NewLeadContacts
        {
            get { return _newLeadContacts; }
            set
            {
                _newLeadContacts = value;
                OnPropertyChanged(nameof(NewLeadContacts));
            }
        }
        private List<Contact> _ContactedLeadContacts;
        public List<Contact> ContactedLeadContacts
        {
            get { return _ContactedLeadContacts; }
            set
            {
                _ContactedLeadContacts = value;
                OnPropertyChanged(nameof(ContactedLeadContacts));
            }
        }
        private List<Contact> _QualifiedLeadContacts;
        public List<Contact> QualifiedLeadContacts
        {
            get { return _QualifiedLeadContacts; }
            set
            {
                _QualifiedLeadContacts = value;
                OnPropertyChanged(nameof(QualifiedLeadContacts));
            }
        }
        private List<Contact> _LostLeadContacts;
        public List<Contact> LostLeadContacts
        {
            get { return _LostLeadContacts; }
            set
            {
                _LostLeadContacts = value;
                OnPropertyChanged(nameof(LostLeadContacts));
            }
        }
        private List<Contact> _ConvertedLeadContacts;
        public List<Contact> ConvertedLeadContacts
        {
            get { return _ConvertedLeadContacts; }
            set
            {
                _ConvertedLeadContacts = value;
                OnPropertyChanged(nameof(ConvertedLeadContacts));
            }
        }
    }
}
