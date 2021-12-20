SELECT * FROM UserInformation;
SELECT * FROM LoginManager;
SELECT * FROM ItemDetails;

UPDATE StoreInventory SET InStock = 15;
DELETE FROM TransactionHistory WHERE TransactionID = 'jpCLbc8Hm39';
DELETE FROM CustomerTransactionHistory WHERE TransactionID = 'jpCLbc8Hm39';

SELECT * FROM LoginManager WHERE (Username = 'ManagerKyler' AND "Password" = 'ManagerPassword');

INSERT TransactionHistory (TransactionID, UserID, StoreID, IsStoreOrder, timestamp)
                    VALUES ('trew', 1, 102, 'FALSE', 'some time');

SELECT ItemDetails.ItemName, StoreInventory.InStock, ItemDetails.ItemPrice
FROM StoreInventory JOIN ItemDetails
ON StoreInventory.ItemID = ItemDetails.ItemID
WHERE StoreInventory.StoreID = 105
ORDER BY ItemDetails.ItemName;

SELECT * FROM UserInformation WHERE UserID = 1;

DELETE FROM TransactionHistory WHERE UserID = 1;
DELETE FROM UserInformation WHERE UserID = 3;
SELECT UserID FROM LoginManager WHERE Username = 'ManagerKyler';

SELECT * FROM TransactionHistory;
SELECT * FROM CustomerTransactionDetails;
SELECT * FROM StoreInventory;

SELECT TransactionHistory.TransactionID, TransactionHistory.StoreID, SUM(CustomerTransactionDetails.Price)
FROM TransactionHistory JOIN CustomerTransactionDetails
ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID
WHERE TransactionHistory.UserID = 2
GROUP BY TransactionHistory.TransactionID, TransactionHistory.StoreID;

SELECT TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, TransactionHistory.Timestamp, SUM(CustomerTransactionDetails.Price)
FROM TransactionHistory JOIN StoreInfo
ON TransactionHistory.StoreID = StoreInfo.StoreID
JOIN CustomerTransactionDetails ON TransactionHistory.TransactionID = CustomerTransactionDetails.TransactionID
WHERE TransactionHistory.UserID = 2 AND TransactionHistory.TransactionID = 'fVIUZDFzlum'
GROUP BY TransactionHistory.TransactionID, StoreInfo.StoreID, StoreInfo.StoreName, TransactionHistory.Timestamp;

SELECT ItemDetails.ItemName, CustomerTransactionDetails.Quantity, CustomerTransactionDetails.Price
FROM CustomerTransactionDetails JOIN ItemDetails
ON CustomerTransactionDetails.ItemID = ItemDetails.ItemID
WHERE CustomerTransactionDetails.TransactionID = 'jpCLbc8Hm39';