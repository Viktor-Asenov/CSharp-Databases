CREATE PROC usp_AssignProject(@emloyeeId INT, @projectId INT)
AS
BEGIN TRANSACTION tr_AssignProject

	DECLARE @employeeProjectsCount INT = 
		(SELECT COUNT(ProjectID) FROM EmployeesProjects WHERE EmployeeID = @emloyeeId)

	IF (@employeeProjectsCount >= 3)
	BEGIN
		ROLLBACK
		RAISERROR('The employee has too many projects!', 16, 1)
		RETURN
	END

		INSERT INTO EmployeesProjects (EmployeeID, ProjectID) VALUES
		(@emloyeeId, @projectId	)

	COMMIT
GO

