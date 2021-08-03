CREATE DATABASE Movies
Use Movies

CREATE TABLE Directors
(
	Id INT PRIMARY KEY NOT NULL,
	DirectorName VARCHAR(40) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Directors VALUES
(1, 'Ivan', NULL),
(2, 'Pesho', NULL),
(3, 'Gosho', NULL),
(4, 'Stamat', NULL),
(5, 'Rado', NULL)

CREATE TABLE Genres
(
	Id INT PRIMARY KEY NOT NULL,
	GenreName VARCHAR(20) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Genres VALUES
(1, 'Horror', NULL),
(2, 'Comedy', NULL),
(3, 'Triller', NULL),
(4, 'Historical', NULL),
(5, 'Drama', NULL)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY NOT NULL,
	CategoryName VARCHAR(20) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Categories VALUES
(1, 'Horror', NULL),
(2, 'Comedy', NULL),
(3, 'Triller', NULL),
(4, 'Historical', NULL),
(5, 'Drama', NULL)

CREATE TABLE Movies
(
	Id INT PRIMARY KEY NOT NULL,
	Title VARCHAR(50) NOT NULL,
	DirectorId INT NOT NULL,
	CopyrightYear INT NOT NULL,
	Lenght FLOAT,
	GenreId INT,
	CategoryId INT,
	Rating INT,
	Notes VARCHAR(MAX)
)

INSERT INTO Movies VALUES
(1, 'Troy', 1, 1999, NULL, 2, 3, 9, NULL),
(2, 'Inception', 1, 2001, NULL, 2, 3, 6, NULL),
(3, 'Lord Of The Rings', 1, 2005, NULL, 2, 3, 10, NULL),
(4, 'The Shawshenk Redemption', 1, 1997, NULL, 2, 3, 8, NULL),
(5, 'Fast And Furious', 1, 1986, NULL, 2, 3, 7, NULL)
