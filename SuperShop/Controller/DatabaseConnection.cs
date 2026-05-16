using Microsoft.Data.SqlClient;

namespace SuperShop.Controller
{
    public class DatabaseConnection
    {
        private static readonly string ConnectionString = @"Data Source=DESKTOP-J01CCIJ\SQLEXPRESS;Initial Catalog=SuperShopDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
