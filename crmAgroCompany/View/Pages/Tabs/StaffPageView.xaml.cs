using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.View.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for StaffPageView.xaml
    /// </summary>
    public partial class StaffPageView : Page
    {
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public StaffPageView()
        {
            InitializeComponent();
            staffDataGrid.IsReadOnly = true;
            LoadStaff();
        }
        private async Task LoadStaff()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7280/api/Users");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var customers = JsonConvert.DeserializeObject<List<User>>(json);

                        staffDataGrid.ItemsSource = customers;

                        customers.Sort((x, y) => y.QuantityOfСreated.CompareTo(x.QuantityOfСreated));
                        var top10 = customers.Take(10);

                        // Load chart data
                        SeriesCollection = new SeriesCollection();
                        Labels = new string[10];
                        ChartValues<int> values = new ChartValues<int>();

                        int index = 0;
                        foreach (var user in top10)
                        {
                            Labels[index] = user.UserName;
                            values.Add(user.QuantityOfСreated);
                            index++;
                        }

                        SeriesCollection.Add(new ColumnSeries
                        {
                            Title = "Quantity of created contacts",
                            Values = values
                        });

                        SeriesCollection.Add(new ColumnSeries
                        {
                            Title = "Quantity of created contacts",
                            Values = values,
                            Fill = values.Count > 30 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22231E")) :
values.Count > 20 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E885B")) :
                                values.Count < 20 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8F71")) :
                                  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8F71"))
                        });


                        Formatter = value => value.ToString("N");

                        DataContext = this;
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve customer data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading customers: " + ex.Message);
            }

        }
        private void staffDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName != "UserName" &&
            e.PropertyName != "PhoneNumber" &&
            e.PropertyName != "Surname" &&
            e.PropertyName != "Role" &&
            e.PropertyName != "Email" &&
            e.PropertyName != "QuantityOfСreated" &&
            e.PropertyName != "UserId")
            {
                e.Cancel = true;
            }
            if (e.PropertyName == "UserId")
                e.Column.Header = "User Id";
            if (e.PropertyName == "UserName")
                e.Column.Header = "Name of employee";
            if (e.PropertyName == "PhoneNumber")
                e.Column.Header = "Phone number";
        }
    }
}
