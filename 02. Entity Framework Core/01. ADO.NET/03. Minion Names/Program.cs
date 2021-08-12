namespace _03._Minion_Names
{
    using System;
    using Microsoft.Data.SqlClient;

    public class Program
    {
        static void Main(string[] args)
        {
            const string SqlConnection = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(SqlConnection))
            {
                connection.Open();

                int id = int.Parse(Console.ReadLine());
                string villainNameQuery = "SELECT Name FROM Villains WHERE Id = @Id";

                using var command = new SqlCommand(villainNameQuery, connection);
                command.Parameters.AddWithValue("@Id", id);
                var result = command.ExecuteScalar();

                string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                            FROM MinionsVillains AS mv
                                            JOIN Minions As m ON mv.MinionId = m.Id
                                            WHERE mv.VillainId = @Id
                                            ORDER BY m.Name";

                if (result == null)
                {
                    Console.WriteLine($"No villain with ID {id} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {result}");

                    using (var minionCommand = new SqlCommand(minionsQuery, connection))
                    {
                        minionCommand.Parameters.AddWithValue("@Id", id);
                        using (var reader = minionCommand.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("(no minions)");
                            }

                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader[0]}. {reader[1]} {reader[2]}");
                            }
                        }
                    }
                }

                Console.WriteLine(result);
            }
        }

        private static object ExecuteScalar(SqlConnection connection, string query)
        {
            using var command = new SqlCommand();
            var result = command.ExecuteScalar();

            return result;
        }
    }
}
