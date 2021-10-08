--TODO: Implement better decision
CREATE OR ALTER PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
	DECLARE @EmployeeDepartment NVARCHAR(30);	
	DECLARE @ReportCategoryDepartment NVARCHAR(30);	

	SET @EmployeeDepartment = 
	(SELECT d.Name
	FROM Employees AS e
	JOIN Departments AS d ON d.Id = e.DepartmentId
	WHERE e.Id = @EmployeeId)

	SET @ReportCategoryDepartment = 
	(SELECT d.Name
	FROM Reports AS r
	JOIN Categories AS c ON c.Id = r.CategoryId
	JOIN Departments AS d ON d.Id = c.DepartmentId
	WHERE r.Id = @ReportId)

	BEGIN TRY
		IF @EmployeeDepartment = @ReportCategoryDepartment
		UPDATE Reports
		SET EmployeeId = @EmployeeId
		WHERE Id = @ReportId
	END TRY
	BEGIN CATCH
		ROLLBACK;
		RAISERROR('Employee doesn''t belong to the appropriate department!', 1, 16)
		RETURN
	END CATCH		
GO

EXEC usp_AssignEmployeeToReport 30, 1
