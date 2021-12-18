INSERT LoginManager (Username, "Password")
VALUES ('ManagerKyler', 'ManagerPassword');

INSERT Employees (EmployeeID, FirstName, LastName, PhoneNumber, AdminAccess)
VALUES (1, 'Kyler', 'Dennis', 4238886190, 'TRUE');

--DELETE FROM Employees WHERE EmployeeID = 1;

INSERT StoreInfo (StoreName)
VALUES ('Spice It Up Chattanooga'), ('Spice It Up Knoxville'), ('Spice It Up Nashville'), ('Spice It Up Memphis');

INSERT ItemDetails (ItemName, ItemPrice)
VALUES ('Basil', .96), ('Cinnamon', 2.98), ('Cumin', 1.04), ('Gralic Powder', 1.98), ('Nutmeg', 1.84),
('Oregano', 1.57), ('Paprika', 1.99), ('Parsley', .78), ('Rosemary', 1.98), ('Thyme', 1.94);

INSERT StoreInventory (StoreID, ItemID, InStock)
VALUES
(101, 1, 25), (101, 2, 25), (101, 3, 25), (101, 4, 25), (101, 5, 25),
(101, 6, 25), (101, 7, 25), (101, 8, 25), (101, 9, 25), (101, 10, 25),

(102, 1, 25), (102, 2, 25), (102, 3, 25), (102, 4, 25), (102, 5, 25),
(102, 6, 25), (102, 7, 25), (102, 8, 25), (102, 9, 25), (102, 10, 25),

(103, 1, 25), (103, 2, 25), (103, 3, 25), (103, 4, 25), (103, 5, 25),
(103, 6, 25), (103, 7, 25), (103, 8, 25), (103, 9, 25), (103, 10, 25),

(104, 1, 25), (104, 2, 25), (104, 3, 25), (104, 4, 25), (104, 5, 25), 
(104, 6, 25), (104, 7, 25), (104, 8, 25), (104, 9, 25), (104, 10, 25);

SELECT * FROM LoginManager;
SELECT * FROM Employees;
SELECT * FROM StoreInfo;
SELECT * FROM ItemDetails;
SELECT * FROM StoreInventory;