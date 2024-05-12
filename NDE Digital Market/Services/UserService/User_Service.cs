
using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;

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

            return _user_DAL.CreateUser(userModel);
        }

        public async Task<object> LoginUser(UserLoginDTO user)
        {
            

            UserModel userModel = new UserModel();

            userModel.PhoneNumber = user.PhoneNumber;
            userModel.Password = user.Password;

            object result = await _user_DAL.LoginUser(userModel);
            dynamic dynamicResult = result; // Convert the result to dynamic
            if (dynamicResult != null)
            {
                dynamicResult.userId = CommonServices.EncryptPassword(dynamicResult.userId);
                dynamicResult.companyCode = CommonServices.EncryptPassword(dynamicResult.companyCode);
            }
            return dynamicResult;
        }

        //public async Task<IActionResult> GenerateRefreshToken()
        //{

        //}



        //public IActionResult getSingleUser(int? userId)
        //{

        //}

        //public IActionResult UpdatePasss(UpdatePasswordModel user)
        //{

        //}

        //public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UserModel userModel)
        //{

        //}

    }
}
