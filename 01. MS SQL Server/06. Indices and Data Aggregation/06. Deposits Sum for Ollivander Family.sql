SELECT wd.DepositGroup, SUM(DepositAmount)	
	FROM WizzardDeposits AS wd
	WHERE wd.MagicWandCreator = 'Ollivander Family'  
	GROUP BY DepositGroup
	