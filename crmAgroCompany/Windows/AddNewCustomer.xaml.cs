using crmAgroCompany.Windows;

namespace crmAgroCompany
{
    public partial class AddNewCustomer : Window
    {

        public AddNewCustomer()
        {
            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer newCustomer = new Customer
                {
                    Name = "John Doe",
                    Company = "ABC Company",
                    Region = "Region1",
                    NumberOfPhone = "1234567890",
                    Email = "john.doe@example.com",
                    Lead = 0
                };

                // Create a new deal object
                Deal newDeal = new Deal
                {
                    Name = "Deal1",
                    Status = "New",
                    Region = "Region2",
                    Cash = 100.0,
                    Lead = 0
                };
                Product newProduct = new Product
                {
                    Name = "Stick",
                    Type = "Helped",
                    Price = 50.0,
                    TotalAmount = 50.0
                };

                // Add the new customer to the deal and vice versa
                newDeal.CostumerId = new List<Customer> { newCustomer };
                newCustomer.DealsId = new List<Deal> { newDeal };
                newDeal.ProductsId = new List<Product> { newProduct };

                // Serialize customer object to JSON with settings to ignore circular references
                var jsonSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                // Serialize customer object to JSON
                var json = JsonConvert.SerializeObject(newCustomer, jsonSettings);

                // Create StringContent with JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send HTTP POST request to API endpoint
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://localhost:7016/api/Customer", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Customer created successfully!");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create customer. Status code: " + response.StatusCode);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
