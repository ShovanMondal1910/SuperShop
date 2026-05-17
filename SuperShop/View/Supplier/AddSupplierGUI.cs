using SuperShop.Models;
using SuperShop.Controller;
using SuperShop.IDGenarator;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace SuperShop.View.Supplier
{
    public partial class AddSupplierGUI : Form
    {
        public AddSupplierGUI()
        {
            InitializeComponent();
            GenerateIDs();
        }

        private void GenerateIDs()
        {
            try
            {
                // Generate and display SupplierID
                string supplierID = SupplierIDGenarator.GenerateSupplierID();
                SupplierIDtextBox.Text = supplierID;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (!ValidateAllFields())
            {
                return;
            }

            // Create Supplier object from form data
            SuperShop.Models.Supplier supplier = new SuperShop.Models.Supplier(
                SupplierIDtextBox.Text,
                SupplierNametextBox.Text,
                PhoneNumbertextBox.Text,
                EmailtextBox.Text,
                AddressrichTextBox.Text,
                CitytextBox.Text,
                CountrytextBox.Text,
                CompanyNametextBox.Text,
                TaxIDtextBox.Text,
                int.Parse(PaymentTermstextBox.Text),
                IsActivecheckBox.Checked
            );

            // Save to database
            if (AddSupplierToDatabase(supplier))
            {
                MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
                this.Close();
            }
        }

        private void ClearAllFields()
        {
            SupplierIDtextBox.Clear();
            SupplierNametextBox.Clear();
            PhoneNumbertextBox.Clear();
            EmailtextBox.Clear();
            AddressrichTextBox.Clear();
            CitytextBox.Clear();
            CountrytextBox.Clear();
            CompanyNametextBox.Clear();
            TaxIDtextBox.Clear();
            PaymentTermstextBox.Clear();
            IsActivecheckBox.Checked = false;
        }

        private bool AddSupplierToDatabase(SuperShop.Models.Supplier supplier)
        {
            // Use From Controller/DatabaseConnection.cs
            SqlConnection conn = DatabaseConnection.GetConnection();
            try
            {
                conn.Open();
                // Insert into SUPPLIER table
                string supplierQuery = "INSERT INTO [SUPPLIER] (SupplierID, SupplierName, PhoneNumber, Email, Address, City, Country, CompanyName, TaxID, PaymentTerms, IsActive) " +
                    "VALUES ('" + supplier.SupplierID + "', '" + supplier.SupplierName + "', '" + supplier.PhoneNumber + "', '" + supplier.Email + "', '" + supplier.Address + "', '" + supplier.City + "', '" + supplier.Country + "', '" + supplier.CompanyName + "', '" + supplier.TaxID + "', " + supplier.PaymentTerms + ", " + (supplier.IsActive ? 1 : 0) + ")";

                SqlCommand cmd = new SqlCommand(supplierQuery, conn);
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
            // Validate Supplier ID
            if (string.IsNullOrWhiteSpace(SupplierIDtextBox.Text))
            {
                MessageBox.Show("Supplier ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SupplierIDtextBox.Focus();
                return false;
            }

            // Validate Supplier Name
            if (string.IsNullOrWhiteSpace(SupplierNametextBox.Text))
            {
                MessageBox.Show("Supplier Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SupplierNametextBox.Focus();
                return false;
            }

            // Validate Phone Number
            if (string.IsNullOrWhiteSpace(PhoneNumbertextBox.Text))
            {
                MessageBox.Show("Phone Number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PhoneNumbertextBox.Focus();
                return false;
            }

            // Validate Phone Number format (digits only, 10-15 characters)
            if (!System.Text.RegularExpressions.Regex.IsMatch(PhoneNumbertextBox.Text, @"^\d{10,11}$"))
            {
                MessageBox.Show("Phone Number must contain 10-11 digits only.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Validate Email format
            try
            {
                var addr = new System.Net.Mail.MailAddress(EmailtextBox.Text);
                if (addr.Address != EmailtextBox.Text)
                {
                    throw new FormatException();
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid Email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Validate City
            if (string.IsNullOrWhiteSpace(CitytextBox.Text))
            {
                MessageBox.Show("City is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CitytextBox.Focus();
                return false;
            }

            // Validate Country
            if (string.IsNullOrWhiteSpace(CountrytextBox.Text))
            {
                MessageBox.Show("Country is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CountrytextBox.Focus();
                return false;
            }

            // Validate Company Name
            if (string.IsNullOrWhiteSpace(CompanyNametextBox.Text))
            {
                MessageBox.Show("Company Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CompanyNametextBox.Focus();
                return false;
            }

            // Validate Tax ID
            if (string.IsNullOrWhiteSpace(TaxIDtextBox.Text))
            {
                MessageBox.Show("Tax ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaxIDtextBox.Focus();
                return false;
            }

            // Validate Payment Terms
            if (string.IsNullOrWhiteSpace(PaymentTermstextBox.Text))
            {
                MessageBox.Show("Payment Terms is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PaymentTermstextBox.Focus();
                return false;
            }

            // Validate Payment Terms is numeric
            if (!int.TryParse(PaymentTermstextBox.Text, out _))
            {
                MessageBox.Show("Payment Terms must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PaymentTermstextBox.Focus();
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
