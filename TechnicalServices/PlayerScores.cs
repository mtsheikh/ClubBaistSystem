using System.Data;
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
    }
}