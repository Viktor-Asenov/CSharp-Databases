CREATE TABLE People
(
	Id INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(3,2),
	[Weight] DECIMAL(5,2),
	Gender CHAR NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People 
([Name], Picture, Height, [Weight], Gender, Birthdate, Biography)
VALUES
('Pesho', 200, 1.76, 78.00, 'm', '1993-10-11', 'Random guy'),
('Siana', 160, 1.60, 55.00, 'f', '1995-11-11', 'Random girl'),
('Vasil', 175, 1.80, 76.00, 'm', '1991-10-15', 'Random guy'),
('Tisho', 193, 1.90, 95.00, 'm', '1989-04-16', 'Random guy'),
('Angela', 156, 1.63, 56.00, 'f', '1996-01-03', 'Random girl')

		
