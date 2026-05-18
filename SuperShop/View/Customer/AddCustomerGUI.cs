using SuperShop.Models;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace SuperShop.View.Customer
{
    public partial class AddCustomerGUI : Form
    {
        public AddCustomerGUI()
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

                // Generate and display CustomerID
                string customerID = CustomerIDGenarator.GenerateCustomerID();
                CustomerIDtextBox.Text = customerID;
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

            // Create Customer object from form data
            SuperShop.Models.Customer customer = new SuperShop.Models.Customer(
                UserIDtextBox.Text,
                UserType.Customer,
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
                CustomerIDtextBox.Text,
                CustomerTypecomboBox.SelectedItem?.ToString() ?? "",
                0.00m,
                IsVIPcheckBox.Checked,
                0.00m
            );

            // Save to database
            if (AddCustomerToDatabase(customer))
            {
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
                this.Close();
            }
        }

        

        private bool AddCustomerToDatabase(SuperShop.Models.Customer customer)
        {
            // Use From Controller/DatabaseConnection.cs
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();
                // Insert into USER table first
                string userQuery = "INSERT INTO [USER] (UserID, UserType, Name, DateOfBirth, Gender, Phone, Email, Address, Username, Password, SecurityQuestion, SecurityAnswer, IsActive) " +
                    "VALUES ('" + customer.UserID.Replace("'", "''") + "', 'Customer', '" + customer.Name.Replace("'", "''") + "', '" + customer.DateOfBirth.ToString("yyyy/MM/dd") + "', '" +
                    customer.Gender.Replace("'", "''") + "', '" + customer.Phone.Replace("'", "''") + "', '" + customer.Email.Replace("'", "''") + "', '" + customer.Address.Replace("'", "''") + "', '" + customer.Username.Replace("'", "''") + "', '" + customer.Password.Replace("'", "''") + "', '" + customer.SecurityQuestion.Replace("'", "''") + "', '" + customer.SecurityAnswer.Replace("'", "''") + "', " + (customer.IsActive ? 1 : 0) + ")";

                // Insert into CUSTOMER table
                string customerQuery = "INSERT INTO [CUSTOMER] (CustomerID, CustomerType, UserID, TotalPurchase, IsVIP, LoyaltyPoints) " +
                    "VALUES ('" + customer.CustomerID.Replace("'", "''") + "', '" + customer.CustomerType.Replace("'", "''") + "', '" + customer.UserID.Replace("'", "''") + "', " + customer.TotalPurchase + ", " + (customer.IsVIP ? 1 : 0) + ", " + customer.LoyaltyPoints + ")";

                SqlCommand cmd = new SqlCommand(userQuery + "; " + customerQuery, conn);
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

            // Validate Customer ID
            if (string.IsNullOrWhiteSpace(CustomerIDtextBox.Text))
            {
                MessageBox.Show("Customer ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerIDtextBox.Focus();
                return false;
            }

            // Validate Customer Type
            if (CustomerTypecomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Customer Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerTypecomboBox.Focus();
                return false;
            }

            return true;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearAllFields()
        {
            FullNametextBox.Clear();
            PhoneNumbertextBox.Clear();
            EmailtextBox.Clear();
            AddressrichTextBox.Clear();
            UsernametextBox.Clear();
            PasswordtextBox.Clear();
            DateOfBirthdatePicker.Value = DateTime.Now;
            SecurityAnswertextBox.Clear();
            CustomerIDtextBox.Clear();
            TotalPurchasetextBox.Clear();
            LoyaltyPointstextBox.Clear();
            MaleradioButton.Checked = false;
            FemaleradioButton.Checked = false;
            SecurityQuestioncomboBox.SelectedIndex = -1;
            CustomerTypecomboBox.SelectedIndex = -1;
            UserTypecomboBox.SelectedIndex = -1;
            IsActivecheckBox.Checked = false;
            IsVIPcheckBox.Checked = false;
        }
        private void Clearbutton_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }
    }
}
