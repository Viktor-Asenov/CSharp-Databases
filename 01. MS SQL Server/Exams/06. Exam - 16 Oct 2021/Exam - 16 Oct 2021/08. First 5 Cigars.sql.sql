SELECT TOP(5) CigarName, PriceForSingleCigar, ImageURL
	FROM Cigars AS c
	JOIN Sizes AS s ON s.Id = c.SizeId
	WHERE 
	s.Length >= 12
	AND (CigarName LIKE ('%ci%')
	OR PriceForSingleCigar > 50)
	AND s.RingRange > 2.55
	ORDER BY CigarName, PriceForSingleCigar DESC