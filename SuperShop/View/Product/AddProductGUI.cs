using SuperShop.Models;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace SuperShop.View.Product
{
    public partial class AddProductGUI : Form
    {
        public AddProductGUI()
        {
            InitializeComponent();
            this.Load += AddProductGUI_Load;
        }

        private void AddProductGUI_Load(object sender, EventArgs e)
        {
            // Generate and display ProductID, Barcode, and SKU
            GenerateIDs();
        }

        private void GenerateIDs()
        {
            try
            {
                ProductIDtextBox.Text = ProductIDGenarator.GenerateProductID();
                ProductIDtextBox.ReadOnly = true;
                
                BarcodetextBox.Text = ProductIDGenarator.GenerateBarcode();
                BarcodetextBox.ReadOnly = true;
                
                SKUtextBox.Text = ProductIDGenarator.GenerateSKU();
                SKUtextBox.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateMRP()
        {
            try
            {
                // Get Selling Price
                if (!decimal.TryParse(SellingPricetextBox.Text, out decimal sellingPrice) || sellingPrice <= 0)
                {
                    MRPtextBox.Clear();
                    return;
                }

                // Get Discount % (default 0 if empty)
                decimal discountPercent = 0;
                if (!string.IsNullOrWhiteSpace(DiscounttextBox.Text))
                {
                    decimal.TryParse(DiscounttextBox.Text, out discountPercent);
                }

                // Get Tax % (default 0 if empty)
                decimal taxPercent = 0;
                if (!string.IsNullOrWhiteSpace(TaxtextBox.Text))
                {
                    decimal.TryParse(TaxtextBox.Text, out taxPercent);
                }

                // Calculate discount amount
                decimal discountAmount = (sellingPrice * discountPercent) / 100;

                // Calculate tax amount
                decimal taxAmount = (sellingPrice * taxPercent) / 100;

                // Formula: MRP = Selling Price - Discount + Tax
                decimal mrp = sellingPrice - discountAmount + taxAmount;

                // Display MRP with 2 decimal places
                MRPtextBox.Text = mrp.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating MRP: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            // Product Name validation
            if (string.IsNullOrWhiteSpace(ProductNametextBox.Text))
            {
                MessageBox.Show("Product Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ProductNametextBox.Focus();
                return false;
            }

            if (ProductNametextBox.Text.Length < 3)
            {
                MessageBox.Show("Product Name must be at least 3 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ProductNametextBox.Focus();
                return false;
            }

            // Brand validation
            if (string.IsNullOrWhiteSpace(BrandtextBox.Text))
            {
                MessageBox.Show("Brand is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BrandtextBox.Focus();
                return false;
            }

            // Category validation
            if (CategorycomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CategorycomboBox.Focus();
                return false;
            }

            // Unit validation
            if (UnitcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Unit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UnitcomboBox.Focus();
                return false;
            }

            // Weight validation
            if (string.IsNullOrWhiteSpace(WeighttextBox.Text))
            {
                MessageBox.Show("Weight is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                WeighttextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(WeighttextBox.Text, out decimal weight) || weight <= 0)
            {
                MessageBox.Show("Weight must be a valid positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                WeighttextBox.Focus();
                return false;
            }

            // Supplier ID validation
            if (string.IsNullOrWhiteSpace(SupplierIDtextBox.Text))
            {
                MessageBox.Show("Please select a Supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SupplierIDtextBox.Focus();
                return false;
            }

            // Buying Price validation
            if (string.IsNullOrWhiteSpace(BuyingPricetextBox.Text))
            {
                MessageBox.Show("Buying Price is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BuyingPricetextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(BuyingPricetextBox.Text, out decimal buyingPrice) || buyingPrice <= 0)
            {
                MessageBox.Show("Buying Price must be a valid positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BuyingPricetextBox.Focus();
                return false;
            }

            // Selling Price validation
            if (string.IsNullOrWhiteSpace(SellingPricetextBox.Text))
            {
                MessageBox.Show("Selling Price is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SellingPricetextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(SellingPricetextBox.Text, out decimal sellingPrice) || sellingPrice <= 0)
            {
                MessageBox.Show("Selling Price must be a valid positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SellingPricetextBox.Focus();
                return false;
            }

            // Selling Price should be greater than Buying Price
            if (sellingPrice <= buyingPrice)
            {
                MessageBox.Show("Selling Price must be greater than Buying Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SellingPricetextBox.Focus();
                return false;
            }

            // Discount validation
            if (!string.IsNullOrWhiteSpace(DiscounttextBox.Text))
            {
                if (!decimal.TryParse(DiscounttextBox.Text, out decimal discount) || discount < 0)
                {
                    MessageBox.Show("Discount must be a valid non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DiscounttextBox.Focus();
                    return false;
                }

                if (discount >= sellingPrice)
                {
                    MessageBox.Show("Discount cannot be equal to or greater than Selling Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DiscounttextBox.Focus();
                    return false;
                }
            }

            // Tax validation
            if (!string.IsNullOrWhiteSpace(TaxtextBox.Text))
            {
                if (!decimal.TryParse(TaxtextBox.Text, out decimal tax) || tax < 0)
                {
                    MessageBox.Show("Tax must be a valid non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TaxtextBox.Focus();
                    return false;
                }
            }

            // MRP validation
            if (string.IsNullOrWhiteSpace(MRPtextBox.Text))
            {
                MessageBox.Show("MRP is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MRPtextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(MRPtextBox.Text, out decimal mrp) || mrp <= 0)
            {
                MessageBox.Show("MRP must be a valid positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MRPtextBox.Focus();
                return false;
            }

            // Stock Quantity validation
            if (string.IsNullOrWhiteSpace(StockQuantitytextBox.Text))
            {
                MessageBox.Show("Stock Quantity is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                StockQuantitytextBox.Focus();
                return false;
            }

            if (!int.TryParse(StockQuantitytextBox.Text, out int stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Stock Quantity must be a valid non-negative integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                StockQuantitytextBox.Focus();
                return false;
            }

            // Reorder Level validation
            if (string.IsNullOrWhiteSpace(ReorderLeveltextBox.Text))
            {
                MessageBox.Show("Reorder Level is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReorderLeveltextBox.Focus();
                return false;
            }

            if (!int.TryParse(ReorderLeveltextBox.Text, out int reorderLevel) || reorderLevel < 0)
            {
                MessageBox.Show("Reorder Level must be a valid non-negative integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReorderLeveltextBox.Focus();
                return false;
            }

            // Min Stock Level validation
            if (string.IsNullOrWhiteSpace(MinStockLeveltextBox.Text))
            {
                MessageBox.Show("Min Stock Level is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MinStockLeveltextBox.Focus();
                return false;
            }

            if (!int.TryParse(MinStockLeveltextBox.Text, out int minStockLevel) || minStockLevel < 0)
            {
                MessageBox.Show("Min Stock Level must be a valid non-negative integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MinStockLeveltextBox.Focus();
                return false;
            }

            // Stock level logic validation
            if (minStockLevel > reorderLevel)
            {
                MessageBox.Show("Min Stock Level cannot be greater than Reorder Level.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MinStockLeveltextBox.Focus();
                return false;
            }

            // Expiry Date validation
            if (IsExpirablecheckBox.Checked)
            {
                if (ExpiryDatedatePicker.Value <= DateTime.Now)
                {
                    MessageBox.Show("Expiry Date must be in the future.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ExpiryDatedatePicker.Focus();
                    return false;
                }
            }

            return true;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            // Convert image to byte array
            byte[]? photoData = null;
            if (ProductPhotopictureBox.Image != null)
            {
                photoData = ImageToByteArray(ProductPhotopictureBox.Image);
            }

            // Create Product object from form data
            SuperShop.Models.Product product = new SuperShop.Models.Product(
                ProductIDtextBox.Text,
                ProductNametextBox.Text,
                BrandtextBox.Text,
                CategorycomboBox.SelectedItem?.ToString() ?? "",
                BarcodetextBox.Text,
                SKUtextBox.Text,
                UnitcomboBox.SelectedItem?.ToString() ?? "",
                decimal.Parse(WeighttextBox.Text),
                photoData,
                SupplierIDtextBox.Text,
                decimal.Parse(BuyingPricetextBox.Text),
                decimal.Parse(SellingPricetextBox.Text),
                decimal.Parse(DiscounttextBox.Text == "" ? "0" : DiscounttextBox.Text),
                decimal.Parse(TaxtextBox.Text == "" ? "0" : TaxtextBox.Text),
                decimal.Parse(MRPtextBox.Text),
                int.Parse(StockQuantitytextBox.Text),
                int.Parse(ReorderLeveltextBox.Text),
                int.Parse(MinStockLeveltextBox.Text),
                IsExpirablecheckBox.Checked,
                IsExpirablecheckBox.Checked ? ExpiryDatedatePicker.Value : null,
                IsActivecheckBox.Checked,
                ReturnablecheckBox.Checked
            );

            // Save to database
            if (AddProductToDatabase(product))
            {
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clearbutton_Click(null, null);
                GenerateIDs();
            }
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private bool AddProductToDatabase(SuperShop.Models.Product product)
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Insert into PRODUCT table with correct column names
                string productQuery = "INSERT INTO [PRODUCT] (PRODUCTID, PRODUCTNAME, BRAND, PRODUCTCATEGORY, BARCODE, SKU, UNIT, WEIGHT, PHOTO, SUPPLIERID, BUYINGPRICE, SELLINGPRICE, DISCOUNT, TAX, MRP, STOCKQUANTITY, REORDERLEVEL, MINSTOCKLEVEL, ISEXPIRABLE, EXPIRYDATE, ISACTIVE, RETURNABLE) " +
                    "VALUES ('" + product.ProductID + "', '" + product.ProductName.Replace("'", "''") + "', '" + product.Brand.Replace("'", "''") + "', '" + product.ProductCategory.Replace("'", "''") + "', '" + product.Barcode + "', '" + product.SKU + "', '" + product.Unit.Replace("'", "''") + "', " + product.Weight + ", @Photo, '" + product.SupplierID + "', " + product.BuyingPrice + ", " + product.SellingPrice + ", " + product.Discount + ", " + product.Tax + ", " + product.MRP + ", " + product.StockQuantity + ", " + product.ReorderLevel + ", " + product.MinStockLevel + ", " + (product.IsExpirable ? 1 : 0) + ", " + (product.ExpiryDate.HasValue ? "'" + product.ExpiryDate.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", " + (product.IsActive ? 1 : 0) + ", " + (product.Returnable ? 1 : 0) + ")";

                SqlCommand cmd = new SqlCommand(productQuery, conn);

                // Add photo parameter
                if (product.Photo != null)
                {
                    cmd.Parameters.AddWithValue("@Photo", product.Photo);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Photo", DBNull.Value);
                }

                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            // Clear all fields
            ProductNametextBox.Clear();
            BrandtextBox.Clear();
            CategorycomboBox.SelectedIndex = -1;
            BarcodetextBox.Clear();
            SKUtextBox.Clear();
            UnitcomboBox.SelectedIndex = -1;
            WeighttextBox.Clear();
            SupplierIDtextBox.Clear();
            SupplierNametextBox.Clear();
            BuyingPricetextBox.Clear();
            SellingPricetextBox.Clear();
            DiscounttextBox.Clear();
            TaxtextBox.Clear();
            MRPtextBox.Clear();
            StockQuantitytextBox.Clear();
            ReorderLeveltextBox.Clear();
            MinStockLeveltextBox.Clear();
            IsExpirablecheckBox.Checked = false;
            ExpiryDatedatePicker.Value = DateTime.Now;
            IsActivecheckBox.Checked = false;
            ReturnablecheckBox.Checked = false;
            ProductPhotopictureBox.Image = null;
        }

        private void IsExpirablecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/disable expiry date picker and label based on checkbox
            if (IsExpirablecheckBox.Checked)
            {
                ExpiryDatedatePicker.Enabled = true;
                ExpiryDatelabel.Enabled = true;
                ExpiryDatedatePicker.Focus();
            }
            else
            {
                ExpiryDatedatePicker.Enabled = false;
                ExpiryDatelabel.Enabled = false;
                ExpiryDatedatePicker.Value = DateTime.Now;
            }
        }

        private void SellingPricetextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMRP();
        }

        private void DiscounttextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMRP();
        }

        private void TaxtextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMRP();
        }

        private void BrosePhotobutton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Product Photo";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ProductPhotopictureBox.Image = new Bitmap(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SupplierIDsearchbutton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SupplierIDtextBox.Text))
            {
                MessageBox.Show("Please enter a Supplier ID to search.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SupplierIDtextBox.Focus();
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT SupplierName FROM Supplier WHERE SupplierID = @SupplierID AND IsActive = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SupplierID", SupplierIDtextBox.Text.Trim());

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string supplierName = reader["SupplierName"].ToString();
                                SupplierNametextBox.Text = supplierName;
                                MessageBox.Show("Supplier found successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                SupplierNametextBox.Clear();
                                MessageBox.Show("Supplier not found. Please check the Supplier ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching for supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
