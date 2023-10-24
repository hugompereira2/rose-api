USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'rose')
BEGIN
    CREATE DATABASE rose;
END
GO

USE rose;
GO

CREATE LOGIN rose WITH PASSWORD = 'password123!@#Rose';
GO

CREATE USER rose FOR LOGIN rose;
GO

EXEC sp_addrolemember 'db_owner', 'rose';
GO
