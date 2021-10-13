CREATE VIEW v_UserWithCountries 
AS
	SELECT CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
			c.Age,
			c.Gender,
			co.Name
		FROM Customers AS c
		LEFT JOIN Countries AS co ON co.Id = c.CountryId
GO