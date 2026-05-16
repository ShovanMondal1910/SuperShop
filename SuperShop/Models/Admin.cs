using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{

    internal class Admin : User
    {
        private string _adminId = string.Empty;
        private string _adminType = string.Empty;
        private string _degree = string.Empty;
        private bool _canManageAdmin = false;
        private bool _canManageCustomer = false;

        public string AdminID
        {
            get { return _adminId; }
            set { _adminId = value; }
        }

        public string AdminType
        {
            get { return _adminType; }
            set { _adminType = value; }
        }
        public string Degree
        {
            get { return _degree; }
            set { _degree = value; }
        }
        public bool CanManageAdmin
        {
            get { return _canManageAdmin; }
            set { _canManageAdmin = value; }
        }
        public bool CanManageCustomer
        {
            get { return _canManageCustomer; }
            set { _canManageCustomer = value; }
        }
        public Admin(string userId, UserType userType, string name, DateTime dateOfBirth, string gender, string phone, string email, string address, string username, 
            string password, string securityQuestion, string securityAnswer, bool isActive, string adminId, string adminType, string degree, bool canManageAdmin, bool canManageCustomer)
            : base(userId, UserType.Admin, name, dateOfBirth, gender, phone, email, address, username, password, securityQuestion, securityAnswer, isActive)
        {
            AdminID = adminId;
            AdminType = adminType;
            Degree = degree;
            CanManageAdmin = canManageAdmin;
            CanManageCustomer = canManageCustomer;
            UserID = userId;
            UserType = UserType.Admin;
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Phone = phone;
            Email = email;
            Address = address;
            Username = username;
            Password = password;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
            IsActive = isActive;
            AdminID = adminId;
            AdminType = adminType;
        }
    }
}
