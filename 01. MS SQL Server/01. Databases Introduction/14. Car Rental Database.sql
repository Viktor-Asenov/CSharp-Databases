CREATE DATABASE CarRental
Use CarRental

CREATE TABLE Categories
(
	Id INT PRIMARY KEY NOT NULL,
	CategoryName VARCHAR(20) NOT NULL,
	DailyRate INT,
	WeeklyRate INT,
	MonthlyRate INT NOT NULL,
	WeekendRate INT
)

INSERT INTO Categories VALUES
(1, 'Sport', NULL, 3, 5, NULL),
(2, 'Offroad', 2, NULL, 6, NULL),
(3, 'Daily', NULL, NULL, 4, NULL)

CREATE TABLE Cars
(
	Id INT PRIMARY KEY NOT NULL,
	PlateNumber CHAR(8) NOT NULL,
	Manufacturer VARCHAR(20) NOT NULL,
	Model VARCHAR(20) NOT NULL,
	CarYear INT NOT NULL,
	CategoryID INT NOT NULL,
	Doors INT NOT NULL,
	Picture VARCHAR(MAX),
	Condition VARCHAR(10),
	Available BIT NOT NULL
)

INSERT INTO Cars VALUES
(1, '98748534', 'Audi', 'RS7', 2019, 1, 3, NULL, NULL, 1),
(2, '65382684', 'BMW', 'M3', 2016, 2, 3, NULL, NULL, 0),
(3, '73648273', 'Skoda', 'Fabia', 2005, 3, 5, NULL, NULL, 1)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY NOT NULL,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Title VARCHAR(20),
	Notes VARCHAR(MAX)
)

INSERT INTO Employees VALUES
(1, 'Pesho', 'Peshev', NULL, NULL),
(2, 'Gencho', 'Ivanov', 'Chief', NULL),
(3, 'Sasho', 'Sashev', NULL, NULL)

CREATE TABLE Customers
(
	Id INT PRIMARY KEY NOT NULL,
	DriverLicenseNumber INT NOT NULL,
	FullName VARCHAR(40) NOT NULL,
	[Address] VARCHAR(200),
	City VARCHAR(40),
	ZIPCode INT,
	Notes VARCHAR(MAX)
)

INSERT INTO Customers VALUES
(1, 8934764, 'Ivan Ivanov', NULL, NULL, NULL, NULL),
(2, 6753828, 'Petar Petrov', NULL, NULL, NULL, NULL),
(3, 8736273, 'Ani Ivanova', NULL, NULL, NULL, NULL)

CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	CustomerId INT NOT NULL,
	CarId INT NOT NULL,
	TankLevel INT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT,
	StartDate Date,
	EndDate Date,
	TotalDays INT,
	RateApplied INT,
	TaxRate Decimal (10,2),
	OrderStatus BIT,
	Notes VARCHAR(MAX)
)

INSERT INTO RentalOrders VALUES
(1, 2, 3, 4, 100, 0, 200, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(2, 5, 2, 6, 80, 0, 260, 300, NULL, NULL, NULL, NULL, NULL, 1, NULL),
(3, 6, 1, 7, 60, 0, 190, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL)