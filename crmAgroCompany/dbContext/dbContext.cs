using Microsoft.Data.SqlClient;
using System.Windows;

namespace crmAgroCompany
{
    class dbContext
    {
        private string connectionString = @"Data Source=DESKTOP-4UM6HKE;Initial Catalog=NEW_AgroproCRM;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public bool Login(string username, string password)
        {
            bool isAuthenticated = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Password FROM LoggerUser WHERE Login = @Login";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", username);

                        string hashedPassword = (string)command.ExecuteScalar();

                        if (hashedPassword != null && HashHelper.VerifyHash(password, hashedPassword))
                        {
                            isAuthenticated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show($"Error executing login request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isAuthenticated;
        }


        public bool SignUp(string name, string surname, string numberofphone, int position, string login, string password, string profileImageSource)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO LoggerUser (Username, Password, Login, PhoneNumber, ProfilePicture, Surname) " +
                                   "VALUES (@Username, @Password, @Login, @PhoneNumber, @ProfilePicture, @Surname)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", name);
                        command.Parameters.AddWithValue("@Surname", surname);
                        command.Parameters.AddWithValue("@PhoneNumber", numberofphone);
                        command.Parameters.AddWithValue("@Position", position);
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@ProfilePicture", profileImageSource);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing sign up request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
