CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT AS
BEGIN
	DECLARE @totalHours INT;

	IF @StartDate IS NULL
		RETURN 0
	ELSE IF @EndDate IS NULL
		RETURN 0
	ELSE
		SET @totalHours = DATEDIFF(HOUR, @StartDate, @EndDate)

	RETURN @totalHours;
END