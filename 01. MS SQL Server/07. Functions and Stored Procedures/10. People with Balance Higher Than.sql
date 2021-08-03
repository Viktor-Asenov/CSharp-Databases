CREATE PROC usp_GetHoldersWithBalanceHigherThan(@value DECIMAL(18,2))
AS
SELECT ah.FirstName, ah.LastName
	FROM AccountHolders AS ah
	JOIN Accounts AS a ON a.AccountHolderId = ah.Id
	GROUP BY ah.FirstName, ah.LastName
	HAVING SUM(Balance) > @value
	ORDER BY ah.FirstName, ah.LastName
GO

EXEC usp_GetHoldersWithBalanceHigherThan 100000

