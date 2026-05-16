using Microsoft.Data.SqlClient;
using SuperShop.Controller;

namespace SuperShop.IDGenarator
{
    public class SaleDetailIDGenarator
    {
        public static string GenerateSaleDetailID()
        {
            try
            {
                Random random = new Random();
                string generatedID;
                bool idExists = true;

                // Keep generating random IDs until we find one that doesn't exist
                while (idExists)
                {
                    // Generate random 6-digit number (100000-999999)
                    int randomNumber = random.Next(100000, 1000000);
                    generatedID = "SALEDET-" + randomNumber.ToString();

                    // Check if this ID already exists in database
                    SqlConnection conn = DatabaseConnection.GetConnection();
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM SALEDETAIL WHERE SALEDETAILID='" + generatedID + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (count == 0)
                    {
                        idExists = false;
                        return generatedID;
                    }
                }

                return "SALEDET-000000"; // Fallback (should never reach here)
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating Sale Detail ID: " + ex.Message);
            }
        }
    }
}
