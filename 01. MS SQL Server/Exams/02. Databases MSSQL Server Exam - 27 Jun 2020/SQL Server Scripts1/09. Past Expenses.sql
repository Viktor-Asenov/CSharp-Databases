SELECT j.JobId, SUM(p.Price)
	FROM PartsNeeded AS pn
	LEFT JOIN Jobs AS j ON j.JobId = pn.JobId
	LEFT JOIN Parts AS p ON p.PartId = pn.PartId
	WHERE Status = 'Finished'
	GROUP BY j.JobId
	ORDER BY SUM(p.Price) DESC, j.JobId