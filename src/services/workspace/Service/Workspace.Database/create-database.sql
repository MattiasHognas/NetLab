IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Workspace')
    BEGIN
        CREATE DATABASE Workspace;
    END
GO
