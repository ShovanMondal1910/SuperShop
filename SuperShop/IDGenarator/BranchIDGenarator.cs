using Microsoft.Data.SqlClient;
using SuperShop.Controller;

namespace SuperShop.IDGenarator
{
    public class BranchIDGenarator
    {
        public static string GenerateBranchID()
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
                    generatedID = "B-" + randomNumber.ToString();

                    // Check if this ID already exists in database
                    SqlConnection conn = DatabaseConnection.GetConnection();
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM BRANCH WHERE BRANCHID='" + generatedID + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (count == 0)
                    {
                        idExists = false;
                        return generatedID;
                    }
                }

                return "B-0000"; // Fallback (should never reach here)
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating Branch ID: " + ex.Message);
            }
        }
    }
}
