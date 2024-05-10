using Client.View.Pages;
using Client.View.Pages.Tabs;
using Client.ViewModels;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.View.Cards
{
    /// <summary>
    /// Interaction logic for ContactCardView.xaml
    /// </summary>
    public partial class ContactCardView : System.Windows.Controls.UserControl
    {
        BrushConverter converter = new BrushConverter();
        public ContactCardView()
        {
            InitializeComponent();
            CheckLeadStatus();
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
        private void CheckLeadStatus()
        {
            var contact = this.DataContext as Contact;

            if (contact != null)
            {
                if (contact.Lead)
                {
                    if (contact.LeadStatus == LeadStatus.New)
                    {
                        Brush? brush = converter.ConvertFromString("#93827F") as Brush;
                        Card.Background = brush;
                    }
                    else if (contact.LeadStatus == LeadStatus.Contacted)
                    {
                        Brush brush = (Brush)converter.ConvertFromString("#F3F9D2");
                        Card.Background = brush;
                    }
                    else if (contact.LeadStatus == LeadStatus.Qualified)
                    {
                        Brush brush = (Brush)converter.ConvertFromString("#BDC4A7");
                        Card.Background = brush;
                    }
                    else if (contact.LeadStatus == LeadStatus.Lost)
                    {
                        Brush brush = (Brush)converter.ConvertFromString("#2F2F2F");
                        Card.Background = brush;
                    }
                    else if (contact.LeadStatus == LeadStatus.Converted)
                    {
                        Brush brush = (Brush)converter.ConvertFromString("#92B4A7");
                        Card.Background = brush;
                    }
                }
                else
                {
                    Brush brush = (Brush)converter.ConvertFromString("#6B8F71");
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

}
