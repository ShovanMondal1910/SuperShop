using Microsoft.Data.SqlClient;
using SuperShop.Controller;

namespace SuperShop.IDGenarator
{
    public class InvoiceGenarator
    {
        public static string GenerateInvoiceNumber()
        {
            try
            {
                Random random = new Random();
                string generatedInvoiceNumber;
                bool invoiceExists = true;

                // Keep generating random invoice numbers until we find one that doesn't exist
                while (invoiceExists)
                {
                    // Generate random 14-digit number
                    long randomNumber = random.Next(10000000, 100000000);
                    long randomNumber2 = random.Next(10000000, 100000000);
                    generatedInvoiceNumber = randomNumber.ToString() + randomNumber2.ToString();

                    // Check if this invoice number already exists in database
                    SqlConnection conn = DatabaseConnection.GetConnection();
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM SALE WHERE SALEID='" + generatedInvoiceNumber + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();

                    if (count == 0)
                    {
                        invoiceExists = false;
                        return generatedInvoiceNumber;
                    }
                }

                return "00000000000000"; // Fallback (should never reach here)
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating Invoice Number: " + ex.Message);
            }
        }
    }
}
