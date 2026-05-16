using System;
using Microsoft.Data.SqlClient;
using SuperShop.Controller;

namespace SuperShop.IDGenarator
{
    public class ProductIDGenarator
    {
        public static string GenerateProductID()
        {
            try
            {
                // Generate Product ID: P-XXXXXXXXX
                // Format: P-{9 digits}
                Random random = new Random();
                string randomDigits = random.Next(100000000, 1000000000).ToString();
                
                return "P-" + randomDigits;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating Product ID: " + ex.Message);
            }
        }

        public static string GenerateBarcode()
        {
            try
            {
                // Generate 13-digit barcode (EAN-13 format)
                // Format: XXXXXXXXXXX + checksum digit
                Random random = new Random();
                string barcode = "";
                
                // Generate first 12 digits
                for (int i = 0; i < 12; i++)
                {
                    barcode += random.Next(0, 10).ToString();
                }
                
                // Calculate checksum digit (EAN-13 algorithm)
                int sum = 0;
                for (int i = 0; i < 12; i++)
                {
                    int digit = int.Parse(barcode[i].ToString());
                    if (i % 2 == 0)
                    {
                        sum += digit;
                    }
                    else
                    {
                        sum += digit * 3;
                    }
                }
                
                int checksum = (10 - (sum % 10)) % 10;
                barcode += checksum.ToString();
                
                return barcode;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating Barcode: " + ex.Message);
            }
        }

        public static string GenerateSKU()
        {
            try
            {
                // Generate SKU: SKU-XXXXXXXXX
                // Format: SKU-{9 digits}
                Random random = new Random();
                string randomDigits = random.Next(100000000, 1000000000).ToString();
                
                return "SKU-" + randomDigits;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating SKU: " + ex.Message);
            }
        }
    }
}
