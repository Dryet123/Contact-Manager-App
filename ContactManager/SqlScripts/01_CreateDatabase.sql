IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ContactManagerDb')
BEGIN
    CREATE DATABASE ContactManagerDb;
END
GO