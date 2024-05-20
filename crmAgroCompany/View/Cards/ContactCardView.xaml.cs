

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
            if (value == null || !(value is Contact))
            {
                return null;
            }
            Contact contact = (Contact)value;
            if (contact.LeadStatus == LeadStatus.New)
            {
                return $"#FFF275";
            }
            else if (contact.LeadStatus == LeadStatus.Contacted)
            {
                return $"#FF8C42";
            }
            else if (contact.LeadStatus == LeadStatus.Qualified)
            {
                return $"#A23E48";
            }
            else if (contact.LeadStatus == LeadStatus.Lost)
            {
                return $"#FF3C38";
            }
            else if (contact.LeadStatus == LeadStatus.Converted)
            {
                return $"#FFBF5C";
            }
            if (contact.Lead == false)
            {
                return $"#6699CC";
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
                _converter = new ConvertorStatusToBackground();
            return _converter;
        }
        private static ConvertorStatusToBackground _converter = null;
    }
    
}
