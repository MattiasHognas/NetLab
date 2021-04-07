IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Content')
    BEGIN
        CREATE DATABASE Content;
    END
GO
