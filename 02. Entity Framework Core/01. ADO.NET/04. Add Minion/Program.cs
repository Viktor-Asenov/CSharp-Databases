﻿namespace _04._Add_Minion
{
    using System;
    using Microsoft.Data.SqlClient;

    public class Program
    {
        static void Main(string[] args)
        {
            const string SqlConnection = "Server=.;Database=MinionsDB;Integrated Security=true";

            using var connection = new SqlConnection(SqlConnection);
            connection.Open();

            string[] minionArgs = Console.ReadLine()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string minionName = minionArgs[1];
            int age = int.Parse(minionArgs[2]);
            string town = minionArgs[3];

            string[] villainArgs = Console.ReadLine()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string villainName = villainArgs[1];

            int? townId = GetTownId(connection, town);
            int? villainId = GetVillainId(connection, villainName);

            if (townId == null)
            {
                string createTownQuery = "INSERT INTO Towns (Id, Name) VALUES (@name)";
                using var sqlCommand = new SqlCommand(createTownQuery, connection);
                sqlCommand.Parameters.AddWithValue("@name", town);
                sqlCommand.ExecuteNonQuery();
                townId = GetTownId(connection, town);

                Console.WriteLine($"Town {town} was added to the database.");
            }

            if (villainId == null)
            {
                string createVillain = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                using var sqlCommand = new SqlCommand(createVillain, connection);
                sqlCommand.Parameters.AddWithValue("@villainName", villainName);
                sqlCommand.ExecuteNonQuery();
                villainId = GetVillainId(connection, villainName);

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }

            CreateMinion(connection, minionName, age, townId);

            var minionId = GetMinionId(connection, minionName);

            InsertIntoMinionsVillains(connection, villainId, minionId);

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static void InsertIntoMinionsVillains(SqlConnection connection, int? villainId, int? minionId)
        {
            string insertIntoMinionVillainsQuery = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";
            var sqlCommand = new SqlCommand(insertIntoMinionVillainsQuery, connection);
            sqlCommand.Parameters.AddWithValue("@villainId", minionId);
            sqlCommand.Parameters.AddWithValue("@minionId", villainId);
            sqlCommand.ExecuteNonQuery();
        }

        private static int? GetMinionId(SqlConnection connection, string minionName)
        {
            string minionQuery = "SELECT Id FROM Minions WHERE Name = @Name";
            var sqlCommand = new SqlCommand(minionQuery, connection);
            sqlCommand.Parameters.AddWithValue("@Name", minionName);
            var minionId = sqlCommand.ExecuteScalar();

            return (int?)minionId;
        }

        private static void CreateMinion(SqlConnection connection, string minionName, int age, int? townId)
        {
            string createMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
            var sqlCommand = new SqlCommand(createMinion, connection);
            sqlCommand.Parameters.AddWithValue("@name", minionName);
            sqlCommand.Parameters.AddWithValue("@age", age);
            sqlCommand.Parameters.AddWithValue("@townId", townId);
            sqlCommand.ExecuteNonQuery();
        }

        private static int? GetVillainId(SqlConnection connection, string villainName)
        {
            string query = "SELECT Id FROM Villains WHERE Name = @Name";
            using var sqlCommand = new SqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Name", villainName);
            var villainId = sqlCommand.ExecuteScalar();

            return (int?)villainId;
        }

        private static int? GetTownId(SqlConnection connection, string town)
        {
            string townIdQuery = "SELECT Id FROM Towns WHERE Name = @townName";
            var sqlCommand = new SqlCommand(townIdQuery, connection);
            sqlCommand.Parameters.AddWithValue("@townName", town);
            sqlCommand.ExecuteScalar();
            int? townId = (int?)sqlCommand.ExecuteScalar();

            return townId;
        }
    }
}
