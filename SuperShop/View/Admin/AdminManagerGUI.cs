using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Admin
{
    public partial class AdminManagerGUI : Form
    {
        public AdminManagerGUI()
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
                    AdminIDtextBox.Text = row.Cells["AdminID"].Value?.ToString() ?? string.Empty;
                    comboBox1.SelectedItem = row.Cells["AdminType"].Value?.ToString() ?? string.Empty;

                    // Handle IsActive checkbox
                    bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value ?? false);
                    IsActivecheckBox.Checked = isActive;

                    // Handle Degree checkboxes
                    string degree = row.Cells["Degree"].Value?.ToString() ?? string.Empty;
                    BCScheckBox.Checked = degree.Contains("BSC");
                    MSCcheckBox.Checked = degree.Contains("MSC");
                    PhDcheckBox.Checked = degree.Contains("PhD");

                    // Handle permissions checkboxes
                    bool canManageAdmin = Convert.ToBoolean(row.Cells["CanManageAdmin"].Value ?? false);
                    bool canManageCustomer = Convert.ToBoolean(row.Cells["CanManageCustomer"].Value ?? false);
                    CanManageAdmincheckBox.Checked = canManageAdmin;
                    CanManageManagercheckBox.Checked = canManageCustomer;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading admin details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Showbutton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, " +
                    "a.AdminID, a.AdminType, a.Degree, a.CanManageAdmin, a.CanManageCustomer FROM [USER] u INNER JOIN [ADMIN] a ON u.UserID = a.UserID WHERE u.UserType = 'Admin'";
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

        private void SearchAdminbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = SearchAdmintextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Please enter Admin ID or Phone Number to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, a.AdminID, a.AdminType, a.Degree, a.CanManageAdmin, a.CanManageCustomer FROM [USER] u INNER JOIN [ADMIN] a ON u.UserID = a.UserID WHERE u.UserType = 'Admin' AND (a.AdminID = '" + searchValue + "' OR u.Phone = '" + searchValue + "')";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No admin found with the given Admin ID or Phone Number.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Please select an admin from the list to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (!ValidateUpdateFields())
                {
                    return;
                }

                // Confirm update
                DialogResult result = MessageBox.Show("Are you sure you want to update this admin?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Update database
                if (UpdateAdminInDatabase())
                {
                    MessageBox.Show("Admin updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAdminList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating admin: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that a row is selected
                if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
                {
                    MessageBox.Show("Please select an admin from the list to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this admin? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete from database
                if (DeleteAdminFromDatabase())
                {
                    MessageBox.Show("Admin deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAdminList();
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting admin: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdateAdminInDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Get selected degrees
                string degree = GetSelectedDegrees();

                // Update USER table
                string userQuery = "UPDATE [USER] SET Name = '" + FullNametextBox.Text + "', Phone = '" + PhoneNumbertextBox.Text + "', " +
                    "Email = '" + EmailtextBox.Text + "', Address = '" + AddressrichTextBox.Text + "', IsActive = " + (IsActivecheckBox.Checked ? 1 : 0) + " " +
                    "WHERE UserID = '" + UserIDtextBox.Text + "'";

                // Update ADMIN table
                string adminQuery = "UPDATE [ADMIN] SET AdminType = '" + comboBox1.SelectedItem?.ToString() + "', Degree = '" + degree + "', " +
                    "CanManageAdmin = " + (CanManageAdmincheckBox.Checked ? 1 : 0) + ", CanManageCustomer = " + (CanManageManagercheckBox.Checked ? 1 : 0) + " " +
                    "WHERE UserID = '" + UserIDtextBox.Text + "'";

                SqlCommand cmd = new SqlCommand(userQuery + "; " + adminQuery, conn);
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
        private string GetSelectedDegrees()
        {
            List<string> degrees = new List<string>();
            if (BCScheckBox.Checked) degrees.Add("BSC");
            if (MSCcheckBox.Checked) degrees.Add("MSC");
            if (PhDcheckBox.Checked) degrees.Add("PhD");
            return string.Join(", ", degrees);
        }
        private bool DeleteAdminFromDatabase()
        {
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();

                // Delete from ADMIN table first (foreign key constraint)
                string adminQuery = "DELETE FROM [ADMIN] WHERE UserID = '" + UserIDtextBox.Text + "'";

                // Delete from USER table
                string userQuery = "DELETE FROM [USER] WHERE UserID = '" + UserIDtextBox.Text + "'";

                SqlCommand cmd = new SqlCommand(adminQuery + "; " + userQuery, conn);
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

            // Validate Admin Type
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Admin Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return false;
            }

            return true;
        }

        

        private void RefreshAdminList()
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT u.UserID, u.Name, u.DateOfBirth, u.Gender, u.Phone, u.Email, u.Address, u.IsActive, " +
                    "a.AdminID, a.AdminType, a.Degree, a.CanManageAdmin, a.CanManageCustomer FROM [USER] u INNER JOIN [ADMIN] a ON u.UserID = a.UserID WHERE u.UserType = 'Admin'";
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
            AdminIDtextBox.Clear();
            comboBox1.SelectedIndex = -1;
            IsActivecheckBox.Checked = false;
            BCScheckBox.Checked = false;
            MSCcheckBox.Checked = false;
            PhDcheckBox.Checked = false;
            CanManageAdmincheckBox.Checked = false;
            CanManageManagercheckBox.Checked = false;
        }

        private void Clearbutton_Click_1(object sender, EventArgs e)
        {
            ClearAllFields();
        }
    }
}
