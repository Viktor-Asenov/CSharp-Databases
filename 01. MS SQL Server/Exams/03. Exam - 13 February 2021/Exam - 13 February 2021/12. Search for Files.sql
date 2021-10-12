CREATE PROCEDURE usp_SearchForFiles(@fileExtension VARCHAR(10))
AS
BEGIN
	SELECT f.Id, f.Name, CAST(f.Size AS VARCHAR(50)) + 'KB'
		FROM Files AS f
		WHERE f.Name LIKE ('%' + @fileExtension)
		ORDER BY f.Id, f.Name, f.Size DESC
END