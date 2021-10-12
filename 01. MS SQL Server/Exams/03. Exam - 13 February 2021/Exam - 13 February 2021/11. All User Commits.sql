CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(50))
RETURNS INT AS
BEGIN

	DECLARE @countCommits INT = (SELECT COUNT(c.Id)
								 FROM Users AS u
								 LEFT JOIN Commits AS c ON c.ContributorId = u.Id
								 WHERE u.Username = @username
								 GROUP BY u.Id)

	IF @countCommits IS NULL
		RETURN 0

	RETURN @countCommits;
END