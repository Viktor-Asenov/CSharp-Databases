CREATE FUNCTION dbo.ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS NVARCHAR(10)
BEGIN
	DECLARE @result NVARCHAR(10);
	
	IF @salary < 30000 
		SET @result = 'Low'
	ELSE IF @salary <= 50000
		SET @result = 'Average'
	ELSE 
		SET @result = 'High'

	RETURN @result
END
GO

SELECT Salary, dbo.ufn_GetSalaryLevel(Salary)
	FROM Employees
