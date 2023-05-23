BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Customer] (
    [CustomerId] int NOT NULL IDENTITY,
    [FirstName] varchar(100) NOT NULL,
    [LastName] varchar(100) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [CreatedDate] datetime2 NULL,
    [ModifiedDate] datetime2 NULL,
    [RowVersion] rowversion NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY ([CustomerId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230522201237_Create-Customer-Table', N'7.0.5');
GO

COMMIT;
GO