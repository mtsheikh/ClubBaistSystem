using System;
using System.Collections.Generic;
using System.Data;
using ClubBaistSystem.Domain;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class StandingTeeTimeRequests
    {
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";

        public List<StandingTeeTimeRequest> GetStandingTeeTimeRequests(string dayOfWeek)
        {
            {
                var golferManager = new ClubBaistUsers();
                var requestedStandingTeeTimeRequests = new List<StandingTeeTimeRequest>();
                using var connection = new SqlConnection(ConnectionString);
                using var command = new SqlCommand("GetAvailableStandingTeeTimeRequestsByDay", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DayOfWeek", SqlDbType.VarChar).Value = dayOfWeek;

                //Open the connection and execute the reader 
                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        // Mapping the program Object to Database
                        var standingTeeTimeRequestDbInstance = new StandingTeeTimeRequest
                        {
                            Time = new DateTime(((TimeSpan) reader[0]).Ticks),
                            DayOfWeek = (string) reader[1],
                            Shareholder1 = !DBNull.Value.Equals(reader[2])
                                ? golferManager.GetUserFromId((string) reader[2])
                                : new Shareholder(),
                            Shareholder2 = !DBNull.Value.Equals(reader[3])
                                ? golferManager.GetUserFromId((string) reader[3])
                                : new Shareholder(),
                            Shareholder3 = !DBNull.Value.Equals(reader[4])
                                ? golferManager.GetUserFromId((string) reader[4])
                                : new Shareholder(),
                            Shareholder4 = !DBNull.Value.Equals(reader[5])
                                ? golferManager.GetUserFromId((string) reader[5])
                                : new Shareholder()
                        };
                        requestedStandingTeeTimeRequests.Add(standingTeeTimeRequestDbInstance);
                    }

                reader.Close();
                return requestedStandingTeeTimeRequests;
            }
        }

        public bool AddStandingTeeTimeRequest(StandingTeeTimeRequest selectedStandingTeeTimeRequest)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("EditStandingTeeTimeRequest", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = selectedStandingTeeTimeRequest.StartDate.Date;
            command.Parameters.Add("@EndDate", SqlDbType.Date).Value = selectedStandingTeeTimeRequest.EndDate.Date;
            command.Parameters.Add("@Time", SqlDbType.Time).Value = selectedStandingTeeTimeRequest.Time.TimeOfDay;
            command.Parameters.Add("@DayOfWeek", SqlDbType.VarChar).Value = selectedStandingTeeTimeRequest.DayOfWeek;
            command.Parameters.Add("@bookerId", SqlDbType.NVarChar).Value = selectedStandingTeeTimeRequest.BookerId;
            command.Parameters.Add("@shareholder1", SqlDbType.VarChar).Value =
                selectedStandingTeeTimeRequest.Shareholder1.FullName == " "
                    ? null
                    : selectedStandingTeeTimeRequest.Shareholder1.FullName;
            command.Parameters.Add("@shareholder2", SqlDbType.VarChar).Value =
                selectedStandingTeeTimeRequest.Shareholder2.FullName == " "
                    ? null
                    : selectedStandingTeeTimeRequest.Shareholder2.FullName;
            command.Parameters.Add("@shareholder3", SqlDbType.VarChar).Value =
                selectedStandingTeeTimeRequest.Shareholder3.FullName == " "
                    ? null
                    : selectedStandingTeeTimeRequest.Shareholder3.FullName;
            command.Parameters.Add("@shareholder4", SqlDbType.VarChar).Value =
                selectedStandingTeeTimeRequest.Shareholder4.FullName == " "
                    ? null
                    : selectedStandingTeeTimeRequest.Shareholder4.FullName;

            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }
        
        public bool RemoveStandingTeeTimeRequest(StandingTeeTimeRequest selectedStandingTeeTimeRequest)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("EditStandingTeeTimeRequest", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = selectedStandingTeeTimeRequest.StartDate.Date;
            command.Parameters.Add("@EndDate", SqlDbType.Date).Value = selectedStandingTeeTimeRequest.EndDate.Date;
            command.Parameters.Add("@Time", SqlDbType.Time).Value = selectedStandingTeeTimeRequest.Time.TimeOfDay;
            command.Parameters.Add("@DayOfWeek", SqlDbType.VarChar).Value = selectedStandingTeeTimeRequest.DayOfWeek;
            command.Parameters.Add("@bookerId", SqlDbType.NVarChar).Value = selectedStandingTeeTimeRequest.BookerId;
            command.Parameters.Add("@shareholder1", SqlDbType.VarChar).Value = null;
            command.Parameters.Add("@shareholder2", SqlDbType.VarChar).Value = null;
            command.Parameters.Add("@shareholder3", SqlDbType.VarChar).Value = null;
            command.Parameters.Add("@shareholder4", SqlDbType.VarChar).Value = null;
            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }
        public StandingTeeTimeRequest GetStandingTeeTimeRequest(string dayOfWeek, DateTime startDate, 
                                                                    DateTime endDate, DateTime time)
            {
                {
                    var golferManager = new ClubBaistUsers();
                    var requestedStandingTeeTimeRequest = new StandingTeeTimeRequest();
                    using var connection = new SqlConnection(ConnectionString);
                    using var command = new SqlCommand("FindStandingTeeTimeRequest", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@dayOfWeek", SqlDbType.VarChar).Value = dayOfWeek;
                    command.Parameters.Add("@startDate", SqlDbType.VarChar).Value = startDate;
                    command.Parameters.Add("@endDate", SqlDbType.VarChar).Value = endDate;
                    command.Parameters.Add("@time", SqlDbType.VarChar).Value = time;

                    //Open the connection and execute the reader 
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            // Mapping the program Object to Database
                            requestedStandingTeeTimeRequest = new StandingTeeTimeRequest
                            {
                                Time = new DateTime(((TimeSpan) reader[0]).Ticks),
                                DayOfWeek = (string) reader[1],
                                Shareholder1 = !DBNull.Value.Equals(reader[2])
                                    ? golferManager.GetUserFromId((string) reader[2])
                                    : new Shareholder(),
                                Shareholder2 = !DBNull.Value.Equals(reader[3])
                                    ? golferManager.GetUserFromId((string) reader[3])
                                    : new Shareholder(),
                                Shareholder3 = !DBNull.Value.Equals(reader[4])
                                    ? golferManager.GetUserFromId((string) reader[4])
                                    : new Shareholder(),
                                Shareholder4 = !DBNull.Value.Equals(reader[5])
                                    ? golferManager.GetUserFromId((string) reader[5])
                                    : new Shareholder()
                            };
                        }

                    reader.Close();
                    return requestedStandingTeeTimeRequest;
            }
        }
    }
}