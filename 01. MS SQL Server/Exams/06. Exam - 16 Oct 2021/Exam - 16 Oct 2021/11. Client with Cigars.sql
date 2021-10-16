CREATE FUNCTION udf_ClientWithCigars(@name NVARCHAR(50))
RETURNS INT AS
BEGIN

	DECLARE @countCigars INT = (SELECT COUNT(ci.Id)
								FROM Clients AS c
								JOIN ClientsCigars AS cc ON cc.ClientId = c.Id
								JOIN Cigars AS ci ON ci.Id = cc.CigarId
								WHERE c.FirstName = @name
								GROUP BY c.Id)

	IF (@countCigars IS NULL)
		RETURN 0

	RETURN @countCigars;
END