using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Product
{
    public partial class ProductManagerGUI : Form
    {
        public ProductManagerGUI()
        {
            InitializeComponent();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            Showbutton.Click += Showbutton_Click;
            SearchProductbutton.Click += SearchProductbutton_Click;
            Updatebutton.Click += Updatebutton_Click;
            Deletebutton.Click += Deletebutton_Click;
            Clearbutton.Click += Clearbutton_Click;
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Populate the detail fields from the selected row
                    ProductIDtextBox.Text = row.Cells["PRODUCTID"].Value?.ToString() ?? string.Empty;
                    WeighttextBox.Text = row.Cells["WEIGHT"].Value?.ToString() ?? string.Empty;
                    BuyingPricetextBox.Text = row.Cells["BUYINGPRICE"].Value?.ToString() ?? string.Empty;
                    SellingPricetextBox.Text = row.Cells["SELLINGPRICE"].Value?.ToString() ?? string.Empty;
                    DiscounttextBox.Text = row.Cells["DISCOUNT"].Value?.ToString() ?? string.Empty;
                    TaxtextBox.Text = row.Cells["TAX"].Value?.ToString() ?? string.Empty;
                    MRPtextBox.Text = row.Cells["MRP"].Value?.ToString() ?? string.Empty;
                    StockQuantitytextBox.Text = row.Cells["STOCKQUANTITY"].Value?.ToString() ?? string.Empty;
                    ReorderLeveltextBox.Text = row.Cells["REORDERLEVEL"].Value?.ToString() ?? string.Empty;
                    MinStockLeveltextBox.Text = row.Cells["MINSTOCKLEVEL"].Value?.ToString() ?? string.Empty;

                    // Handle IsExpirable checkbox
                    bool isExpirable = Convert.ToBoolean(row.Cells["ISEXPIRABLE"].Value ?? false);
                    IsExpirablecheckBox.Checked = isExpirable;

                    // Handle ExpiryDate
                    if (isExpirable && row.Cells["EXPIRYDATE"].Value != null && row.Cells["EXPIRYDATE"].Value != DBNull.Value)
                    {
                        ExpiryDatedatePicker.Value = Convert.ToDateTime(row.Cells["EXPIRYDATE"].Value);
                    }
                    else
                    {
                        ExpiryDatedatePicker.Value = DateTime.Now;
                    }

                    // Handle IsActive checkbox
                    bool isActive = Convert.ToBoolean(row.Cells["ISACTIVE"].Value ?? false);
                    IsActivecheckBox.Checked = isActive;

                    // Handle Returnable checkbox
                    bool returnable = Convert.ToBoolean(row.Cells["RETURNABLE"].Value ?? false);
                    ReturnablecheckBox.Checked = returnable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Showbutton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT PRODUCTID, PRODUCTNAME, BRAND, PRODUCTCATEGORY, BARCODE, SKU, UNIT, WEIGHT, SUPPLIERID, BUYINGPRICE, SELLINGPRICE, DISCOUNT, TAX, MRP, STOCKQUANTITY, REORDERLEVEL, MINSTOCKLEVEL, ISEXPIRABLE, EXPIRYDATE, ISACTIVE, RETURNABLE FROM [PRODUCT]";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchProductbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = SearchProducttextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Please enter Product ID, Product Name, or Barcode to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT PRODUCTID, PRODUCTNAME, BRAND, PRODUCTCATEGORY, BARCODE, SKU, UNIT, WEIGHT, SUPPLIERID, BUYINGPRICE, SELLINGPRICE, DISCOUNT, TAX, MRP, STOCKQUANTITY, REORDERLEVEL, MINSTOCKLEVEL, ISEXPIRABLE, EXPIRYDATE, ISACTIVE, RETURNABLE FROM [PRODUCT] WHERE PRODUCTID = '" + searchValue + "' OR PRODUCTNAME LIKE '%" + searchValue + "%' OR BARCODE = '" + searchValue + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No product found with the given search criteria.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = null;
                }
                else
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void Updatebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(ProductIDtextBox.Text))
                {
                    MessageBox.Show("Please select a product from the list to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (!ValidateUpdateFields())
                {
                    return;
                }

                // Confirm update
                DialogResult result = MessageBox.Show("Are you sure you want to update this product?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update database
                if (UpdateProductInDatabase())
                {
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshProductList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(ProductIDtextBox.Text))
                {
                    MessageBox.Show("Please select a product from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this product? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete from database
                if (DeleteProductFromDatabase())
                {
                    MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshProductList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdateProductInDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Update PRODUCT table
                string productQuery = "UPDATE [PRODUCT] SET " +
                    "WEIGHT = " + decimal.Parse(WeighttextBox.Text) + ", BUYINGPRICE = " + decimal.Parse(BuyingPricetextBox.Text) + ", " +
                    "SELLINGPRICE = " + decimal.Parse(SellingPricetextBox.Text) + ", DISCOUNT = " + decimal.Parse(DiscounttextBox.Text == "" ? "0" : DiscounttextBox.Text) + ", " +
                    "TAX = " + decimal.Parse(TaxtextBox.Text == "" ? "0" : TaxtextBox.Text) + ", MRP = " + decimal.Parse(MRPtextBox.Text) + ", " +
                    "STOCKQUANTITY = " + int.Parse(StockQuantitytextBox.Text) + ", REORDERLEVEL = " + int.Parse(ReorderLeveltextBox.Text) + ", " +
                    "MINSTOCKLEVEL = " + int.Parse(MinStockLeveltextBox.Text) + ", ISEXPIRABLE = " + (IsExpirablecheckBox.Checked ? 1 : 0) + ", " +
                    "EXPIRYDATE = " + (IsExpirablecheckBox.Checked ? "'" + ExpiryDatedatePicker.Value.ToString("yyyy-MM-dd") + "'" : "NULL") + ", " +
                    "ISACTIVE = " + (IsActivecheckBox.Checked ? 1 : 0) + ", RETURNABLE = " + (ReturnablecheckBox.Checked ? 1 : 0) + " " +
                    "WHERE PRODUCTID = '" + ProductIDtextBox.Text + "'";

                SqlCommand cmd = new SqlCommand(productQuery, conn);
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

        private bool DeleteProductFromDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Delete from PRODUCT table
                string productQuery = "DELETE FROM [PRODUCT] WHERE PRODUCTID = '" + ProductIDtextBox.Text + "'";

                SqlCommand cmd = new SqlCommand(productQuery, conn);
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

        private bool ValidateUpdateFields()
        {
            // Validate Weight
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

            // Validate Buying Price
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

            // Validate Selling Price
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

            // Validate MRP
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

            // Validate Stock Quantity
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

            // Validate Reorder Level
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

            // Validate Min Stock Level
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

        private void RefreshProductList()
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT PRODUCTID, PRODUCTNAME, BRAND, PRODUCTCATEGORY, BARCODE, SKU, UNIT, WEIGHT, SUPPLIERID, BUYINGPRICE, SELLINGPRICE, DISCOUNT, TAX, MRP, STOCKQUANTITY, REORDERLEVEL, MINSTOCKLEVEL, ISEXPIRABLE, EXPIRYDATE, ISACTIVE, RETURNABLE FROM [PRODUCT]";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAllFields()
        {
            ProductIDtextBox.Clear();
            WeighttextBox.Clear();
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
        }
    }
}
