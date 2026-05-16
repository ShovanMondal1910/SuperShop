using Microsoft.Data.SqlClient;
using SuperShop.Controller;

namespace SuperShop.IDGenarator
{
    public class CustomerIDGenarator
    {
        public static string GenerateCustomerID()
        {
            try
            {
                Random random = new Random();
                string generatedID;
                bool idExists = true;

                // Keep generating random IDs until we find one that doesn't exist
                while (idExists)
                {
                    // Generate random 4-digit number (1000-9999)
                    int randomNumber = random.Next(1000, 10000);
                    generatedID = "CUS-" + randomNumber.ToString();

                    // Check if this ID already exists in database
                    SqlConnection conn = DatabaseConnection.GetConnection();
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM CUSTOMER WHERE CUSTOMERID='" + generatedID + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (count == 0)
                    {
                        idExists = false;
                        return generatedID;
                    }
                }

                return "CUS-0000"; // Fallback (should never reach here)
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating Customer ID: " + ex.Message);
            }
        }
    }
}
