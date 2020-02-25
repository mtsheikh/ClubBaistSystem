using System;
using System.Collections.Generic;
using System.Data;
using ClubBaistSystem.Domain;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class DailyTeeSheets
    {
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";

        private readonly ClubBaistUsers _userManager = new ClubBaistUsers();

        public DailyTeeSheet GetDailyTeeSheet(DateTime dailyTeeSheetDate, ClubBaistUser authenticatedUser)
        {
            var requestedTeeTimesByDate = new List<TeeTime>();
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetTeeTimesByDate", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Date", SqlDbType.VarChar).Value = dailyTeeSheetDate;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    // Mapping the program Object to Database
                    var teeTimeDbInstance = new TeeTime
                    {
                        Date = (DateTime) reader[0],
                        Time = new DateTime(((TimeSpan) reader[1]).Ticks),
                        Golfer1 = !DBNull.Value.Equals(reader[2])
                            ? _userManager.GetUserFromId((string) reader[2])
                            : new Shareholder(),
                        Golfer2 = !DBNull.Value.Equals(reader[3])
                            ? _userManager.GetUserFromId((string) reader[3])
                            :  new Shareholder(),
                        Golfer3 = !DBNull.Value.Equals(reader[4])
                            ? _userManager.GetUserFromId((string) reader[4])
                            :  new Shareholder(),
                        Golfer4 = !DBNull.Value.Equals(reader[5])
                            ? _userManager.GetUserFromId((string) reader[5])
                            : new Shareholder(),
                    };
                    var teeSheetDay = teeTimeDbInstance.Date.DayOfWeek;
                    var teeTime = teeTimeDbInstance.Time;

                    if (teeSheetDay == DayOfWeek.Saturday || teeSheetDay == DayOfWeek.Sunday)
                    {
                        if (teeTime >= authenticatedUser.weekendHolidayPlayingHoursAfter)
                            requestedTeeTimesByDate.Add(teeTimeDbInstance);
                    }
                    else
                    {
                        if (!(authenticatedUser.regularPlayingHoursBefore < teeTime &&
                              authenticatedUser.regularPlayingHoursAfter > teeTime))
                            requestedTeeTimesByDate.Add(teeTimeDbInstance);
                    }
                }
            reader.Close();
            return new DailyTeeSheet(requestedTeeTimesByDate) {Date = dailyTeeSheetDate};
        }
    }
}