using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Supplier
{
    public partial class SupplierManagerGUI : Form
    {
        public SupplierManagerGUI()
        {
            InitializeComponent();
        }

        private void Showbutton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT SupplierID, SupplierName, PhoneNumber, Email, Address, City, Country, CompanyName, TaxID, PaymentTerms, IsActive FROM [SUPPLIER]";
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

        private void SearchSupplierbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = SearchSuppliertextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    MessageBox.Show("Please enter Supplier ID or Phone Number to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT SupplierID, SupplierName, PhoneNumber, Email, Address, City, Country, CompanyName, TaxID, PaymentTerms, IsActive FROM [SUPPLIER] WHERE SupplierID = '" + searchValue + "' OR PhoneNumber = '" + searchValue + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No supplier found with the given Supplier ID or Phone Number.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (string.IsNullOrWhiteSpace(SupplierIDtextBox.Text))
                {
                    MessageBox.Show("Please select a supplier to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection conn = DatabaseConnection.GetConnection();
                conn.Open();

                string updateQuery = "UPDATE [SUPPLIER] SET SupplierName = '" + SupplierNametextBox.Text + "', PhoneNumber = '" + PhoneNumbertextBox.Text + "', Email = '" + EmailtextBox.Text + "', Address = '" + AddressrichTextBox.Text + "', City = '" + CitytextBox.Text + "', Country = '" + CountrytextBox.Text + "', CompanyName = '" + CompanyNametextBox.Text + "', TaxID = '" + TaxIDtextBox.Text + "', PaymentTerms = " + PaymentTermstextBox.Text + ", IsActive = " + (IsActivecheckBox.Checked ? 1 : 0) + " WHERE SupplierID = '" + SupplierIDtextBox.Text + "'";

                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
                RefreshSupplierList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SupplierIDtextBox.Text))
                {
                    MessageBox.Show("Please select a supplier to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                SqlConnection conn = DatabaseConnection.GetConnection();
                conn.Open();

                string deleteQuery = "DELETE FROM [SUPPLIER] WHERE SupplierID = '" + SupplierIDtextBox.Text + "'";
                SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAllFields();
                RefreshSupplierList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            ClearAllFields();
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

        private void RefreshSupplierList()
        {
            try
            {
                SqlConnection conn = DatabaseConnection.GetConnection();
                string query = "SELECT SupplierID, SupplierName, PhoneNumber, Email, Address, City, Country, CompanyName, TaxID, PaymentTerms, IsActive FROM [SUPPLIER]";
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    SupplierIDtextBox.Text = row.Cells["SupplierID"].Value?.ToString() ?? "";
                    SupplierNametextBox.Text = row.Cells["SupplierName"].Value?.ToString() ?? "";
                    PhoneNumbertextBox.Text = row.Cells["PhoneNumber"].Value?.ToString() ?? "";
                    EmailtextBox.Text = row.Cells["Email"].Value?.ToString() ?? "";
                    AddressrichTextBox.Text = row.Cells["Address"].Value?.ToString() ?? "";
                    CitytextBox.Text = row.Cells["City"].Value?.ToString() ?? "";
                    CountrytextBox.Text = row.Cells["Country"].Value?.ToString() ?? "";
                    CompanyNametextBox.Text = row.Cells["CompanyName"].Value?.ToString() ?? "";
                    TaxIDtextBox.Text = row.Cells["TaxID"].Value?.ToString() ?? "";
                    PaymentTermstextBox.Text = row.Cells["PaymentTerms"].Value?.ToString() ?? "";
                    IsActivecheckBox.Checked = (bool)(row.Cells["IsActive"].Value ?? false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading row data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
