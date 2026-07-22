/* ============================================================
   AtDrive Order Management - SQL Server schema
   ------------------------------------------------------------
   Reference script for the AtDriveOrders database.

   NOTE: you do NOT need to run this script. The application applies
   the equivalent EF Core migrations automatically on startup and
   seeds sample data. It is included so the schema and stored
   procedure can be reviewed as plain SQL.

   If you DO create the database with this script instead, the app's
   automatic migration will conflict with the existing tables - so
   either let the app create everything (recommended) or drop the
   database again before first run.
   ============================================================ */

IF DB_ID('AtDriveOrders') IS NULL
    CREATE DATABASE AtDriveOrders;
GO

USE AtDriveOrders;
GO

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
