SELECT CONCAT(c.FirstName, ' ', c.LastName), 
	DATEDIFF(DAY, j.IssueDate, '2017-04-24') AS DaysGoing, j.Status
	FROM Jobs AS j
	JOIN Clients AS c ON c.ClientId = j.ClientId
	WHERE j.Status != 'Finished'
	ORDER BY DaysGoing DESC, c.ClientId