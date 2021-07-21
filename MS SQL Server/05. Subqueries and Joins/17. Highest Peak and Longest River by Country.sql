SELECT TOP(5) CountryName, MAX(p.Elevation) AS HighestPeak, 
	MAX(r.Length) AS LongestRiver
	FROM Countries AS c
		LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
		LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
		LEFT JOIN Peaks AS p ON p.MountainId = m.Id
		LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
		LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
GROUP BY c.CountryName
ORDER BY HighestPeak DESC, LongestRiver DESC, CountryName