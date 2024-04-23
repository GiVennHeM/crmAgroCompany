using crmAgroCompany;
using MaterialDesignThemes.Wpf;
using System.Net.Http;
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
            productBorder.Visibility = Visibility.Hidden;
            dealBorder.Visibility = Visibility.Hidden;
            customersDataGrid.IsReadOnly = true;
        }
    private void customersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCustomer = customersDataGrid.SelectedItem as Customer;

        if (selectedCustomer != null)
        {
            addUserName.Text = selectedCustomer.Name;
            addUserCompany.Text = selectedCustomer.Company;
            AddUserNumberofphoneTextBox.Text = selectedCustomer.NumberOfPhone;
            addUserEmail.Text = selectedCustomer.Email;
            comboboxRegion.Text = selectedCustomer.Region;
        }
    }

    private async void LoadCustomers()
        {
            try
            {
                // Create HTTP client
                using (var client = new HttpClient())
                {
                    // Send GET request to API to fetch customers
                    var response = await client.GetAsync("http://localhost:5169/api/Customer");

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read response content as JSON string

                    // Deserialize JSON string to list of Customer objects
                    var json = await response.Content.ReadAsStringAsync();
                    var customers = JsonConvert.DeserializeObject<List<Customer>>(json);

                    
                    // Bind customers to the customersDataGrid
                    customersDataGrid.ItemsSource = customers;
                    var columndealsid = customersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "DealsId");
                    var columnlead = customersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Lead");
                    // Перевіряємо, чи стовбець знайдено
                    if (columndealsid != null && columnlead !=null)
                    {
                        // Приховуємо стовбець
                        columndealsid.Visibility = Visibility.Collapsed;
                        columnlead.Visibility = Visibility.Collapsed;
                    }
                }
                    else
                    {
                        // Display error message if request fails
                        MessageBox.Show("Failed to retrieve customer data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                MessageBox.Show("An error occurred while loading customers: " + ex.Message);
            }
        }
    private void CustomersDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        
    }

    private void ClientsButton_Click(object sender, RoutedEventArgs e)
    {
        homeBorder.Visibility = Visibility.Hidden;
        productBorder.Visibility = Visibility.Hidden;
        dealBorder.Visibility = Visibility.Hidden;
        clientsBorder.Visibility = Visibility.Visible;
        LoadCustomers();
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        homeBorder.Visibility = Visibility.Visible;
        clientsBorder.Visibility = Visibility.Hidden;
        productBorder.Visibility = Visibility.Hidden;
        dealBorder.Visibility = Visibility.Hidden;
    }

    private void addClientButton_Click(object sender, RoutedEventArgs e)
    {
        AddNewCustomer addNewCustomer = new AddNewCustomer();
        addNewCustomer.Visibility=Visibility.Visible;
    }

    private void DealButton_Click(object sender, RoutedEventArgs e)
    {
        homeBorder.Visibility = Visibility.Hidden;
        clientsBorder.Visibility = Visibility.Hidden;
        productBorder.Visibility = Visibility.Hidden;
        dealBorder.Visibility = Visibility.Visible;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ProductsButton_Click(object sender, RoutedEventArgs e)
    {
        homeBorder.Visibility = Visibility.Hidden;
        clientsBorder.Visibility = Visibility.Hidden;
        productBorder.Visibility = Visibility.Visible;
        dealBorder.Visibility = Visibility.Hidden;
    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var selectedCustomer = customersDataGrid.SelectedItem as Customer;
        try
        {
            var updatedCustomer = new
            {
                Name = addUserName.Text,
                Company = addUserCompany.Text,
                Region = comboboxRegion.Text,
                NumberOfPhone = AddUserNumberofphoneTextBox.Text,
                Email = addUserEmail.Text,
            };

            var json = JsonConvert.SerializeObject(updatedCustomer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                // Відправка PUT-запиту до API для оновлення клієнта
                var response = await client.PutAsync($"http://localhost:5169/api/Customer/{selectedCustomer.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Customer updated successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to update customer. Status code: " + response.StatusCode);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }

}