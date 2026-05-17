using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Customer
{
    public partial class CustomerManagerGUI : Form
    {
        private string? selectedUserId = null;

        public CustomerManagerGUI()
        {
            InitializeComponent();
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            dataGridView1.CellClick += DataGridView1_CellClick;
            Clearbutton.Click += Clearbutton_Click;
        }

        private void Showbutton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, c.CustomerID, c.CustomerType, c.TotalPurchase, c.IsVIP, c.LoyaltyPoints FROM [USER] u INNER JOIN [CUSTOMER] c ON u.UserID = c.UserID WHERE u.UserType = 'Customer'";
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

        private void SearchCustomerbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = SearchCustomertextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Please enter Customer ID or Phone Number to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, c.CustomerID, c.CustomerType, c.TotalPurchase, c.IsVIP, c.LoyaltyPoints FROM [USER] u INNER JOIN [CUSTOMER] c ON u.UserID = c.UserID WHERE u.UserType = 'Customer' AND (c.CustomerID = @SearchValue OR u.Phone = @SearchValue)";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SearchValue", searchValue);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No customer found with the given Customer ID or Phone Number.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void DataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    selectedUserId = row.Cells["UserID"].Value?.ToString();

                    UserIDtextBox.Text = row.Cells["UserID"].Value?.ToString() ?? "";
                    FullNametextBox.Text = row.Cells["Name"].Value?.ToString() ?? "";
                    PhoneNumbertextBox.Text = row.Cells["Phone"].Value?.ToString() ?? "";
                    EmailtextBox.Text = row.Cells["Email"].Value?.ToString() ?? "";
                    AddressrichTextBox.Text = row.Cells["Address"].Value?.ToString() ?? "";
                    CustomerIDtextBox.Text = row.Cells["CustomerID"].Value?.ToString() ?? "";
                    CustomerTypecomboBox.Text = row.Cells["CustomerType"].Value?.ToString() ?? "";
                    IsVIPcheckBox.Checked = Convert.ToBoolean(row.Cells["IsVIP"].Value ?? false);
                    IsActivecheckBox.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value ?? false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Updatebutton_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(selectedUserId))
                {
                    MessageBox.Show("Please select a customer from the list to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(FullNametextBox.Text))
                {
                    MessageBox.Show("Please enter Full Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(PhoneNumbertextBox.Text))
                {
                    MessageBox.Show("Please enter Phone Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(EmailtextBox.Text))
                {
                    MessageBox.Show("Please enter Email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm update
                DialogResult result = MessageBox.Show("Are you sure you want to update this customer?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update database
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    // Update USER table
                    string userQuery = "UPDATE [USER] SET Name = '" + FullNametextBox.Text.Replace("'", "''") + "', Phone = '" + PhoneNumbertextBox.Text.Replace("'", "''") + "', " +
                        "Email = '" + EmailtextBox.Text.Replace("'", "''") + "', Address = '" + AddressrichTextBox.Text.Replace("'", "''") + "', IsActive = " + (IsActivecheckBox.Checked ? 1 : 0) + " " +
                        "WHERE UserID = '" + selectedUserId.Replace("'", "''") + "'";

                    // Update CUSTOMER table
                    string customerQuery = "UPDATE [CUSTOMER] SET CustomerType = '" + CustomerTypecomboBox.Text.Replace("'", "''") + "', IsVIP = " + (IsVIPcheckBox.Checked ? 1 : 0) + " " +
                        "WHERE UserID = '" + selectedUserId.Replace("'", "''") + "'";

                    SqlCommand cmd = new SqlCommand(userQuery + "; " + customerQuery, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Showbutton_Click(null, EventArgs.Empty);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(selectedUserId))
                {
                    MessageBox.Show("Please select a customer from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this customer? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete from database
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    // Delete from CUSTOMER table first (foreign key constraint)
                    string deleteCustomerQuery = "DELETE FROM [CUSTOMER] WHERE UserID = '" + selectedUserId.Replace("'", "''") + "'";

                    // Delete from USER table
                    string deleteUserQuery = "DELETE FROM [USER] WHERE UserID = '" + selectedUserId.Replace("'", "''") + "'";

                    SqlCommand cmd = new SqlCommand(deleteCustomerQuery + "; " + deleteUserQuery, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Showbutton_Click(null, EventArgs.Empty);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            // Clear all text boxes
            UserIDtextBox.Clear();
            FullNametextBox.Clear();
            PhoneNumbertextBox.Clear();
            EmailtextBox.Clear();
            AddressrichTextBox.Clear();
            CustomerIDtextBox.Clear();
            SearchCustomertextBox.Clear();

            // Clear combo box
            CustomerTypecomboBox.SelectedIndex = -1;

            // Uncheck checkboxes
            IsVIPcheckBox.Checked = false;
            IsActivecheckBox.Checked = false;

            // Reset selected user ID
            selectedUserId = null;

            // Clear data grid view
            dataGridView1.DataSource = null;
        }
    }
}
