--Implement correct decision
SELECT c.Name, COUNT(c.Counted)
	FROM(
	SELECT CONCAT(e.FirstName, ' ', e.LastName) AS Name,
		   COUNT(u.Id) AS Counted
	FROM Reports AS r
	JOIN Employees AS e ON e.Id = r.EmployeeId
	JOIN Users AS u ON u.Id = r.UserId 
	GROUP BY CONCAT(e.FirstName, ' ', e.LastName), u.Id) AS c
	ORDER BY c.Counted DESC, Name