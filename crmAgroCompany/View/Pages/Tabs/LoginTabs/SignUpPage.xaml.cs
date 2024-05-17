using Client.ViewModel;

namespace Client.View.Pages.Tabs.LoginTabs
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
            DataContext = new SignUpPageViewModel();
        }
    }
}
