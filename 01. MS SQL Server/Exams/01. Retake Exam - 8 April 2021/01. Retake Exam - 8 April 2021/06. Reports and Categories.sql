SELECT Description, Name
	FROM Reports AS r
	JOIN Categories AS c ON c.Id = r.CategoryId
	WHERE CategoryId IS NOT NULL
	ORDER BY Description, Name