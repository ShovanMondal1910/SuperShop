-- Drop existing table if it exists
DROP TABLE IF EXISTS [USER];

-- Create the table
CREATE TABLE [USER](
    UserID NVARCHAR(50) PRIMARY KEY,
    UserType NVARCHAR(50) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Phone NVARCHAR(20) UNIQUE,
    Email NVARCHAR(100),
    Address NVARCHAR(255) NOT NULL,
    Username NVARCHAR(50) UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    SecurityQuestion NVARCHAR(255) NOT NULL,
    SecurityAnswer NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 0
);