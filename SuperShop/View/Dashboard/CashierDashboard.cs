using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;
using SuperShop.View.Customer;
using SuperShop.View.POS;

namespace SuperShop.View.Dashboard
{
    public partial class CashierDashboard : Form
    {
        private string? cashierId;
        private string? cashierName;

        public CashierDashboard(string userId = "")
        {
            InitializeComponent();
            cashierId = userId;
            LoadCashierInfo();
            AttachEventHandlers();
            LoadPOSOnStartup();
        }

        private void LoadCashierInfo()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cashierId))
                {
                    CashierIDlabel.Text = "Cashier ID : N/A";
                    Welcomelabel.Text = "Welcome,";
                    return;
                }

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT UserID, Name FROM [USER] WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", cashierId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cashierName = reader["Name"].ToString();
                                CashierIDlabel.Text = $"Cashier ID : {cashierId}";
                                Welcomelabel.Text = $"Welcome, {cashierName}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cashier information: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CashierIDlabel.Text = "Cashier ID : Error";
                Welcomelabel.Text = "Welcome,";
            }
        }

        private void LoadPOSOnStartup()
        {
            try
            {
                LoadPanel.Controls.Clear();
                POSforCashier posForm = new POSforCashier(cashierId);
                posForm.TopLevel = false;
                posForm.FormBorderStyle = FormBorderStyle.None;
                posForm.Dock = DockStyle.Fill;
                LoadPanel.Controls.Add(posForm);
                posForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading POS: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AttachEventHandlers()
        {
            AddCustomerbutton.Click += AddCustomerbutton_Click;
            CustomerManagerbutton.Click += CustomerManagerbutton_Click;
            POSbutton.Click += POSbutton_Click;
        }

        private void AddCustomerbutton_Click(object? sender, EventArgs e)
        {
            LoadPanel.Controls.Clear();
            AddCustomerGUI addCustomerGUI = new AddCustomerGUI();
            addCustomerGUI.TopLevel = false;
            addCustomerGUI.FormBorderStyle = FormBorderStyle.None;
            addCustomerGUI.Dock = DockStyle.Fill;
            LoadPanel.Controls.Add(addCustomerGUI);
            addCustomerGUI.Show();
        }

        private void CustomerManagerbutton_Click(object? sender, EventArgs e)
        {
            LoadPanel.Controls.Clear();
            CustomerManagerGUI customerManagerGUI = new CustomerManagerGUI();
            customerManagerGUI.TopLevel = false;
            customerManagerGUI.FormBorderStyle = FormBorderStyle.None;
            customerManagerGUI.Dock = DockStyle.Fill;
            LoadPanel.Controls.Add(customerManagerGUI);
            customerManagerGUI.Show();
        }

        private void POSbutton_Click(object? sender, EventArgs e)
        {
            LoadPOSOnStartup();
        }
    }
}
