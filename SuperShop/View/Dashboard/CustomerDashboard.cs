using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using SuperShop.View.Auth;

namespace SuperShop.View.Dashboard
{
    public partial class CustomerDashboard : Form
    {
        // ─── Cart state ───────────────────────────────────────────────────────────
        private DataTable cartTable = null!;
        private DataGridView cartGrid = null!;
        private string _userId = string.Empty;

        public CustomerDashboard(string userId = "")
        {
            InitializeComponent();
            _userId = userId;
            SetupCartPanel();
            SetupProductListPanel();
            LoadActiveProducts();
            WireManagingPanelButtons();
            GenerateInvoiceNumber();

            if (!string.IsNullOrWhiteSpace(userId))
                LoadCustomerData(userId);
        }

        // ─── Invoice ──────────────────────────────────────────────────────────────

        private void GenerateInvoiceNumber()
        {
            try
            {
                InvoiceNumbertextBox.Text = InvoiceGenarator.GenerateInvoiceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating invoice number: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─── Customer Data ────────────────────────────────────────────────────────

        private void LoadCustomerData(string userId)
        {
            try
            {
                using SqlConnection conn = DatabaseConnection.GetConnection();
                conn.Open();

                // Join User + Customer tables to get all fields in one query
                string query = @"SELECT u.UserID, u.Name, u.Phone,
                                        c.CustomerID, c.CustomerType, c.TotalPurchase,
                                        c.IsVIP, c.LoyaltyPoints
                                 FROM   [User] u
                                 INNER JOIN [CUSTOMER] c ON c.UserID = u.UserID
                                 WHERE  u.UserID = @UserId";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using SqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    string customerId   = r["CustomerID"]?.ToString()   ?? "";
                    string name         = r["Name"]?.ToString()          ?? "";
                    string phone        = r["Phone"]?.ToString()         ?? "";
                    string customerType = r["CustomerType"]?.ToString()  ?? "";
                    bool   isVip        = r["IsVIP"] != DBNull.Value && Convert.ToBoolean(r["IsVIP"]);
                    decimal loyalty     = r["LoyaltyPoints"] == DBNull.Value ? 0 : Convert.ToDecimal(r["LoyaltyPoints"]);
                    decimal totalPurch  = r["TotalPurchase"]  == DBNull.Value ? 0 : Convert.ToDecimal(r["TotalPurchase"]);

                    // Populate the existing Managingpanel fields
                    CustomerIDtextBox.Text   = customerId;
                    CustomerNametextBox.Text = name;
                    CustomerTypetextBox.Text = isVip ? $"{customerType}  ⭐ VIP" : customerType;
                    PhoneNumbertextBox.Text  = phone;

                    // Update the form title to greet the customer
                    Text = $"SuperShop  —  Welcome, {name}  |  Loyalty Points: {loyalty:N0}  |  Total Purchases: ৳{totalPurch:N2}";
                }
                else
                {
                    // Fallback: user record exists but no matching CUSTOMER row
                    // Try loading just from User table
                    r.Close();
                    LoadCustomerFromUserTable(conn, userId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerFromUserTable(SqlConnection conn, string userId)
        {
            string query = @"SELECT UserID, Name, Phone FROM [User] WHERE UserID = @UserId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            using SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                CustomerIDtextBox.Text   = r["UserID"]?.ToString() ?? "";
                CustomerNametextBox.Text = r["Name"]?.ToString()   ?? "";
                PhoneNumbertextBox.Text  = r["Phone"]?.ToString()  ?? "";
                Text = $"SuperShop  —  Welcome, {CustomerNametextBox.Text}";
            }
        }

        // ─── Setup ────────────────────────────────────────────────────────────────

        private FlowLayoutPanel productFlowPanel = null!;

        private void SetupProductListPanel()
        {
            // Add a title label at the top of ProductListpanel
            Label titleLabel = new Label
            {
                Text = "🛒  Available Products",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 60),
                Dock = DockStyle.Top,
                Height = 44,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                BackColor = Color.FromArgb(240, 244, 255)
            };

            // Scrollable flow panel that holds the product cards
            productFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(245, 247, 252)
            };

            ProductListpanel.Controls.Add(productFlowPanel);
            ProductListpanel.Controls.Add(titleLabel);   // added last → rendered on top (Dock.Top)
            ProductListpanel.BackColor = Color.FromArgb(240, 244, 255);
        }

        // ─── Cart Panel Setup ─────────────────────────────────────────────────────

        private void SetupCartPanel()
        {
            Cartpanel.BackColor = Color.FromArgb(240, 244, 255);

            // Title bar
            Label cartTitle = new Label
            {
                Text = "🧺  Cart",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 60),
                Dock = DockStyle.Top,
                Height = 34,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0),
                BackColor = Color.FromArgb(220, 228, 255)
            };

            // DataTable backing the cart
            cartTable = new DataTable();
            cartTable.Columns.Add("ProductID",   typeof(string));
            cartTable.Columns.Add("ProductName", typeof(string));
            cartTable.Columns.Add("Unit",        typeof(string));
            cartTable.Columns.Add("MRP",         typeof(decimal));
            cartTable.Columns.Add("Discount",    typeof(decimal));
            cartTable.Columns.Add("Tax",         typeof(decimal));
            cartTable.Columns.Add("Qty",         typeof(int));
            cartTable.Columns.Add("Total",       typeof(decimal));

            // DataGridView
            cartGrid = new DataGridView
            {
                Dock            = DockStyle.Fill,
                DataSource      = cartTable,
                AllowUserToAddRows    = false,
                AllowUserToDeleteRows = false,
                ReadOnly        = false,
                SelectionMode   = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible   = false,
                BackgroundColor     = Color.White,
                BorderStyle         = BorderStyle.None,
                Font                = new Font("Segoe UI", 8.5F),
                ColumnHeadersHeight = 26,
                RowTemplate         = { Height = 24 }
            };

            cartGrid.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            cartGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(200, 210, 240);
            cartGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 60);
            cartGrid.EnableHeadersVisualStyles = false;
            cartGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);

            cartGrid.DataBindingComplete += (s, e) => StyleCartColumns();
            cartGrid.CellEndEdit        += CartGrid_CellEndEdit;

            Cartpanel.Controls.Add(cartGrid);
            Cartpanel.Controls.Add(cartTitle);   // Dock.Top renders on top
        }

        private void StyleCartColumns()
        {
            if (cartGrid.Columns.Count == 0) return;

            // Hide ProductID
            if (cartGrid.Columns.Contains("ProductID"))
            {
                var pidCol = cartGrid.Columns["ProductID"];
                if (pidCol != null) pidCol.Visible = false;
            }

            SetColHeader("ProductName", "Product",   120, false);
            SetColHeader("Unit",        "Unit",       40,  true);
            SetColHeader("MRP",         "Price",      55,  true);
            SetColHeader("Discount",    "Disc%",      45,  true);
            SetColHeader("Tax",         "Tax%",       40,  true);
            SetColHeader("Qty",         "Qty",        38,  false);   // editable
            SetColHeader("Total",       "Total",      60,  true);

            // Right-align numeric columns
            foreach (string col in new[] { "MRP", "Discount", "Tax", "Total" })
                if (cartGrid.Columns.Contains(col))
                {
                    var c = cartGrid.Columns[col];
                    if (c != null)
                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

            // Add a Remove button column (only once)
            if (!cartGrid.Columns.Contains("Remove"))
            {
                var removeCol = new DataGridViewButtonColumn
                {
                    Name       = "Remove",
                    HeaderText = "",
                    Text       = "✕",
                    UseColumnTextForButtonValue = true,
                    Width      = 28,
                    FlatStyle  = FlatStyle.Flat
                };
                cartGrid.Columns.Add(removeCol);
                cartGrid.CellClick += CartGrid_RemoveClick;
            }
        }

        private void SetColHeader(string col, string header, int width, bool readOnly)
        {
            if (!cartGrid.Columns.Contains(col)) return;
            var c = cartGrid.Columns[col];
            if (c == null) return;
            c.HeaderText = header;
            c.Width      = width;
            c.ReadOnly   = readOnly;
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        // ─── Wire existing Managingpanel buttons ──────────────────────────────────

        private void WireManagingPanelButtons()
        {
            // Clear Cart
            ClearCartbutton.Click += (s, e) =>
            {
                cartTable.Rows.Clear();
                RefreshCartTotals();
            };

            // Live change calculation as paid amount is typed
            PaidAmounttextBox.TextChanged += (s, e) =>
            {
                if (decimal.TryParse(PaidAmounttextBox.Text, out decimal paid) &&
                    decimal.TryParse(TotalAmounttextBox.Text, out decimal total) && total > 0)
                {
                    decimal change = paid - total;
                    ChangeAmountlabel.Text = change >= 0 ? $"৳ {change:N2}" : "Insufficient";
                    ChangeAmountlabel.ForeColor = change >= 0 ? Color.SeaGreen : Color.Crimson;
                }
                else
                {
                    ChangeAmountlabel.Text = "";
                }
            };
        }

        // ─── Complete Sale ────────────────────────────────────────────────────────

        private void CompleateSalebutton_Click(object sender, EventArgs e)
        {
            // ── Validations ────────────────────────────────────────────────────
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty. Please add products before completing the sale.",
                                "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(TotalAmounttextBox.Text, out decimal totalAmount) || totalAmount <= 0)
            {
                MessageBox.Show("Invalid total amount. Please check the cart.",
                                "Invalid Total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(PaidAmounttextBox.Text, out decimal paidAmount) || paidAmount <= 0)
            {
                MessageBox.Show("Please enter a valid paid amount.",
                                "Invalid Paid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PaidAmounttextBox.Focus();
                return;
            }

            if (paidAmount < totalAmount)
            {
                MessageBox.Show("Paid amount cannot be less than the grand total.",
                                "Insufficient Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PaidAmounttextBox.Focus();
                return;
            }

            if (PaymentMethodcomboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a payment method.",
                                "Payment Method Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PaymentMethodcomboBox.Focus();
                return;
            }

            // ── Gather values ──────────────────────────────────────────────────
            string saleID       = SaleIDGenarator.GenerateSaleID();
            string invoiceNo    = InvoiceNumbertextBox.Text;
            string customerID   = CustomerIDtextBox.Text;
            decimal changeAmt   = paidAmount - totalAmount;
            string paymentMethod = PaymentMethodcomboBox.SelectedItem?.ToString() ?? "Cash";

            // ── DB transaction ─────────────────────────────────────────────────
            SqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();
            SqlTransaction tx = conn.BeginTransaction();

            try
            {
                // 1. Insert SALE header
                string saleQuery = @"INSERT INTO [SALE]
                    (SALEID, INVOICENUMBER, CUSTOMERID, CASHIERID, SALEDATE,
                     TOTALAMOUNT, DISCOUNTPERCENTAGE, DISCOUNTAMOUNT,
                     TAXPERCENTAGE, TAXAMOUNT, GRANDTOTAL,
                     PAIDAMOUNT, CHANGEAMOUNT, PAYMENTMETHOD, SALESTATUS, CREATEDDATE)
                    VALUES
                    (@SaleID, @InvoiceNumber, @CustomerID, @CashierID, @SaleDate,
                     @TotalAmount, 0, 0,
                     0, 0, @GrandTotal,
                     @PaidAmount, @ChangeAmount, @PaymentMethod, 'COMPLETED', GETDATE())";

                using (SqlCommand cmd = new SqlCommand(saleQuery, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@SaleID",        saleID);
                    cmd.Parameters.AddWithValue("@InvoiceNumber",  invoiceNo);
                    cmd.Parameters.AddWithValue("@CustomerID",     string.IsNullOrEmpty(customerID) ? (object)DBNull.Value : customerID);
                    cmd.Parameters.AddWithValue("@CashierID",      string.IsNullOrEmpty(_userId) ? "C-1001" : _userId);
                    cmd.Parameters.AddWithValue("@SaleDate",       DateTime.Now);
                    cmd.Parameters.AddWithValue("@TotalAmount",    totalAmount);
                    cmd.Parameters.AddWithValue("@GrandTotal",     totalAmount);
                    cmd.Parameters.AddWithValue("@PaidAmount",     paidAmount);
                    cmd.Parameters.AddWithValue("@ChangeAmount",   changeAmt);
                    cmd.Parameters.AddWithValue("@PaymentMethod",  paymentMethod);
                    cmd.ExecuteNonQuery();
                }

                // 2. Insert SALEITEM rows + deduct stock
                foreach (DataRow row in cartTable.Rows)
                {
                    string saleItemID  = SaleItemIDGenarator.GenerateSaleItemID();
                    string productID   = row["ProductID"].ToString() ?? "";
                    int    quantity    = Convert.ToInt32(row["Qty"]);
                    string unit        = row["Unit"].ToString() ?? "";
                    decimal unitPrice  = Convert.ToDecimal(row["MRP"]);
                    decimal discPct    = Convert.ToDecimal(row["Discount"]);
                    decimal taxPct     = Convert.ToDecimal(row["Tax"]);
                    decimal discAmt    = Math.Round(unitPrice * quantity * discPct / 100, 2);
                    decimal taxAmt     = Math.Round((unitPrice * quantity - discAmt) * taxPct / 100, 2);
                    decimal lineTotal  = Convert.ToDecimal(row["Total"]);

                    string itemQuery = @"INSERT INTO [SALEITEM]
                        (SALEITEMID, SALEID, PRODUCTID, BARCODE, QUANTITY, UNIT, UNITPRICE,
                         DISCOUNTPERCENTAGE, DISCOUNTAMOUNT, TAXPERCENTAGE, TAXAMOUNT, LINETOTAL, CREATEDDATE)
                        VALUES
                        (@SaleItemID, @SaleID, @ProductID, '', @Quantity, @Unit, @UnitPrice,
                         @DiscPct, @DiscAmt, @TaxPct, @TaxAmt, @LineTotal, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(itemQuery, conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@SaleItemID", saleItemID);
                        cmd.Parameters.AddWithValue("@SaleID",     saleID);
                        cmd.Parameters.AddWithValue("@ProductID",  productID);
                        cmd.Parameters.AddWithValue("@Quantity",   quantity);
                        cmd.Parameters.AddWithValue("@Unit",       unit);
                        cmd.Parameters.AddWithValue("@UnitPrice",  unitPrice);
                        cmd.Parameters.AddWithValue("@DiscPct",    discPct);
                        cmd.Parameters.AddWithValue("@DiscAmt",    discAmt);
                        cmd.Parameters.AddWithValue("@TaxPct",     taxPct);
                        cmd.Parameters.AddWithValue("@TaxAmt",     taxAmt);
                        cmd.Parameters.AddWithValue("@LineTotal",  lineTotal);
                        cmd.ExecuteNonQuery();
                    }

                    // Deduct stock
                    using (SqlCommand stockCmd = new SqlCommand(
                        "UPDATE [PRODUCT] SET STOCKQUANTITY = STOCKQUANTITY - @Qty WHERE PRODUCTID = @PID",
                        conn, tx))
                    {
                        stockCmd.Parameters.AddWithValue("@Qty", quantity);
                        stockCmd.Parameters.AddWithValue("@PID", productID);
                        stockCmd.ExecuteNonQuery();
                    }
                }

                // 3. Update customer loyalty points & total purchase
                if (!string.IsNullOrEmpty(customerID))
                {
                    decimal pointsEarned = Math.Round(totalAmount / 100, 2); // 1 pt per ৳100
                    using SqlCommand custCmd = new SqlCommand(
                        @"UPDATE [CUSTOMER]
                          SET TotalPurchase = TotalPurchase + @Total,
                              LoyaltyPoints = LoyaltyPoints + @Points
                          WHERE CustomerID = @CID",
                        conn, tx);
                    custCmd.Parameters.AddWithValue("@Total",  totalAmount);
                    custCmd.Parameters.AddWithValue("@Points", pointsEarned);
                    custCmd.Parameters.AddWithValue("@CID",    customerID);
                    custCmd.ExecuteNonQuery();
                }

                tx.Commit();

                MessageBox.Show(
                    $"✅ Sale completed successfully!\n\n" +
                    $"Invoice  :  {invoiceNo}\n" +
                    $"Sale ID  :  {saleID}\n" +
                    $"Total    :  ৳ {totalAmount:N2}\n" +
                    $"Paid     :  ৳ {paidAmount:N2}\n" +
                    $"Change   :  ৳ {changeAmt:N2}",
                    "Sale Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset for next sale
                cartTable.Rows.Clear();
                RefreshCartTotals();
                PaidAmounttextBox.Clear();
                ChangeAmountlabel.Text = "";
                GenerateInvoiceNumber();
                LoadActiveProducts();   // refresh stock counts on cards
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Error completing sale: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        // ─── Logout ───────────────────────────────────────────────────────────────

        private void Logoutbutton_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count > 0)
            {
                var result = MessageBox.Show(
                    "You have items in your cart. Are you sure you want to log out?\nYour cart will be cleared.",
                    "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            // Find the existing hidden LogInGUI, clear its fields and show it,
            // then close this dashboard. ShowDialog() in LogInGUI will return,
            // and since we already called Show() the login window stays open.
            foreach (Form f in Application.OpenForms)
            {
                if (f is LogInGUI loginForm)
                {
                    loginForm.LoggedOut = true;   // tell LogInGUI not to close itself
                    loginForm.ClearAndShow();      // clear fields + show login
                    break;
                }
            }

            this.Close();   // closes dashboard; ShowDialog() returns; LogInGUI stays open
        }

        // ─── Add to Cart ──────────────────────────────────────────────────────────

        private void AddToCart(string productId, string productName, string unit,
                               decimal mrp, decimal discount, decimal tax, int stock)
        {
            if (stock <= 0)
            {
                MessageBox.Show($"{productName} is out of stock.", "Out of Stock",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find existing row
            DataRow[] existing = cartTable.Select($"ProductID = '{productId}'");

            if (existing.Length > 0)
            {
                int currentQty = Convert.ToInt32(existing[0]["Qty"]);
                if (currentQty >= stock)
                {
                    MessageBox.Show($"Only {stock} unit(s) available in stock.", "Stock Limit",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing[0]["Qty"]   = currentQty + 1;
                existing[0]["Total"] = CalculateLineTotal(mrp, discount, tax, currentQty + 1);
            }
            else
            {
                decimal lineTotal = CalculateLineTotal(mrp, discount, tax, 1);
                cartTable.Rows.Add(productId, productName, unit, mrp, discount, tax, 1, lineTotal);
            }

            RefreshCartTotals();
        }

        private static decimal CalculateLineTotal(decimal mrp, decimal discount, decimal tax, int qty)
        {
            // MRP already includes discount/tax baked in from AddProductGUI formula,
            // so line total = MRP × qty
            return Math.Round(mrp * qty, 2);
        }

        private void RefreshCartTotals()
        {
            decimal total = 0;
            foreach (DataRow row in cartTable.Rows)
                total += Convert.ToDecimal(row["Total"]);

            TotalAmounttextBox.Text    = total.ToString("N2");
            GrandTotalAmountlabel.Text = $"৳ {total:N2}";
        }

        // ─── Cart grid events ─────────────────────────────────────────────────────

        private void CartGrid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (cartGrid.Columns[e.ColumnIndex].Name != "Qty") return;

            DataRow row = cartTable.Rows[e.RowIndex];
            if (!int.TryParse(cartGrid.Rows[e.RowIndex].Cells["Qty"].Value?.ToString(), out int qty) || qty < 1)
            {
                row["Qty"] = 1;
                qty = 1;
            }

            decimal mrp      = Convert.ToDecimal(row["MRP"]);
            decimal discount = Convert.ToDecimal(row["Discount"]);
            decimal tax      = Convert.ToDecimal(row["Tax"]);
            row["Total"]     = CalculateLineTotal(mrp, discount, tax, qty);

            RefreshCartTotals();
        }

        private void CartGrid_RemoveClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (cartGrid.Columns[e.ColumnIndex].Name != "Remove") return;

            cartTable.Rows.RemoveAt(e.RowIndex);
            RefreshCartTotals();
        }

        // ─── Data Loading ─────────────────────────────────────────────────────────

        private void LoadActiveProducts()
        {
            productFlowPanel.Controls.Clear();

            try
            {
                using SqlConnection conn = DatabaseConnection.GetConnection();
                conn.Open();

                string query = @"SELECT PRODUCTID, PRODUCTNAME, BRAND, PRODUCTCATEGORY,
                                        PHOTO, DISCOUNT, TAX, MRP, UNIT, STOCKQUANTITY
                                 FROM   PRODUCT
                                 WHERE  ISACTIVE = 1
                                 ORDER  BY PRODUCTNAME";

                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();

                bool hasProducts = false;

                while (reader.Read())
                {
                    hasProducts = true;

                    string productId   = reader["PRODUCTID"]?.ToString()   ?? "";
                    string productName = reader["PRODUCTNAME"]?.ToString()  ?? "Unknown";
                    string brand       = reader["BRAND"]?.ToString()        ?? "";
                    string category    = reader["PRODUCTCATEGORY"]?.ToString() ?? "";
                    decimal mrp        = reader["MRP"]      == DBNull.Value ? 0 : Convert.ToDecimal(reader["MRP"]);
                    decimal discount   = reader["DISCOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DISCOUNT"]);
                    decimal tax        = reader["TAX"]      == DBNull.Value ? 0 : Convert.ToDecimal(reader["TAX"]);
                    string unit        = reader["UNIT"]?.ToString()         ?? "";
                    int stock          = reader["STOCKQUANTITY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["STOCKQUANTITY"]);

                    byte[]? photoBytes = reader["PHOTO"] == DBNull.Value ? null : (byte[])reader["PHOTO"];

                    Panel card = CreateProductCard(productId, productName, brand, category,
                                                   photoBytes, mrp, discount, tax, unit, stock);
                    productFlowPanel.Controls.Add(card);
                }

                if (!hasProducts)
                {
                    Label noProducts = new Label
                    {
                        Text = "No active products available.",
                        Font = new Font("Segoe UI", 12F),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Margin = new Padding(20)
                    };
                    productFlowPanel.Controls.Add(noProducts);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─── Card Builder ─────────────────────────────────────────────────────────

        private Panel CreateProductCard(string productId, string productName, string brand,
                                        string category, byte[]? photoBytes,
                                        decimal mrp, decimal discount, decimal tax,
                                        string unit, int stock)
        {
            const int CARD_W = 175;
            const int CARD_H = 305;   // taller to fit the Add to Cart button

            // Outer card panel
            Panel card = new Panel
            {
                Width  = CARD_W,
                Height = CARD_H,
                Margin = new Padding(8),
                BackColor = Color.White,
                Cursor = Cursors.Default,
                Tag = productId
            };

            // Rounded-corner border via Paint
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(200, 210, 235), 1.5f);
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                DrawRoundedRectangle(e.Graphics, pen, rect, 10);
            };

            int y = 0;

            // ── Product photo ──────────────────────────────────────────────────
            PictureBox photo = new PictureBox
            {
                Width  = CARD_W,
                Height = 130,
                Location = new Point(0, y),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 247, 252)
            };

            if (photoBytes != null && photoBytes.Length > 0)
            {
                try
                {
                    using var ms = new MemoryStream(photoBytes);
                    photo.Image = new Bitmap(ms);
                }
                catch
                {
                    photo.Image = null;
                }
            }

            if (photo.Image == null)
            {
                photo.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(230, 235, 248)),
                                             0, 0, photo.Width, photo.Height);
                    using var f = new Font("Segoe UI", 28F);
                    string icon = "📦";
                    var sz = e.Graphics.MeasureString(icon, f);
                    e.Graphics.DrawString(icon, f, Brushes.LightSlateGray,
                                          (photo.Width - sz.Width) / 2,
                                          (photo.Height - sz.Height) / 2);
                };
            }

            card.Controls.Add(photo);
            y += photo.Height + 4;

            // ── Product name ───────────────────────────────────────────────────
            Label nameLabel = new Label
            {
                Text      = productName,
                Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 60),
                Location  = new Point(6, y),
                Width     = CARD_W - 12,
                Height    = 34,
                AutoEllipsis = true
            };
            card.Controls.Add(nameLabel);
            y += nameLabel.Height;

            // ── Price (MRP) ────────────────────────────────────────────────────
            Label priceLabel = new Label
            {
                Text      = $"৳ {mrp:N2}  / {unit}",
                Font      = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 128, 64),
                Location  = new Point(6, y),
                Width     = CARD_W - 12,
                Height    = 22,
                AutoEllipsis = true
            };
            card.Controls.Add(priceLabel);
            y += priceLabel.Height + 2;

            // ── Discount badge ─────────────────────────────────────────────────
            Panel discountBadge = CreateBadge(
                discount > 0 ? $"Discount  {discount:0.##}%" : "No Discount",
                discount > 0 ? Color.FromArgb(255, 87, 34) : Color.FromArgb(180, 180, 180),
                new Point(6, y), CARD_W - 12);
            card.Controls.Add(discountBadge);
            y += discountBadge.Height + 3;

            // ── Tax badge ──────────────────────────────────────────────────────
            Panel taxBadge = CreateBadge(
                tax > 0 ? $"Tax  {tax:0.##}%" : "Tax Free",
                tax > 0 ? Color.FromArgb(33, 150, 243) : Color.FromArgb(76, 175, 80),
                new Point(6, y), CARD_W - 12);
            card.Controls.Add(taxBadge);
            y += taxBadge.Height + 3;

            // ── Stock indicator ────────────────────────────────────────────────
            Label stockLabel = new Label
            {
                Text      = stock > 0 ? $"In Stock ({stock})" : "Out of Stock",
                Font      = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                ForeColor = stock > 0 ? Color.SeaGreen : Color.Crimson,
                Location  = new Point(6, y),
                Width     = CARD_W - 12,
                Height    = 18,
                AutoEllipsis = true
            };
            card.Controls.Add(stockLabel);
            y += stockLabel.Height + 4;

            // ── Add to Cart button ─────────────────────────────────────────────
            Button addBtn = new Button
            {
                Text      = stock > 0 ? "+ Add to Cart" : "Out of Stock",
                Font      = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = stock > 0 ? Color.FromArgb(37, 99, 235) : Color.FromArgb(180, 180, 180),
                FlatStyle = FlatStyle.Flat,
                Location  = new Point(6, y),
                Width     = CARD_W - 12,
                Height    = 30,
                Cursor    = stock > 0 ? Cursors.Hand : Cursors.Default,
                Enabled   = stock > 0,
                Tag       = productId
            };
            addBtn.FlatAppearance.BorderSize = 0;

            // Hover effect
            if (stock > 0)
            {
                addBtn.MouseEnter += (s, e) => addBtn.BackColor = Color.FromArgb(29, 78, 216);
                addBtn.MouseLeave += (s, e) => addBtn.BackColor = Color.FromArgb(37, 99, 235);
                addBtn.Click += (s, e) =>
                    AddToCart(productId, productName, unit, mrp, discount, tax, stock);
            }

            card.Controls.Add(addBtn);

            // Forward hover to card background (exclude button — it has its own style)
            ForwardMouseEvents(card, photo, nameLabel, priceLabel, stockLabel);

            return card;
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private static Panel CreateBadge(string text, Color backColor, Point location, int width)
        {
            Panel badge = new Panel
            {
                Location  = location,
                Width     = width,
                Height    = 20,
                BackColor = backColor
            };

            // Rounded corners
            badge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(backColor);
                var rect = new Rectangle(0, 0, badge.Width - 1, badge.Height - 1);
                DrawRoundedRectangle(e.Graphics, brush, rect, 6);
            };

            Label lbl = new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            badge.Controls.Add(lbl);
            return badge;
        }

        private static void DrawRoundedRectangle(System.Drawing.Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using var path = GetRoundedPath(rect, radius);
            g.DrawPath(pen, path);
        }

        private static void DrawRoundedRectangle(System.Drawing.Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using var path = GetRoundedPath(rect, radius);
            g.FillPath(brush, path);
        }

        private static System.Drawing.Drawing2D.GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void ForwardMouseEvents(Panel card, params Control[] controls)
        {
            foreach (var ctrl in controls)
            {
                ctrl.MouseEnter += (s, e) => { card.BackColor = Color.FromArgb(235, 242, 255); };
                ctrl.MouseLeave += (s, e) => { card.BackColor = Color.White; };
            }
        }
    }
}
