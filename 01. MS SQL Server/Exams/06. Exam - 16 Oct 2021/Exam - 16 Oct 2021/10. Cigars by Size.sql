SELECT c.LastName, AVG(s.Length), CEILING(AVG(s.RingRange))
	FROM Clients AS c
	JOIN ClientsCigars AS cc ON cc.ClientId = c.Id
	JOIN Cigars AS ci ON ci.Id = cc.CigarId
	JOIN Sizes AS s ON s.Id = ci.SizeId
	GROUP BY c.LastName
	ORDER BY AVG(s.Length) DESC