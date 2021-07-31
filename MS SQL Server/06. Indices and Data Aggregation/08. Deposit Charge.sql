SELECT wd.DepositGroup, wd.MagicWandCreator, MIN(DepositCharge)
	FROM WizzardDeposits AS wd
	GROUP BY wd.DepositGroup, wd.MagicWandCreator
	ORDER BY wd.MagicWandCreator, wd.DepositGroup