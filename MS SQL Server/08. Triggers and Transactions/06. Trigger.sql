CREATE TRIGGER tr_RestrictItems on UserGameItems INSTEAD OF INSERT
AS
	DECLARE @itemId INT = (SELECT ItemId FROM inserted)
	DECLARE @userGameId INT = (SELECT UserGameId FROM inserted)

	DECLARE @itemLevel INT = (SELECT MinLevel FROM Items WHERE Id = @itemId)
	DECLARE @userGameLevel INT = (SELECT Level FROM UsersGames WHERE Id = @userGameId)

	IF (@userGameLevel >= @itemLevel)
	BEGIN
		INSERT INTO UserGameItems (ItemId, UserGameId) VALUES
		(@itemId, @userGameId)
	END
GO

SELECT *
	FROM Users AS u
	JOIN UsersGames AS ug ON ug.UserId = u.Id
	JOIN Games AS g ON g.Id = ug.GameId
	WHERE g.Name = 'Bali' AND u.Username IN ('baleremuda', 'loosenoise', ', inguinalself',
	'buildingdeltoid', 'monoxidecos')

UPDATE UsersGames
SET Cash += 50000
WHERE GameId = (SELECT Id FROM Games WHERE Name = 'Bali') AND 
		UserId IN (SELECT Id FROM Users WHERE Username IN 
		('baleremuda', 'loosenoise', ', inguinalself', 'buildingdeltoid',
		'monoxidecos'))

DECLARE @itemId INT = 251;

WHILE (@itemId <= 299)
BEGIN
	EXEC usp_BuyItem 22, 2@itemId, 212
	EXEC usp_BuyItem 37, 2@itemId, 212
	EXEC usp_BuyItem 52, 2@itemId, 212
	EXEC usp_BuyItem 61, 2@itemId, 212

	SET @itemId += 1;
END

CREATE PROC usp_BuyItems @userId INT, @itemId INT, @gameId INT
AS
BEGIN TRANSACTION 

	DECLARE @user INT = (SELECT Id FROM Users WHERE Id = @userId)
	DECLARE @item INT = (SELECT Id FROM Items WHERE Id = @itemId)

	IF (@user IS NULL OR @item IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('Invalid user or item id!', 16, 1)
		RETURN
	END

	DECLARE @userCash DECIMAL(18,2) = (SELECT Cash FROM UsersGames 
		WHERE UserId = @userId AND GameId = @gameId)
	DECLARE @itemPrice DECIMAL(18,2) = (SELECT Price FROM Items 
		WHERE Id = @itemId) 

	IF (@userCash - @itemPrice < 0)
	BEGIN
		ROLLBACK
		RAISERROR('Insufficient funds!', 16, 1)
		RETURN
	END

	UPDATE UsersGames
	SET Cash -= @itemPrice
	WHERE @userId = @userId AND GameId = @gameId

	DECLARE @userGameId INT = (SELECT Id FROM UsersGames 
		WHERE UserId = @userId AND GameId = @gameId)

	INSERT INTO UserGameItems (ItemId, UserGameId) VALUES 
	(@itemId, @userGameId)

COMMIT

SELECT u.Username, g.Name, ug.Cash, i.Name
	FROM Users AS u
	JOIN UsersGames AS ug ON ug.UserId = u.Id
	JOIN Games AS g ON g.Id = ug.Id
	JOIN UserGameItems AS ugi ON ugi.UserGameId = ug.Id 
	JOIN Items AS i ON i.Id = ugi.ItemId
	WHERE g.Name = 'Bali'
ORDER BY u.Username, i.Name