SELECT TOP(5) e.EmployeeID, e.JobTitle, e.AddressID, a.AddressText
	FROM Employees AS e	
	JOIN Addresses AS a ON a.AddressID = e.EmployeeID
	ORDER BY a.AddressID