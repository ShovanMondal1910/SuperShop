using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class SaleItem
    {
        private string _saleDetailId = string.Empty;
        private string _saleId = string.Empty;
        private string _productId = string.Empty;
        private int _quantity = 0;
        private decimal _unitPrice = 0;
        private decimal _discount = 0;
        private decimal _tax = 0;
        private string _batchNumber = string.Empty;
        private DateTime? _expiryDate = null;
        private bool _isReturned = false;
        private DateTime? _returnDate = null;
        private DateTime _createdDate = DateTime.Now;

        public string SaleDetailID
        {
            get { return _saleDetailId; }
            set { _saleDetailId = value; }
        }

        public string SaleID
        {
            get { return _saleId; }
            set { _saleId = value; }
        }

        public string ProductID
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        public decimal Tax
        {
            get { return _tax; }
            set { _tax = value; }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }

        public DateTime? ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        public bool IsReturned
        {
            get { return _isReturned; }
            set { _isReturned = value; }
        }

        public DateTime? ReturnDate
        {
            get { return _returnDate; }
            set { _returnDate = value; }
        }

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public SaleItem(string saleDetailId, string saleId, string productId, int quantity, decimal unitPrice, 
            decimal discount, decimal tax, string batchNumber, DateTime? expiryDate, bool isReturned, 
            DateTime? returnDate, DateTime createdDate)
        {
            SaleDetailID = saleDetailId;
            SaleID = saleId;
            ProductID = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            Tax = tax;
            BatchNumber = batchNumber;
            ExpiryDate = expiryDate;
            IsReturned = isReturned;
            ReturnDate = returnDate;
            CreatedDate = createdDate;
        }

        public SaleItem()
        {
        }
    }
}
