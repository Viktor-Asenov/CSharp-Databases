CREATE DATABASE UniversityDatabase
USE UniversityDatabase

CREATE TABLE Majors
(
	MajorID INT IDENTITY PRIMARY KEY,
	[Name] VARCHAR(50)
)

CREATE TABLE Students
(
	StudentID INT IDENTITY PRIMARY KEY,
	StudentNumber CHAR(10) NOT NULL,
	StudentName VARCHAR(50) NOT NULL,
	MajorID INT NOT NULL

	CONSTRAINT FK_Students_Majors
		FOREIGN KEY (MajorID)
			REFERENCES Majors (MajorID)
)

CREATE TABLE Payments
(
	PaymentID INT IDENTITY PRIMARY KEY,
	PaymentDate DATE NOT NULL,
	PaymentAmount DECIMAL(10,2),
	StudentID INT NOT NULL

	CONSTRAINT FK_Payments_Students
		FOREIGN KEY (StudentID)
			REFERENCES Students (StudentID)
)

CREATE TABLE Subjects
(
	SubjectID INT IDENTITY PRIMARY KEY,
	SubjectName VARCHAR(50) NOT NULL
)

CREATE TABLE Agenda
(
	StudentID INT,
	SubjectID INT

	CONSTRAINT PK_StudentID_SubjectID
		PRIMARY KEY (StudentID, SubjectID)

	CONSTRAINT FK_Agenda_Students
		FOREIGN KEY (StudentID)
			REFERENCES Students (StudentID),

	CONSTRAINT FK_Agenda_Subjects
		FOREIGN KEY (SubjectID)
			REFERENCES Subjects (SubjectID)
)
