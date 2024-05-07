namespace Client.View.Windows
{
    /// <summary>
    /// Interaction logic for AddNewDeal.xaml
    /// </summary>
    public partial class AddNewDeal : Window
    {
        public AddNewDeal()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic newDeal = new
                {
                    Name = addDealName.Text,
                    Status = "New",
                    Region = comboboxRegion.Text,
                    Cash = DealCash.Text,
                    DateRegion = DateTime.Now,
                    Lead = 0
                };
                var json = JsonConvert.SerializeObject(newDeal);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://localhost:5169/api/Customer", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Deal saved successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to save deal. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
