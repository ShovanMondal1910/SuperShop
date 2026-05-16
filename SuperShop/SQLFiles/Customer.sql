-- Drop existing table if it exists
DROP TABLE IF EXISTS [CUSTOMER];

-- Create Customer Table
CREATE TABLE [CUSTOMER] (
    CustomerID NVARCHAR(50) PRIMARY KEY,
    CustomerType NVARCHAR(50) NOT NULL,
    UserID NVARCHAR(50) NOT NULL,
    TotalPurchase DECIMAL(18, 2) NULL DEFAULT 0,
    IsVIP BIT NOT NULL DEFAULT 0,
    LoyaltyPoints DECIMAL(18, 2) NULL DEFAULT 0
);
