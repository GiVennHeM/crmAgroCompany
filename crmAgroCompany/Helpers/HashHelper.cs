using System.Security.Cryptography;

namespace Client.ViewModels
{
    internal class HashHelper
    {
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyHash(string input, string hashedPassword)
        {
            string inputHash = ComputeHash(input);

            return inputHash.Equals(hashedPassword);
        }
    }
}

