using MaterialDesignThemes.Wpf;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace crmAgroCompany;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        homeBorder.Visibility = Visibility.Visible;
        clientsBorder.Visibility = Visibility.Hidden;
        ClientsButton.IsEnabled = true;
        if(HomeButton.IsEnabled)
           HomeButton.Background = new SolidColorBrush(Colors.Red);
    }

    private void ClientsButton_Click(object sender, RoutedEventArgs e)
    {
        homeBorder.Visibility = Visibility.Hidden;
        clientsBorder.Visibility = Visibility.Visible;
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        homeBorder.Visibility = Visibility.Visible;
        clientsBorder.Visibility = Visibility.Hidden;
    }

    private void addClientButton_Click(object sender, RoutedEventArgs e)
    {
        AddNewCustomer addNewCustomer = new AddNewCustomer();
        addNewCustomer.ShowDialog();
    }
}