using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Sale
    {
        private string _saleId = string.Empty;
        private string _cashierId = string.Empty;
        private string _customerId = string.Empty;
        private DateTime _saleDate = DateTime.Now;
        private string _paymentMethod = string.Empty;
        private string _invoiceNumber = string.Empty;
        private string _notes = string.Empty;
        private DateTime _createdDate = DateTime.Now;
        private DateTime _modifiedDate = DateTime.Now;

        public string SaleID
        {
            get { return _saleId; }
            set { _saleId = value; }
        }

        public string CashierID
        {
            get { return _cashierId; }
            set { _cashierId = value; }
        }

        public string CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public DateTime SaleDate
        {
            get { return _saleDate; }
            set { _saleDate = value; }
        }

        public string PaymentMethod
        {
            get { return _paymentMethod; }
            set { _paymentMethod = value; }
        }

        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set { _invoiceNumber = value; }
        }

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public Sale(string saleId, string cashierId, string customerId, DateTime saleDate, string paymentMethod, 
            string invoiceNumber, string notes, DateTime createdDate, DateTime modifiedDate)
        {
            SaleID = saleId;
            CashierID = cashierId;
            CustomerID = customerId;
            SaleDate = saleDate;
            PaymentMethod = paymentMethod;
            InvoiceNumber = invoiceNumber;
            Notes = notes;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }

        public Sale()
        {
        }
    }
}
