SELECT DepartmentID, MIN(Salary)
	FROM Employees
	WHERE DepartmentID IN (2, 5, 7)
	GROUP BY DepartmentID