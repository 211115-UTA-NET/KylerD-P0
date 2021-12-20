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
    TransactionID VARCHAR(50) PRIMARY KEY,
    UserID INT NOT NULL,
    StoreID INT NOT NULL,
    IsStoreOrder VARCHAR(5) NOT NULL,
    Timestamp NVARCHAR(50) NOT NULL
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
    TransactionID VARCHAR(50),
    ItemID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity <= 10),
    Price MONEY NOT NULL
)

CREATE TABLE EmployeeOrderingDetails
(
    TransactionID VARCHAR(50),
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

DROP TABLE LoginManager;
DROP TABLE StoreInfo;
DROP TABLE ItemDetails;
DROP TABLE TransactionHistory;
DROP TABLE UserInformation;
DROP TABLE StoreInventory;
DROP TABLE CustomerTransactionDetails;
DROP TABLE EmployeeOrderingDetails;