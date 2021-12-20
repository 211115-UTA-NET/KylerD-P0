INSERT LoginManager (Username, "Password")
VALUES ('ManagerKyler', 'ManagerPassword');

INSERT UserInformation (UserID, FirstName, LastName, PhoneNumber, IsEmployee)
VALUES (1, 'Kyler', 'Dennis', 4238886190, 'TRUE');

INSERT StoreInfo (StoreName)
VALUES ('Spice It Up Chattanooga'), ('Spice It Up Knoxville'), ('Spice It Up Nashville'), ('Spice It Up Memphis');

INSERT ItemDetails (ItemName, ItemPrice)
VALUES ('Basil', .96), ('Cinnamon', 2.98), ('Cumin', 1.04), ('Garlic Powder', 1.98), ('Nutmeg', 1.84),
('Oregano', 1.57), ('Paprika', 1.99), ('Parsley', .78), ('Rosemary', 1.98), ('Thyme', 1.94);

INSERT StoreInventory (StoreID, ItemID, InStock)
VALUES
(101, 1, 15), (101, 2, 15), (101, 3, 15), (101, 4, 15), (101, 5, 15),
(101, 6, 15), (101, 7, 15), (101, 8, 15), (101, 9, 15), (101, 10, 15),

(102, 1, 15), (102, 2, 15), (102, 3, 15), (102, 4, 15), (102, 5, 15),
(102, 6, 15), (102, 7, 15), (102, 8, 15), (102, 9, 15), (102, 10, 15),

(103, 1, 15), (103, 2, 15), (103, 3, 15), (103, 4, 15), (103, 5, 15),
(103, 6, 15), (103, 7, 15), (103, 8, 15), (103, 9, 15), (103, 10, 15),

(104, 1, 15), (104, 2, 15), (104, 3, 15), (104, 4, 15), (104, 5, 15), 
(104, 6, 15), (104, 7, 15), (104, 8, 15), (104, 9, 15), (104, 10, 15);

SELECT * FROM LoginManager;
SELECT * FROM UserInformation;
SELECT * FROM StoreInfo;
SELECT * FROM ItemDetails;
SELECT * FROM StoreInventory;