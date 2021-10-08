SELECT u.Username, c.Name
	FROM Reports AS r
	JOIN Users AS u ON u.Id = r.UserId
	JOIN Categories AS c ON c.Id = r.CategoryId
	WHERE FORMAT(r.OpenDate, 'dd -MM') = FORMAT(u.Birthdate, 'dd -MM')
	ORDER BY u.Username, c.Name

