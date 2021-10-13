SELECT CONCAT(c.FirstName, ' ', c.LastName), c.PhoneNumber, c.Gender
	FROM Customers AS c
	LEFT JOIN Feedbacks AS f ON f.CustomerId = c.Id
	WHERE f.CustomerId IS NULL
	ORDER BY c.Id