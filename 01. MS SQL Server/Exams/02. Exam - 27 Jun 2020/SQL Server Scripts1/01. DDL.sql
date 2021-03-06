CREATE DATABASE WMS
USE WMS

CREATE TABLE Clients
(
	ClientId INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Phone CHAR(12) CHECK(LEN(Phone) = 12) NOT NULL
)

CREATE TABLE Mechanics
(
	MechanicId INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(250) NOT NULL
)

CREATE TABLE Models
(
	ModelId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Jobs
(
	JobId INT IDENTITY PRIMARY KEY,
	ModelId INT FOREIGN KEY REFERENCES Models (ModelId) NOT NULL,
	[Status] VARCHAR(11) DEFAULT 'Pending'
		CHECK([Status] IN ('Pending', 'In Progress', 'Finished')) NOT NULL,
	ClientId INT FOREIGN KEY REFERENCES Clients (ClientId) NOT NULL,
	MechanicId INT FOREIGN KEY REFERENCES Mechanics (MechanicId),
	IssueDate DATE NOT NULL,
	FinishDate DATE
)

CREATE TABLE Orders
(
	OrderId INT IDENTITY PRIMARY KEY,
	JobId INT FOREIGN KEY REFERENCES Jobs (JobId) NOT NULL,
	IssueDate DATE,
	Delivered BIT DEFAULT 0
)

CREATE TABLE Vendors
(
	VendorId INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts
(
	PartId INT IDENTITY PRIMARY KEY,
	SerialNumber VARCHAR(50) UNIQUE NOT NULL,
	[Description] VARCHAR(255),
	Price DECIMAL(15,2) CHECK(Price > 0 AND Price <= 9999.99) NOT NULL,
	VendorId INT FOREIGN KEY REFERENCES Vendors (VendorId) NOT NULL,
	StockQty INT  DEFAULT 0 CHECK(StockQty >= 0) NOT NULL
)

CREATE TABLE OrderParts
(
	OrderId INT FOREIGN KEY REFERENCES Orders (OrderId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts (PartId) NOT NULL,
	Quantity INT DEFAULT 1 CHECK(Quantity > 0) NOT NULL,

	CONSTRAINT PK_OrdersParts
	PRIMARY KEY (OrderId, PartId)
)

CREATE TABLE PartsNeeded
(
	JobId INT FOREIGN KEY REFERENCES Jobs (JobId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts (PartId) NOT NULL,
	Quantity INT CHECK(Quantity > 0) DEFAULT 1 NOT NULL,

	CONSTRAINT PK_JobsParts
	PRIMARY KEY (JobId, PartId)
)