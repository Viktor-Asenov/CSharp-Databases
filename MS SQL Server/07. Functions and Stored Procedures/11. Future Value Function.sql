CREATE FUNCTION ufn_CalculateFutureValue
(@sum DECIMAL(15,4), @yearlyInterest FLOAT, @years INT)
RETURNS DECIMAL(15,4)
BEGIN
	DECLARE @result DECIMAL(15,4) = 
	@sum * (POWER((1 + @yearlyInterest), @years));

	RETURN @result;
END

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5)