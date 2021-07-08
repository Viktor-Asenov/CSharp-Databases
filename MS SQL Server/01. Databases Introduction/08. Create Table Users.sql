CREATE TABLE Users 
(
	Id BIGINT IDENTITY PRIMARY KEY,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARCHAR(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT
)

INSERT INTO Users (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted)
VALUES
('Vasko', 'austere123', 'kakdkjs', '2021-06-30', 0),
('Tisho', 'strongman34', 'hsgkas', '2020-02-01', 0),
('Vankata', 'shofiora267', 'kakdkjs', '2021-08-05', 1),
('Toncho', 'toni34978', 'sjdka', '2019-01-09', 0),
('Ivo', 'masterofcomp29651', 'ysdkks', '2020-02-09', 1)