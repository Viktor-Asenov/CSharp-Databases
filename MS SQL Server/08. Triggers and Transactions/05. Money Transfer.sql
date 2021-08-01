CREATE PROC usp_TransferMoney
(@senderId INT, @receiverId INT, @amount DECIMAL(18,4))
AS
BEGIN TRANSACTION
	EXEC usp_WithdrawMoney @senderId, @amount
	EXEC usp_DepositMoney @receiverID, @amount
	COMMIT
GO


