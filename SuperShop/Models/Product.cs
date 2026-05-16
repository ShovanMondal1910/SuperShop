using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Models
{
    internal class Product
    {
        private string _productId = string.Empty;
        private string _productName = string.Empty;
        private string _brand = string.Empty;
        private string _productCategory = string.Empty;
        private string _barcode = string.Empty;
        private string _sku = string.Empty;
        private string _unit = string.Empty;
        private decimal _weight = 0;
        private byte[]? _photo = null;
        private string _supplierId = string.Empty;
        private decimal _buyingPrice = 0;
        private decimal _sellingPrice = 0;
        private decimal _discount = 0;
        private decimal _tax = 0;
        private decimal _mrp = 0;
        private int _stockQuantity = 0;
        private int _reorderLevel = 0;
        private int _minStockLevel = 0;
        private bool _isExpirable = false;
        private DateTime? _expiryDate = null;
        private bool _isActive = true;
        private bool _returnable = false;

        public string ProductID
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        public string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }

        public string ProductCategory
        {
            get { return _productCategory; }
            set { _productCategory = value; }
        }

        public string Barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }

        public string SKU
        {
            get { return _sku; }
            set { _sku = value; }
        }

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public decimal Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public byte[]? Photo
        {
            get { return _photo; }
            set { _photo = value; }
        }

        public string SupplierID
        {
            get { return _supplierId; }
            set { _supplierId = value; }
        }

        public decimal BuyingPrice
        {
            get { return _buyingPrice; }
            set { _buyingPrice = value; }
        }

        public decimal SellingPrice
        {
            get { return _sellingPrice; }
            set { _sellingPrice = value; }
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

        public decimal MRP
        {
            get { return _mrp; }
            set { _mrp = value; }
        }

        public int StockQuantity
        {
            get { return _stockQuantity; }
            set { _stockQuantity = value; }
        }

        public int ReorderLevel
        {
            get { return _reorderLevel; }
            set { _reorderLevel = value; }
        }

        public int MinStockLevel
        {
            get { return _minStockLevel; }
            set { _minStockLevel = value; }
        }

        public bool IsExpirable
        {
            get { return _isExpirable; }
            set { _isExpirable = value; }
        }

        public DateTime? ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool Returnable
        {
            get { return _returnable; }
            set { _returnable = value; }
        }

        public Product(string productId, string productName, string brand, string productCategory, string barcode, string sku, 
            string unit, decimal weight, byte[]? photo, string supplierId, decimal buyingPrice, decimal sellingPrice, 
            decimal discount, decimal tax, decimal mrp, int stockQuantity, int reorderLevel, int minStockLevel, 
            bool isExpirable, DateTime? expiryDate, bool isActive, bool returnable)
        {
            ProductID = productId;
            ProductName = productName;
            Brand = brand;
            ProductCategory = productCategory;
            Barcode = barcode;
            SKU = sku;
            Unit = unit;
            Weight = weight;
            Photo = photo;
            SupplierID = supplierId;
            BuyingPrice = buyingPrice;
            SellingPrice = sellingPrice;
            Discount = discount;
            Tax = tax;
            MRP = mrp;
            StockQuantity = stockQuantity;
            ReorderLevel = reorderLevel;
            MinStockLevel = minStockLevel;
            IsExpirable = isExpirable;
            ExpiryDate = expiryDate;
            IsActive = isActive;
            Returnable = returnable;
        }
    }
}
