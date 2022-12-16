IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [AppConfig] (
        [Key] nvarchar(450) NOT NULL,
        [Value] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AppConfig] PRIMARY KEY ([Key])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Categories] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Image] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Roles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [ShippingFees] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Distance] int NOT NULL,
        [Description] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ShippingFees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [StatusOrders] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_StatusOrders] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Transactions] (
        [Id] nvarchar(450) NOT NULL,
        [Image] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Vouchers] (
        [Id] nvarchar(450) NOT NULL,
        [Code] nvarchar(450) NOT NULL,
        [Discountprice] float NOT NULL,
        [Discountfreeship] float NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        [DateExpiration] datetime2 NOT NULL,
        CONSTRAINT [PK_Vouchers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Products] (
        [Id] nvarchar(450) NOT NULL,
        [CategoryId] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Description] nvarchar(450) NOT NULL,
        [Price] float NOT NULL,
        [Originalprice] float NOT NULL,
        [Stock] int NOT NULL,
        [Discount] int NOT NULL,
        [Image] nvarchar(450) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Staffs] (
        [Id] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        [Fullname] nvarchar(450) NOT NULL,
        [Username] nvarchar(450) NOT NULL,
        [Password] nvarchar(450) NOT NULL,
        [Email] nvarchar(450) NOT NULL,
        [Phonenumber] nvarchar(450) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        [Salary] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Staffs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Staffs_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Users] (
        [Id] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        [Fullname] nvarchar(450) NOT NULL,
        [Username] nvarchar(450) NOT NULL,
        [Password] nvarchar(450) NOT NULL,
        [Email] nvarchar(450) NOT NULL,
        [Phonenumber] nvarchar(450) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [ImageProducts] (
        [Id] nvarchar(450) NOT NULL,
        [Url] nvarchar(450) NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ImageProducts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ImageProducts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Costs] (
        [Id] nvarchar(450) NOT NULL,
        [Cost] int NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        [StaffId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Costs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Costs_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Costs_Staffs_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staffs] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [AddressUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [Address] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AddressUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AddressUsers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Carts] (
        [Id] nvarchar(450) NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        [Quantity] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Carts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE TABLE [Orders] (
        [Id] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [AddressShip] nvarchar(450) NOT NULL,
        [ShippingFeePrice] float NOT NULL,
        [UnitPrice] float NOT NULL,
        [ShippingFeeId] nvarchar(450) NOT NULL,
        [PhoneNumber] nvarchar(450) NOT NULL,
        [TransactionId] nvarchar(450) NOT NULL,
        [StatusOrderId] nvarchar(450) NOT NULL,
        [VoucherId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_ShippingFees_ShippingFeeId] FOREIGN KEY ([ShippingFeeId]) REFERENCES [ShippingFees] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Orders_StatusOrders_StatusOrderId] FOREIGN KEY ([StatusOrderId]) REFERENCES [StatusOrders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Orders_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Orders_Vouchers_VoucherId] FOREIGN KEY ([VoucherId]) REFERENCES [Vouchers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_AddressUsers_UserId] ON [AddressUsers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Carts_ProductId] ON [Carts] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Costs_ProductId] ON [Costs] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Costs_StaffId] ON [Costs] ([StaffId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_ImageProducts_ProductId] ON [ImageProducts] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Orders_ShippingFeeId] ON [Orders] ([ShippingFeeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Orders_StatusOrderId] ON [Orders] ([StatusOrderId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Orders_TransactionId] ON [Orders] ([TransactionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Orders_VoucherId] ON [Orders] ([VoucherId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Staffs_RoleId] ON [Staffs] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220919134623_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220919134623_initial', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920085134_update-user-table')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Avatar');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Users] ADD DEFAULT N'' FOR [Avatar];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920085134_update-user-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220920085134_update-user-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920090706_update-staff-table')
BEGIN
    ALTER TABLE [Staffs] ADD [Avatar] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920090706_update-staff-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220920090706_update-staff-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920160215_update-gender')
BEGIN
    ALTER TABLE [Users] ADD [Gender] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920160215_update-gender')
BEGIN
    ALTER TABLE [Staffs] ADD [Gender] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220920160215_update-gender')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220920160215_update-gender', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921082035_update-categories-table')
BEGIN
    ALTER TABLE [Categories] ADD [Description] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921082035_update-categories-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220921082035_update-categories-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Vouchers] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Users] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Transactions] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [StatusOrders] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Staffs] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Roles] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Products] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [Categories] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    ALTER TABLE [AddressUsers] ADD [isDeleted] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921083141_update-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220921083141_update-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vouchers]') AND [c].[name] = N'isDeleted');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Vouchers] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Vouchers] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'isDeleted');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Users] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Transactions]') AND [c].[name] = N'isDeleted');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Transactions] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Transactions] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StatusOrders]') AND [c].[name] = N'isDeleted');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [StatusOrders] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [StatusOrders] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Staffs]') AND [c].[name] = N'isDeleted');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Staffs] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Staffs] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    ALTER TABLE [ShippingFees] ADD [isDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'isDeleted');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Roles] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'isDeleted');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Products] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'isDeleted');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Categories] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AddressUsers]') AND [c].[name] = N'isDeleted');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [AddressUsers] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [AddressUsers] ADD DEFAULT CAST(0 AS bit) FOR [isDeleted];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921084425_update-table1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220921084425_update-table1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921092958_update-table-category')
BEGIN
    ALTER TABLE [Categories] ADD [DateCreated] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921092958_update-table-category')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220921092958_update-table-category', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921232146_salarystaff-table')
BEGIN
    CREATE TABLE [SalaryStaff] (
        [Id] nvarchar(450) NOT NULL,
        [NumberOfWorking] int NOT NULL DEFAULT 1,
        [ListDayWorking] nvarchar(max) NOT NULL,
        [Month] nvarchar(max) NOT NULL,
        [SalaryOfThisMonth] nvarchar(max) NOT NULL,
        [Salary] nvarchar(max) NOT NULL,
        [isWorking] bit NOT NULL,
        [StaffId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_SalaryStaff] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SalaryStaff_Staffs_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staffs] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921232146_salarystaff-table')
BEGIN
    CREATE INDEX [IX_SalaryStaff_StaffId] ON [SalaryStaff] ([StaffId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220921232146_salarystaff-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220921232146_salarystaff-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922132720_cost-product-table')
BEGIN
    CREATE TABLE [CostProduct] (
        [Id] nvarchar(450) NOT NULL,
        [count] nvarchar(max) NOT NULL,
        [price] nvarchar(max) NOT NULL,
        [TotalCost] nvarchar(max) NOT NULL,
        [Month] nvarchar(max) NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_CostProduct] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CostProduct_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922132720_cost-product-table')
BEGIN
    CREATE INDEX [IX_CostProduct_ProductId] ON [CostProduct] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922132720_cost-product-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220922132720_cost-product-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922133545_update-product-table')
BEGIN
    ALTER TABLE [Products] ADD [Count] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922133545_update-product-table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220922133545_update-product-table', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    EXEC sp_rename N'[CostProduct].[price]', N'Price', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    EXEC sp_rename N'[CostProduct].[count]', N'Count', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    ALTER TABLE [ImageProducts] ADD [isDefaut] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CostProduct]') AND [c].[name] = N'Price');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [CostProduct] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [CostProduct] ALTER COLUMN [Price] float NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CostProduct]') AND [c].[name] = N'Count');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [CostProduct] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [CostProduct] ALTER COLUMN [Count] int NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CostProduct]') AND [c].[name] = N'TotalCost');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [CostProduct] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [CostProduct] ALTER COLUMN [TotalCost] float NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220922140409_update-table-product-imageproduct-costprod')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220922140409_update-table-product-imageproduct-costprod', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220923194149_chatusertb')
BEGIN
    CREATE TABLE [ChatUsers] (
        [Id] int NOT NULL IDENTITY,
        [Message] nvarchar(max) NOT NULL,
        [isNewMessage] bit NOT NULL DEFAULT CAST(1 AS bit),
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ChatUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ChatUsers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220923194149_chatusertb')
BEGIN
    CREATE INDEX [IX_ChatUsers_UserId] ON [ChatUsers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220923194149_chatusertb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220923194149_chatusertb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220924043053_update-chatusertb')
BEGIN
    EXEC sp_rename N'[ChatUsers].[Id]', N'ChatId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220924043053_update-chatusertb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220924043053_update-chatusertb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220924083304_update-displaypriority')
BEGIN
    ALTER TABLE [ChatUsers] ADD [DisplayPriority] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220924083304_update-displaypriority')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220924083304_update-displaypriority', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220926101252_admin-tb')
BEGIN
    CREATE TABLE [Admins] (
        [Id] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        [Fullname] nvarchar(max) NOT NULL,
        [Username] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Phonenumber] nvarchar(max) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        [Avatar] nvarchar(max) NOT NULL DEFAULT N'',
        [Gender] nvarchar(max) NOT NULL DEFAULT N'',
        [isDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK_Admins] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Admins_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220926101252_admin-tb')
BEGIN
    CREATE INDEX [IX_Admins_RoleId] ON [Admins] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220926101252_admin-tb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220926101252_admin-tb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927033408_updatevoucher')
BEGIN
    ALTER TABLE [Vouchers] ADD [Count] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927033408_updatevoucher')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220927033408_updatevoucher', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927055932_updatevouchercount')
BEGIN
    EXEC sp_rename N'[Vouchers].[Count]', N'AmountRemaining', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927055932_updatevouchercount')
BEGIN
    ALTER TABLE [Vouchers] ADD [AmountInput] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927055932_updatevouchercount')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220927055932_updatevouchercount', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927083905_updateshippingfee')
BEGIN
    EXEC sp_rename N'[ShippingFees].[Distance]', N'PricePerKm', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220927083905_updateshippingfee')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220927083905_updateshippingfee', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002040125_updatecarttb')
BEGIN
    ALTER TABLE [Carts] ADD [DateCreated] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002040125_updatecarttb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221002040125_updatecarttb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002084710_updateaddressuser')
BEGIN
    ALTER TABLE [AddressUsers] ADD [lat] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002084710_updateaddressuser')
BEGIN
    ALTER TABLE [AddressUsers] ADD [lng] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002084710_updateaddressuser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221002084710_updateaddressuser', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002161311_updateaddressusertb')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AddressUsers]') AND [c].[name] = N'lng');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [AddressUsers] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [AddressUsers] ALTER COLUMN [lng] real NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002161311_updateaddressusertb')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AddressUsers]') AND [c].[name] = N'lat');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [AddressUsers] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [AddressUsers] ALTER COLUMN [lat] real NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002161311_updateaddressusertb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221002161311_updateaddressusertb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002170819_updatetb')
BEGIN
    ALTER TABLE [AddressUsers] ADD [isAddressDefaut] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221002170819_updatetb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221002170819_updatetb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221003080053_updateshipfee')
BEGIN
    EXEC sp_rename N'[ShippingFees].[PricePerKm]', N'Price', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221003080053_updateshipfee')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221003080053_updateshipfee', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221003083217_addinfoshop')
BEGIN
    CREATE TABLE [InfoShop] (
        [Id] nvarchar(450) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Phonenumber] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_InfoShop] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221003083217_addinfoshop')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221003083217_addinfoshop', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221003152920_updateusertb')
BEGIN
    ALTER TABLE [Users] ADD [isVerify] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221003152920_updateusertb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221003152920_updateusertb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006075911_updateordertb')
BEGIN
    ALTER TABLE [Orders] ADD [ProductId] nvarchar(450) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006075911_updateordertb')
BEGIN
    CREATE INDEX [IX_Orders_ProductId] ON [Orders] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006075911_updateordertb')
BEGIN
    ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006075911_updateordertb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221006075911_updateordertb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006091655_updateorder')
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'ShippingFeePrice');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Orders] DROP COLUMN [ShippingFeePrice];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006091655_updateorder')
BEGIN
    ALTER TABLE [Orders] ADD [Quantity] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006091655_updateorder')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221006091655_updateorder', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006092855_updateorder1')
BEGIN
    ALTER TABLE [Orders] ADD [Message] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221006092855_updateorder1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221006092855_updateorder1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221007072747_notifitb')
BEGIN
    CREATE TABLE [Notifi] (
        [Id] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        CONSTRAINT [PK_Notifi] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifi_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221007072747_notifitb')
BEGIN
    CREATE INDEX [IX_Notifi_UserId] ON [Notifi] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221007072747_notifitb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221007072747_notifitb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008075918_updatetborder')
BEGIN
    ALTER TABLE [Orders] DROP CONSTRAINT [FK_Orders_Products_ProductId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008075918_updatetborder')
BEGIN
    DROP INDEX [IX_Orders_ProductId] ON [Orders];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008075918_updatetborder')
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'ProductId');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [Orders] DROP COLUMN [ProductId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008075918_updatetborder')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221008075918_updatetborder', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008080538_orderdetail')
BEGIN
    CREATE TABLE [OrderDetails] (
        [Id] nvarchar(450) NOT NULL,
        [OrderId] nvarchar(450) NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        [ProductCount] int NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008080538_orderdetail')
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008080538_orderdetail')
BEGIN
    CREATE INDEX [IX_OrderDetails_ProductId] ON [OrderDetails] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221008080538_orderdetail')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221008080538_orderdetail', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221013132047_updatesalarytb')
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SalaryStaff]') AND [c].[name] = N'NumberOfWorking');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [SalaryStaff] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [SalaryStaff] ADD DEFAULT 0 FOR [NumberOfWorking];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221013132047_updatesalarytb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221013132047_updatesalarytb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014111304_updatecostvoucher')
BEGIN
    DROP TABLE [Costs];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014111304_updatecostvoucher')
BEGIN
    CREATE TABLE [CostVouchers] (
        [Id] nvarchar(450) NOT NULL,
        [Cost] int NOT NULL,
        [VoucherId] nvarchar(450) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        CONSTRAINT [PK_CostVouchers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CostVouchers_Vouchers_VoucherId] FOREIGN KEY ([VoucherId]) REFERENCES [Vouchers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014111304_updatecostvoucher')
BEGIN
    CREATE INDEX [IX_CostVouchers_VoucherId] ON [CostVouchers] ([VoucherId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014111304_updatecostvoucher')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221014111304_updatecostvoucher', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221014120743_updatecostvoucher1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221014120743_updatecostvoucher1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135724_infoshoptb')
BEGIN
    ALTER TABLE [InfoShop] ADD [DistrictID] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135724_infoshoptb')
BEGIN
    ALTER TABLE [InfoShop] ADD [ProvinceID] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135724_infoshoptb')
BEGIN
    ALTER TABLE [InfoShop] ADD [WardCode] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135724_infoshoptb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221015135724_infoshoptb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135926_addressusertb')
BEGIN
    ALTER TABLE [AddressUsers] ADD [DistrictID] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135926_addressusertb')
BEGIN
    ALTER TABLE [AddressUsers] ADD [ProvinceID] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135926_addressusertb')
BEGIN
    ALTER TABLE [AddressUsers] ADD [WardCode] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015135926_addressusertb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221015135926_addressusertb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015140548_addressusertb1')
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AddressUsers]') AND [c].[name] = N'lat');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [AddressUsers] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [AddressUsers] DROP COLUMN [lat];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015140548_addressusertb1')
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AddressUsers]') AND [c].[name] = N'lng');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [AddressUsers] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [AddressUsers] DROP COLUMN [lng];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221015140548_addressusertb1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221015140548_addressusertb1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    ALTER TABLE [Orders] DROP CONSTRAINT [FK_Orders_ShippingFees_ShippingFeeId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    DROP TABLE [ShippingFees];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    DROP INDEX [IX_Orders_ShippingFeeId] ON [Orders];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'ShippingFeeId');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [Orders] DROP COLUMN [ShippingFeeId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    ALTER TABLE [Orders] ADD [ShippingFee] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    ALTER TABLE [Orders] ADD [isPaid] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    ALTER TABLE [Orders] ADD [isPaymentOnline] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221016052154_updatetb1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221016052154_updatetb1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221028100037_orderdetailtb')
BEGIN
    ALTER TABLE [OrderDetails] ADD [DiscountProduct] float NOT NULL DEFAULT 0.0E0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221028100037_orderdetailtb')
BEGIN
    ALTER TABLE [OrderDetails] ADD [DiscountVoucher] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221028100037_orderdetailtb')
BEGIN
    ALTER TABLE [OrderDetails] ADD [PriceProduct] float NOT NULL DEFAULT 0.0E0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221028100037_orderdetailtb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221028100037_orderdetailtb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221028102539_orderdetailtb1')
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderDetails]') AND [c].[name] = N'DiscountVoucher');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [OrderDetails] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [OrderDetails] DROP COLUMN [DiscountVoucher];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221028102539_orderdetailtb1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221028102539_orderdetailtb1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110223012_updchat')
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChatUsers]') AND [c].[name] = N'isNewMessageUser');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [ChatUsers] DROP CONSTRAINT [' + @var22 + '];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110223012_updchat')
BEGIN
    DECLARE @var23 sysname;
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChatUsers]') AND [c].[name] = N'isNewMessageAdmin');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [ChatUsers] DROP CONSTRAINT [' + @var23 + '];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110223012_updchat')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221110223012_updchat', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221112112306_updatchat')
BEGIN
    DECLARE @var24 sysname;
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ChatUsers]') AND [c].[name] = N'isNewMessageAdmin');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [ChatUsers] DROP CONSTRAINT [' + @var24 + '];');
    ALTER TABLE [ChatUsers] DROP COLUMN [isNewMessageAdmin];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221112112306_updatchat')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221112112306_updatchat', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221112112400_updatchat1')
BEGIN
    ALTER TABLE [ChatUsers] ADD [isNewMessageAdmin] bit NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221112112400_updatchat1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221112112400_updatchat1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221112171631_updatenotifi1')
BEGIN
    ALTER TABLE [Notifi] ADD [isNewNotifi] bit NOT NULL DEFAULT CAST(1 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221112171631_updatenotifi1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221112171631_updatenotifi1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212134900_update-prodtb')
BEGIN
    ALTER TABLE [Products] ADD [isSize] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212134900_update-prodtb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221212134900_update-prodtb', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212140107_add-prodetail')
BEGIN
    CREATE TABLE [ProductDetails] (
        [Id] nvarchar(450) NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        [S] int NOT NULL,
        [M] int NOT NULL,
        [L] int NOT NULL,
        [XL] int NOT NULL,
        [XXL] int NOT NULL,
        CONSTRAINT [PK_ProductDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212140107_add-prodetail')
BEGIN
    CREATE INDEX [IX_ProductDetails_ProductId] ON [ProductDetails] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212140107_add-prodetail')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221212140107_add-prodetail', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212202002_update-carttb1')
BEGIN
    ALTER TABLE [Carts] ADD [SizeProduct] nvarchar(450) NOT NULL DEFAULT N'0';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212202002_update-carttb1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221212202002_update-carttb1', N'6.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212225354_update-orderdetail1312')
BEGIN
    ALTER TABLE [OrderDetails] ADD [ProductSize] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221212225354_update-orderdetail1312')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221212225354_update-orderdetail1312', N'6.0.9');
END;
GO

COMMIT;
GO

