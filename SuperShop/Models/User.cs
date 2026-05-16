using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    enum UserType
    {
        Admin = 1,
        Manager = 2,
        Cashier = 3,
        Customer = 4
    }
    internal class User
    {
        private string _userId = string.Empty;
        private UserType _userType  ;
        private string _name = string.Empty;
        private DateTime _dateOfBirth;
        private string _gender = string.Empty;
        private string _phone = string.Empty;
        private string _email = string.Empty;
        private string _address = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _securityQuestion = string.Empty;
        private string _securityAnswer = string.Empty;
        private bool _isActive = false;
        public string UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public UserType UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        public string Gender
        {
            get { return _gender; }
            set {_gender = value; }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string SecurityQuestion
        {
            get { return _securityQuestion; }
            set { _securityQuestion = value; }
        }
        public string SecurityAnswer
        {
            get { return _securityAnswer; }
            set { _securityAnswer = value; }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        public User(string userId, UserType userType, string name, DateTime dateOfBirth, string gender
            , string phone, string email, string address, string username, string password, string securityQuestion, string securityAnswer, bool isActive)
        {
            _userId = userId;
            _userType = userType;
            _name = name;
            _dateOfBirth = dateOfBirth;
            _gender = gender;
            _phone = phone;
            _email = email;
            _address = address;
            _username = username;
            _password = password;
            _securityQuestion = securityQuestion;
            _securityAnswer = securityAnswer;
            IsActive = isActive;
        }
    }
}
