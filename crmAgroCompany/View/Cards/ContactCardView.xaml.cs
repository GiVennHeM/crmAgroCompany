

using Client.Models;
using MaterialDesignThemes.Wpf;
using System.Windows.Markup;

namespace Client.View.Cards
{
    /// <summary>
    /// Interaction logic for ContactCardView.xaml
    /// </summary>
    public partial class ContactCardView : UserControl
    {
        BrushConverter converter = new BrushConverter();
        public ContactCardView()
        {
            InitializeComponent();
            var contact = this.DataContext as Contact;
            if (contact != null)
            {
                if (contact.LeadStatus == LeadStatus.New)
                {
                    NextColoumnButton.Visibility = Visibility.Collapsed;
                }
                if (contact.Lead == false)
                {
                    BackColoumnButton.Visibility = Visibility.Collapsed;
                    NextColoumnButton.Visibility = Visibility.Collapsed;
                }
                if (contact.LeadStatus == LeadStatus.Converted)
                {
                    NextColoumnButton.Visibility = Visibility.Collapsed;
                }
            }
        }
        public event EventHandler<ContactEventArgs> ContactSelected;

        protected virtual void OnContactSelected(Contact contact)
        {
            ContactSelected?.Invoke(this, new ContactEventArgs(contact));
        }

        private void Card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var contact = this.DataContext as Contact;
            if (contact != null)
            {
                OnContactSelected(contact);
            }
        }

        private void NextColoumnButton_Click(object sender, RoutedEventArgs e)
        {

            var contact = this.DataContext as Contact;
            if (contact != null)
            {
                switch (contact.LeadStatus)
                {
                    case LeadStatus.New:
                        contact.LeadStatus = LeadStatus.Contacted;
                        break;
                    case LeadStatus.Contacted:
                        contact.LeadStatus = LeadStatus.Qualified;
                        break;
                    case LeadStatus.Qualified:
                        contact.LeadStatus = LeadStatus.Converted;
                        break;
                    case LeadStatus.Converted:
                        contact.LeadStatus = LeadStatus.Lost;
                        break;
                }

            }
        }

        private void BackColoumnButton_Click(object sender, RoutedEventArgs e)
        {

            var contact = this.DataContext as Contact;
            if (contact != null)
            {
                switch (contact.LeadStatus)
                {
                    case LeadStatus.Contacted:
                        contact.LeadStatus = LeadStatus.New;
                        break;
                    case LeadStatus.Qualified:
                        contact.LeadStatus = LeadStatus.Contacted;
                        break;
                    case LeadStatus.Converted:
                        contact.LeadStatus = LeadStatus.Qualified;
                        break;
                    case LeadStatus.Lost:
                        contact.LeadStatus = LeadStatus.Converted;
                        break;
                }

            }
        }
    }

    public class LetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
                return str[0].ToString().ToUpper();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ContactEventArgs : EventArgs
    {
        public Contact Contact { get; }

        public ContactEventArgs(Contact contact)
        {
            Contact = contact;
        }
    }
    public class FromUserNameToColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string username = value.ToString();

            // Hash the username using SHA-256 for a strong and consistent hash
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(username));
            string hexHash = BitConverter.ToString(hashBytes).Replace("-", "");
            string colorHash = hexHash.Substring(0, 6); // Extract first 6 characters


            // Convert the hash to a complete hex color code (including #)
            return $"#40{colorHash}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new FromUserNameToColorConverter();
            return _converter;
        }

        private static FromUserNameToColorConverter _converter = null;
    }

    public class ConvertorStatusToBackground : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            Contact status = value as Contact;
            if (status.LeadStatus == LeadStatus.New)
            {
                return $"#2F2F2F";
            }
            else if (status.LeadStatus == LeadStatus.Contacted)
            {
                return $"#93827F";
            }
            else if (status.LeadStatus == LeadStatus.Qualified)
            {
                return $"#F3F9D2";
            }
            else if (status.LeadStatus == LeadStatus.Lost)
            {
                return $"#BDC4A7";
            }
            else if (status.LeadStatus == LeadStatus.Converted)
            {
                return $"#92B4A7";
            } if (status.Lead == false)
            {
                return $"#61726B";
            }

            return "White";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new FromUserNameToColorConverter();
            return _converter;
        }
        private static FromUserNameToColorConverter _converter = null;
    }
}
