-- Drop existing table if it exists
DROP TABLE IF EXISTS [CASHIER];

-- Create Cashier Table
CREATE TABLE [CASHIER] (
    CashierID NVARCHAR(50) PRIMARY KEY,
    CashierType NVARCHAR(50) NOT NULL,
    UserID NVARCHAR(50) NOT NULL,
    Shift NVARCHAR(50) NULL,
    CanProcessSale BIT NOT NULL DEFAULT 0,
    CanHandleReturn BIT NOT NULL DEFAULT 0
);
