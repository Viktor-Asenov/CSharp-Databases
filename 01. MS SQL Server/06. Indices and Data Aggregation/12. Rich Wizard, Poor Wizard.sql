SELECT SUM(	Guest.DepositAmount - Host.DepositAmount)
	FROM WizzardDeposits AS Host
	JOIN WizzardDeposits AS Guest ON Guest.Id + 1 = Host.Id