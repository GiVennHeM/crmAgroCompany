using crmAgroCompany.Windows;

namespace crmAgroCompany
{
    public partial class AddNewCustomer : Window
    {
        private readonly RestClient _client;

        public AddNewCustomer()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                dynamic customer = new
                {
                    Name = addUserName.Text,
                    Company = addUserCompany.Text,
                    Region = comboboxRegion.Text,
                    NumberOfPhone = AAddUserNumberofphoneTextBox.Text,
                    Email = addUserEmail.Text,
                    
                    Lead = 0
                };

                // Serialize customer object to JSON
                var json = JsonConvert.SerializeObject(customer);

                // Create StringContent with JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send HTTP POST request to API endpoint
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://localhost:5169/api/Customer", content);

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
