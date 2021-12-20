SELECT * FROM UserInformation;
SELECT * FROM LoginManager;

UPDATE StoreInventory SET InStock = 25 WHERE StoreID = 102;
DELETE FROM TransactionHistory WHERE TransactionID = 'jpCLbc8Hm39';
DELETE FROM CustomerTransactionHistory WHERE TransactionID = 'jpCLbc8Hm39';

SELECT * FROM LoginManager WHERE (Username = 'ManagerKyler' AND "Password" = 'ManagerPassword');

INSERT TransactionHistory (TransactionID, UserID, StoreID, IsStoreOrder, timestamp)
                    VALUES ('trew', 1, 102, 'FALSE', 'some time');

SELECT ItemDetails.ItemName, StoreInventory.InStock, ItemDetails.ItemPrice
FROM StoreInventory JOIN ItemDetails
ON StoreInventory.ItemID = ItemDetails.ItemID
WHERE StoreInventory.StoreID = 102
ORDER BY ItemDetails.ItemName;

SELECT * FROM UserInformation WHERE UserID = 1;

DELETE FROM TransactionHistory WHERE UserID = 1;
DELETE FROM UserInformation WHERE UserID = 3;
SELECT UserID FROM LoginManager WHERE Username = 'ManagerKyler';

SELECT * FROM TransactionHistory;
SELECT * FROM CustomerTransactionDetails;
SELECT * FROM StoreInventory;