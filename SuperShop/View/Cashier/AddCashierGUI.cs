using SuperShop.Models;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace SuperShop.View.Cashier
{
    public partial class AddCashierGUI : Form
    {
        public AddCashierGUI()
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

                // Generate and display CashierID
                string cashierID = CashierIDGenarator.GenerateCashierID();
                CashierIDtextBox.Text = cashierID;
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

            // Create Cashier object from form data
            SuperShop.Models.Cashier cashier = new SuperShop.Models.Cashier(
                UserIDtextBox.Text,
                UserType.Cashier,
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
                CashierIDtextBox.Text,
                CashierTypecomboBox.SelectedItem?.ToString() ?? "",
                ShiftcomboBox.SelectedItem?.ToString() ?? "",
                CanProcessSalecheckBox.Checked,
                CanHandleReturncheckBox.Checked
            );

            // Save to database
            if (AddCashierToDatabase(cashier))
            {
                MessageBox.Show("Cashier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            CashierIDtextBox.Clear();
            MaleradioButton.Checked = false;
            FemaleradioButton.Checked = false;
            SecurityQuestioncomboBox.SelectedIndex = -1;
            CashierTypecomboBox.SelectedIndex = -1;
            ShiftcomboBox.SelectedIndex = -1;
            IsActivecheckBox.Checked = false;
            CanProcessSalecheckBox.Checked = false;
            CanHandleReturncheckBox.Checked = false;
        }

        private bool AddCashierToDatabase(SuperShop.Models.Cashier cashier)
        {
            // Use From Controller/DatabaseConnection.cs
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();
                // Insert into USER table first
                string userQuery = "INSERT INTO [USER] (UserID, UserType, Name, DateOfBirth, Gender, Phone, Email, Address, Username, Password, SecurityQuestion, SecurityAnswer, IsActive) " +
                    "VALUES ('" + cashier.UserID + "', 'Cashier', '" + cashier.Name + "', '" + cashier.DateOfBirth.ToString("dd/MM/yyyy") + "', '" + cashier.Gender + "', '" + cashier.Phone + "', '" + cashier.Email + "', '" + cashier.Address + "', '" + cashier.Username + "', '" + cashier.Password + "', '" + cashier.SecurityQuestion + "', '" + cashier.SecurityAnswer + "', " + (cashier.IsActive ? 1 : 0) + ")";

                // Insert into CASHIER table
                string cashierQuery = "INSERT INTO [CASHIER] (CashierID, CashierType, UserID, Shift, CanProcessSale, CanHandleReturn) " +
                    "VALUES ('" + cashier.CashierID + "', '" + cashier.CashierType + "', '" + cashier.UserID + "', '" + cashier.Shift + "', " + (cashier.CanProcessSale ? 1 : 0) + ", " + (cashier.CanHandleReturn ? 1 : 0) + ")";

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

            // Validate Cashier ID
            if (string.IsNullOrWhiteSpace(CashierIDtextBox.Text))
            {
                MessageBox.Show("Cashier ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CashierIDtextBox.Focus();
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
