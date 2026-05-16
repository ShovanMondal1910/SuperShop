-- Drop existing table if it exists
DROP TABLE IF EXISTS [MANAGER];

CREATE TABLE [MANAGER] (
    ManagerID NVARCHAR(50) PRIMARY KEY,
    ManagerType NVARCHAR(50) NOT NULL,
    UserID NVARCHAR(50) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    CanManageCashier BIT NOT NULL DEFAULT 0,
    CanManageProduct BIT NOT NULL DEFAULT 0
);
