using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Cart
    {
        private string _cartId = string.Empty;
        private string _customerId = string.Empty;
        private string _productId = string.Empty;
        private int _quantity = 0;
        private decimal _unitPrice = 0;
        private decimal _discount = 0;
        private decimal _tax = 0;
        private DateTime _addedDate = DateTime.Now;
        private DateTime _modifiedDate = DateTime.Now;

        public string CartID
        {
            get { return _cartId; }
            set { _cartId = value; }
        }

        public string CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; }
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

        public DateTime AddedDate
        {
            get { return _addedDate; }
            set { _addedDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public Cart(string cartId, string customerId, string productId, int quantity, decimal unitPrice, 
            decimal discount, decimal tax, DateTime addedDate, DateTime modifiedDate)
        {
            CartID = cartId;
            CustomerID = customerId;
            ProductID = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            Tax = tax;
            AddedDate = addedDate;
            ModifiedDate = modifiedDate;
        }

        public Cart()
        {
        }
    }
}
