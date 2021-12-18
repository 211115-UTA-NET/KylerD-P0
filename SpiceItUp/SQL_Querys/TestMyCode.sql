SELECT * FROM UserInformation;
SELECT * FROM LoginManager;

SELECT * FROM LoginManager WHERE (Username = 'ManagerKyler' AND "Password" = 'ManagerPassword');

SELECT * FROM UserInformation WHERE UserID = 1;

DELETE FROM LoginManager WHERE UserID = 3;
DELETE FROM UserInformation WHERE UserID = 3;
SELECT UserID FROM LoginManager WHERE Username = 'ManagerKyler';