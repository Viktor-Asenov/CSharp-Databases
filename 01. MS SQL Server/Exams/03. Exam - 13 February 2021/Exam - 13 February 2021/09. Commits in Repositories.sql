SELECT TOP(5) r.Id, r.Name, COUNT(c.Id)
	FROM Repositories AS r
	JOIN Commits AS c ON c.RepositoryId = r.Id
	JOIN RepositoriesContributors AS rc ON r.Id = rc.RepositoryId
	GROUP BY r.Id, r.Name
	ORDER BY COUNT(c.Id) DESC, r.Id, r.Name
	