CREATE TABLE Logs
(
	ID INT IDENTITY PRIMARY KEY NOT NULL,
	AccountId INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL,
	OldSum DECIMAL(18,4) NOT NULL,
	NewSum DECIMAL(18,4) NOT NULL
)

CREATE TRIGGER tr_AddEntryInLogsAfterBalanceChange ON Accounts
	FOR UPDATE
AS
	DECLARE @oldSum DECIMAL(18,2) = (SELECT Balance FROM deleted)
	DECLARE @newSum DECIMAL(18,2) = (SELECT Balance FROM inserted)
	DECLARE @accountId INT = (SELECT Id FROM inserted)

	INSERT INTO Logs(AccountId, OldSum, NewSum) 
	VALUES(@accountId, @oldSum, @newSum)
GO