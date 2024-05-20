using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;

namespace Client.View.Pages
{
    /// <summary>
    /// Interaction logic for ContactPage.xaml
    /// </summary>
    public partial class ContactInfoPageView : Page
    {
        private Contact _selectedContact;


        public ContactInfoPageView(Contact selectedContact)
        {
            InitializeComponent();

            _selectedContact = selectedContact;
            LoadContactInfo(_selectedContact);

            ContactLeadStatus.Items.Add(LeadStatus.Contacted);
            ContactLeadStatus.Items.Add(LeadStatus.Qualified);
            ContactLeadStatus.Items.Add(LeadStatus.Converted);
            ContactLeadStatus.Items.Add(LeadStatus.Lost);
        }
        public void LoadContactInfo(Contact contact)
        {
            Name.Text = contact.Name;
            Surname.Text = contact.Surname;
            NumberofPhone.Text = contact.PhoneNumber;
            Email.Text = contact.Email;
            Age.Text = contact.Age.ToString();
            Address.Text = contact.Address;
            City.Text = contact.City;
            Region.Text = contact.Region;
            PostalCode.Text = contact.PostalCode;
            Country.Text = contact.Country;
            DataCreated.Text = "Date created:" + contact.CreatedDate.ToShortDateString();
            Creator.Text = "Creator:" + contact.CreatorUserId.ToString(); // Assuming CreatorUserId is an int
            addContactDescription.Text = contact.Description;
            IdContact.Text = "Id - " + contact.ContactId.ToString();

            if (contact.Lead)
            {
                Lead.IsChecked = true;
                ContactLeadSource.Text = contact.LeadSource?.ToString();
                ContactLeadPriority.Text = contact.Priority?.ToString();
                ContactLeadStatus.SelectedItem = contact.LeadStatus;
                LastContactDate.SelectedDate = contact.LastContactDate;
                LastContactBy.Text = contact.LastContactedBy;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditSaveButton.Content.ToString() == "Edit")
            {
                SetEditMode(true);
                EditSaveButton.Content = "Save";
            }
            else
            {
                SaveChanges();
                EditSaveButton.Content = "Edit";
            }
        }
        private void SetEditMode(bool isEditMode)
        {
            Name.IsEnabled = isEditMode;
            Surname.IsEnabled = isEditMode;
            NumberofPhone.IsEnabled = isEditMode;
            Email.IsEnabled = isEditMode;
            Age.IsEnabled = isEditMode;
            Address.IsEnabled = isEditMode;
            City.IsEnabled = isEditMode;
            Region.IsEnabled = isEditMode;
            PostalCode.IsEnabled = isEditMode;
            Country.IsEnabled = isEditMode;
            addContactDescription.IsEnabled = isEditMode;
            Lead.IsEnabled = isEditMode;
            ContactLeadSource.IsEnabled = isEditMode;
            ContactLeadPriority.IsEnabled = isEditMode;
            ContactLeadStatus.IsEnabled = isEditMode;
            LastContactDate.IsEnabled = isEditMode;
            LastContactBy.IsEnabled = isEditMode;
        }
        private async Task SaveChanges()
        {
            try
            {
                var name = Name.Text;
                var surname = Surname.Text;
                var numberofphone = NumberofPhone.Text;
                var email = Email.Text;
                var age = Age.Text;
                var address = Address.Text;
                var city = City.Text;
                var region = Region.Text;
                var postalCode = PostalCode.Text;
                var country = Country.Text;
                var description = addContactDescription.Text;
                var lead = Lead;


                int contactId = _selectedContact.ContactId;

                _selectedContact.Name = name;
                _selectedContact.Surname = surname;
                _selectedContact.PhoneNumber = numberofphone;
                _selectedContact.Email = email;
                _selectedContact.Address = address;
                _selectedContact.City = city;
                _selectedContact.Region = region;
                _selectedContact.Country = country;
                _selectedContact.PostalCode = postalCode;
                _selectedContact.Age = Convert.ToInt32(age);
                _selectedContact.Description = description;
                _selectedContact.CreatedDate = DateTime.Now;
                if (Lead.IsChecked == false)
                    _selectedContact.Lead = false;
                else _selectedContact.Lead = true;

                if (_selectedContact.Lead)
                {
                    if (ContactLeadStatus.SelectedIndex >= 0)
                    {
                        switch (ContactLeadStatus.SelectedItem)
                        {
                            case LeadStatus.Qualified:
                                _selectedContact.LeadStatus = LeadStatus.Qualified;
                                break;
                            case LeadStatus.Lost:
                                _selectedContact.LeadStatus = LeadStatus.Lost;
                                break;
                            case LeadStatus.Converted:
                                _selectedContact.LeadStatus = LeadStatus.Converted;
                                break;
                            case LeadStatus.Contacted:
                                _selectedContact.LeadStatus = LeadStatus.Contacted;
                                break;
                            default:
                                _selectedContact.LeadStatus = LeadStatus.New;
                                break;
                        }
                    }
                    _selectedContact.LastContactDate = LastContactDate.SelectedDate;

                    if (ContactLeadSource.SelectedIndex >= 0)
                    {
                        switch (ContactLeadSource.SelectedIndex)
                        {
                            case 0:
                                _selectedContact.LeadSource = LeadSource.Personal;
                                break;
                            case 1:
                                _selectedContact.LeadSource = LeadSource.International;
                                break;
                            case 2:
                                _selectedContact.LeadSource = LeadSource.Client;
                                break;
                            default:
                                _selectedContact.LeadSource = LeadSource.Personal;
                                break;
                        }
                    }

                    if (ContactLeadSource.SelectedIndex >= 0)
                    {
                        switch (ContactLeadSource.SelectedIndex)
                        {
                            case 0:
                                _selectedContact.Priority = LeadPriority.Low;
                                break;
                            case 1:
                                _selectedContact.Priority = LeadPriority.Medium;
                                break;
                            case 2:
                                _selectedContact.Priority = LeadPriority.High;
                                break;
                            default:
                                _selectedContact.Priority = LeadPriority.Low;
                                break;
                        }
                    }
                }
                else
                {
                    _selectedContact.LeadStatus = null;
                    _selectedContact.LeadSource = null;
                    _selectedContact.Priority = null;
                    _selectedContact.LastContactDate = null;
                    _selectedContact.LastContactedBy = null;
                }

                var jsonContact = JsonConvert.SerializeObject(_selectedContact);

                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonContact, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync($"https://localhost:7280/api/Contact/{contactId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Contact updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update contact. Status code: " + response.StatusCode, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
