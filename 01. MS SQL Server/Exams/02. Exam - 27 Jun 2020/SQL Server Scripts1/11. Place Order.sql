CREATE PROCEDURE usp_PlaceOrder 
	(@JobId INT, @SerialNumber VARCHAR(50), @Quantity INT)
AS

	DECLARE @status VARCHAR(15) = (SELECT Status 
									FROM Jobs WHERE JobId = @JobId)

	DECLARE @partId INT = (SELECT PartId 
									FROM Parts WHERE SerialNumber = @SerialNumber)

	
	IF (@Quantity <= 0)
		THROW 50012, 'Part quantity must be more than zero!', 1
	ELSE IF (@status = 'Finished')
		THROW 50011, 'This job is not active!', 1
	ELSE IF (@status IS NULL)
		THROW 50013, 'Job not found!', 1
	ELSE IF (@partId IS NULL)
		THROW 50014, 'Part not found!', 1

	DECLARE @orderId INT = (SELECT OrderId 
							FROM Orders WHERE JobId = @JobId)

	IF (@orderId IS NULL)
	BEGIN		
		INSERT INTO Orders (JobId, IssueDate) VALUES
		(@JobId, NULL)

		SET @orderId = (SELECT OrderId 
						FROM Orders WHERE JobId = @JobId
						AND IssueDate IS NULL)

		INSERT INTO OrderParts (OrderId, PartId, Quantity) VALUES
		(@orderId, @partId, @Quantity)
	END
	ELSE
	BEGIN
		DECLARE @issueDate DATE = (SELECT IssueDate 
									FROM Orders WHERE OrderId = @orderId
									AND JobId = @JobId)

		IF (@issueDate IS NULL)
			INSERT INTO OrderParts (OrderId, PartId, Quantity) VALUES
			(@orderId, @partId, @Quantity)
		ELSE
			UPDATE OrderParts
			SET Quantity += @Quantity
			WHERE OrderId = @orderId AND PartId = @partId
	END
