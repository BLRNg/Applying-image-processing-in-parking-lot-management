using MeTroUIDemo.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class UserRepository
    {
        private Paking_SlotEntities dbContext;

        public UserRepository()
        {
            dbContext = new Paking_SlotEntities();
        }
        // Method to authenticate a user
        public bool AuthenticateUser(string username, string password)
        {

            var user = dbContext.users.FirstOrDefault(u => u.user_name == username);
            if (user != null)
            {
                // Hash the provided password with the stored salt
                byte[] hashedPassword = PasswordManager.HashPassword(password, user.Salt);

                // Compare the generated hash with the stored PasswordHash
                if (hashedPassword.SequenceEqual(user.PasswordHash))
                {
                    // Authentication successful
                    return true;
                }
            }
            // Authentication failed
            return false;

        }
        public DataTable sp_User()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_User";
                command.CommandType = CommandType.StoredProcedure;


                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }
        // Method to create a new user
        public void CreateUser(string username, string password)
        {
            byte[] salt = PasswordManager.GenerateSalt();
            byte[] passwordHash = PasswordManager.HashPassword(password, salt);

            var newUser = new user
            {
                user_name = username,
                Salt = salt,
                PasswordHash = passwordHash
            };

            dbContext.users.Add(newUser);
            dbContext.SaveChanges();
        }

        public void CreateUserWithRoles(string username, string password, string lisRole)
        {
            byte[] salt = PasswordManager.GenerateSalt();
            byte[] passwordHash = PasswordManager.HashPassword(password, salt);

            var newUser = new user
            {
                user_name = username,
                Salt = salt,
                PasswordHash = passwordHash
            };

            user usercreated = dbContext.users.Add(newUser);
            dbContext.SaveChanges();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "InsertValuesFromSemicolonSeparatedString";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@stringValue", lisRole));
                command.Parameters.Add(new SqlParameter("@userID", usercreated.user_name));
                command.Parameters.Add(new SqlParameter("@delimiter", ";"));
               

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }
        }

        // Method to read a user by username
        public user GetUserByUsername(string username)
        {
            return dbContext.users.FirstOrDefault(u => u.user_name == username);
        }

        // Method to update a user's password
        public void UpdateUserPassword(string username, string newPassword, string lisRole)
        {
            if(newPassword.Length > 0)
            {
                var user = dbContext.users.FirstOrDefault(u => u.user_name == username);
                if (user != null)
                {
                    byte[] salt = PasswordManager.GenerateSalt();
                    byte[] passwordHash = PasswordManager.HashPassword(newPassword, salt);

                    user.Salt = salt;
                    user.PasswordHash = passwordHash;

                    dbContext.SaveChanges();
                }
            }
           

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "UpdateValuesFromSemicolonSeparatedString";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@stringValue", lisRole));
                command.Parameters.Add(new SqlParameter("@userID", username));
                command.Parameters.Add(new SqlParameter("@delimiter", ";"));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }
        }

        // Method to delete a user by username
        public void DeleteUser(string username)
        {
            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "DeleteUser_role";
                command.CommandType = CommandType.StoredProcedure;


               
                command.Parameters.Add(new SqlParameter("@userID", username));
               

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }
            var user = dbContext.users.FirstOrDefault(u => u.user_name == username);
            if (user != null)
            {
                dbContext.users.Remove(user);
                dbContext.SaveChanges();
            }
        }

        public string sp_UserByUsername(string UserName)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_UserByUsername";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@UserName", UserName));
                

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                DataRow r = dataTable.Rows[0];

                return r[0].ToString();
            }

            return "";
        }

        public void sp_DeleteHistoryInOut(
         string time_In, string licensePlateNumberIn)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_DeleteHistoryInOut";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@time_In", time_In));
                command.Parameters.Add(new SqlParameter("@licensePlateNumberIn", licensePlateNumberIn));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public int sp_laneIn(Nullable<int> loginID, string plateNumber, string cardNo, Nullable<System.DateTime> checkTime, ObjectParameter customerName, ObjectParameter message, ObjectParameter plateNum, ObjectParameter total_Amount, ObjectParameter time, ObjectParameter lanceTye, ObjectParameter photoCustomerIn, ObjectParameter photoLicensePlateNumberIN)
        {
            return dbContext.sp_laneIn(loginID, plateNumber, cardNo, checkTime, customerName, message, plateNum, total_Amount, time, lanceTye, photoCustomerIn, photoLicensePlateNumberIN);
        }

        public int sp_laneOut(Nullable<int> loginID, string plateNumber, string cardNo, Nullable<System.DateTime> checkTime, ObjectParameter customerName, ObjectParameter message, ObjectParameter plateNum, ObjectParameter total_Amount, ObjectParameter time, ObjectParameter lanceTye, ObjectParameter photoCustomerIn, ObjectParameter photoLicensePlateNumberIN, string photoCustomerOut, string photoLicensePlateNumberOut, ObjectParameter isMatch)
        {
            return dbContext.sp_laneOut(loginID, plateNumber, cardNo, checkTime, customerName, message, plateNum, total_Amount, time, lanceTye, photoCustomerIn, photoLicensePlateNumberIN, photoCustomerOut, photoLicensePlateNumberOut, isMatch);
        }
    }
}
