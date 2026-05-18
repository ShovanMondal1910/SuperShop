using SuperShop.Models;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace SuperShop.View.Admin
{
    public partial class AddAdminGUI : Form
    {
        public AddAdminGUI()
        {
            InitializeComponent();
            GenerateIDs();
        }

        private void GenerateIDs()
        {
            try
            {
                // Generate and display UserID
                string userID = UserIDGenarator.GenerateUserID();
                UserIDtextBox.Text = userID;

                // Generate and display AdminID
                string adminID = AdminIDGenarator.GenerateAdminID();
                AdminIDtextBox.Text = adminID;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating IDs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (!ValidateAllFields())
            {
                return;
            }


            // Create Admin object from form data
            SuperShop.Models.Admin admin = new SuperShop.Models.Admin(
                UserIDtextBox.Text,
                UserType.Admin,
                FullNametextBox.Text,
                DateOfBirthdatePicker.Value,
                MaleradioButton.Checked ? "Male" : "Female",
                PhoneNumbertextBox.Text,
                EmailtextBox.Text,
                AddressrichTextBox.Text,
                UsernametextBox.Text,
                PasswordtextBox.Text,
                SecurityQuestioncomboBox.SelectedItem?.ToString() ?? "",
                SecurityAnswertextBox.Text,
                IsActivecheckBox.Checked,
                AdminIDtextBox.Text,
                comboBox1.SelectedItem?.ToString() ?? "",
                GetSelectedDegrees(),
                CanManageAdmincheckBox.Checked,
                CanManageManagercheckBox.Checked
            );

            // Save to database
            if (AddAdminToDatabase(admin))
            {
                MessageBox.Show("Admin added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
                this.Close();
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

        

        private bool AddAdminToDatabase(SuperShop.Models.Admin admin)
        {
            // Use From Controller/DatabaseConnection.cs
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();
                // Insert into USER table first
                string userQuery = "INSERT INTO [USER] (UserID, UserType, Name, DateOfBirth, Gender, Phone, Email, Address, Username, Password, SecurityQuestion, SecurityAnswer, IsActive) " +
                    "VALUES ('" + admin.UserID + "', 'Admin', '" + admin.Name + "', '" + admin.DateOfBirth.ToString("yyyy/MM/dd") + "', '" + admin.Gender + "', '" + admin.Phone + "', '" +
                    admin.Email + "', '" + admin.Address + "', '" + admin.Username + "', '" + admin.Password + "', '" + admin.SecurityQuestion + "', '" + admin.SecurityAnswer + "', " + (admin.IsActive ? 1 : 0) + ")";

                // Insert into ADMIN table
                string adminQuery = "INSERT INTO [ADMIN] (AdminID, AdminType, UserID, Degree, CanManageAdmin, CanManageCustomer) " +
                    "VALUES ('" + admin.AdminID + "', '" + admin.AdminType + "', '" + admin.UserID + "', '" + admin.Degree + "', " + (admin.CanManageAdmin ? 1 : 0) + ", " + (admin.CanManageCustomer ? 1 : 0) + ")";

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

        private bool ValidateAllFields()
        {
            // Validate User ID
            if (string.IsNullOrWhiteSpace(UserIDtextBox.Text))
            {
                MessageBox.Show("User ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UserIDtextBox.Focus();
                return false;
            }

            // Validate Full Name
            if (string.IsNullOrWhiteSpace(FullNametextBox.Text))
            {
                MessageBox.Show("Full Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FullNametextBox.Focus();
                return false;
            }

            // Validate Gender selection
            if (!MaleradioButton.Checked && !FemaleradioButton.Checked)
            {
                MessageBox.Show("Please select a Gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Validate Username
            if (string.IsNullOrWhiteSpace(UsernametextBox.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UsernametextBox.Focus();
                return false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(PasswordtextBox.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PasswordtextBox.Focus();
                return false;
            }

            // Validate Security Question
            if (SecurityQuestioncomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Security Question.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SecurityQuestioncomboBox.Focus();
                return false;
            }

            // Validate Security Answer
            if (string.IsNullOrWhiteSpace(SecurityAnswertextBox.Text))
            {
                MessageBox.Show("Security Answer is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SecurityAnswertextBox.Focus();
                return false;
            }

            // Validate Admin ID
            if (string.IsNullOrWhiteSpace(AdminIDtextBox.Text))
            {
                MessageBox.Show("Admin ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AdminIDtextBox.Focus();
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
        // Cancel button event handler to close the form
        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ClearAllFields()
        {
            UserIDtextBox.Clear();
            FullNametextBox.Clear();
            PhoneNumbertextBox.Clear();
            EmailtextBox.Clear();
            AddressrichTextBox.Clear();
            UsernametextBox.Clear();
            PasswordtextBox.Clear();
            SecurityAnswertextBox.Clear();
            AdminIDtextBox.Clear();
            MaleradioButton.Checked = false;
            FemaleradioButton.Checked = false;
            SecurityQuestioncomboBox.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            AdminTypecomboBox.SelectedIndex = -1;
            IsActivecheckBox.Checked = false;
            BCScheckBox.Checked = false;
            MSCcheckBox.Checked = false;
            PhDcheckBox.Checked = false;
            CanManageAdmincheckBox.Checked = false;
            CanManageManagercheckBox.Checked = false;
            IsActivecheckBox.Checked = false;
        }
        // Clear button event handler to clear all fields
        private void Clearbutton_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }
    }
}
