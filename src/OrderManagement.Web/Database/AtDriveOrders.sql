/* ============================================================
   AtDrive Order Management - SQL Server schema + seed data
   ------------------------------------------------------------
   This script is the single source of truth for the database:
   tables, the GetOrdersByStatus stored procedure, and seed data.

   The application runs it AUTOMATICALLY at startup (after creating
   the empty AtDriveOrders database if needed), so no manual setup
   is required. Every section is idempotent, so re-running is safe.

   To set the database up manually instead: create an empty
   AtDriveOrders database, then execute this script against it.
   ============================================================ */

/* ------------------------------------------------------------
   Tables
   ------------------------------------------------------------ */

IF OBJECT_ID('dbo.Orders', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        OrderId      INT IDENTITY(1, 1) NOT NULL,
        CustomerName NVARCHAR(100)      NOT NULL,
        OrderDate    DATETIME2          NOT NULL,
        Status       NVARCHAR(20)       NOT NULL,

        CONSTRAINT PK_Orders PRIMARY KEY (OrderId)
    );
END
GO

IF OBJECT_ID('dbo.OrderItems', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderItems
    (
        OrderItemId INT IDENTITY(1, 1) NOT NULL,
        OrderId     INT                NOT NULL,
        ProductName NVARCHAR(100)      NOT NULL,
        Quantity    INT                NOT NULL,
        UnitPrice   DECIMAL(18, 2)     NOT NULL,

        CONSTRAINT PK_OrderItems PRIMARY KEY (OrderItemId),
        CONSTRAINT FK_OrderItems_Orders_OrderId
            FOREIGN KEY (OrderId) REFERENCES dbo.Orders (OrderId)
            ON DELETE CASCADE
    );

    CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems (OrderId);
END
GO

/* ------------------------------------------------------------
   Stored procedures
   ------------------------------------------------------------ */

CREATE OR ALTER PROCEDURE dbo.GetOrdersByStatus
    @Status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT OrderId, CustomerName, OrderDate, Status
    FROM dbo.Orders
    WHERE Status = @Status
    ORDER BY OrderDate DESC;
END
GO

/* ------------------------------------------------------------
   Seed data (only when the Orders table is empty)
   ------------------------------------------------------------ */

IF NOT EXISTS (SELECT 1 FROM dbo.Orders)
BEGIN
    DECLARE @today   DATE = CAST(GETDATE() AS DATE);
    DECLARE @orderId INT;

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Lionel Messi', DATEADD(DAY, -28, @today), 'Completed');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Home Jersey', 2, 89.99),
           (@orderId, 'Captain''s Armband', 1, 14.50);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Cristiano Ronaldo', DATEADD(DAY, -24, @today), 'Completed');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Football Boots', 1, 219.00),
           (@orderId, 'Training Bibs', 5, 9.99);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Neymar Jr', DATEADD(DAY, -21, @today), 'Cancelled');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Away Jersey', 3, 84.99);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Kylian Mbappe', DATEADD(DAY, -16, @today), 'Shipped');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Match Ball', 4, 39.99),
           (@orderId, 'Shin Guards', 2, 19.95);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Erling Haaland', DATEADD(DAY, -13, @today), 'Processing');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Goalkeeper Gloves', 1, 54.00),
           (@orderId, 'Water Bottle', 6, 7.25);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Jude Bellingham', DATEADD(DAY, -10, @today), 'Pending');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Training Cones (Set of 20)', 2, 24.99);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Vinicius Junior', DATEADD(DAY, -8, @today), 'Shipped');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Home Jersey', 1, 89.99),
           (@orderId, 'Team Scarf', 2, 16.50);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Lamine Yamal', DATEADD(DAY, -5, @today), 'Processing');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Football Boots', 1, 179.00),
           (@orderId, 'Grip Socks', 4, 12.99);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Mohamed Salah', DATEADD(DAY, -3, @today), 'Pending');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Match Ball', 2, 39.99);

    INSERT INTO dbo.Orders (CustomerName, OrderDate, Status)
    VALUES ('Kevin De Bruyne', DATEADD(DAY, -1, @today), 'Pending');
    SET @orderId = SCOPE_IDENTITY();
    INSERT INTO dbo.OrderItems (OrderId, ProductName, Quantity, UnitPrice)
    VALUES (@orderId, 'Away Jersey', 1, 84.99),
           (@orderId, 'Shin Guards', 1, 19.95),
           (@orderId, 'Team Scarf', 1, 16.50);
END
GO
