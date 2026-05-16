using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Cashier : User
    {
        private string _cashierId = string.Empty;
        private string _cashierType = string.Empty;
        private string _shift = string.Empty;
        private bool _canProcessSale = false;
        private bool _canHandleReturn = false;

        public string CashierID
        {
            get { return _cashierId; }
            set { _cashierId = value; }
        }

        public string CashierType
        {
            get { return _cashierType; }
            set { _cashierType = value; }
        }

        public string Shift
        {
            get { return _shift; }
            set { _shift = value; }
        }

        public bool CanProcessSale
        {
            get { return _canProcessSale; }
            set { _canProcessSale = value; }
        }

        public bool CanHandleReturn
        {
            get { return _canHandleReturn; }
            set { _canHandleReturn = value; }
        }

        public Cashier(string userId, UserType userType, string name, DateTime dateOfBirth, string gender, string phone, string email, string address, string username,
            string password, string securityQuestion, string securityAnswer, bool isActive, string cashierId, string cashierType, string shift, bool canProcessSale, bool canHandleReturn)
            : base(userId, UserType.Cashier, name, dateOfBirth, gender, phone, email, address, username, password, securityQuestion, securityAnswer, isActive)
        {
            CashierID = cashierId;
            CashierType = cashierType;
            Shift = shift;
            CanProcessSale = canProcessSale;
            CanHandleReturn = canHandleReturn;
            UserID = userId;
            UserType = UserType.Cashier;
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
            CashierID = cashierId;
            CashierType = cashierType;
            Shift = shift;
        }
    }
}
