using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Customer : User
    {
        private string _customerId = string.Empty;
        private string _customerType = string.Empty;
        private decimal _totalPurchase = 0.00m;
        private bool _isVIP = false;
        private decimal _loyaltyPoints = 0.00m;

        public string CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public string CustomerType
        {
            get { return _customerType; }
            set { _customerType = value; }
        }

        public decimal TotalPurchase
        {
            get { return _totalPurchase; }
            set { _totalPurchase = value; }
        }

        public bool IsVIP
        {
            get { return _isVIP; }
            set { _isVIP = value; }
        }

        public decimal LoyaltyPoints
        {
            get { return _loyaltyPoints; }
            set { _loyaltyPoints = value; }
        }

        public Customer(string userId, UserType userType, string name, DateTime dateOfBirth, string gender, string phone, string email, string address, string username,
            string password, string securityQuestion, string securityAnswer, bool isActive, string customerId, string customerType, decimal totalPurchase, bool isVIP, decimal loyaltyPoints)
            : base(userId, UserType.Customer, name, dateOfBirth, gender, phone, email, address, username, password, securityQuestion, securityAnswer, isActive)
        {
            CustomerID = customerId;
            CustomerType = customerType;
            TotalPurchase = totalPurchase;
            IsVIP = isVIP;
            LoyaltyPoints = loyaltyPoints;
            UserID = userId;
            UserType = UserType.Customer;
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
            CustomerID = customerId;
            CustomerType = customerType;
        }
    }
}
