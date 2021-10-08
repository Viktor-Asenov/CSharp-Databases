SELECT TOP(5) c.Name, COUNT(CategoryId)
	FROM Reports AS r
	JOIN Categories AS c ON c.Id = r.CategoryId
	GROUP BY CategoryId, Name
	ORDER BY COUNT(CategoryId) DESC, Name
	