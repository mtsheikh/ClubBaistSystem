using System;
using System.Collections.Generic;
using System.Data;
using ClubBaistSystem.Domain;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class MemberAccounts
    {
        
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";
        public MemberAccount FindMemberAccount(string memberId)
        {
            var retrievedMemberAccountEntries = new List<MemberAccountEntry>();
            var requestedMemberAccount = new MemberAccount();
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetMemberAccount", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@memberId", SqlDbType.NVarChar).Value = memberId;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) // Mapping the program Object to Database
                {
                    requestedMemberAccount.MemberId = memberId;
                    requestedMemberAccount.TotalBalance = (decimal) reader[4];
                    
                    var retrievedMemberAccountEntry = new MemberAccountEntry()
                    {
                        WhenCharged = (DateTime) reader[0],
                        WhenMade = (DateTime) reader[1],
                        Amount = (decimal) reader[2],
                        EntryDecription = (string) reader[3]
                    };
                    
                    retrievedMemberAccountEntries.Add(retrievedMemberAccountEntry);
                }
            reader.Close();
            requestedMemberAccount.AccountEntries = retrievedMemberAccountEntries;
            return requestedMemberAccount;
         }
    }
}