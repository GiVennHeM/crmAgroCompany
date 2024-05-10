using Client.View.Windows;
namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginWindow newCustomer = new LoginWindow();
            newCustomer.Show();
        }
    }

}
