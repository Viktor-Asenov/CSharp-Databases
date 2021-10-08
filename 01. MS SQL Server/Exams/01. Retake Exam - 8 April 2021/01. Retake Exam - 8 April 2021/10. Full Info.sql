SELECT
	CASE
		WHEN e.FirstName IS NULL THEN 'None'
		WHEN e.LastName IS NULL THEN 'None'
		ELSE CONCAT(e.FirstName, ' ', e.LastName)
	END AS Employee,
	ISNULL(d.Name, 'None') AS Department,
	ISNULL(c.Name, 'None') AS Category,
	ISNULL(r.Description, 'None') AS [Description],
	ISNULL(FORMAT(r.OpenDate, 'dd.MM.yyyy'), 'None') AS OpenDate,
	ISNULL(s.Label, 'None') AS Status,
	ISNULL(u.Name, 'None') AS [User]
	FROM Reports AS r
	LEFT JOIN Categories AS c ON c.Id = r.CategoryId
	LEFT JOIN Status AS s ON  s.Id = r.StatusId
	LEFT JOIN Users AS u ON u.Id = r.UserId
	LEFT JOIN Employees AS e ON e.Id = r.EmployeeId
	LEFT JOIN Departments AS d ON d.Id = e.DepartmentId
	ORDER BY e.FirstName DESC, e.LastName DESC,
	d.Name, c.Name, r.Description, r.OpenDate,
	s.Label, u.Name
	

