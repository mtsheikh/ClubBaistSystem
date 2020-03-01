using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClubBaistSystem.Domain;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class PlayerScores
    {
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";
        public bool AddPlayerScores(Scorecard submittedScoreCard)
        {
            var success = 0;
            foreach (var round in submittedScoreCard.Rounds)
            {
                using var connection = new SqlConnection(ConnectionString);
                using var command = new SqlCommand("RecordGolferScore", connection);
                command.CommandType = CommandType.StoredProcedure;
            
                command.Parameters.Add("@golferId", SqlDbType.NVarChar).Value = submittedScoreCard.Golfer.Id;
                command.Parameters.Add("@courseName", SqlDbType.NVarChar).Value = submittedScoreCard.Course;
                command.Parameters.Add("@date", SqlDbType.Date).Value = submittedScoreCard.Date.Date;
                command.Parameters.Add("@hole", SqlDbType.Int).Value = round.Hole;
                command.Parameters.Add("@score", SqlDbType.Int).Value = round.Score;
                command.Parameters.Add("@rating", SqlDbType.Decimal).Value = round.Rating;
                command.Parameters.Add("@slope", SqlDbType.Decimal).Value = round.Slope;
                  
                //Open the connection and execute the reader 
                connection.Open();
                success = command.ExecuteNonQuery();
                connection.Close();
            }
            return success != 0;
        }

        public decimal CalculatePlayerHandicap(ClubBaistUser authenticatedPlayer)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("ViewPlayerHandicap", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@golferId", SqlDbType.NVarChar).Value = authenticatedPlayer.Id;

            var Rounds = new List<Round>();
            
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    // Mapping the program Object to Database
                {
                    var roundDbInst = new Round()
                    {
                        Hole = (int)reader[3],
                        Score = (int)reader[4],
                        Rating = (decimal)reader[5],
                        Slope = (decimal)reader[6],
                    };
                    Rounds.Add(roundDbInst);
                }
            reader.Close(); 
            
            // Determine Handicap Differentials
            var handicapDifferentials = Rounds.Select(round => ((round.Score - round.Rating) * 113) / round.Slope).ToList();
            // Average it 
            var average = handicapDifferentials.Average();
            // Multiply by 0.96
            average *= (decimal)0.96;

            return Math.Truncate(100 * average) / 100;
        }
    }
}