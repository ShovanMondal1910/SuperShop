using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using SuperShop.Controller;

namespace SuperShop.View.Auth
{
    public partial class ForgetPasswordGUI : Form
    {
        private string? _userId;
        private bool _isValidated = false;

        public ForgetPasswordGUI()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            Validatebutton.Click += Validatebutton_Click;
            ChangePasswordbutton.Click += ChangePasswordbutton_Click;
            SetPasswordFieldsState(false);
        }

        private void SetPasswordFieldsState(bool enabled)
        {
            NewPasswordtextBox.Enabled = enabled;
            ConfirmPasswordtextBox.Enabled = enabled;
            ChangePasswordbutton.Enabled = enabled;
        }

        private void Validatebutton_Click(object? sender, EventArgs e)
        {
            if (ValidateSecurityQuestion())
            {
                _isValidated = true;
                SetPasswordFieldsState(true);
                MessageBox.Show("Security question validated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ValidateSecurityQuestion()
        {
            string username = UsernametextBox.Text.Trim();
            string adminId = AdminIDtextBox.Text.Trim();
            string selectedQuestion = SecurityQuestioncomboBox.SelectedItem?.ToString() ?? "";
            string securityAnswer = SecurityAnswertextBox.Text.Trim();

            if (!ValidateInputs(username, adminId, selectedQuestion, securityAnswer))
                return false;

            try
            {
                using SqlConnection connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = @"SELECT UserID FROM [User] 
                               WHERE Username = @Username 
                               AND UserID = @AdminId 
                               AND SecurityQuestion = @SecurityQuestion 
                               AND LOWER(SecurityAnswer) = LOWER(@SecurityAnswer)";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@AdminId", adminId);
                command.Parameters.AddWithValue("@SecurityQuestion", selectedQuestion);
                command.Parameters.AddWithValue("@SecurityAnswer", securityAnswer);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    _userId = reader["UserID"].ToString();
                    return true;
                }

                MessageBox.Show("Invalid username, Admin ID, or security answer.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool ValidateInputs(string username, string adminId, string question, string answer)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(adminId))
            {
                MessageBox.Show("Please enter Admin ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(question))
            {
                MessageBox.Show("Please select a security question.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(answer))
            {
                MessageBox.Show("Please enter security answer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ChangePasswordbutton_Click(object? sender, EventArgs e)
        {
            if (!_isValidated)
            {
                MessageBox.Show("Please validate your security question first.", "Validation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidatePasswordFields())
                UpdatePassword();
        }

        private bool ValidatePasswordFields()
        {
            string newPassword = NewPasswordtextBox.Text;
            string confirmPassword = ConfirmPasswordtextBox.Text;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter a new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (newPassword.Length > 100)
            {
                MessageBox.Show("Password must not exceed 100 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UpdatePassword()
        {
            try
            {
                using SqlConnection connection = DatabaseConnection.GetConnection();
                connection.Open();

                string query = "UPDATE [User] SET Password = @Password WHERE UserID = @UserId";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Password", NewPasswordtextBox.Text);
                command.Parameters.AddWithValue("@UserId", _userId);

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Failed to update password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            UsernametextBox.Clear();
            AdminIDtextBox.Clear();
            SecurityQuestioncomboBox.SelectedIndex = -1;
            SecurityAnswertextBox.Clear();
            NewPasswordtextBox.Clear();
            ConfirmPasswordtextBox.Clear();
            _isValidated = false;
            SetPasswordFieldsState(false);
        }
    }
}
