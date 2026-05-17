namespace SuperShop.View.Dashboard
{
    partial class CashierDashboard
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
            Header = new Panel();
            CashierIDlabel = new Label();
            Welcomelabel = new Label();
            Operationpanel = new Panel();
            POSbutton = new Button();
            CustomerManagerbutton = new Button();
            AddCustomerbutton = new Button();
            LoadPanel = new Panel();
            Header.SuspendLayout();
            Operationpanel.SuspendLayout();
            SuspendLayout();
            // 
            // Header
            // 
            Header.BackColor = Color.Tomato;
            Header.Controls.Add(CashierIDlabel);
            Header.Controls.Add(Welcomelabel);
            Header.Dock = DockStyle.Top;
            Header.Location = new Point(0, 0);
            Header.Name = "Header";
            Header.Size = new Size(1310, 56);
            Header.TabIndex = 0;
            // 
            // CashierIDlabel
            // 
            CashierIDlabel.AutoSize = true;
            CashierIDlabel.Location = new Point(108, 18);
            CashierIDlabel.Name = "CashierIDlabel";
            CashierIDlabel.Size = new Size(82, 19);
            CashierIDlabel.TabIndex = 2;
            CashierIDlabel.Text = "Cashier ID :";
            // 
            // Welcomelabel
            // 
            Welcomelabel.AutoSize = true;
            Welcomelabel.Location = new Point(1019, 18);
            Welcomelabel.Name = "Welcomelabel";
            Welcomelabel.Size = new Size(70, 19);
            Welcomelabel.TabIndex = 1;
            Welcomelabel.Text = "Welcome,";
            // 
            // Operationpanel
            // 
            Operationpanel.Controls.Add(POSbutton);
            Operationpanel.Controls.Add(CustomerManagerbutton);
            Operationpanel.Controls.Add(AddCustomerbutton);
            Operationpanel.Dock = DockStyle.Left;
            Operationpanel.Location = new Point(0, 56);
            Operationpanel.Name = "Operationpanel";
            Operationpanel.Size = new Size(197, 653);
            Operationpanel.TabIndex = 1;
            // 
            // POSbutton
            // 
            POSbutton.BackColor = Color.OliveDrab;
            POSbutton.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold);
            POSbutton.ForeColor = Color.White;
            POSbutton.Location = new Point(9, 109);
            POSbutton.Name = "POSbutton";
            POSbutton.Size = new Size(180, 45);
            POSbutton.TabIndex = 2;
            POSbutton.Text = "POS";
            POSbutton.UseVisualStyleBackColor = false;
            POSbutton.Click += POSbutton_Click;
            // 
            // CustomerManagerbutton
            // 
            CustomerManagerbutton.Location = new Point(9, 63);
            CustomerManagerbutton.Name = "CustomerManagerbutton";
            CustomerManagerbutton.Size = new Size(180, 36);
            CustomerManagerbutton.TabIndex = 1;
            CustomerManagerbutton.Text = "Manage Customers";
            CustomerManagerbutton.UseVisualStyleBackColor = true;
            CustomerManagerbutton.Click += CustomerManagerbutton_Click;
            // 
            // AddCustomerbutton
            // 
            AddCustomerbutton.Location = new Point(9, 18);
            AddCustomerbutton.Name = "AddCustomerbutton";
            AddCustomerbutton.Size = new Size(180, 36);
            AddCustomerbutton.TabIndex = 0;
            AddCustomerbutton.Text = "Add Customer";
            AddCustomerbutton.UseVisualStyleBackColor = true;
            AddCustomerbutton.Click += AddCustomerbutton_Click;
            // 
            // LoadPanel
            // 
            LoadPanel.Dock = DockStyle.Fill;
            LoadPanel.Location = new Point(197, 56);
            LoadPanel.Name = "LoadPanel";
            LoadPanel.Size = new Size(1113, 653);
            LoadPanel.TabIndex = 2;
            // 
            // CashierDashboard
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1310, 709);
            Controls.Add(LoadPanel);
            Controls.Add(Operationpanel);
            Controls.Add(Header);
            Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "CashierDashboard";
            Text = "CashierDashboard";
            WindowState = FormWindowState.Maximized;
            Header.ResumeLayout(false);
            Header.PerformLayout();
            Operationpanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel Header;
        private Label CashierIDlabel;
        private Label Welcomelabel;
        private Panel Operationpanel;
        private Button AddCustomerbutton;
        private Button CustomerManagerbutton;
        private Button POSbutton;
        private Panel LoadPanel;
    }
}