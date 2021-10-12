SELECT u.Username, AVG(f.Size)
	FROM Commits AS c
	JOIN Users AS u ON u.Id = c.ContributorId
	JOIN Files AS f ON f.CommitId = c.Id
	WHERE c.ContributorId IS NOT NULL
	GROUP BY u.Username
	ORDER BY AVG(f.Size) DESC, u.Username
	