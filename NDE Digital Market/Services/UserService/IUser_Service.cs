using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.UserService
{
    public interface IUser_Service
    {
        Task<bool> UserExist(UserCreationDTO user);

        Task<object> CreateUser(UserCreationDTO user);

        Task<object> LoginUser(UserLoginDTO user);

        Task<object> GenerateRefreshToken(string token);

        Task<GetSingleUserDetailsDTO> getSingleUser(string userId);

        Task<object> UpdatePasss(UserPasswordUpdateDTO user);

        Task<object> UpdateUserProfileAsync(UserInfoUpdateDTO userModel);
    }
}
