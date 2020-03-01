using System;
using System.Collections.Generic;
using System.Data;
using ClubBaistSystem.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ClubBaistSystem.TechnicalServices
{
    public class MembershipApplications
    {
        private const string ConnectionString = @"server=(LocalDB)\MSSQLLocalDB;" +
                                                "Initial Catalog=aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502";
        public bool AddMembershipApplication(MembershipApplication newMembershipApplication)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("RecordMembershipApplication", connection);
            command.CommandType = CommandType.StoredProcedure;
            
            command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = newMembershipApplication.LastName;
            command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = newMembershipApplication.FirstName;
            command.Parameters.Add("@address", SqlDbType.NVarChar).Value = newMembershipApplication.Address;
            command.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = newMembershipApplication.PostalCode;
            command.Parameters.Add("@city", SqlDbType.NVarChar).Value = newMembershipApplication.City;
            command.Parameters.Add("@dateOfBirth", SqlDbType.Date).Value = newMembershipApplication.DateOfBirth;
            command.Parameters.Add("@shareholder1", SqlDbType.NVarChar).Value = newMembershipApplication.Shareholder1;
            command.Parameters.Add("@shareholder2", SqlDbType.NVarChar).Value = newMembershipApplication.Shareholder2;
            command.Parameters.Add("@membershipType", SqlDbType.NVarChar).Value = newMembershipApplication.MembershipType;
            command.Parameters.Add("@occupation", SqlDbType.NVarChar).Value = newMembershipApplication.Occupation;
            command.Parameters.Add("@companyName", SqlDbType.NVarChar).Value = newMembershipApplication.CompanyName;
            command.Parameters.Add("@companyAddress", SqlDbType.NVarChar).Value = newMembershipApplication.CompanyAddress;
            command.Parameters.Add("@companyPostalCode", SqlDbType.NVarChar).Value = newMembershipApplication.CompanyPostalCode;
            command.Parameters.Add("@companyCity", SqlDbType.NVarChar).Value = newMembershipApplication.CompanyCity;
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = newMembershipApplication.Email;
            command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = newMembershipApplication.Phone;
            command.Parameters.Add("@alternatePhone", SqlDbType.NVarChar).Value = newMembershipApplication.AlternatePhone ?? "0000000000";
            
            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }
        public List<MembershipApplication> GetOnHoldMembershipApplications()
        {
            var onHoldMembershipApplications = new List<MembershipApplication>();
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetAllOnholdMembershipApplications", connection);
            command.CommandType = CommandType.StoredProcedure;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) // Mapping the program Object to Database
                {
                   var onHoldMembershipApplication = new MembershipApplication();
                   
                   onHoldMembershipApplication.Id = (int) reader[0];
                   onHoldMembershipApplication.LastName = (string) reader[1];
                   onHoldMembershipApplication.FirstName = (string) reader[2];
                   onHoldMembershipApplication.Address = (string) reader[3];
                   onHoldMembershipApplication.PostalCode = (string) reader[4];
                   onHoldMembershipApplication.City = (string) reader[5];
                   onHoldMembershipApplication.DateOfBirth = (DateTime) reader[6];
                   onHoldMembershipApplication.Shareholder1 = (string) reader[7];
                   onHoldMembershipApplication.Shareholder2 = (string) reader[8];
                   onHoldMembershipApplication.MembershipType = (string) reader[9];
                   onHoldMembershipApplication.Occupation = (string) reader[10];
                   onHoldMembershipApplication.CompanyName = (string) reader[11];
                   onHoldMembershipApplication.CompanyAddress = (string) reader[12];
                   onHoldMembershipApplication.CompanyPostalCode = (string) reader[13];
                   onHoldMembershipApplication.CompanyCity = (string) reader[14];
                   onHoldMembershipApplication.Email = (string) reader[15];
                   onHoldMembershipApplication.Phone = (string) reader[16];
                   onHoldMembershipApplication.AlternatePhone = (string) reader[17];
                   onHoldMembershipApplication.ApplicationStatus = "On Hold";
                   
                   onHoldMembershipApplications.Add(onHoldMembershipApplication);
                }
            reader.Close();
            return onHoldMembershipApplications;
        }
        public List<MembershipApplication> GetWaitlistedMembershipApplications()
        {
            var onHoldMembershipApplications = new List<MembershipApplication>();
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetAllWaitlistedMembershipApplications", connection);
            command.CommandType = CommandType.StoredProcedure;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) // Mapping the program Object to Database
                {
                   var onHoldMembershipApplication = new MembershipApplication();
                   
                   onHoldMembershipApplication.Id = (int) reader[0];
                   onHoldMembershipApplication.LastName = (string) reader[1];
                   onHoldMembershipApplication.FirstName = (string) reader[2];
                   onHoldMembershipApplication.Address = (string) reader[3];
                   onHoldMembershipApplication.PostalCode = (string) reader[4];
                   onHoldMembershipApplication.City = (string) reader[5];
                   onHoldMembershipApplication.DateOfBirth = (DateTime) reader[6];
                   onHoldMembershipApplication.Shareholder1 = (string) reader[7];
                   onHoldMembershipApplication.Shareholder2 = (string) reader[8];
                   onHoldMembershipApplication.MembershipType = (string) reader[9];
                   onHoldMembershipApplication.Occupation = (string) reader[10];
                   onHoldMembershipApplication.CompanyName = (string) reader[11];
                   onHoldMembershipApplication.CompanyAddress = (string) reader[12];
                   onHoldMembershipApplication.CompanyPostalCode = (string) reader[13];
                   onHoldMembershipApplication.CompanyCity = (string) reader[14];
                   onHoldMembershipApplication.Email = (string) reader[15];
                   onHoldMembershipApplication.Phone = (string) reader[16];
                   onHoldMembershipApplication.AlternatePhone = (string) reader[17];
                   onHoldMembershipApplication.ApplicationStatus = "Waitlisted";
                   
                   onHoldMembershipApplications.Add(onHoldMembershipApplication);
                }
            reader.Close();
            return onHoldMembershipApplications;
        }
        public MembershipApplication FindMembershipApplication(int membershipApplicationId)
         {
             var onHoldMembershipApplication = new MembershipApplication();
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("GetMembershipApplication", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@membershipApplicationId", SqlDbType.Int).Value = membershipApplicationId;

            //Open the connection and execute the reader 
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read()) // Mapping the program Object to Database
                {
                    onHoldMembershipApplication.Id = membershipApplicationId;
                    onHoldMembershipApplication.LastName = (string) reader[0];
                    onHoldMembershipApplication.FirstName = (string) reader[1];
                    onHoldMembershipApplication.Address = (string) reader[2];
                    onHoldMembershipApplication.PostalCode = (string) reader[3];
                    onHoldMembershipApplication.City = (string) reader[4];
                    onHoldMembershipApplication.DateOfBirth = (DateTime) reader[5];
                    onHoldMembershipApplication.Shareholder1 = (string) reader[6];
                    onHoldMembershipApplication.Shareholder2 = (string) reader[7];
                    onHoldMembershipApplication.MembershipType = (string) reader[8];
                    onHoldMembershipApplication.Occupation = (string) reader[9];
                    onHoldMembershipApplication.CompanyName = (string) reader[10];
                    onHoldMembershipApplication.CompanyAddress = (string) reader[11];
                    onHoldMembershipApplication.CompanyPostalCode = (string) reader[12];
                    onHoldMembershipApplication.CompanyCity = (string) reader[13];
                    onHoldMembershipApplication.Email = (string) reader[14];
                    onHoldMembershipApplication.Phone = (string) reader[15];
                    onHoldMembershipApplication.AlternatePhone = (string) reader[16];
                    onHoldMembershipApplication.ApplicationStatus = (string) reader[17];
                }
            reader.Close();
            return onHoldMembershipApplication;
         }
        public bool CancelMembershipApplication(int membershipApplicationId)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("CancelMembershipApplication", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@membershipApplicationId", SqlDbType.Int).Value = membershipApplicationId;

            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }
        public bool UpdateMembershipApplication(int membershipApplicationId)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("WaitlistMembershipApplication", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@membershipApplicationId", SqlDbType.Int).Value = membershipApplicationId;

            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            return success != 0;
        }
        
        public bool AddApprovedMemberAccount(MembershipApplication approvedMembershipApplication)
        {
            approvedMembershipApplication.NewGeneratedMemberId = Guid.NewGuid().ToString();
            var passwordHasher = new PasswordHasher<string>();
            var hashedPassword = passwordHasher.HashPassword(approvedMembershipApplication.Email, "Baist123$");
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(approvedMembershipApplication.Email, hashedPassword, "Baist123$");
            
            // Create New Member Account
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("CreateNewAccount", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@newMemberId", SqlDbType.NVarChar).Value = approvedMembershipApplication.NewGeneratedMemberId;
            command.Parameters.Add("@userName", SqlDbType.NVarChar).Value = approvedMembershipApplication.Email;
            command.Parameters.Add("@normalizedUserName", SqlDbType.NVarChar).Value = approvedMembershipApplication.Email.ToUpper();
            command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = hashedPassword;
            command.Parameters.Add("@fullName", SqlDbType.NVarChar).Value = approvedMembershipApplication.FirstName + " " + approvedMembershipApplication.LastName;
            command.Parameters.Add("@userType", SqlDbType.NVarChar).Value = approvedMembershipApplication.MembershipType;
            command.Parameters.Add("@membershipApplicationId", SqlDbType.Int).Value = approvedMembershipApplication.Id;

            //Open the connection and execute the reader 
            connection.Open();
            var success = command.ExecuteNonQuery();
            connection.Close();
            
            return success != 0;   
        }
    }
}