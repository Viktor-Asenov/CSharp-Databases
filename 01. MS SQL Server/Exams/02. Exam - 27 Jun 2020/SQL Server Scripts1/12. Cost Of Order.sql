CREATE FUNCTION udf_GetCost (@JobId INT)
RETURNS DECIMAL(15,2)
AS
BEGIN
	DECLARE @totalCost DECIMAL(15,2);
	
	IF (@JobId > 0)
		SET @totalCost = (SELECT IIF(SUM(p.Price) IS NULL, 0, SUM(p.Price))
							FROM Jobs AS j
							JOIN Orders AS o ON o.JobId = j.JobId
							JOIN OrderParts AS op ON op.OrderId = o.OrderId
							JOIN Parts AS p ON p.PartId = op.PartId
							WHERE o.JobId = @JobId)

	RETURN @totalCost
END