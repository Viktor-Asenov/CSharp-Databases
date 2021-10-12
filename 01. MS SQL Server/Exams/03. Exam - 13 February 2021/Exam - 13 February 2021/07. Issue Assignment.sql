SELECT i.Id, CONCAT(u.Username, ' : ', i.Title)
	FROM Issues AS i
	JOIN Users AS u on u.Id = i.AssigneeId
	ORDER BY i.Id DESC, CONCAT(u.Username, ' : ', i.Title)
