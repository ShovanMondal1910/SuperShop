using System;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SuperShop.Controller;
using SuperShop.View.Dashboard;

namespace SuperShop.View.Auth
{
    public partial class LogInGUI : Form
    {
        // Set to true by a dashboard's logout handler before it closes.
        // Prevents LogInGUI from closing itself after ShowDialog() returns.
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool LoggedOut { get; set; } = false;
        public LogInGUI()
        {
            InitializeComponent();
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            Loginbutton.Click += Loginbutton_Click;
            Cancelbutton.Click += Cancelbutton_Click;
            ShowPasswordcheckBox.CheckedChanged += ShowPasswordcheckBox_CheckedChanged;
            UsernametextBox.TextChanged += UsernametextBox_TextChanged;
            PasswordtextBox.TextChanged += PasswordtextBox_TextChanged;
        }

        private void Loginbutton_Click(object? sender, EventArgs e)
        {
            if (ValidateLoginForm())
            {
                AuthenticateUser();
            }
        }

        private void Cancelbutton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void ShowPasswordcheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            PasswordtextBox.PasswordChar = ShowPasswordcheckBox.Checked ? '\0' : '*';
        }

        private void UsernametextBox_TextChanged(object? sender, EventArgs e)
        {
            // Clear error message when user starts typing
            UserNameErrorlabel.Text = "";
            UserNameErrorlabel.ForeColor = Color.Red;
        }

        private void PasswordtextBox_TextChanged(object? sender, EventArgs e)
        {
            // Clear error message when user starts typing
            PasswordErrorlabel.Text = "";
            PasswordErrorlabel.ForeColor = Color.Red;
        }

        private bool ValidateLoginForm()
        {
            bool isValid = true;

            // Clear previous error messages
            UserNameErrorlabel.Text = "";
            PasswordErrorlabel.Text = "";

            // Validate Username
            if (string.IsNullOrWhiteSpace(UsernametextBox.Text))
            {
                UserNameErrorlabel.Text = "Username is required";
                UserNameErrorlabel.ForeColor = Color.Red;
                isValid = false;
            }
            else if (UsernametextBox.Text.Length < 3)
            {
                UserNameErrorlabel.Text = "Username must be at least 3 characters";
                UserNameErrorlabel.ForeColor = Color.Red;
                isValid = false;
            }
            else if (UsernametextBox.Text.Length > 50)
            {
                UserNameErrorlabel.Text = "Username must not exceed 50 characters";
                UserNameErrorlabel.ForeColor = Color.Red;
                isValid = false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(PasswordtextBox.Text))
            {
                PasswordErrorlabel.Text = "Password is required";
                PasswordErrorlabel.ForeColor = Color.Red;
                isValid = false;
            }
            else if (PasswordtextBox.Text.Length < 6)
            {
                PasswordErrorlabel.Text = "Password must be at least 6 characters";
                PasswordErrorlabel.ForeColor = Color.Red;
                isValid = false;
            }
            else if (PasswordtextBox.Text.Length > 100)
            {
                PasswordErrorlabel.Text = "Password must not exceed 100 characters";
                PasswordErrorlabel.ForeColor = Color.Red;
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show("Please correct the errors before logging in.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private void AuthenticateUser()
        {
            string usernameOrPhone = UsernametextBox.Text.Trim();
            string password = PasswordtextBox.Text;

            try
            {
                using (SqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    // Query to find user by username or phone number
                    string query = @"SELECT UserID, UserType, Password, IsActive, Name 
                                   FROM [User] 
                                   WHERE (Username = @UsernameOrPhone OR Phone = @UsernameOrPhone)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UsernameOrPhone", usernameOrPhone);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                object? storedPasswordObj = reader["Password"];
                                object? userTypeObj = reader["UserType"];
                                object? isActiveObj = reader["IsActive"];
                                object? userIdObj = reader["UserID"];
                                object? nameObj = reader["Name"];

                                if (storedPasswordObj == null || storedPasswordObj is DBNull ||
                                    userTypeObj == null || userTypeObj is DBNull ||
                                    isActiveObj == null || isActiveObj is DBNull ||
                                    userIdObj == null || userIdObj is DBNull ||
                                    nameObj == null || nameObj is DBNull)
                                {
                                    MessageBox.Show("Invalid user data in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string storedPassword = storedPasswordObj.ToString() ?? "";
                                int userType = ConvertUserType(userTypeObj.ToString() ?? "");
                                bool isActive = Convert.ToBoolean(isActiveObj);
                                string userId = userIdObj.ToString() ?? "";
                                string name = nameObj.ToString() ?? "";

                                // Check if user is active
                                if (!isActive)
                                {
                                    MessageBox.Show("Your account is inactive. Please contact the administrator.", "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                // Verify password
                                if (storedPassword == password)
                                {
                                    // Login successful - navigate to appropriate dashboard
                                    NavigateToDashboard(userType, userId, name);
                                }
                                else
                                {
                                    PasswordErrorlabel.Text = "Incorrect password";
                                    PasswordErrorlabel.ForeColor = Color.Red;
                                    MessageBox.Show("Invalid username/phone or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                UserNameErrorlabel.Text = "User not found";
                                UserNameErrorlabel.ForeColor = Color.Red;
                                MessageBox.Show("Invalid username/phone or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ConvertUserType(string userTypeValue)
        {
            // Handle both numeric and text values for UserType
            if (int.TryParse(userTypeValue, out int numericValue))
            {
                return numericValue;
            }

            // Convert text values to numeric
            return userTypeValue.ToLower() switch
            {
                "admin" => 1,
                "manager" => 2,
                "cashier" => 3,
                "customer" => 4,
                _ => 0
            };
        }

        private void NavigateToDashboard(int userType, string userId, string name)
        {
            // UserType: 1 = Admin, 2 = Manager, 3 = Cashier, 4 = Customer
            Form? dashboard = null;

            switch (userType)
            {
                case 1: // Admin
                    dashboard = new AdminDashboard(userId, name);
                    break;
                case 2: // Manager
                    dashboard = new ManagerDashboard();
                    break;
                case 3: // Cashier
                    dashboard = new CashierDashboard(userId);
                    break;
                case 4: // Customer
                    dashboard = new CustomerDashboard(userId);
                    break;
                default:
                    MessageBox.Show("Unknown user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            if (dashboard != null)
            {
                MessageBox.Show("Login successful! Welcome.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Hide();
                dashboard.ShowDialog();
                // If a dashboard triggered logout it set LoggedOut = true and
                // called ClearAndShow() — the form is already visible, don't close it.
                if (!LoggedOut)
                    Close();
                LoggedOut = false;   // reset for next login cycle
            }
        }

        // Called by dashboards on logout — clears all fields and shows the login form fresh
        public void ClearAndShow()
        {
            UsernametextBox.Clear();
            PasswordtextBox.Clear();
            UserNameErrorlabel.Text  = "";
            PasswordErrorlabel.Text  = "";
            ShowPasswordcheckBox.Checked = false;
            PasswordtextBox.PasswordChar = '*';
            UsernametextBox.Focus();
            Show();
        }

        private void ForgerPasswordlabel_Click(object sender, EventArgs e)
        {
            ForgetPasswordGUI forgetPasswordGUI = new ForgetPasswordGUI();
            forgetPasswordGUI.ShowDialog();
        }
    }
}
