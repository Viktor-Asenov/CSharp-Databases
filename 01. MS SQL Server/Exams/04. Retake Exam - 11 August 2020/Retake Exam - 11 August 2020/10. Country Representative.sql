SELECT k.CountryName, k.DisributorName
	FROM(SELECT 
		c.Name AS CountryName,
		d.Name AS DisributorName,
		DENSE_RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(I.Id) DESC )
			AS Ranking
		FROM Countries AS c
		LEFT JOIN Distributors AS d ON d.CountryId = c.Id
		LEFT JOIN Ingredients AS i ON i.DistributorId = d.Id
		GROUP BY c.Name, d.Name) AS k
	WHERE Ranking = 1
	ORDER BY k.CountryName, k.DisributorName