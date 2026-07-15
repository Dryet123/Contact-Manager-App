USE ContactManagerDb;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contacts')
BEGIN
    CREATE TABLE Contacts (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        DateOfBirth DATE NOT NULL,
        Married BIT NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        Salary DECIMAL(18,2) NOT NULL
    );
END
GO