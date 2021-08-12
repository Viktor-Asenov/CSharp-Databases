namespace _02._Villain_Names
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

                string query = @"SELECT v.Name, COUNT(mv.MinionId)
                                    FROM Villains AS v
                                    JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
                                    GROUP BY v.Id, v.Name
                                    HAVING COUNT(mv.MinionId) > 3";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader[0];
                            var count = reader[1];

                            Console.WriteLine($"{name} - {count}");
                        }
                    }
                }
            }
        }
    }
}
