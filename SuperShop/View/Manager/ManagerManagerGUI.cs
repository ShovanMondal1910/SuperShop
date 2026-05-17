using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Manager
{
    public partial class ManagerManagerGUI : Form
    {
        public ManagerManagerGUI()
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
                    ManagerIDtextBox.Text = row.Cells["ManagerID"].Value?.ToString() ?? string.Empty;
                    ManagerTypecomboBox.SelectedItem = row.Cells["ManagerType"].Value?.ToString() ?? string.Empty;
                    DepartmentcomboBox.SelectedItem = row.Cells["Department"].Value?.ToString() ?? string.Empty;

                    // Handle IsActive checkbox
                    bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value ?? false);
                    IsActivecheckBox.Checked = isActive;

                    // Handle permissions checkboxes
                    bool canManageCashier = Convert.ToBoolean(row.Cells["CanManageCashier"].Value ?? false);
                    bool canManageProduct = Convert.ToBoolean(row.Cells["CanManageProduct"].Value ?? false);
                    CanManageCashiercheckBox.Checked = canManageCashier;
                    CanManageProductcheckBox.Checked = canManageProduct;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading manager details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Showbutton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, m.ManagerID, m.ManagerType, m.Department, m.CanManageCashier, m.CanManageProduct FROM [USER] u INNER JOIN [MANAGER] m ON u.UserID = m.UserID WHERE u.UserType = 'Manager'";
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

        private void SearchManagerbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = SearchManagertextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Please enter Manager ID or Phone Number to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, m.ManagerID, m.ManagerType, m.Department, m.CanManageCashier, m.CanManageProduct FROM [USER] u INNER JOIN [MANAGER] m ON u.UserID = m.UserID WHERE u.UserType = 'Manager' AND (m.ManagerID = '" + searchValue.Replace("'", "''") + "' OR u.Phone = '" + searchValue.Replace("'", "''") + "')";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No manager found with the given Manager ID or Phone Number.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select a manager from the list to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (!ValidateUpdateFields())
                {
                    return;
                }

                // Confirm update
                DialogResult result = MessageBox.Show("Are you sure you want to update this manager?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update database
                if (UpdateManagerInDatabase())
                {
                    MessageBox.Show("Manager updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshManagerList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating manager: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select a manager from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this manager? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete from database
                if (DeleteManagerFromDatabase())
                {
                    MessageBox.Show("Manager deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshManagerList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting manager: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdateManagerInDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Update USER table
                string userQuery = "UPDATE [USER] SET Name = '" + FullNametextBox.Text.Replace("'", "''") + "', Phone = '" + PhoneNumbertextBox.Text.Replace("'", "''") + "', " +
                    "Email = '" + EmailtextBox.Text.Replace("'", "''") + "', Address = '" + AddressrichTextBox.Text.Replace("'", "''") + "', IsActive = " + (IsActivecheckBox.Checked ? 1 : 0) + " " +
                    "WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                // Update MANAGER table
                string managerQuery = "UPDATE [MANAGER] SET ManagerType = '" + ManagerTypecomboBox.SelectedItem?.ToString().Replace("'", "''") + "', Department = '" + DepartmentcomboBox.SelectedItem?.ToString().Replace("'", "''") + "', " +
                    "CanManageCashier = " + (CanManageCashiercheckBox.Checked ? 1 : 0) + ", CanManageProduct = " + (CanManageProductcheckBox.Checked ? 1 : 0) + " " +
                    "WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                SqlCommand cmd = new SqlCommand(userQuery + "; " + managerQuery, conn);
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

        private bool DeleteManagerFromDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Delete from MANAGER table first (foreign key constraint)
                string managerQuery = "DELETE FROM [MANAGER] WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                // Delete from USER table
                string userQuery = "DELETE FROM [USER] WHERE UserID = '" + UserIDtextBox.Text.Replace("'", "''") + "'";

                SqlCommand cmd = new SqlCommand(managerQuery + "; " + userQuery, conn);
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

            return true;
        }

        private void RefreshManagerList()
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, " +
                    "m.ManagerID, m.ManagerType, m.Department, m.CanManageCashier, m.CanManageProduct FROM [USER] u INNER JOIN [MANAGER] m ON u.UserID = m.UserID WHERE u.UserType = 'Manager'";
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
            UserIDtextBox.Clear();
            FullNametextBox.Clear();
            PhoneNumbertextBox.Clear();
            EmailtextBox.Clear();
            AddressrichTextBox.Clear();
            ManagerIDtextBox.Clear();
            ManagerTypecomboBox.SelectedIndex = -1;
            DepartmentcomboBox.SelectedIndex = -1;
            CanManageCashiercheckBox.Checked = false;
            CanManageProductcheckBox.Checked = false;
            IsActivecheckBox.Checked = false;
        }
    }
}
