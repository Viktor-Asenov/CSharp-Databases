CREATE TABLE Passports
(
	PassportID INT PRIMARY KEY NOT NULL,
	PassportNumber CHAR(8)
)

CREATE TABLE Persons
(
	PersonID INT IDENTITY PRIMARY KEY,
	FirstName NVARCHAR(20) NOT NULL,
	Salary DECIMAL(7,2) NOT NULL,
	PassportID INT NOT NULL

	CONSTRAINT FK_Persons_Passports
        FOREIGN KEY (PassportID)
            REFERENCES Passports (PassportID)
)

INSERT INTO Passports (PassportID, PassportNumber)
VALUES (101, 'N34FG21B'),
		(102, 'K65LO4R7'),
		(103, 'ZE657QP2')

INSERT INTO Persons (FirstName, Salary, PassportID) 
VALUES ('Roberto', 43300.00, 102),
		('Tom', 56100.00, 103),
		('Yana', 60200.00, 101)