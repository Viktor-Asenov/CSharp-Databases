CREATE PROC usp_GetTownsStartingWith @string NVARCHAR(MAX)
AS
	SELECT Name
		FROM Towns AS t
		WHERE t.Name LIKE @string + '%'
GO

EXEC usp_GetTownsStartingWith 'b'