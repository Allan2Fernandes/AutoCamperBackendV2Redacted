using AutoCamperBackendV2.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;

namespace AutoCamperBackendV2.Services
{
    public class UserServices
    {
        ParkInPeaceProjectContext context;

        public UserServices(ParkInPeaceProjectContext context)
        {
            this.context = context; 
        }

        public List<TblUser> GetUserOnEmail(string Email)
        {
            var queriedUsers = context.TblUsers.Where(EveryUser => EveryUser.FldEmail == Email).ToList();
            return queriedUsers;
        }
        public void AddUserToDatabase(TblUser NewUser)
        {
            // Encrypt the password
            NewUser.FldEncryptedPassword = Functions.Functions.EncryptPassword(NewUser.FldEncryptedPassword);
            // Add it to list
            context.TblUsers.Add(NewUser);
            // Save the list
            context.SaveChanges();
        }

        public TblUser FindSingleUserOnEmailAndPassword(UserLoginDTO UserToLogin)
        {
            // Encrypt the password
            string EncryptedPassword = Functions.Functions.EncryptPassword(UserToLogin.FldEncryptedPassword);

            // Check if there is any user with those username and password
            // To do: Problem here with it not being case sensitive
            var user = context.TblUsers.SingleOrDefault(EveryUser => EveryUser.FldEmail == UserToLogin.FldEmail && EveryUser.FldEncryptedPassword == EncryptedPassword);
            return user;
        }

        public TblUser FindUserOnUserID(int UserID)
        {
            var user = context.TblUsers.SingleOrDefault(EveryUser => EveryUser.FldUserId == UserID);
            return user;
        }

        public string UpdateUserDetailsOnUserID(UpdateUserDetailsDTO UserDetails)
        {
            // Find the user with the specified User ID
            var UserToUpdate = FindUserOnUserID(UserDetails.FldUserId);
            if (UserToUpdate == null)
            {
                return "User not found";
            }
            else
            {
                // Check if the password is also being updated
                if (UserDetails.FldPassword == null || UserDetails.FldPassword == "")
                {
                    UserToUpdate.FldEmail = UserDetails.FldEmail;
                    UserToUpdate.FldName = UserDetails.FldName;
                    UserToUpdate.FldPhoneNumber = UserDetails.FldPhoneNumber;
                    UserToUpdate.FldAdress = UserDetails.FldAddress;
                }
                else
                {
                    UserToUpdate.FldEmail = UserDetails.FldEmail;
                    UserToUpdate.FldName = UserDetails.FldName;
                    UserToUpdate.FldPhoneNumber = UserDetails.FldPhoneNumber;
                    UserToUpdate.FldAdress = UserDetails.FldAddress;
                    UserToUpdate.FldEncryptedPassword = Functions.Functions.EncryptPassword(UserDetails.FldPassword);

                }
                context.SaveChanges();

                return "User Details Updated";
            }

        }
    }
}
