CREATE FUNCTION udf_GetAvailableRoom (@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @RoomId INT = (SELECT TOP 1 r.Id
                               FROM Trips AS t
                                        JOIN Rooms AS r
                                             ON t.RoomId = r.Id
                                        JOIN Hotels AS h
                                             ON r.HotelId = h.Id
                               WHERE h.Id = @HotelId
                                 AND @Date NOT BETWEEN t.ArrivalDate AND t.ReturnDate
                                 AND t.CancelDate IS NULL
                                 AND r.Beds >= @People
                                 AND YEAR(@Date) = YEAR(t.ArrivalDate)
                               ORDER BY r.Price DESC)

    IF @RoomId IS NULL
        RETURN 'No rooms available'

    DECLARE @RoomPrice DECIMAL(15, 2) = (SELECT Price
                                             FROM Rooms
                                             WHERE Id = @RoomId)

    DECLARE @RoomType VARCHAR(50) = (SELECT Type
                                         FROM Rooms
                                         WHERE Id = @RoomId)

    DECLARE @BedsCount INT = (SELECT Beds
                                  FROM Rooms
                                  WHERE Id = @RoomId)

    DECLARE @HotelBaseRate DECIMAL(15, 2) = (SELECT BaseRate
                                                 FROM Hotels
                                                 WHERE Id = @HotelId)

    DECLARE @TotalPrice DECIMAL(15, 2) = (@HotelBaseRate + @RoomPrice) * @People

    RETURN CONCAT('Room ', @RoomId, ': ', @RoomType, ' (', @BedsCount, ' beds', ') - $', @TotalPrice)
END