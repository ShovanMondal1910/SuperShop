using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Supplier
    {
        private string _supplierId = string.Empty;
        private string _supplierName = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;
        private string _address = string.Empty;
        private string _city = string.Empty;
        private string _country = string.Empty;
        private string _companyName = string.Empty;
        private string _taxId = string.Empty;
        private int _paymentTerms = 0;
        private bool _isActive = true;

        public string SupplierID
        {
            get { return _supplierId; }
            set { _supplierId = value; }
        }

        public string SupplierName
        {
            get { return _supplierName; }
            set { _supplierName = value; }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
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

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public string TaxID
        {
            get { return _taxId; }
            set { _taxId = value; }
        }

        public int PaymentTerms
        {
            get { return _paymentTerms; }
            set { _paymentTerms = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Supplier(string supplierId, string supplierName, string phoneNumber, string email, string address, 
            string city, string country, string companyName, string taxId, int paymentTerms, bool isActive)
        {
            SupplierID = supplierId;
            SupplierName = supplierName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            City = city;
            Country = country;
            CompanyName = companyName;
            TaxID = taxId;
            PaymentTerms = paymentTerms;
            IsActive = isActive;
        }

        public Supplier()
        {
        }
    }
}
