USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'rose')
BEGIN
    CREATE DATABASE rose;
END
GO

USE rose;
GO

CREATE TABLE log_requisition (
    id INT PRIMARY KEY IDENTITY(1, 1),
    endPoint NVARCHAR(MAX),
    parameters NVARCHAR(MAX),
    data NVARCHAR(MAX),
    ip NVARCHAR(255),
    createdDate DATETIME
);
GO

CREATE TABLE log_error (
    id INT PRIMARY KEY IDENTITY(1, 1),
    endPoint NVARCHAR(MAX),
    parameters NVARCHAR(MAX),
    error NVARCHAR(MAX),
    ip NVARCHAR(255),
    createdDate DATETIME
);
GO

CREATE LOGIN rose WITH PASSWORD = 'password123!@#Rose';
GO

CREATE USER rose FOR LOGIN rose;
GO

EXEC sp_addrolemember 'db_owner', 'rose';
GO
