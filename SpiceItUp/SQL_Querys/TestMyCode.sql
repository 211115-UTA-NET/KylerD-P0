SELECT * FROM Customers;
SELECT * FROM LoginManager;
SELECT * FROM Employees;

DELETE FROM LoginManager WHERE UserID = 11;
DELETE FROM Customers WHERE CustomerID = 11;
SELECT UserID FROM LoginManager WHERE Username = 'ManagerKyler';