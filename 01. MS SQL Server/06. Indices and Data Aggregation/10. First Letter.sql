SELECT SUBSTRING(FirstName, 1, 1)
	FROM WizzardDeposits AS wd
	WHERE DepositGroup = 'Troll chest'
	GROUP BY SUBSTRING(FirstName, 1, 1)
	ORDER BY SUBSTRING(FirstName, 1, 1)
