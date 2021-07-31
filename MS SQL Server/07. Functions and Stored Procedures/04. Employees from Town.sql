CREATE PROC usp_GetEmployeesFromTown @townName NVARCHAR(MAX)
AS
	SELECT e.FirstName, e.LastName
		FROM Employees AS e
		JOIN Addresses AS a ON e.AddressID = a.AddressID
		JOIN Towns AS t ON t.TownID = a.TownID
	WHERE t.Name = @townName
GO

EXEC usp_GetEmployeesFromTown 'Sofia'