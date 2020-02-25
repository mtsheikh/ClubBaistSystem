using System.Data;
using ClubBaistSystem.Domain;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class ClubBaistUsers
    {
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";

        public static ClubBaistUser GetUserFromUserName(string userName)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetUserFromUserName", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    switch ((string) reader[2])
                    {
                        case "Shareholder":
                            return new Shareholder(){FullName = (string)reader[1], Id = (string)reader[0]};
                        case "ProShop":
                            return new ProShopStaff(){FullName = (string)reader[1], Id = (string)reader[0]};
                        case "Associate":
                            return new Associate(){FullName = (string)reader[1], Id = (string)reader[0]};
                        case "ShareholderSpouse":
                            return new ShareholderSpouse(){FullName = (string)reader[1], Id = (string)reader[0]};
                        case "Intermediate":
                            return new Intermediate(){FullName = (string)reader[1], Id = (string)reader[0]};
                        case "Clerk":
                            return new Clerk(){FullName = (string)reader[1], Id = (string)reader[0]};
                    }
            reader.Close();
            return null;
        }

        public ClubBaistUser GetUserFromId(string userId)
        {
            ClubBaistUser clubBaistUser = null;
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetUserNameFromId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    clubBaistUser = GetUserFromUserName((string) reader[0]);
                }

            reader.Close();
            return clubBaistUser;
        }
    }
}