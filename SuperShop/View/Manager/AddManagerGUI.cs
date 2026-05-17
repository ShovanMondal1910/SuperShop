using SuperShop.Models;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace SuperShop.View.Manager
{
    public partial class AddManagerGUI : Form
    {
        public AddManagerGUI()
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

                // Generate and display ManagerID
                string managerID = ManagerIDGenarator.GenerateManagerID();
                ManagerIDtextBox.Text = managerID;
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

            // Create Manager object from form data
            SuperShop.Models.Manager manager = new SuperShop.Models.Manager(
                UserIDtextBox.Text,
                UserType.Manager,
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
                ManagerIDtextBox.Text,
                comboBox1.SelectedItem?.ToString() ?? "",
                DepartmentcomboBox.SelectedItem?.ToString() ?? "",
                CanManageCashiercheckBox.Checked,
                CanManageProductcheckBox.Checked
            );

            // Save to database
            if (AddManagerToDatabase(manager))
            {
                MessageBox.Show("Manager added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
                this.Close();
            }
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
            ManagerIDtextBox.Clear();
            DepartmentcomboBox.SelectedIndex = -1;
            MaleradioButton.Checked = false;
            FemaleradioButton.Checked = false;
            SecurityQuestioncomboBox.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            UserTypecomboBox.SelectedIndex = -1;
            IsActivecheckBox.Checked = false;
            CanManageCashiercheckBox.Checked = false;
            CanManageProductcheckBox.Checked = false;
        }

        private bool AddManagerToDatabase(SuperShop.Models.Manager manager)
        {
            // Use From Controller/DatabaseConnection.cs
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();
                // Insert into USER table first
                string userQuery = "INSERT INTO [USER] (UserID, UserType, Name, DateOfBirth, Gender, Phone, Email, Address, Username, Password, SecurityQuestion, SecurityAnswer, IsActive) " +
                    "VALUES ('" + manager.UserID + "', 'Manager', '" + manager.Name + "', '" + manager.DateOfBirth.ToString("dd/MM/yyyy") + "', '" + manager.Gender + "', '" + manager.Phone + "', '" + manager.Email + "', '" + manager.Address + "', '" + manager.Username + "', '" + manager.Password + "', '" + manager.SecurityQuestion + "', '" + manager.SecurityAnswer + "', " + (manager.IsActive ? 1 : 0) + ")";

                // Insert into MANAGER table
                string managerQuery = "INSERT INTO [MANAGER] (ManagerID, ManagerType, UserID, Department, CanManageCashier, CanManageProduct) " +
                    "VALUES ('" + manager.ManagerID + "', '" + manager.ManagerType + "', '" + manager.UserID + "', '" + manager.Department + "', " + (manager.CanManageCashier ? 1 : 0) + ", " + (manager.CanManageProduct ? 1 : 0) + ")";

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

            // Validate Manager ID
            if (string.IsNullOrWhiteSpace(ManagerIDtextBox.Text))
            {
                MessageBox.Show("Manager ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ManagerIDtextBox.Focus();
                return false;
            }

            // Validate Manager Type
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Manager Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return false;
            }

            // Validate Department
            if (DepartmentcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Department is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DepartmentcomboBox.Focus();
                return false;
            }

            return true;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }
    }
}
