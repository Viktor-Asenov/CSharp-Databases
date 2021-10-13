SELECT d.Name, i.Name, p.Name, AVG(f.Rate)
	FROM ProductsIngredients AS pri
	JOIN Products AS p ON p.Id = pri.ProductId
	JOIN Feedbacks AS f ON f.ProductId = p.Id
	JOIN Ingredients AS i ON i.Id = pri.IngredientId
	JOIN Distributors AS d ON d.Id = i.DistributorId
	GROUP BY p.Id, p.Name, d.Name, i.Name
	HAVING AVG(f.Rate) BETWEEN 5 AND 8
	ORDER BY d.Name, i.Name, p.Name