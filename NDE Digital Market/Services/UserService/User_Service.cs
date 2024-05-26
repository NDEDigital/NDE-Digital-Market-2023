
using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.UserService
{
    public class User_Service: IUser_Service
    {
        private readonly User_DAL _user_DAL;
        public User_Service(User_DAL user_DAL)
        {
            _user_DAL = user_DAL;
        }
        public async Task<bool> UserExist(UserCreationDTO user)
        {
            
            UserModel userModel = new UserModel();

            userModel.FullName = user.FullName;
            userModel.Address = user.Address;
            userModel.Email = user.Email;
            userModel.PhoneNumber = user.PhoneNumber;
            userModel.CompanyCode = user.CompanyCode ?? string.Empty;

            userModel.AddedDate = DateTime.UtcNow;
            bool res = await _user_DAL.UserExist(userModel);
            return res;
        }


        public async Task<object> CreateUser(UserCreationDTO user)
        {
            CommonServices.createPasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            UserModel userModel = new UserModel();
            userModel.FullName = user.FullName;
            userModel.Address = user.Address;
            userModel.Email = user.Email;
            userModel.PhoneNumber = user.PhoneNumber;
            userModel.CompanyCode = user.CompanyCode ?? string.Empty;
            userModel.IsBuyer = user.IsBuyer;
            userModel.IsSeller = user.IsSeller;
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            userModel.AddedDate = DateTime.UtcNow;

            return await _user_DAL.CreateUser(userModel);
        }

        public async Task<object> LoginUser(UserLoginDTO user)
        {
            

            UserModel userModel = new UserModel();

            userModel.PhoneNumber = user.PhoneNumber;
            userModel.Password = user.Password;

            return await _user_DAL.LoginUser(userModel);
        }

        public async Task<object> GenerateRefreshToken(string token)
        {
            return await _user_DAL.GenerateRefreshToken(token);
        }



        public async Task<GetSingleUserDetailsDTO> getSingleUser(string UserId)
        {
            string decryptedUserId = CommonServices.DecryptPassword(UserId);
            DataTable dataTable = await _user_DAL.getSingleUser(decryptedUserId);

            GetSingleUserDetailsDTO user = new GetSingleUserDetailsDTO();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {

                user.UserId = (int)row["UserId"];
                user.UserCode = row["UserCode"].ToString();
                user.FullName = row["FullName"].ToString();
                user.IsAdmin = row["IsAdmin"] as bool?;
                user.IsBuyer = row["IsBuyer"] as bool?;
                user.IsSeller = row["IsSeller"] as bool?;
                user.PhoneNumber = row["PhoneNumber"].ToString();
                user.Email = row["Email"].ToString();
                user.Address = row["Address"].ToString();
                if (user.IsSeller == true)
                {
                    user.CompanyName = row["CompanyName"].ToString();
                    user.YearsInBusiness = (int)row["YearsInBusiness"];
                    user.BusinessRegistrationNumber = row["BusinessRegistrationNumber"].ToString();
                    user.TaxIdentificationNumber = row["TaxIdentificationNumber"].ToString();
                    user.PreferredPaymentMethodID = row["PreferredPaymentMethodID"] as int?;
                    user.PMName = row["PMName"].ToString();
                    user.BankNameID = row["BankNameID"] as int?;
                    user.PMBankName = row["PMBankName"].ToString();
                    user.AccountNumber = row["AccountNumber"].ToString();
                    user.AccountHolderName = row["AccountHolderName"].ToString();

                }

            }

            return user;
        }

        public async Task<object>UpdatePasss(UserPasswordUpdateDTO user)
        {

            UserModel userModel = new UserModel();
            string decryptedUserId = CommonServices.DecryptPassword(user.UserId);
            int decryptedUserIdInt;
            if (int.TryParse(decryptedUserId, out decryptedUserIdInt))
            {
                userModel.UserId = decryptedUserIdInt;
            }
            userModel.Password = user.NewPassword;
            userModel.OldPassword = user.OldPassword;
            return await _user_DAL.UpdatePasss(userModel);
        }

        public async Task<object> UpdateUserProfileAsync(UserInfoUpdateDTO user)
        {
            CommonServices.createPasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            UserModel userModel = new UserModel();
            string decryptedUserId = CommonServices.Decrypt<string>(user.UserId);
            int decryptedUserIdInt;
            if (int.TryParse(decryptedUserId, out decryptedUserIdInt))
            {
                userModel.UserId = decryptedUserIdInt;
            }
            userModel.FullName = user.FullName;
            userModel.Address = user.Address;
            userModel.Email = user.Email;
            userModel.PhoneNumber = user.PhoneNumber;
            userModel.CompanyCode = user.CompanyCode ?? string.Empty;
            userModel.IsBuyer = user.IsBuyer;
            userModel.IsSeller = user.IsSeller;
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            userModel.AddedDate = DateTime.UtcNow;

            return await _user_DAL.CreateUser(userModel);
        }

    }
}
