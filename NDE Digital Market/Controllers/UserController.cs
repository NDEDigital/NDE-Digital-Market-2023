
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

                    return Ok(new { message = "Login successful", dynamicResult.userId, dynamicResult.role, dynamicResult.IsSellerAdmin, dynamicResult.companyCode });
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
            var handler = new JwtSecurityTokenHandler();
            bool tokenRefreshed = true;
            JwtSecurityToken jwtToken;
            string token = HttpContext.Request.Cookies["refreshToken"];
            try
            {
                jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null) throw new ArgumentException("Invalid token");
            }
            catch (ArgumentException)
            {
                tokenRefreshed = false;
                return Ok(new
                {
                    message = "Token is not in a valid JWT format.",
                    tokenRefreshed
                });
            }

            var issueDate = jwtToken.ValidFrom;
            var expireDate = jwtToken.ValidTo;

            if (DateTime.UtcNow > expireDate)
            {
               
                // Return a forbidden (403) response
              //  return Unauthorized();
                return Forbid();
                //return Ok(new
                //{
                //    message = "reFreshToken Expired",
                //    tokenRefreshed

                //});
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>  c.Type == ClaimTypes.NameIdentifier);
            var encryptedUserId = userIdClaim != null ? userIdClaim.Value : null;
            string userId = encryptedUserId;
            //if (encryptedUserId != null)
            //{
            //    userId = CommonServices.DecryptPassword(encryptedUserId).ToString();
            //}
            //else
            //{
            //    return BadRequest("The token does not contain the expected claim.");
            //}



            DateTime? timeStamp = null;
            bool? isBuyer = null;
            bool? isSeller = null;
            bool? isAdmin = null;

            string query = "SELECT * FROM UserRegistration WHERE UserId = @userId";

            using (SqlCommand cmd = new SqlCommand(query, _healthCareConnection))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                  await _healthCareConnection.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            timeStamp = reader["TimeStamp"] as DateTime?;
                            isBuyer = reader.GetBoolean(reader.GetOrdinal("IsBuyer"));
                            isSeller = reader.GetBoolean(reader.GetOrdinal("IsSeller"));
                            isAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin"));

                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            string role = "";
            if ((bool)isBuyer)
            {
                role = "buyer";
            }
            if ((bool)isSeller)
            {
                role = "seller";
            }
            if ((bool)isAdmin)
            {
                role = "admin";
            }


            if (timeStamp == null || timeStamp.Value > issueDate)
            {
                timeStamp = DateTime.UtcNow.AddSeconds(1); // Assign any value greater than issueDate
            }

            // If the timestamp is more recent than the token issue date, it means the token should be considered invalid
            if (timeStamp > issueDate)
            {
                return Unauthorized("Token has been invalidated.");
            }

            // At this point, the token is considered valid, and we can generate a new access and refresh token
            string newAccessToken = CreateToken(role); // Replace "RoleFromYourSystem" with actual role retrieval logic
            string newRefreshToken = CreateRefreshToken(encryptedUserId); // This method should be defined to create a refresh token
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10),
                SameSite = SameSiteMode.None,
                Secure = true,

            };
            var cookieOptions2 = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(3)
            };
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);
            Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions2);
            return Ok(new
            {
                
                accessToken = "newAccessToken",
                refreshToken = "newRefreshToken", tokenRefreshed,
            });
        }



        //====================== Token code added by utshow ======================================
        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var token = new JwtSecurityToken(
                claims: claims,
               expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private string CreateRefreshToken(string userId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
              
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
 
                expires: DateTime.UtcNow.AddDays(3),
 
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
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
        public IActionResult getSingleUser(int? userId)
        {
            UserDetailsDTO user = new UserDetailsDTO();
            //byte[] userCodeBytes = Encoding.UTF8.GetBytes(userCode);

            //string DecryptedUserCode = ConvertBytesToHexString(user.UserCode);
            SqlCommand cmd = new SqlCommand("SELECT\r\n     UR.UserId,\r\n\tUR.UserCode,   UR.FullName,\r\n    UR.IsAdmin,\r\n    UR.IsBuyer,\r\n    UR.IsSeller,\r\n    UR.PhoneNumber,\r\n    UR.Email,\r\n    UR.Address,\r\n    CR.CompanyName,\r\n   DATEDIFF(YEAR, CR.CompanyFoundationDate, GETDATE()) as YearsInBusiness,\r\n\tCR.BusinessRegistrationNumber,\r\n\tCR.TaxIdentificationNumber,\r\n\tCR.PreferredPaymentMethodID,\r\n\tPM.PMName,\r\n\tCR.BankNameID,\r\n\tPD.PMBankName,\r\n\tCR.AccountNumber,\r\n\tCR.AccountHolderName\r\n\r\n\r\nFROM\r\n    UserRegistration UR\r\nLEFT JOIN\r\n    CompanyRegistration CR ON UR.CompanyCode = CR.CompanyCode\r\nLEFT JOIN\r\n    HK_PaymentMethodMaster PM ON CR.PreferredPaymentMethodID = PM.PMMasterID\r\nLEFT JOIN\r\n    HK_PaymentMethodDetails PD ON CR.BankNameID = PD.PMDetailsID\r\nWHERE\r\n    UR.UserId = @UserId ", _healthCareConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserId", userId);
            _healthCareConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user.UserId = (int)reader["UserId"];
                user.UserCode = reader["UserCode"].ToString();
                user.FullName = reader["FullName"].ToString();
                user.IsAdmin = reader["IsAdmin"] as bool?;
                user.IsBuyer = reader["IsBuyer"] as bool?;
                user.IsSeller = reader["IsSeller"] as bool?;
                user.PhoneNumber = reader["PhoneNumber"].ToString();
                user.Email = reader["Email"].ToString();
                user.Address = reader["Address"].ToString();
                if (user.IsSeller == true)
                {
                    user.CompanyName = reader["CompanyName"].ToString();
                    user.YearsInBusiness = (int)reader["YearsInBusiness"];
                    user.BusinessRegistrationNumber = reader["BusinessRegistrationNumber"].ToString();
                    user.TaxIdentificationNumber = reader["TaxIdentificationNumber"].ToString();
                    user.PreferredPaymentMethodID = reader["PreferredPaymentMethodID"] as int?;
                    user.PMName = reader["PMName"].ToString();
                    user.BankNameID = reader["BankNameID"] as int?;
                    user.PMBankName = reader["PMBankName"].ToString();
                    user.AccountNumber = reader["AccountNumber"].ToString();
                    user.AccountHolderName = reader["AccountHolderName"].ToString();

                }

                _healthCareConnection.Close();
                // Return the user object as a response
                return Ok(new { message = "GET single data successful", user });
            }
            else
            {
                _healthCareConnection.Close();
                return BadRequest(new { message = "Invalid Inforamtion" });
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
        public IActionResult UpdatePasss(UpdatePasswordModel user)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserRegistration WHERE UserId = @UserId", _healthCareConnection);
                cmd.Parameters.AddWithValue("@UserId", user.userId);

                createPasswordHash(user.newPassword, out byte[] passwordHash, out byte[] passwordSalt);

                _healthCareConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    byte[] storedPasswordHash = (byte[])reader["PasswordHash"];
                    byte[] storedPasswordSalt = (byte[])reader["PasswordSalt"];
                    reader.Close();

                    if (!VerifyPasswordHash(user.oldPassword, storedPasswordHash, storedPasswordSalt))
                    {
                        return BadRequest(new { message = "Password did not match!" });
                    }

                    SqlCommand cmd2 = new SqlCommand("UPDATE UserRegistration SET [PasswordHash] = @passwordHash, [PasswordSalt] = @passwordSalt WHERE UserId = @userId", _healthCareConnection);
                    cmd2.Parameters.AddWithValue("@userId", user.userId);
                    cmd2.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd2.Parameters.AddWithValue("@passwordSalt", passwordSalt);

                    int rowsAffected = cmd2.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        _healthCareConnection.Close();
                        return Ok(new { message = "Password updated successfully!", user.userCode });
                    }
                }

                _healthCareConnection.Close();
                return BadRequest(new { message = "Password did not match!" });
            }
            catch (Exception ex)
            {
                // Handle the exception here. You can log the exception or perform any other necessary actions.
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to return a specific error response or customize as needed.
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }




        //========================tushar=========================
        [HttpPut]
        [Route("UpdateUserProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UserModel userModel)
        {
            try
            {
                string query = @"UPDATE UserRegistration SET Email = @email, Address = @address WHERE UserId = @userID";
                if (userModel.Email == null || userModel.Email == "")
                {
                    return BadRequest(new { message = $"User Email is not Provided." });
                }
                if (userModel.Address == null || userModel.Address == "")
                {
                    return BadRequest(new { message = $"User Address is not Provided." });
                }
                using (SqlCommand command = new SqlCommand(query, _healthCareConnection))
                {
                    command.Parameters.AddWithValue("@email", userModel.Email);
                    command.Parameters.AddWithValue("@address", userModel.Address);
                    command.Parameters.AddWithValue("@userID", userModel.UserId);

                    await _healthCareConnection.OpenAsync();
                    // Execute the command
                    int Res = await command.ExecuteNonQueryAsync();
                    if (Res == 0)
                    {
                        return BadRequest(new { message = $"User didnot found." });
                    }
                    await _healthCareConnection.CloseAsync();
                }
                return Ok(new { message = $"User Profile Updated." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"User profile update error: {ex.Message}" });
            }
        }

    }
}
