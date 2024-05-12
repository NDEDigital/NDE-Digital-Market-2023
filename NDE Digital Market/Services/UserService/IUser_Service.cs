using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.UserService
{
    public interface IUser_Service
    {
        Task<bool> UserExist(UserCreationDTO user);

        Task<object> CreateUser(UserCreationDTO user);

        Task<object> LoginUser(UserLoginDTO user);

        //Task<IActionResult> GenerateRefreshToken();

        //IActionResult getSingleUser(int? userId);

        //IActionResult UpdatePasss(UpdatePasswordModel user);

        //Task<IActionResult> UpdateUserProfileAsync([FromBody] UserModel userModel);
    }
}
