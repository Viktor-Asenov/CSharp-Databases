CREATE OR ALTER PROCEDURE usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
BEGIN
    IF ((SELECT TOP(1) h.Id
            FROM Trips AS t
            JOIN Rooms AS r ON r.Id = t.RoomId
            JOIN Hotels AS h ON h.Id = r.HotelId
            WHERE t.Id = @TripId) != (SELECT HotelId
                                         FROM Rooms
                                         WHERE Id = @TargetRoomId))
        THROW 50001, 'Target room is in another hotel!', 1

    IF ((SELECT Beds
            FROM Rooms
            WHERE Id = @TargetRoomId ) < (SELECT COUNT(*) AS Count
                                           FROM AccountsTrips
                                           WHERE TripId = @TripId))
        THROW 50002, 'Not enough beds in target room!', 1

    UPDATE Trips
    SET RoomId = @TargetRoomId
    WHERE Id = @TripId
END
