SELECT CONCAT(m.FirstName, ' ', m.LastName),
	AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS AverageDays
	FROM Mechanics AS m
	JOIN Jobs AS j ON j.MechanicId = m.MechanicId
	GROUP BY m.FirstName, m.LastName, m.MechanicId
	ORDER BY m.MechanicId