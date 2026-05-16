using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Cashier
{
    public partial class CashierManagerGUI : Form
    {
        public CashierManagerGUI()
        {
            InitializeComponent();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Populate the detail fields from the selected row
                    UserIDtextBox.Text = row.Cells["UserID"].Value?.ToString() ?? string.Empty;
                    FullNametextBox.Text = row.Cells["Name"].Value?.ToString() ?? string.Empty;
                    PhoneNumbertextBox.Text = row.Cells["Phone"].Value?.ToString() ?? string.Empty;
                    EmailtextBox.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
                    AddressrichTextBox.Text = row.Cells["Address"].Value?.ToString() ?? string.Empty;
                    CashierIDtextBox.Text = row.Cells["CashierID"].Value?.ToString() ?? string.Empty;
                    CashierTypecomboBox.SelectedItem = row.Cells["CashierType"].Value?.ToString() ?? string.Empty;
                    ShiftcomboBox.SelectedItem = row.Cells["Shift"].Value?.ToString() ?? string.Empty;

                    // Handle IsActive checkbox
                    bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value ?? false);
                    IsActivecheckBox.Checked = isActive;

                    // Handle permissions checkboxes
                    bool canProcessSale = Convert.ToBoolean(row.Cells["CanProcessSale"].Value ?? false);
                    bool canHandleReturn = Convert.ToBoolean(row.Cells["CanHandleReturn"].Value ?? false);
                    CanProcessSalecheckBox.Checked = canProcessSale;
                    CanHandleReturncheckBox.Checked = canHandleReturn;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cashier details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Showbutton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, c.CashierID, c.CashierType, c.Shift, c.CanProcessSale, c.CanHandleReturn FROM [USER] u INNER JOIN [CASHIER] c ON u.UserID = c.UserID WHERE u.UserType = 'Cashier'";
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

        private void SearchCashierbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = SearchCashiertextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Please enter Cashier ID or Phone Number to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, c.CashierID, c.CashierType, c.Shift, c.CanProcessSale, c.CanHandleReturn FROM [USER] u INNER JOIN [CASHIER] c ON u.UserID = c.UserID WHERE u.UserType = 'Cashier' AND (c.CashierID = '" + searchValue.Replace("'", "''") + "' OR u.Phone = '" + searchValue.Replace("'", "''") + "')";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No cashier found with the given Cashier ID or Phone Number.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Updatebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select a cashier from the list to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (!ValidateUpdateFields())
                {
                    return;
                }

                // Confirm update
                DialogResult result = MessageBox.Show("Are you sure you want to update this cashier?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update database
                if (UpdateCashierInDatabase())
                {
                    MessageBox.Show("Cashier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCashierList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating cashier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select a cashier from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this cashier? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete from database
                if (DeleteCashierFromDatabase())
                {
                    MessageBox.Show("Cashier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCashierList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting cashier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdateCashierInDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Update USER table
                string userQuery = "UPDATE [USER] SET Name = '" + FullNametextBox.Text.Replace("'", "''") + "', Phone = '" + PhoneNumbertextBox.Text.Replace("'", "''") + "', " +
                    "Email = '" + EmailtextBox.Text.Replace("'", "''") + "', Address = '" + AddressrichTextBox.Text.Replace("'", "''") + "', IsActive = " + (IsActivecheckBox.Checked ? 1 : 0) + " " +
                    "WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                // Update CASHIER table
                string cashierQuery = "UPDATE [CASHIER] SET CashierType = '" + CashierTypecomboBox.SelectedItem?.ToString().Replace("'", "''") + "', Shift = '" + ShiftcomboBox.SelectedItem?.ToString().Replace("'", "''") + "', " +
                    "CanProcessSale = " + (CanProcessSalecheckBox.Checked ? 1 : 0) + ", CanHandleReturn = " + (CanHandleReturncheckBox.Checked ? 1 : 0) + " " +
                    "WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                SqlCommand cmd = new SqlCommand(userQuery + "; " + cashierQuery, conn);
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

        private bool DeleteCashierFromDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Delete from CASHIER table first (foreign key constraint)
                string cashierQuery = "DELETE FROM [CASHIER] WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                // Delete from USER table
                string userQuery = "DELETE FROM [USER] WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                SqlCommand cmd = new SqlCommand(cashierQuery + "; " + userQuery, conn);
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
            // Validate Full Name
            if (string.IsNullOrWhiteSpace(FullNametextBox.Text))
            {
                MessageBox.Show("Full Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FullNametextBox.Focus();
                return false;
            }

            // Validate Phone Number
            if (string.IsNullOrWhiteSpace(PhoneNumbertextBox.Text))
            {
                MessageBox.Show("Phone Number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PhoneNumbertextBox.Focus();
                return false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(EmailtextBox.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EmailtextBox.Focus();
                return false;
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(AddressrichTextBox.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AddressrichTextBox.Focus();
                return false;
            }

            // Validate Cashier Type
            if (CashierTypecomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Cashier Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CashierTypecomboBox.Focus();
                return false;
            }

            // Validate Shift
            if (ShiftcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Shift.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ShiftcomboBox.Focus();
                return false;
            }

            return true;
        }

        private void RefreshCashierList()
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, " +
                    "c.CashierID, c.CashierType, c.Shift, c.CanProcessSale, c.CanHandleReturn FROM [USER] u INNER JOIN [CASHIER] c ON u.UserID = c.UserID WHERE u.UserType = 'Cashier'";
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

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void Updatebutton_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select a cashier from the list to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (!ValidateUpdateFields())
                {
                    return;
                }

                // Confirm update
                DialogResult result = MessageBox.Show("Are you sure you want to update this cashier?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update database
                if (UpdateCashierInDatabase())
                {
                    MessageBox.Show("Cashier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCashierList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating cashier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select a cashier from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this cashier? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete from database
                if (DeleteCashierFromDatabase())
                {
                    MessageBox.Show("Cashier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCashierList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting cashier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAllFields()
        {
            UserIDtextBox.Clear();
            FullNametextBox.Clear();
            PhoneNumbertextBox.Clear();
            EmailtextBox.Clear();
            AddressrichTextBox.Clear();
            CashierIDtextBox.Clear();
            CashierTypecomboBox.SelectedIndex = -1;
            ShiftcomboBox.SelectedIndex = -1;
            CanProcessSalecheckBox.Checked = false;
            CanHandleReturncheckBox.Checked = false;
            IsActivecheckBox.Checked = false;
        }

        private void Clearbutton_Click_1(object sender, EventArgs e)
        {
            ClearAllFields();
        }
    }
}
