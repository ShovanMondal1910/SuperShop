using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SuperShop.Controller;
using SuperShop.Models;
using SuperShop.IDGenarator;

namespace SuperShop.View.POS
{
    public partial class POSforCashier : Form
    {
        private DataTable cartDataTable = new DataTable();
        private string? cashierId;

        public POSforCashier(string userId = "")
        {
            InitializeComponent();
            cashierId = userId;
            InitializeCartTable();
            LoadProductList();
            GenerateInvoiceNumber();
            ProductListdataGridView.CellDoubleClick += ProductListdataGridView_CellDoubleClick;
            DiscounttextBox.TextChanged += DiscounttextBox_TextChanged;
            TaxtextBox.TextChanged += TaxtextBox_TextChanged;
            PaidAmounttextBox.TextChanged += PaidAmounttextBox_TextChanged;
            SearchCustomerbutton.Click += SearchCustomerbutton_Click;
        }

        private void LoadProductList()
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = @"SELECT PRODUCTID, PRODUCTNAME, PRODUCTCATEGORY, BARCODE, DISCOUNT, TAX, MRP, UNIT, RETURNABLE 
                                FROM PRODUCT 
                                WHERE ISACTIVE = 1 
                                ORDER BY PRODUCTNAME";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                ProductListdataGridView.DataSource = dt;

                // Set column headers
                SetColumnHeader("PRODUCTID", "Product ID");
                SetColumnHeader("PRODUCTNAME", "Product Name");
                SetColumnHeader("PRODUCTCATEGORY", "Category");
                SetColumnHeader("BARCODE", "Barcode");
                SetColumnHeader("DISCOUNT", "Discount (%)");
                SetColumnHeader("TAX", "Tax (%)");
                SetColumnHeader("MRP", "MRP");
                SetColumnHeader("UNIT", "Unit");
                SetColumnHeader("RETURNABLE", "Returnable");

                // Auto-fit columns
                ProductListdataGridView.AutoResizeColumns();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateInvoiceNumber()
        {
            try
            {
                string invoiceNumber = InvoiceGenarator.GenerateInvoiceNumber();
                InvoiceNumbertextBox.Text = invoiceNumber;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating invoice number: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            try
            {
                var column = ProductListdataGridView.Columns[columnName];
                if (column != null)
                {
                    column.HeaderText = headerText;
                }
            }
            catch (ArgumentException)
            {
                // Column not found, continue
            }
        }

        private void InitializeCartTable()
        {
            cartDataTable.Columns.Add("PRODUCTID", typeof(string));
            cartDataTable.Columns.Add("PRODUCTNAME", typeof(string));
            cartDataTable.Columns.Add("BARCODE", typeof(string));
            cartDataTable.Columns.Add("QUANTITY", typeof(int));
            cartDataTable.Columns.Add("UNIT", typeof(string));
            cartDataTable.Columns.Add("UNITPRICE", typeof(decimal));
            cartDataTable.Columns.Add("DISCOUNT", typeof(decimal));
            cartDataTable.Columns.Add("TAX", typeof(decimal));
            cartDataTable.Columns.Add("TOTAL", typeof(decimal));

            CartProductListdataGridView.DataSource = cartDataTable;

            // Set column headers
            CartProductListdataGridView.Columns["PRODUCTID"].HeaderText = "Product ID";
            CartProductListdataGridView.Columns["PRODUCTNAME"].HeaderText = "Product Name";
            CartProductListdataGridView.Columns["BARCODE"].HeaderText = "Barcode";
            CartProductListdataGridView.Columns["QUANTITY"].HeaderText = "Quantity";
            CartProductListdataGridView.Columns["UNIT"].HeaderText = "Unit";
            CartProductListdataGridView.Columns["UNITPRICE"].HeaderText = "Unit Price";
            CartProductListdataGridView.Columns["DISCOUNT"].HeaderText = "Discount (%)";
            CartProductListdataGridView.Columns["TAX"].HeaderText = "Tax (%)";
            CartProductListdataGridView.Columns["TOTAL"].HeaderText = "Total";
        }

        private void ProductListdataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = ProductListdataGridView.Rows[e.RowIndex];

                string productId = row.Cells["PRODUCTID"].Value?.ToString() ?? "";
                string productName = row.Cells["PRODUCTNAME"].Value?.ToString() ?? "";
                string barcode = row.Cells["BARCODE"].Value?.ToString() ?? "";
                string unit = row.Cells["UNIT"].Value?.ToString() ?? "";
                decimal mrp = Convert.ToDecimal(row.Cells["MRP"].Value ?? 0);

                // Check if product already exists in cart
                DataRow[] existingRows = cartDataTable.Select($"PRODUCTID = '{productId}'");

                if (existingRows.Length > 0)
                {
                    // Increase quantity
                    existingRows[0]["QUANTITY"] = Convert.ToInt32(existingRows[0]["QUANTITY"]) + 1;
                    UpdateCartTotal(existingRows[0]);
                }
                else
                {
                    // Add new row to cart
                    DataRow newRow = cartDataTable.NewRow();
                    newRow["PRODUCTID"] = productId;
                    newRow["PRODUCTNAME"] = productName;
                    newRow["BARCODE"] = barcode;
                    newRow["QUANTITY"] = 1;
                    newRow["UNIT"] = unit;
                    newRow["UNITPRICE"] = mrp;
                    newRow["DISCOUNT"] = 0;  // Always start at 0
                    newRow["TAX"] = 0;       // Always start at 0
                    newRow["TOTAL"] = mrp;

                    cartDataTable.Rows.Add(newRow);
                }

                UpdateGrandTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product to cart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCartTotal(DataRow row)
        {
            decimal unitPrice = Convert.ToDecimal(row["UNITPRICE"]);
            int quantity = Convert.ToInt32(row["QUANTITY"]);

            decimal subtotal = unitPrice * quantity;
            row["TOTAL"] = subtotal;
        }

        private bool isUpdatingDiscount = false;
        private bool isUpdatingTax = false;

        private void UpdateGrandTotal()
        {
            decimal subtotalBeforeDiscount = 0;

            foreach (DataRow row in cartDataTable.Rows)
            {
                decimal unitPrice = Convert.ToDecimal(row["UNITPRICE"]);
                int quantity = Convert.ToInt32(row["QUANTITY"]);
                subtotalBeforeDiscount += unitPrice * quantity;
            }

            TotalAmounttextBox.Text = subtotalBeforeDiscount.ToString("F2");

            // Get discount and tax percentages from textboxes (only if they have values)
            decimal discountPercent = 0;
            decimal taxPercent = 0;

            // Parse discount percentage
            if (!string.IsNullOrEmpty(DiscounttextBox.Text) && decimal.TryParse(DiscounttextBox.Text, out decimal discountValue))
            {
                discountPercent = discountValue;
            }

            // Parse tax percentage
            if (!string.IsNullOrEmpty(TaxtextBox.Text) && decimal.TryParse(TaxtextBox.Text, out decimal taxValue))
            {
                taxPercent = taxValue;
            }

            // Calculate discount amount
            decimal discountAmount = (subtotalBeforeDiscount * discountPercent) / 100;

            // Calculate tax amount (tax is applied after discount)
            decimal taxAmount = ((subtotalBeforeDiscount - discountAmount) * taxPercent) / 100;

            // Calculate final grand total
            decimal finalGrandTotal = subtotalBeforeDiscount - discountAmount + taxAmount;

            GrandTotalAmountlabel.Text = finalGrandTotal.ToString("F2");
        }

        private void DiscounttextBox_TextChanged(object sender, EventArgs e)
        {
            if (isUpdatingDiscount)
                return;

            isUpdatingDiscount = true;

            try
            {
                // Only update if user entered a valid number
                if (!string.IsNullOrEmpty(DiscounttextBox.Text) && decimal.TryParse(DiscounttextBox.Text, out decimal value))
                {
                    // Keep only the percentage value entered by user
                    DiscounttextBox.Text = value.ToString("F2");
                    DiscounttextBox.SelectionStart = DiscounttextBox.Text.Length;
                }

                UpdateGrandTotal();
            }
            catch
            {
                // Ignore parsing errors
            }
            finally
            {
                isUpdatingDiscount = false;
            }
        }

        private void TaxtextBox_TextChanged(object sender, EventArgs e)
        {
            if (isUpdatingTax)
                return;

            isUpdatingTax = true;

            try
            {
                // Only update if user entered a valid number
                if (!string.IsNullOrEmpty(TaxtextBox.Text) && decimal.TryParse(TaxtextBox.Text, out decimal value))
                {
                    // Keep only the percentage value entered by user
                    TaxtextBox.Text = value.ToString("F2");
                    TaxtextBox.SelectionStart = TaxtextBox.Text.Length;
                }

                UpdateGrandTotal();
            }
            catch
            {
                // Ignore parsing errors
            }
            finally
            {
                isUpdatingTax = false;
            }
        }

        private void PaidAmounttextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the grand total amount
                if (!decimal.TryParse(GrandTotalAmountlabel.Text, out decimal grandTotal))
                {
                    ChangeAmountlabel.Text = "0.00";
                    return;
                }

                // Get the paid amount
                if (!string.IsNullOrEmpty(PaidAmounttextBox.Text) && decimal.TryParse(PaidAmounttextBox.Text, out decimal paidAmount))
                {
                    // Calculate change
                    decimal change = paidAmount - grandTotal;

                    // Display change (show 0.00 if negative, otherwise show the change)
                    if (change < 0)
                    {
                        ChangeAmountlabel.Text = "0.00";
                    }
                    else
                    {
                        ChangeAmountlabel.Text = change.ToString("F2");
                    }
                }
                else
                {
                    ChangeAmountlabel.Text = "0.00";
                }
            }
            catch
            {
                ChangeAmountlabel.Text = "0.00";
            }
        }

        private void ClearCartbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear all cart items
                cartDataTable.Rows.Clear();

                // Reset all calculation fields
                TotalAmounttextBox.Text = "0.00";
                DiscounttextBox.Text = "";
                TaxtextBox.Text = "";
                GrandTotalAmountlabel.Text = "0.00";
                PaidAmounttextBox.Text = "";
                ChangeAmountlabel.Text = "0.00";

                MessageBox.Show("Cart cleared successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing cart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchCustomerbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchInput = SearchCustomertextBox.Text.Trim();

                // Check if search input is empty
                if (string.IsNullOrEmpty(searchInput))
                {
                    MessageBox.Show("Please enter a Customer ID or Phone Number to search.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Query to search customer by ID or Phone Number
                SqlConnection conn = DatabaseConnection.GetConnection();
                conn.Open();

                string query = @"SELECT c.CustomerID, u.Name, c.CustomerType, u.Phone 
                               FROM CUSTOMER c
                               INNER JOIN [USER] u ON c.UserID = u.UserID
                               WHERE c.CustomerID = @SearchInput OR u.Phone = @SearchInput";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchInput", searchInput);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Customer found - populate the fields
                    CustomerIDtextBox.Text = reader["CustomerID"].ToString();
                    CustomerNametextBox.Text = reader["Name"].ToString();
                    CustomerTypetextBox.Text = reader["CustomerType"].ToString();
                    PhoneNumbertextBox.Text = reader["Phone"].ToString();

                    MessageBox.Show("Customer found successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Customer not found
                    MessageBox.Show("Customer not found. Please check the Customer ID or Phone Number.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear customer fields
                    CustomerIDtextBox.Text = "";
                    CustomerNametextBox.Text = "";
                    CustomerTypetextBox.Text = "";
                    PhoneNumbertextBox.Text = "";
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CompleateSalebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate cart is not empty
                if (cartDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Cart is empty. Please add products before completing the sale.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate grand total
                if (!decimal.TryParse(GrandTotalAmountlabel.Text, out decimal grandTotal) || grandTotal <= 0)
                {
                    MessageBox.Show("Invalid grand total. Please check the cart.", "Invalid Total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate paid amount
                if (!decimal.TryParse(PaidAmounttextBox.Text, out decimal paidAmount) || paidAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid paid amount.", "Invalid Paid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate paid amount is not less than grand total
                if (paidAmount < grandTotal)
                {
                    MessageBox.Show("Paid amount cannot be less than grand total.", "Insufficient Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get values
                string saleID = SaleIDGenarator.GenerateSaleID();
                string invoiceNumber = InvoiceNumbertextBox.Text;
                string customerID = CustomerIDtextBox.Text;
                decimal totalAmount = decimal.Parse(TotalAmounttextBox.Text);
                decimal discountPercent = string.IsNullOrEmpty(DiscounttextBox.Text) ? 0 : decimal.Parse(DiscounttextBox.Text);
                decimal discountAmount = (totalAmount * discountPercent) / 100;
                decimal taxPercent = string.IsNullOrEmpty(TaxtextBox.Text) ? 0 : decimal.Parse(TaxtextBox.Text);
                decimal taxAmount = ((totalAmount - discountAmount) * taxPercent) / 100;
                decimal changeAmount = paidAmount - grandTotal;
                string paymentMethod = PaymentMethodcomboBox.SelectedItem?.ToString() ?? "Cash";

                // Open database connection
                SqlConnection conn = DatabaseConnection.GetConnection();
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert into SALE table
                    string saleQuery = @"INSERT INTO [SALE] 
                        (SALEID, INVOICENUMBER, CUSTOMERID, CASHIERID, SALEDATE, TOTALAMOUNT, DISCOUNTPERCENTAGE, 
                         DISCOUNTAMOUNT, TAXPERCENTAGE, TAXAMOUNT, GRANDTOTAL, PAIDAMOUNT, CHANGEAMOUNT, PAYMENTMETHOD, SALESTATUS, CREATEDDATE)
                        VALUES 
                        (@SaleID, @InvoiceNumber, @CustomerID, @CashierID, @SaleDate, @TotalAmount, @DiscountPercent, 
                         @DiscountAmount, @TaxPercent, @TaxAmount, @GrandTotal, @PaidAmount, @ChangeAmount, @PaymentMethod, 'COMPLETED', GETDATE())";

                    SqlCommand saleCmd = new SqlCommand(saleQuery, conn, transaction);
                    saleCmd.Parameters.AddWithValue("@SaleID", saleID);
                    saleCmd.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                    saleCmd.Parameters.AddWithValue("@CustomerID", string.IsNullOrEmpty(customerID) ? DBNull.Value : (object)customerID);
                    saleCmd.Parameters.AddWithValue("@CashierID", string.IsNullOrEmpty(cashierId) ? "C-1001" : cashierId);
                    saleCmd.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                    saleCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    saleCmd.Parameters.AddWithValue("@DiscountPercent", discountPercent);
                    saleCmd.Parameters.AddWithValue("@DiscountAmount", discountAmount);
                    saleCmd.Parameters.AddWithValue("@TaxPercent", taxPercent);
                    saleCmd.Parameters.AddWithValue("@TaxAmount", taxAmount);
                    saleCmd.Parameters.AddWithValue("@GrandTotal", grandTotal);
                    saleCmd.Parameters.AddWithValue("@PaidAmount", paidAmount);
                    saleCmd.Parameters.AddWithValue("@ChangeAmount", changeAmount);
                    saleCmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                    saleCmd.ExecuteNonQuery();

                    // Insert into SALEITEM table for each cart item
                    foreach (DataRow row in cartDataTable.Rows)
                    {
                        string saleItemID = SaleItemIDGenarator.GenerateSaleItemID();
                        string productID = row["PRODUCTID"].ToString();
                        string barcode = row["BARCODE"].ToString();
                        int quantity = Convert.ToInt32(row["QUANTITY"]);
                        string unit = row["UNIT"].ToString();
                        decimal unitPrice = Convert.ToDecimal(row["UNITPRICE"]);
                        decimal itemDiscountPercent = Convert.ToDecimal(row["DISCOUNT"]);
                        decimal itemDiscountAmount = (unitPrice * quantity * itemDiscountPercent) / 100;
                        decimal itemTaxPercent = Convert.ToDecimal(row["TAX"]);
                        decimal itemTaxAmount = ((unitPrice * quantity - itemDiscountAmount) * itemTaxPercent) / 100;
                        decimal lineTotal = Convert.ToDecimal(row["TOTAL"]);

                        string saleItemQuery = @"INSERT INTO [SALEITEM] 
                            (SALEITEMID, SALEID, PRODUCTID, BARCODE, QUANTITY, UNIT, UNITPRICE, DISCOUNTPERCENTAGE, 
                             DISCOUNTAMOUNT, TAXPERCENTAGE, TAXAMOUNT, LINETOTAL, CREATEDDATE)
                            VALUES 
                            (@SaleItemID, @SaleID, @ProductID, @Barcode, @Quantity, @Unit, @UnitPrice, @DiscountPercent, 
                             @DiscountAmount, @TaxPercent, @TaxAmount, @LineTotal, GETDATE())";

                        SqlCommand saleItemCmd = new SqlCommand(saleItemQuery, conn, transaction);
                        saleItemCmd.Parameters.AddWithValue("@SaleItemID", saleItemID);
                        saleItemCmd.Parameters.AddWithValue("@SaleID", saleID);
                        saleItemCmd.Parameters.AddWithValue("@ProductID", productID);
                        saleItemCmd.Parameters.AddWithValue("@Barcode", barcode);
                        saleItemCmd.Parameters.AddWithValue("@Quantity", quantity);
                        saleItemCmd.Parameters.AddWithValue("@Unit", unit);
                        saleItemCmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                        saleItemCmd.Parameters.AddWithValue("@DiscountPercent", itemDiscountPercent);
                        saleItemCmd.Parameters.AddWithValue("@DiscountAmount", itemDiscountAmount);
                        saleItemCmd.Parameters.AddWithValue("@TaxPercent", itemTaxPercent);
                        saleItemCmd.Parameters.AddWithValue("@TaxAmount", itemTaxAmount);
                        saleItemCmd.Parameters.AddWithValue("@LineTotal", lineTotal);

                        saleItemCmd.ExecuteNonQuery();
                    }

                    // Update customer loyalty points and total spent if customer is selected
                    if (!string.IsNullOrEmpty(customerID))
                    {
                        // Calculate loyalty points: 1 point for every 100 taka
                        decimal loyaltyPointsEarned = grandTotal / 100;

                        string updateCustomerQuery = @"UPDATE CUSTOMER 
                            SET TotalPurchase = TotalPurchase + @GrandTotal,
                                LoyaltyPoints = LoyaltyPoints + @LoyaltyPoints
                            WHERE CustomerID = @CustomerID";

                        SqlCommand updateCustomerCmd = new SqlCommand(updateCustomerQuery, conn, transaction);
                        updateCustomerCmd.Parameters.AddWithValue("@GrandTotal", grandTotal);
                        updateCustomerCmd.Parameters.AddWithValue("@LoyaltyPoints", loyaltyPointsEarned);
                        updateCustomerCmd.Parameters.AddWithValue("@CustomerID", customerID);

                        updateCustomerCmd.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();

                    // Show success message
                    MessageBox.Show($"Sale completed successfully!\n\nSale ID: {saleID}\nInvoice: {invoiceNumber}\nGrand Total: {grandTotal:F2}\nChange: {changeAmount:F2}", 
                        "Sale Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear cart and reset form
                    cartDataTable.Rows.Clear();
                    TotalAmounttextBox.Text = "0.00";
                    DiscounttextBox.Text = "";
                    TaxtextBox.Text = "";
                    GrandTotalAmountlabel.Text = "0.00";
                    PaidAmounttextBox.Text = "";
                    ChangeAmountlabel.Text = "0.00";
                    CustomerIDtextBox.Text = "";
                    CustomerNametextBox.Text = "";
                    CustomerTypetextBox.Text = "";
                    PhoneNumbertextBox.Text = "";
                    SearchCustomertextBox.Text = "";
                    GenerateInvoiceNumber();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error completing sale: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
