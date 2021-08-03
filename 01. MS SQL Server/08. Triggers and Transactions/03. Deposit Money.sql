CREATE PROC usp_DepositMoney(@accountId INT, @moneyAmount DECIMAL(18,4))
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
		SET Balance += @moneyAmount
		FROM Accounts
		WHERE Id = @accountId
	COMMIT
GO

