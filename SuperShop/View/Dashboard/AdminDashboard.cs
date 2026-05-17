using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SuperShop.View.Admin;
using SuperShop.View.Cashier;
using SuperShop.View.Manager;
using SuperShop.View.Supplier;
using SuperShop.View.Product;

namespace SuperShop.View.Dashboard
{
    public partial class AdminDashboard : Form
    {
        private string _adminUserId = string.Empty;
        private string _adminName = string.Empty;

        public AdminDashboard()
        {
            InitializeComponent();
        }

        public AdminDashboard(string userId, string name) : this()
        {
            _adminUserId = userId;
            _adminName = name;
            UpdateHeaderWithUserInfo();
        }

        private void UpdateHeaderWithUserInfo()
        {
            AdminIDlabel.Text = $"Admin ID : {_adminUserId}";
            Welcomelabel.Text = $"Welcome, {_adminName}";
        }

        private void AddAdminbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            AddAdminGUI addAdminGUI = new AddAdminGUI();
            addAdminGUI.TopLevel = false;
            addAdminGUI.FormBorderStyle = FormBorderStyle.None;
            addAdminGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(addAdminGUI);
            addAdminGUI.Show();
        }

        private void AdminManagerbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            AdminManagerGUI adminManagerGUI = new AdminManagerGUI();
            adminManagerGUI.TopLevel = false;
            adminManagerGUI.FormBorderStyle = FormBorderStyle.None;
            adminManagerGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(adminManagerGUI);
            adminManagerGUI.Show();
        }

        private void AddCashierbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            AddCashierGUI addCashierGUI = new AddCashierGUI();
            addCashierGUI.TopLevel = false;
            addCashierGUI.FormBorderStyle = FormBorderStyle.None;
            addCashierGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(addCashierGUI);
            addCashierGUI.Show();
        }

        private void CashierManagerbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            CashierManagerGUI cashierManagerGUI = new CashierManagerGUI();
            cashierManagerGUI.TopLevel = false;
            cashierManagerGUI.FormBorderStyle = FormBorderStyle.None;
            cashierManagerGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(cashierManagerGUI);
            cashierManagerGUI.Show();
        }

        private void AddManagerbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            AddManagerGUI addManagerGUI = new AddManagerGUI();
            addManagerGUI.TopLevel = false;
            addManagerGUI.FormBorderStyle = FormBorderStyle.None;
            addManagerGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(addManagerGUI);
            addManagerGUI.Show();
        }

        private void ManagerManagerbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            ManagerManagerGUI managerManagerGUI = new ManagerManagerGUI();
            managerManagerGUI.TopLevel = false;
            managerManagerGUI.FormBorderStyle = FormBorderStyle.None;
            managerManagerGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(managerManagerGUI);
            managerManagerGUI.Show();
        }

        private void AddSupplierbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            AddSupplierGUI addSupplierGUI = new AddSupplierGUI();
            addSupplierGUI.TopLevel = false;
            addSupplierGUI.FormBorderStyle = FormBorderStyle.None;
            addSupplierGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(addSupplierGUI);
            addSupplierGUI.Show();
        }

        private void SupplierManagerbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            SupplierManagerGUI supplierManagerGUI = new SupplierManagerGUI();
            supplierManagerGUI.TopLevel = false;
            supplierManagerGUI.FormBorderStyle = FormBorderStyle.None;
            supplierManagerGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(supplierManagerGUI);
            supplierManagerGUI.Show();
        }

        private void AddProductbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            AddProductGUI addProductGUI = new AddProductGUI();
            addProductGUI.TopLevel = false;
            addProductGUI.FormBorderStyle = FormBorderStyle.None;
            addProductGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(addProductGUI);
            addProductGUI.Show();
        }

        private void ProductManagerbutton_Click(object sender, EventArgs e)
        {
            GUILoadpanel.Controls.Clear();
            ProductManagerGUI productManagerGUI = new ProductManagerGUI();
            productManagerGUI.TopLevel = false;
            productManagerGUI.FormBorderStyle = FormBorderStyle.None;
            productManagerGUI.Dock = DockStyle.Fill;
            GUILoadpanel.Controls.Add(productManagerGUI);
            productManagerGUI.Show();
        }
    }
}
