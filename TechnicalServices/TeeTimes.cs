using System;
using System.Collections.Generic;
using System.Data;
using ClubBaistSystem.Domain;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class TeeTimes
    {
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";
        private readonly ClubBaistUsers _userManager = new ClubBaistUsers();

        public bool AddTeeTime(TeeTime selectedTeeTime)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("EditTeeTime", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Date", SqlDbType.Date).Value = selectedTeeTime.Date.Date;
            command.Parameters.Add("@Time", SqlDbType.Time).Value = selectedTeeTime.Time.TimeOfDay;
            command.Parameters.Add("@golfer1", SqlDbType.VarChar).Value =
                selectedTeeTime.Golfer1.FullName == "Vacant" ? null : selectedTeeTime.Golfer1.FullName;
            command.Parameters.Add("@golfer2", SqlDbType.VarChar).Value =
                selectedTeeTime.Golfer2.FullName == "Vacant" ? null : selectedTeeTime.Golfer2.FullName;
            command.Parameters.Add("@golfer3", SqlDbType.VarChar).Value =
                selectedTeeTime.Golfer3.FullName == "Vacant" ? null : selectedTeeTime.Golfer3.FullName;
            command.Parameters.Add("@golfer4", SqlDbType.VarChar).Value =
                selectedTeeTime.Golfer4.FullName == "Vacant" ? null : selectedTeeTime.Golfer4.FullName;
            command.Parameters.Add("@bookerId", SqlDbType.NVarChar).Value =
                selectedTeeTime.BookerId == "Vacant" ? null : selectedTeeTime.BookerId;

            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }
        public bool RemoveTeeTime(TeeTime bookedTeeTime, List<string> cancelledGolfers)
        {
         foreach (var golfer in cancelledGolfers)
         {
                bookedTeeTime.Golfer1 = bookedTeeTime.Golfer1?.FullName == golfer
                    ? new Shareholder()
                    : bookedTeeTime.Golfer1;
                bookedTeeTime.Golfer2 = bookedTeeTime.Golfer2?.FullName == golfer
                    ? new Shareholder()
                    : bookedTeeTime.Golfer2;
                bookedTeeTime.Golfer3 = bookedTeeTime.Golfer3?.FullName == golfer
                    ? new Shareholder()
                    : bookedTeeTime.Golfer3;
                bookedTeeTime.Golfer4 = bookedTeeTime.Golfer4?.FullName == golfer
                    ? new Shareholder()
                    : bookedTeeTime.Golfer4;
         }
         using var connection = new SqlConnection(ConnectionString);
         using var command = new SqlCommand("EditTeeTime", connection);
         command.CommandType = CommandType.StoredProcedure;
         command.Parameters.Add("@Date", SqlDbType.Date).Value = bookedTeeTime.Date.Date;
         command.Parameters.Add("@Time", SqlDbType.Time).Value = bookedTeeTime.Time.TimeOfDay;
         command.Parameters.Add("@golfer1", SqlDbType.VarChar).Value =
             bookedTeeTime.Golfer1?.FullName == "Vacant" ? null : bookedTeeTime.Golfer1?.FullName;
         command.Parameters.Add("@golfer2", SqlDbType.VarChar).Value =
             bookedTeeTime.Golfer2?.FullName == "Vacant" ? null : bookedTeeTime.Golfer2?.FullName;
         command.Parameters.Add("@golfer3", SqlDbType.VarChar).Value =
             bookedTeeTime.Golfer3?.FullName == "Vacant" ? null : bookedTeeTime.Golfer3?.FullName;
         command.Parameters.Add("@golfer4", SqlDbType.VarChar).Value =
             bookedTeeTime.Golfer4?.FullName == "Vacant" ? null : bookedTeeTime.Golfer4?.FullName;
         command.Parameters.Add("@bookerId", SqlDbType.NVarChar).Value =
             bookedTeeTime.BookerId == "Vacant" ? null : bookedTeeTime.BookerId;

         //Open the connection and execute the NonQuery 
         connection.Open();
         var success = command.ExecuteNonQuery();
         connection.Close();
         return success != 0;
        }

        public bool CheckInGolfersForTeeTime(TeeTime bookedTeeTime)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("CheckInGolfer", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Date", SqlDbType.Date).Value = bookedTeeTime.Date.Date;
            command.Parameters.Add("@Time", SqlDbType.Time).Value = bookedTeeTime.Time.TimeOfDay;
            command.Parameters.Add("@golfer1CheckedIn", SqlDbType.Bit).Value =
                bookedTeeTime.Golfer1CheckedIn == false ? 0 : 1;
            command.Parameters.Add("@golfer2CheckedIn", SqlDbType.Bit).Value =
                bookedTeeTime.Golfer2CheckedIn == false ? 0 : 1;
            command.Parameters.Add("@golfer3CheckedIn", SqlDbType.Bit).Value =
                bookedTeeTime.Golfer3CheckedIn == false ? 0 : 1;
            command.Parameters.Add("@golfer4CheckedIn", SqlDbType.Bit).Value =
                bookedTeeTime.Golfer4CheckedIn == false ? 0 : 1;

            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }

        public TeeTime GetTeeTime(DateTime selectedTime, DateTime selectedDate)
        {
            var requestedTeeTime = new TeeTime();
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetTeeTimeByDateAndTime", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Date", SqlDbType.VarChar).Value = selectedDate;
            command.Parameters.Add("@Time", SqlDbType.VarChar).Value = selectedTime;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    // Mapping the program Object to Database
                {
                    requestedTeeTime.Date = (DateTime) reader[0];
                    requestedTeeTime.Time = new DateTime(((TimeSpan) reader[1]).Ticks);
                    requestedTeeTime.Golfer1 = !DBNull.Value.Equals(reader[2])
                        ? _userManager.GetUserFromId((string) reader[2])
                        : new Shareholder();
                    requestedTeeTime.Golfer2 = !DBNull.Value.Equals(reader[3])
                        ? _userManager.GetUserFromId((string) reader[3])
                        : new Shareholder();
                    requestedTeeTime.Golfer3 = !DBNull.Value.Equals(reader[4])
                        ? _userManager.GetUserFromId((string) reader[4])
                        : new Shareholder();
                    requestedTeeTime.Golfer4 = !DBNull.Value.Equals(reader[5])
                        ? _userManager.GetUserFromId((string) reader[5])
                        : new Shareholder();
                    requestedTeeTime.BookerId = !DBNull.Value.Equals(reader[6])
                        ? (string) reader[6]
                        : " ";
                    requestedTeeTime.Golfer1CheckedIn = (bool) reader[7];
                    requestedTeeTime.Golfer2CheckedIn = (bool) reader[8];
                    requestedTeeTime.Golfer3CheckedIn = (bool) reader[9];
                    requestedTeeTime.Golfer4CheckedIn = (bool) reader[10];
                }
            reader.Close();
            return requestedTeeTime;
        }
    }
}