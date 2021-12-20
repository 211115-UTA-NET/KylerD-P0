CREATE TABLE LoginManager
(
    UserID INT IDENTITY (1, 1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    "Password" VARCHAR(50) NOT NULL
)

CREATE TABLE StoreInfo
(
    StoreID INT IDENTITY (101, 1) PRIMARY KEY,
    StoreName VARCHAR(100) NOT NULL UNIQUE
)

CREATE TABLE ItemDetails
(
    ItemID INT IDENTITY (1, 1) PRIMARY KEY,
    ItemName VARCHAR(100) NOT NULL UNIQUE,
    ItemPrice MONEY NOT NULL
)

CREATE TABLE TransactionHistory
(
    TransactionID INT IDENTITY (1, 1) PRIMARY KEY,
    UserID INT NOT NULL,
    StoreID INT NOT NULL,
    IsStoreOrder VARCHAR(5) NOT NULL,
    "Timestamp" NVARCHAR(25) NOT NULL
)

CREATE TABLE UserInformation
(
    UserID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    PhoneNumber BIGINT NOT NULL,
    IsEmployee VARCHAR(5) NOT NULL
)

CREATE TABLE StoreInventory
(
    StoreID INT,
    ItemID INT NOT NULL,
    InStock INT NOT NULL CHECK (InStock >= 0 AND InStock <= 25)
)

CREATE TABLE CustomerTransactionDetails
(
    TransactionID INT,
    ItemID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity <= 10),
    Price MONEY NOT NULL
)

CREATE TABLE EmployeeOrderingDetails
(
    TransactionID INT,
    ItemID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity <= 15)
)

ALTER TABLE TransactionHistory ADD FOREIGN KEY (UserID) REFERENCES LoginManager(UserID);
ALTER TABLE TransactionHistory ADD FOREIGN KEY (StoreID) REFERENCES StoreInfo(StoreID);
ALTER TABLE UserInformation ADD FOREIGN KEY (UserID) REFERENCES LoginManager(UserID);
ALTER TABLE StoreInventory ADD FOREIGN KEY (StoreID) REFERENCES StoreInfo(StoreID);
ALTER TABLE StoreInventory ADD FOREIGN KEY (ItemID) REFERENCES ItemDetails(ItemID);
ALTER TABLE CustomerTransactionDetails ADD FOREIGN KEY (TransactionID) REFERENCES TransactionHistory(TransactionID);
ALTER TABLE CustomerTransactionDetails ADD FOREIGN KEY (ItemID) REFERENCES ItemDetails(ItemID);
ALTER TABLE EmployeeOrderingDetails ADD FOREIGN KEY (TransactionID) REFERENCES TransactionHistory(TransactionID);
ALTER TABLE EmployeeOrderingDetails ADD FOREIGN KEY (ItemID) REFERENCES ItemDetails(ItemID);

ALTER TABLE [dbo].[TransactionHistory] DROP CONSTRAINT [PK__Transact__55433A4B4A4E56FF] WITH ( ONLINE = OFF )
GO
ALTER TABLE [dbo].[TransactionHistory] DROP CONSTRAINT [FK__Transacti__Store__2BFE89A6]
GO
ALTER TABLE [dbo].[TransactionHistory] DROP CONSTRAINT [FK__Transacti__UserI__2B0A656D]
GO
ALTER TABLE [dbo].[StoreInfo] DROP CONSTRAINT [PK__StoreInf__3B82F0E171D92ED6] WITH ( ONLINE = OFF )
GO
ALTER TABLE [dbo].[StoreInfo] DROP CONSTRAINT [UQ__StoreInf__520DB652B6E00061]
GO
ALTER TABLE [dbo].[LoginManager] DROP CONSTRAINT [PK__LoginMan__1788CCACF9A84ECF] WITH ( ONLINE = OFF )
GO
ALTER TABLE [dbo].[LoginManager] DROP CONSTRAINT [UQ__LoginMan__536C85E4C8B5848F]
GO
ALTER TABLE [dbo].[ItemDetails] DROP CONSTRAINT [PK__ItemDeta__727E83EB9DC6258C] WITH ( ONLINE = OFF )
GO
ALTER TABLE [dbo].[ItemDetails] DROP CONSTRAINT [UQ__ItemDeta__4E4373F7C319DDE5]
GO

DROP TABLE LoginManager;
DROP TABLE StoreInfo;
DROP TABLE ItemDetails;
DROP TABLE TransactionHistory;
DROP TABLE Customers;
DROP TABLE Employees;
DROP TABLE StoreInventory;
DROP TABLE CustomerTransactionDetails;
DROP TABLE EmployeeOrderingDetails;