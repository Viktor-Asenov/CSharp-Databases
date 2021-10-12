SELECT f1.Id, f1.Name, CAST(f1.Size AS VARCHAR(50)) + 'KB'
	FROM Files AS f
	RIGHT JOIN Files AS f1 ON f.ParentId = f1.Id
	WHERE f.ParentId IS NULL
	ORDER BY f.Id, f.Name, f.Size DESC