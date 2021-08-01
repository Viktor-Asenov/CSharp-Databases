CREATE PROC usp_WithdrawMoney(@accountId INT, @moneyAmount DECIMAL(18,4))
AS
BEGIN TRANSACTION

	IF (@accountId IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Invalid account id!', 16, 1)
		RETURN
	END

	IF (@moneyAmount < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Money amount cannot be negative!', 16, 1)
		RETURN
	END

	UPDATE Accounts
		SET Balance -= @moneyAmount
		FROM Accounts
		WHERE Id = @accountId
	COMMIT
GO

-- Testing before and after executing the procedure

SELECT a.Id, ah.Id, a.Balance
	FROM Accounts AS a
	JOIN AccountHolders AS ah ON ah.Id = a.AccountHolderId
	WHERE a.Id = 5

EXEC usp_WithdrawMoney 5, 25

SELECT a.Id, ah.Id, a.Balance
	FROM Accounts AS a
	JOIN AccountHolders AS ah ON ah.Id = a.AccountHolderId
	WHERE a.Id = 5