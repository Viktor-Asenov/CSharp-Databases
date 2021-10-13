CREATE TRIGGER tr_DeleteProducts
    ON Products
	INSTEAD OF DELETE
AS
BEGIN
    DECLARE @deletedProducts INT = (
        SELECT p.Id
            FROM Products AS p
                     JOIN deleted AS d
                          ON p.Id = d.Id)

    DELETE
        FROM Feedbacks
        WHERE ProductId = @deletedProducts

    DELETE
        FROM ProductsIngredients
        WHERE ProductId = @deletedProducts

    DELETE Products
        WHERE Id = @deletedProducts
END