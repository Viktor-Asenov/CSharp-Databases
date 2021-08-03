CREATE DATABASE Hotel
USE Hotel

CREATE TABLE Employees
(
	Id INT PRIMARY KEY NOT NULL,
	FirstName VARCHAR(10) NOT NULL,
	LastName VARCHAR(10) NOT NULL,
	Title VARCHAR(20),
	Notes VARCHAR(MAX)
)

INSERT INTO Employees VALUES
(1, 'Stancho', 'Stanchev', NULL, NULL),
(2, 'Ivan', 'Ivanov', 'Chief', NULL),
(3, 'Pesho', 'Peshev', NULL, NULL)

CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY NOT NULL,
	FirstName VARCHAR(10) NOT NULL,
	LastName VARCHAR(10) NOT NULL,
	PhoneNumber CHAR(8) NOT NULL,
	EmergencyName VARCHAR(10),
	EmergencyNumber CHAR(8),
	Notes VARCHAR(MAX)
)

INSERT INTO Customers VALUES
(1, 'Misho', 'Mishev', '08937821', NULL, NULL, NULL),
(2, 'Gosho', 'Goshev', '08652574', NULL, '112', NULL),
(3, 'Misho', 'Mishev', '08983729', NULL, NULL, NULL)

CREATE TABLE RoomStatus
(
	RoomStatus VARCHAR(20) PRIMARY KEY NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO RoomStatus VALUES
('Apartment', NULL),
('Child', 'Not taken.'),
('Two beds', NULL)

CREATE TABLE RoomTypes 
(
	RoomType VARCHAR(20) PRIMARY KEY NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO RoomTypes VALUES
('Apartment', NULL),
('Child', 'Taken.'),
('Two beds', NULL)

CREATE TABLE BedTypes
(
	BedType VARCHAR(20) PRIMARY KEY NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO BedTypes VALUES
('Single', NULL),
('Double', NULL),
('President', NULL)

CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY NOT NULL,
	RoomType VARCHAR(10) NOT NULL,
	BedType VARCHAR(10),
	Rate INT NOT NULL,
	RoomStatus BIT NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Rooms VALUES
(245, 'Apartment', 'President', 6, 0, NULL),
(327, 'One bed', 'Single', 4, 0, NULL),
(376, 'Two beds', 'Double', 5, 1, NULL)

CREATE TABLE Payments 
(
	Id INT PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	PaymentDate DATE NOT NULL,
	AccountNumber CHAR(8),
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays INT,
	AmountCharged DECIMAL NOT NULL,
	TaxRate DECIMAL,
	TaxAmount DECIMAL,
	PaymentTotal DECIMAL,
	Notes VARCHAR(MAX)
)

INSERT INTO Payments VALUES
(1, 23, '2019-06-07', '2', '2019-06-07', '2019-07-06', 29, 300, NULL, NULL, NULL, NULL),
(2, 43, '2020-05-01', '7', '2020-05-01', '2020-05-07', 6, 70, NULL, NULL, NULL, NULL),
(3, 76, '2018-01-07', '5', '2018-01-10', '2019-01-15', 5, 60, NULL, NULL, NULL, NULL)

CREATE TABLE Occupancies 
(
	Id INT PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	DateOccupied DATE NOT NULL,
	AccountNumber INT NOT NULL,
	RoomNumber INT NOT NULL,
	RateApplied INT,
	PhoneCharge DECIMAL,
	Notes VARCHAR(MAX)
)

INSERT INTO Occupancies VALUES
(1, 2, '2021-07-11', 1, 34, NULL, NULL, NULL),
(2, 4, '2012-10-15', 5, 56, NULL, NULL, NULL),
(3, 3, '2020-11-11', 8, 61, NULL, NULL, NULL)