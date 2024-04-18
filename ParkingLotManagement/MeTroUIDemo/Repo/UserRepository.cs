using MeTroUIDemo.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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

        // Method to read a user by username
        public user GetUserByUsername(string username)
        {
            return dbContext.users.FirstOrDefault(u => u.user_name == username);
        }

        // Method to update a user's password
        public void UpdateUserPassword(string username, string newPassword)
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

        // Method to delete a user by username
        public void DeleteUser(string username)
        {
            var user = dbContext.users.FirstOrDefault(u => u.user_name == username);
            if (user != null)
            {
                dbContext.users.Remove(user);
                dbContext.SaveChanges();
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
