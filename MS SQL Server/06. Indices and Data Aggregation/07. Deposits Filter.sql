SELECT wd.DepositGroup, SUM(wd.DepositAmount) AS TotalSum	
	FROM WizzardDeposits AS wd
	WHERE wd.MagicWandCreator = 'Ollivander Family'
	GROUP BY DepositGroup
	HAVING SUM(wd.DepositAmount) < 150000
	ORDER BY TotalSum DESC