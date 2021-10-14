SELECT c.Name, COUNT(h.Id)
	FROM Cities AS c
	JOIN Hotels AS h ON h.CityId = c.Id
	GROUP BY c.Name
	ORDER BY COUNT(h.Id) DESC, c.Name