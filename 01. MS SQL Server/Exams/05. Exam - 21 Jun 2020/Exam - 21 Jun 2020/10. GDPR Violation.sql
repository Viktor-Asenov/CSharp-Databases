SELECT t.Id,
       a.FirstName + ' ' + ISNULL(a.MiddleName + ' ', '') + a.LastName AS FullName,
       c.Name,
       ci.Name,
       IIF(t.CancelDate IS NOT NULL, 'Canceled',
           CAST(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate) AS NVARCHAR(100)) + ' days') AS Duration
    FROM Trips AS t
    JOIN AccountsTrips AS at ON t.Id = at.TripId
    JOIN Accounts AS a ON a.Id = at.AccountId
    JOIN Cities AS c ON c.Id = a.CityId
    JOIN Rooms AS r ON r.Id = t.RoomId
    JOIN Hotels AS h ON h.Id = r.HotelId
    JOIN Cities AS ci ON ci.Id = h.CityId
    ORDER BY FullName, t.Id