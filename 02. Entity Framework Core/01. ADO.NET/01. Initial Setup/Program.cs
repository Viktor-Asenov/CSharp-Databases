namespace ADO.NET
{
    using System;
    using System.Data.SqlClient;

    public class Program
    {
        static void Main(string[] args)
        {
            const string SqlConnection = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(SqlConnection))
            {
                connection.Open();

                var createTableStatements = GetCreateTableStatements();
                foreach (var query in createTableStatements)
                {
                    ExecuteNonQuery(connection, query);
                }

                var insertTableStatements = GetInsertStatements();
                foreach (var query in insertTableStatements)
                {
                    ExecuteNonQuery(connection, query);
                }
            }
        }

        private static void ExecuteNonQuery(SqlConnection connection, string query)
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string[] GetCreateTableStatements()
        {
            var result = new string[]
            {
                "CREATE TABLE Countries(Id INT IDENTITY PRIMARY KEY, Name VARCHAR(50))",
                "CREATE TABLE Towns(Id INT IDENTITY PRIMARY KEY, Name VARCHAR(50), " +
                "CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                "CREATE TABLE Minions(Id INT IDENTITY PRIMARY KEY, Name VARCHAR(50), Age INT," +
                "TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                "CREATE TABLE EvilnessFactors(Id INT IDENTITY PRIMARY KEY, Name VARCHAR(50))",
                "CREATE TABLE Villains(Id INT IDENTITY PRIMARY KEY, Name VARCHAR(50)," +
                "EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",
                "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id)," +
                "VillainId INT FOREIGN KEY REFERENCES Villains(Id)" +
                "CONSTRAINT PK_MinionsVillains PRIMARY KEY(MinionId, VillainId))"
            };

            return result;
        }

        private static string[] GetInsertStatements()
        {
            var result = new string[]
            {
                "INSERT INTO Countries VALUES ('Bulgaria'), ('Finland'), ('Italy'), ('UK'), ('Germany')",
                "INSERT INTO Towns VALUES ('Sofia', 1), ('Helsinki', 2), ('Milan', 3), ('Manchester', 4), ('Bayern', 5)",
                "INSERT INTO Minions VALUES ('Ivan', 13, 1), ('Ville', 19, 2), ('Nikolo', 26, 3), ('George', 39, 4), ('Hans', 17, 5)",
                "INSERT INTO EvilnessFactors VALUES ('super good'), ('good'), ('bad'), ('evil'), ('super evil')",
                "INSERT INTO Villains VALUES ('Gru', 1), ('Vik', 2), ('Jilly', 3), ('Ivo', 4), ('Lil', 5)"
            };

            return result;
        }
    }
}
