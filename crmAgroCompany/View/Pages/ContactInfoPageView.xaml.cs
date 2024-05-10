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
            DataCreated.Text = contact.CreatedDate.ToShortDateString();
            Creator.Text = contact.CreatorUserId.ToString(); // Assuming CreatorUserId is an int
            addContactDescription.Text = contact.Description;
            IdContact.Text = contact.ContactId.ToString();

            // Populate lead-related fields if it's a lead contact
            if (contact.Lead)
            {
                LeadInfo.IsChecked = true;
                ContactLeadSource.SelectedItem = contact.LeadSource?.ToString();
                ContactLeadPriority.SelectedItem = contact.Priority?.ToString();
                ContactLeadStatus.SelectedItem = contact.LeadStatus?.ToString();
                LastContactDateLeadStatus.SelectedDate = contact.LastContactDate;
                LastContactByLeadStatus.Text = contact.LastContactedBy;
            }

        }
    }
}
