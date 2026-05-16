-- Drop existing table if it exists
DROP TABLE IF EXISTS [ADMIN];

-- Create Admin Table
CREATE TABLE [ADMIN] (
    AdminID NVARCHAR(50) PRIMARY KEY,
    AdminType NVARCHAR(50) NOT NULL,
    UserID NVARCHAR(50) NOT NULL,
    Degree NVARCHAR(50) NULL,
    CanManageAdmin BIT NOT NULL DEFAULT 0,
    CanManageCustomer BIT NOT NULL DEFAULT 0
);
