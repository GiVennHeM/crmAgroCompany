
namespace Client.View.Widows
{
    public partial class AddNewCustomer : Window
    {

        public AddNewCustomer()
        {
            InitializeComponent();
        }
      //  private async void Button_Click(object sender, RoutedEventArgs e)
      //  {
      //      try
      //      {
      //          Customer newCustomer = new Customer
      //          {
      //              Name = "John Doe",
      //              Company = "ABC Company",
      //              Region = "Region1",
      //              NumberOfPhone = "1234567890",
      //              Email = "john.doe@example.com",
      //              DealsId = new List<Deal>() { new Deal() {
      //          Name = "MFME",
      //Status = "Stick",
      //Region = "Stick",
      //Cash = 8321830,
      //Lead = 0,
      //CostumerId = new List<Customer>(),
      //ProductsId = new List<Product>() { new Product { Name = "Stick", Type = "Stick", Price = 10, TotalAmount = 10, DealId = new List<Deal>()} } } } ,
      //                  Lead = 0
      //              };
               

      //          // Add the new customer to the deal and vice versa

      //          // Serialize customer object to JSON with settings to ignore circular references
      //          var jsonSettings = new JsonSerializerSettings
      //          {
      //              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      //          };
      //          // Serialize customer object to JSON
      //          var json = JsonConvert.SerializeObject(newCustomer, jsonSettings);

      //          // Create StringContent with JSON data
      //          var content = new StringContent(json, Encoding.UTF8, "application/json");

      //          // Send HTTP POST request to API endpoint
      //          using (var client = new HttpClient())
      //          {
      //              var response = await client.PostAsync("https://localhost:7280/api/Customer", content);

      //              if (response.IsSuccessStatusCode)
      //              {
      //                  MessageBox.Show("Customer created successfully!");
      //                  MainWindow mainWindow = new MainWindow();
      //                  mainWindow.Show();
      //              }
      //              else
      //              {
      //                  MessageBox.Show("Failed to create customer. Status code: " + response.StatusCode);
      //              }
      //          }

      //      }
      //      catch (Exception ex)
      //      {
      //          MessageBox.Show("An error occurred: " + ex.Message);
      //      }
      //  }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
