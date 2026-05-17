namespace SuperShop.View.Dashboard
{
    partial class AdminDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Headerpanel = new Panel();
            Welcomelabel = new Label();
            AdminIDlabel = new Label();
            Operationpanel = new Panel();
            ProductManagerbutton = new Button();
            AddProductbutton = new Button();
            ProductManagementlabel = new Label();
            Separator4 = new Panel();
            SupplierManagerbutton = new Button();
            AddSupplierbutton = new Button();
            SupplierManagementlabel = new Label();
            Separator3 = new Panel();
            ManagerManagerbutton = new Button();
            AddManagerbutton = new Button();
            ManagerManagementlabel = new Label();
            Separator2 = new Panel();
            CashierManagerbutton = new Button();
            AddCashierbutton = new Button();
            CashierManagementlabel = new Label();
            Separator1 = new Panel();
            AdminManagerbutton = new Button();
            AddAdminbutton = new Button();
            AdminManagementlabel = new Label();
            GUILoadpanel = new Panel();
            Headerpanel.SuspendLayout();
            Operationpanel.SuspendLayout();
            SuspendLayout();
            // 
            // Headerpanel
            // 
            Headerpanel.BackColor = Color.FromArgb(255, 127, 80);
            Headerpanel.Controls.Add(Welcomelabel);
            Headerpanel.Controls.Add(AdminIDlabel);
            Headerpanel.Dock = DockStyle.Top;
            Headerpanel.Location = new Point(0, 0);
            Headerpanel.Name = "Headerpanel";
            Headerpanel.Size = new Size(1274, 54);
            Headerpanel.TabIndex = 2;
            // 
            // Welcomelabel
            // 
            Welcomelabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Welcomelabel.AutoSize = true;
            Welcomelabel.Font = new Font("Times New Roman", 12F);
            Welcomelabel.ForeColor = Color.Black;
            Welcomelabel.Location = new Point(1037, 16);
            Welcomelabel.Name = "Welcomelabel";
            Welcomelabel.Size = new Size(70, 19);
            Welcomelabel.TabIndex = 1;
            Welcomelabel.Text = "Welcome,";
            // 
            // AdminIDlabel
            // 
            AdminIDlabel.AutoSize = true;
            AdminIDlabel.Font = new Font("Times New Roman", 12F);
            AdminIDlabel.ForeColor = Color.Black;
            AdminIDlabel.Location = new Point(175, 16);
            AdminIDlabel.Name = "AdminIDlabel";
            AdminIDlabel.Size = new Size(76, 19);
            AdminIDlabel.TabIndex = 0;
            AdminIDlabel.Text = "Admin ID :";
            // 
            // Operationpanel
            // 
            Operationpanel.BackColor = Color.LightSalmon;
            Operationpanel.Controls.Add(ProductManagerbutton);
            Operationpanel.Controls.Add(AddProductbutton);
            Operationpanel.Controls.Add(ProductManagementlabel);
            Operationpanel.Controls.Add(Separator4);
            Operationpanel.Controls.Add(SupplierManagerbutton);
            Operationpanel.Controls.Add(AddSupplierbutton);
            Operationpanel.Controls.Add(SupplierManagementlabel);
            Operationpanel.Controls.Add(Separator3);
            Operationpanel.Controls.Add(ManagerManagerbutton);
            Operationpanel.Controls.Add(AddManagerbutton);
            Operationpanel.Controls.Add(ManagerManagementlabel);
            Operationpanel.Controls.Add(Separator2);
            Operationpanel.Controls.Add(CashierManagerbutton);
            Operationpanel.Controls.Add(AddCashierbutton);
            Operationpanel.Controls.Add(CashierManagementlabel);
            Operationpanel.Controls.Add(Separator1);
            Operationpanel.Controls.Add(AdminManagerbutton);
            Operationpanel.Controls.Add(AddAdminbutton);
            Operationpanel.Controls.Add(AdminManagementlabel);
            Operationpanel.Dock = DockStyle.Left;
            Operationpanel.Location = new Point(0, 54);
            Operationpanel.Name = "Operationpanel";
            Operationpanel.Size = new Size(183, 712);
            Operationpanel.TabIndex = 0;
            // 
            // ProductManagerbutton
            // 
            ProductManagerbutton.BackColor = Color.FromArgb(255, 240, 100);
            ProductManagerbutton.Dock = DockStyle.Top;
            ProductManagerbutton.FlatAppearance.BorderSize = 0;
            ProductManagerbutton.FlatStyle = FlatStyle.Flat;
            ProductManagerbutton.Location = new Point(0, 500);
            ProductManagerbutton.Name = "ProductManagerbutton";
            ProductManagerbutton.Size = new Size(183, 25);
            ProductManagerbutton.TabIndex = 15;
            ProductManagerbutton.Text = "Product Manager";
            ProductManagerbutton.UseVisualStyleBackColor = false;
            ProductManagerbutton.Click += ProductManagerbutton_Click;
            // 
            // AddProductbutton
            // 
            AddProductbutton.BackColor = Color.FromArgb(255, 255, 150);
            AddProductbutton.Dock = DockStyle.Top;
            AddProductbutton.FlatAppearance.BorderSize = 0;
            AddProductbutton.FlatStyle = FlatStyle.Flat;
            AddProductbutton.Location = new Point(0, 475);
            AddProductbutton.Name = "AddProductbutton";
            AddProductbutton.Size = new Size(183, 25);
            AddProductbutton.TabIndex = 14;
            AddProductbutton.Text = "Add Product";
            AddProductbutton.UseVisualStyleBackColor = false;
            AddProductbutton.Click += AddProductbutton_Click;
            // 
            // ProductManagementlabel
            // 
            ProductManagementlabel.AutoSize = true;
            ProductManagementlabel.Dock = DockStyle.Top;
            ProductManagementlabel.Location = new Point(0, 446);
            ProductManagementlabel.Name = "ProductManagementlabel";
            ProductManagementlabel.Padding = new Padding(9, 5, 0, 5);
            ProductManagementlabel.Size = new Size(148, 29);
            ProductManagementlabel.TabIndex = 13;
            ProductManagementlabel.Text = "Product Management";
            // 
            // Separator4
            // 
            Separator4.BackColor = Color.SaddleBrown;
            Separator4.Dock = DockStyle.Top;
            Separator4.Location = new Point(0, 414);
            Separator4.Name = "Separator4";
            Separator4.Size = new Size(183, 32);
            Separator4.TabIndex = 20;
            // 
            // SupplierManagerbutton
            // 
            SupplierManagerbutton.BackColor = Color.FromArgb(255, 200, 150);
            SupplierManagerbutton.Dock = DockStyle.Top;
            SupplierManagerbutton.FlatAppearance.BorderSize = 0;
            SupplierManagerbutton.FlatStyle = FlatStyle.Flat;
            SupplierManagerbutton.Location = new Point(0, 389);
            SupplierManagerbutton.Name = "SupplierManagerbutton";
            SupplierManagerbutton.Size = new Size(183, 25);
            SupplierManagerbutton.TabIndex = 14;
            SupplierManagerbutton.Text = "Supplier Manager";
            SupplierManagerbutton.UseVisualStyleBackColor = false;
            SupplierManagerbutton.Click += SupplierManagerbutton_Click;
            // 
            // AddSupplierbutton
            // 
            AddSupplierbutton.BackColor = Color.FromArgb(255, 220, 150);
            AddSupplierbutton.Dock = DockStyle.Top;
            AddSupplierbutton.FlatAppearance.BorderSize = 0;
            AddSupplierbutton.FlatStyle = FlatStyle.Flat;
            AddSupplierbutton.Location = new Point(0, 364);
            AddSupplierbutton.Name = "AddSupplierbutton";
            AddSupplierbutton.Size = new Size(183, 25);
            AddSupplierbutton.TabIndex = 13;
            AddSupplierbutton.Text = "Add Supplier";
            AddSupplierbutton.UseVisualStyleBackColor = false;
            AddSupplierbutton.Click += AddSupplierbutton_Click;
            // 
            // SupplierManagementlabel
            // 
            SupplierManagementlabel.AutoSize = true;
            SupplierManagementlabel.Dock = DockStyle.Top;
            SupplierManagementlabel.Location = new Point(0, 335);
            SupplierManagementlabel.Name = "SupplierManagementlabel";
            SupplierManagementlabel.Padding = new Padding(9, 5, 0, 5);
            SupplierManagementlabel.Size = new Size(150, 29);
            SupplierManagementlabel.TabIndex = 12;
            SupplierManagementlabel.Text = "Supplier Management";
            // 
            // Separator3
            // 
            Separator3.BackColor = Color.Sienna;
            Separator3.Dock = DockStyle.Top;
            Separator3.Location = new Point(0, 301);
            Separator3.Name = "Separator3";
            Separator3.Size = new Size(183, 34);
            Separator3.TabIndex = 19;
            // 
            // ManagerManagerbutton
            // 
            ManagerManagerbutton.BackColor = Color.FromArgb(240, 220, 255);
            ManagerManagerbutton.Dock = DockStyle.Top;
            ManagerManagerbutton.FlatAppearance.BorderSize = 0;
            ManagerManagerbutton.FlatStyle = FlatStyle.Flat;
            ManagerManagerbutton.Location = new Point(0, 276);
            ManagerManagerbutton.Name = "ManagerManagerbutton";
            ManagerManagerbutton.Size = new Size(183, 25);
            ManagerManagerbutton.TabIndex = 11;
            ManagerManagerbutton.Text = "Manager Manager";
            ManagerManagerbutton.UseVisualStyleBackColor = false;
            ManagerManagerbutton.Click += ManagerManagerbutton_Click;
            // 
            // AddManagerbutton
            // 
            AddManagerbutton.BackColor = Color.FromArgb(255, 240, 200);
            AddManagerbutton.Dock = DockStyle.Top;
            AddManagerbutton.FlatAppearance.BorderSize = 0;
            AddManagerbutton.FlatStyle = FlatStyle.Flat;
            AddManagerbutton.Location = new Point(0, 251);
            AddManagerbutton.Name = "AddManagerbutton";
            AddManagerbutton.Size = new Size(183, 25);
            AddManagerbutton.TabIndex = 10;
            AddManagerbutton.Text = "Add Manager";
            AddManagerbutton.UseVisualStyleBackColor = false;
            AddManagerbutton.Click += AddManagerbutton_Click;
            // 
            // ManagerManagementlabel
            // 
            ManagerManagementlabel.AutoSize = true;
            ManagerManagementlabel.Dock = DockStyle.Top;
            ManagerManagementlabel.Location = new Point(0, 222);
            ManagerManagementlabel.Name = "ManagerManagementlabel";
            ManagerManagementlabel.Padding = new Padding(9, 5, 0, 5);
            ManagerManagementlabel.Size = new Size(154, 29);
            ManagerManagementlabel.TabIndex = 9;
            ManagerManagementlabel.Text = "Manager Management";
            // 
            // Separator2
            // 
            Separator2.BackColor = Color.PeachPuff;
            Separator2.Dock = DockStyle.Top;
            Separator2.Location = new Point(0, 190);
            Separator2.Name = "Separator2";
            Separator2.Size = new Size(183, 32);
            Separator2.TabIndex = 18;
            // 
            // CashierManagerbutton
            // 
            CashierManagerbutton.BackColor = Color.FromArgb(180, 255, 180);
            CashierManagerbutton.Dock = DockStyle.Top;
            CashierManagerbutton.FlatAppearance.BorderSize = 0;
            CashierManagerbutton.FlatStyle = FlatStyle.Flat;
            CashierManagerbutton.Location = new Point(0, 165);
            CashierManagerbutton.Name = "CashierManagerbutton";
            CashierManagerbutton.Size = new Size(183, 25);
            CashierManagerbutton.TabIndex = 8;
            CashierManagerbutton.Text = "Cashier Manager";
            CashierManagerbutton.UseVisualStyleBackColor = false;
            CashierManagerbutton.Click += CashierManagerbutton_Click;
            // 
            // AddCashierbutton
            // 
            AddCashierbutton.BackColor = Color.FromArgb(200, 255, 200);
            AddCashierbutton.Dock = DockStyle.Top;
            AddCashierbutton.FlatAppearance.BorderSize = 0;
            AddCashierbutton.FlatStyle = FlatStyle.Flat;
            AddCashierbutton.Location = new Point(0, 140);
            AddCashierbutton.Name = "AddCashierbutton";
            AddCashierbutton.Size = new Size(183, 25);
            AddCashierbutton.TabIndex = 7;
            AddCashierbutton.Text = "Add Cashier";
            AddCashierbutton.UseVisualStyleBackColor = false;
            AddCashierbutton.Click += AddCashierbutton_Click;
            // 
            // CashierManagementlabel
            // 
            CashierManagementlabel.AutoSize = true;
            CashierManagementlabel.Dock = DockStyle.Top;
            CashierManagementlabel.Location = new Point(0, 111);
            CashierManagementlabel.Name = "CashierManagementlabel";
            CashierManagementlabel.Padding = new Padding(9, 5, 0, 5);
            CashierManagementlabel.Size = new Size(146, 29);
            CashierManagementlabel.TabIndex = 6;
            CashierManagementlabel.Text = "Cashier Management";
            // 
            // Separator1
            // 
            Separator1.BackColor = Color.SaddleBrown;
            Separator1.Dock = DockStyle.Top;
            Separator1.Location = new Point(0, 79);
            Separator1.Name = "Separator1";
            Separator1.Size = new Size(183, 32);
            Separator1.TabIndex = 17;
            // 
            // AdminManagerbutton
            // 
            AdminManagerbutton.BackColor = Color.FromArgb(0, 192, 0);
            AdminManagerbutton.Dock = DockStyle.Top;
            AdminManagerbutton.FlatAppearance.BorderSize = 0;
            AdminManagerbutton.FlatStyle = FlatStyle.Flat;
            AdminManagerbutton.Location = new Point(0, 54);
            AdminManagerbutton.Name = "AdminManagerbutton";
            AdminManagerbutton.Size = new Size(183, 25);
            AdminManagerbutton.TabIndex = 2;
            AdminManagerbutton.Text = "Admin Manager";
            AdminManagerbutton.UseVisualStyleBackColor = false;
            AdminManagerbutton.Click += AdminManagerbutton_Click;
            // 
            // AddAdminbutton
            // 
            AddAdminbutton.BackColor = Color.Gray;
            AddAdminbutton.Dock = DockStyle.Top;
            AddAdminbutton.FlatAppearance.BorderSize = 0;
            AddAdminbutton.FlatStyle = FlatStyle.Flat;
            AddAdminbutton.Location = new Point(0, 29);
            AddAdminbutton.Name = "AddAdminbutton";
            AddAdminbutton.Size = new Size(183, 25);
            AddAdminbutton.TabIndex = 1;
            AddAdminbutton.Text = "Add Admin";
            AddAdminbutton.UseVisualStyleBackColor = false;
            AddAdminbutton.Click += AddAdminbutton_Click;
            // 
            // AdminManagementlabel
            // 
            AdminManagementlabel.AutoSize = true;
            AdminManagementlabel.Dock = DockStyle.Top;
            AdminManagementlabel.Location = new Point(0, 0);
            AdminManagementlabel.Name = "AdminManagementlabel";
            AdminManagementlabel.Padding = new Padding(9, 5, 0, 5);
            AdminManagementlabel.Size = new Size(140, 29);
            AdminManagementlabel.TabIndex = 0;
            AdminManagementlabel.Text = "Admin Management";
            // 
            // GUILoadpanel
            // 
            GUILoadpanel.Dock = DockStyle.Fill;
            GUILoadpanel.Location = new Point(183, 54);
            GUILoadpanel.Name = "GUILoadpanel";
            GUILoadpanel.Size = new Size(1091, 712);
            GUILoadpanel.TabIndex = 1;
            // 
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1274, 766);
            Controls.Add(GUILoadpanel);
            Controls.Add(Operationpanel);
            Controls.Add(Headerpanel);
            Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "AdminDashboard";
            Text = "AdminDashboard";
            WindowState = FormWindowState.Maximized;
            Headerpanel.ResumeLayout(false);
            Headerpanel.PerformLayout();
            Operationpanel.ResumeLayout(false);
            Operationpanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel Headerpanel;
        private Label AdminIDlabel;
        private Label Welcomelabel;
        private Panel Operationpanel;
        private Panel GUILoadpanel;
        private Button AddAdminbutton;
        private Label AdminManagementlabel;
        private Button AdminManagerbutton;
        private Label CashierManagementlabel;
        private Button AddCashierbutton;
        private Button CashierManagerbutton;
        private Panel Separator1;
        private Label ManagerManagementlabel;
        private Button AddManagerbutton;
        private Button ManagerManagerbutton;
        private Panel Separator2;
        private Label SupplierManagementlabel;
        private Button AddSupplierbutton;
        private Button SupplierManagerbutton;
        private Panel Separator3;
        private Label ProductManagementlabel;
        private Button AddProductbutton;
        private Button ProductManagerbutton;
        private Panel Separator4;
    }
}