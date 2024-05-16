
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.DTOs;
using System.Data.SqlClient;
using System.Data;

using System.Security.Cryptography;
using System.Text;
using NDE_Digital_Market.SharedServices;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.Services.UserService;

namespace NDE_Digital_Market.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionDigitalMarket;
        private readonly SqlConnection con;
        private readonly SqlConnection _healthCareConnection;
        private readonly IUser_Service _user_Service;
        public UserController(IConfiguration configuration, IUser_Service user_Service)
        {
            _configuration = configuration;
            CommonServices commonServices = new CommonServices(configuration);
            con = new SqlConnection(_configuration.GetConnectionString("ProminentConnection"));
            _healthCareConnection = new SqlConnection(commonServices.HealthCareConnection);
            _user_Service = user_Service;
        }

        //===================================== Create User ================================
        [HttpPost]
        [Route("UserExist")]
        public async Task<IActionResult> UserExist(UserCreationDTO user)
        {
            bool res = await _user_Service.UserExist(user);
            return Ok(res);
            //   return BadRequest(new { message = "User does not exist" , userExist });
        }


        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreationDTO user)
        {
             object res = await _user_Service.CreateUser(user);
            return Ok(res);
        }


        // =================================================== Login ===================================
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(UserLoginDTO user)
        {
            try
            {
                object result = await _user_Service.LoginUser(user); // Await the asynchronous operation
                dynamic dynamicResult = result; // Convert the result to dynamic
                if (dynamicResult != null && dynamicResult.message == "Login successful")
                {
                    var token = dynamicResult.token;
                    var newRefreshToken = dynamicResult.newRefreshToken;
                    var cookieOptions = dynamicResult.cookieOptions;
                    var cookieOptions2 = dynamicResult.cookieOptions2;

                    // Delete existing cookies
                    Response.Cookies.Delete("accessToken");
                    Response.Cookies.Delete("refreshToken");

                    // Add new cookies
                    Response.Cookies.Append("accessToken", token, cookieOptions);
                    Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions2);

                    return Ok(new { message = "Login successful", dynamicResult.UserId, dynamicResult.role, dynamicResult.IsSellerAdmin, dynamicResult.companyCode });
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }





        }


        [HttpGet]
        [Route("GenerateRefreshToken")]
        public async Task<IActionResult> GenerateRefreshToken()
        {

            string token = HttpContext.Request.Cookies["refreshToken"];
            object result = await _user_Service.GenerateRefreshToken(token);
            dynamic dynamicResult = result; // Convert the result to dynamic
            if (dynamicResult != null && dynamicResult.tokenRefreshed == true)
            {
                var newAccessToken = dynamicResult.newAccessToken;
                var newRefreshToken = dynamicResult.newRefreshToken;
                var cookieOptions = dynamicResult.cookieOptions;
                var cookieOptions2 = dynamicResult.cookieOptions2;


                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");
                Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);
                Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions2);

                return Ok(new { message = "Token Renew Successfully.",});
            }
            else
            {
                return BadRequest(result);
            }

        }





        // ==================================== UPDATE USER ===========================
        //[HttpPut]
        //[Route("UpdateUser")]

        //public IActionResult UpdateUser(UserModel user)
        //{

        //    //string encryptedPassword = EncryptPassword(user.Password);
        //    //string decryptedUserCode = DecryptPassword(user.UserCode);
        //    SqlCommand cmd = new SqlCommand("UPDATE UserRegistration SET CountryRegion = @contryRegion, IsBuyer = @isBuyer, IsSeller = @isSeller, FullName = @fullName, PhoneNumber = @phoneNumber, Email = @email, Address = @address, CompanyName = @companyName, Website = @website, ProductCategory = @productCategory, YearsInBusiness = @yearsInBusiness, BusinessRegistrationNumber = @businessRegNum, TaxIdNumber = @TaxIDNum, PreferredPaymentMethod = @preferredPaymentMethod WHERE UserId = @userID", con);
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Parameters.AddWithValue("@userID", user.UserID);
        //    //cmd.Parameters.AddWithValue("@userCode", decryptedUserCode);
        //    cmd.Parameters.AddWithValue("@contryRegion", user.CounteryRegion);
        //    cmd.Parameters.AddWithValue("@isBuyer", user.IsBuyer);
        //    cmd.Parameters.AddWithValue("@isSeller", user.IsSeller);
        //    //cmd.Parameters.AddWithValue("@isBoth", user.IsBoth);
        //    cmd.Parameters.AddWithValue("@fullName", user.FullName);
        //    cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        //    cmd.Parameters.AddWithValue("@email", user.Email);
        //    //cmd.Parameters.AddWithValue("@Password", encryptedPassword);
        //    cmd.Parameters.AddWithValue("@address", user.Address);
        //    cmd.Parameters.AddWithValue("@companyName", user.CompanyName);
        //    cmd.Parameters.AddWithValue("@website", user.Website);
        //    cmd.Parameters.AddWithValue("@productCategory", user.ProductCategory);
        //    cmd.Parameters.AddWithValue("@yearsInBusiness", user.YearsInBusiness);
        //    cmd.Parameters.AddWithValue("@businessRegNum", user.BusinessRegistrationNumber);
        //    cmd.Parameters.AddWithValue("@TaxIDNum", user.TaxIDNumber);
        //    cmd.Parameters.AddWithValue("@preferredPaymentMethod", user.PreferredPaymentMethod);
        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //    string encryptedUserCode = CommonServices.EncryptPassword(user.UserCode);
        //    return Ok(new { message = "User updated successfully", user });

        //}




        // =================================================== getSingleUserInfo ===================================
        [HttpGet]
        [Route("getSingleUserInfo")]
        [Authorize]
        public async Task<IActionResult> getSingleUser(string userId)
        {
            if(userId is null){
                return BadRequest(new { message = "Give a Valid UserId" });
            }
            object result = _user_Service.getSingleUser(userId);
            if (result is null)
            {
                return NotFound(new { message = "User Not Found." });
            }
            else
            {
                return Ok(result);
            }
        }


        //// =================================================== isAdmin ===================================
        //[HttpGet]
        //[Route("isAdmin")]
        //public IActionResult isAdmin(string userCode)
        //{
        //    UserModel user = new UserModel();

        //    string decryptedUserCode = CommonServices.DecryptPassword(userCode);

        //    SqlCommand cmd = new SqlCommand("SELECT PhoneNumber FROM UserRegistration WHERE UserCode = @userCode ", con);
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Parameters.AddWithValue("@userCode", decryptedUserCode);
        //    //Console.WriteLine(decryptedUserCode);
        //    con.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    if (reader.Read())
        //    {
        //        con.Close();

        //        return Ok(new { message = "User is an Admin" });
        //    }
        //    else
        //    {
        //        con.Close();
        //        return BadRequest(new { message = "User is not an Admin" });
        //    }
        //}

        // ============================= Update Pass =============================


        [HttpPut]
        [Route("updatePass")]
        [Authorize]
        public async Task<IActionResult> UpdatePasss(UserPasswordUpdateDTO user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "Give Valid Inputs!" });
            }
            else
            {
                object result = _user_Service.UpdatePasss(user);
                return Ok(result);
            }

        }




        //========================tushar=========================
        [HttpPut]
        [Route("UpdateUserProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UserInfoUpdateDTO userModel)
        {
            if(userModel == null)
            {
                return BadRequest(new { message = "Send a Valid User Information!" });
            }
            object res = _user_Service.UpdateUserProfileAsync(userModel);
            return Ok(res);
        }

    }
}
