using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Manager : User
    {
        private string _managerId = string.Empty;
        private string _managerType = string.Empty;
        private string _department = string.Empty;
        private bool _canManageCashier = false;
        private bool _canManageProduct = false;

        public string ManagerID
        {
            get { return _managerId; }
            set { _managerId = value; }
        }

        public string ManagerType
        {
            get { return _managerType; }
            set { _managerType = value; }
        }

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        public bool CanManageCashier
        {
            get { return _canManageCashier; }
            set { _canManageCashier = value; }
        }

        public bool CanManageProduct
        {
            get { return _canManageProduct; }
            set { _canManageProduct = value; }
        }

        public Manager(string userId, UserType userType, string name, DateTime dateOfBirth, string gender, string phone, string email, string address, string username,
            string password, string securityQuestion, string securityAnswer, bool isActive, string managerId, string managerType, string department, bool canManageCashier, bool canManageProduct)
            : base(userId, UserType.Manager, name, dateOfBirth, gender, phone, email, address, username, password, securityQuestion, securityAnswer, isActive)
        {
            ManagerID = managerId;
            ManagerType = managerType;
            Department = department;
            CanManageCashier = canManageCashier;
            CanManageProduct = canManageProduct;
            UserID = userId;
            UserType = UserType.Manager;
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
            ManagerID = managerId;
            ManagerType = managerType;
            Department = department;
        }
    }
}
