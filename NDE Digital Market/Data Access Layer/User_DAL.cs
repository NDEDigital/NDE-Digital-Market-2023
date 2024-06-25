using Microsoft.IdentityModel.Tokens;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class User_DAL
    {
        private readonly string _healthCareConnection;
        private readonly  IConfiguration _configuration;

        public User_DAL(IConfiguration config)
        {
            _configuration = config;
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }
        public async Task<bool> UserExist(UserModel user)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM  [UserRegistration] WHERE PhoneNumber = @phoneNumber", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
            await con.OpenAsync();
            int count = (int)await cmd.ExecuteScalarAsync();
            await con.CloseAsync();
            Boolean userExist = false;
            if (count > 0)
            {
                userExist = true;
            }
            return userExist;
            //   return BadRequest(new { message = "User does not exist" , userExist });
        }

        private async Task<int?> CompanyExistAsync(string CompanyCode)
        {
            string query = @"SELECT COALESCE(CASE WHEN COUNT(UR.CompanyCode) < CR.MaxUser THEN 1 ELSE 0 END, 0) AS UserCount
                                FROM CompanyRegistration CR
                                LEFT JOIN UserRegistration UR ON UR.CompanyCode = CR.CompanyCode AND UR.IsActive = 1
                                WHERE CR.CompanyCode = @CompanyCode
								and CR.IsActive = 1
                                GROUP BY CR.MaxUser;";

            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                await con.OpenAsync();

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);

                    // Execute the query and store the result in the 'userCount' variable
                    var result = await cmd.ExecuteScalarAsync();

                    await con.CloseAsync();

                    // Check if the result is not null and cast it to int
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // Returning null in case of an exception
                return null;
            }
        }


        public async Task<object> CreateUser(UserModel userModel)
        {
            try
            {
                int? companyExist = 0;

                if (!string.IsNullOrEmpty(userModel.CompanyCode))
                {
                    companyExist = await CompanyExistAsync(userModel.CompanyCode);
                    if (companyExist == null)
                    {
                        return new
                        {
                            message = "No Campany Found with this ID"
                        };
                    }
                    else if (companyExist == 0)
                    {
                        return new
                        {
                            message = "Max user count exited for this company!"
                        };
                    }

                }
                var userExistResult = await UserExist(userModel);
                if (userExistResult)
                {
                    return new { message = "User already exists" };
                }

                SqlConnection con = new SqlConnection(_healthCareConnection);
                string systemCode = string.Empty;

                // Execute the stored procedure to generate the system code
                SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con);
                {
                    cmdSP.CommandType = CommandType.StoredProcedure;
                    cmdSP.Parameters.AddWithValue("@TableName", "UserRegistration");
                    cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSP.Parameters.AddWithValue("@AddNumber", 1);


                    await con.OpenAsync();
                    systemCode = cmdSP.ExecuteScalar()?.ToString();
                    await con.CloseAsync();
                }

                //SP END

                if (companyExist == 1)
                {
                    string updateCompanyAdmin = @"
                      UPDATE CompanyRegistration
                        SET CompanyAdminId = @CompanyAdminId
                        WHERE CompanyAdminId IS NULL AND CompanyCode = @CompanyCode;
                        ";
                    SqlCommand cmd1 = new SqlCommand(updateCompanyAdmin, con);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@CompanyCode", userModel.CompanyCode);
                    cmd1.Parameters.AddWithValue("@CompanyAdminId", int.Parse(systemCode.Split('%')[0]));

                    await con.OpenAsync();
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    await con.CloseAsync();
                }




                // Encrypt the Password
                //string encryptedPassword = CommonServices.EncryptPassword(user.Password);
                //createPasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                //UserModel userModel = new UserModel();
                userModel.UserId = int.Parse(systemCode.Split('%')[0]);
                userModel.UserCode = systemCode.Split('%')[1];
                //userModel.IsBuyer = user.IsBuyer;
                //userModel.IsSeller = user.IsSeller;
                //userModel.IsAdmin = user.IsAdmin;
                //userModel.FullName = user.FullName;
                //userModel.PhoneNumber = user.PhoneNumber;
                //userModel.Email = user.Email;
                //userModel.PasswordHash = passwordHash;
                //userModel.PasswordSalt = passwordSalt;
                //userModel.Address = user.Address;
                //userModel.AddedDate = DateTime.UtcNow;
                //userModel.CompanyCode = user.CompanyCode ?? string.Empty;

                string query = @"
                            INSERT INTO UserRegistration (
                                UserId, UserCode, IsBuyer, IsSeller, IsAdmin, 
                                FullName, PhoneNumber, Email, PasswordHash, PasswordSalt, Address,                           
                                TimeStamp,IsActive, AddedDate,CompanyCode
                            ) VALUES (
                                @UserID, @UserCode,  @IsBuyer, @IsSeller, @IsAdmin, 
                                @FullName, @PhoneNumber, @Email, @PasswordHash, @PasswordSalt, @Address, 
                                @TimeStamp,@IsActive,@AddedDate,@CompanyCode
                            )";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@UserID", userModel.UserId);
                cmd.Parameters.AddWithValue("@UserCode", userModel.UserCode);
                cmd.Parameters.AddWithValue("@IsBuyer", userModel.IsBuyer.HasValue ? (object)userModel.IsBuyer.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@IsSeller", userModel.IsSeller.HasValue ? (object)userModel.IsSeller.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@IsAdmin", userModel.IsAdmin.HasValue ? (object)userModel.IsAdmin.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@FullName", userModel.FullName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", userModel.PhoneNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", userModel.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PasswordHash", userModel.PasswordHash ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PasswordSalt", userModel.PasswordSalt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", userModel.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AddedDate", userModel.AddedDate.HasValue ? (object)userModel.AddedDate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@TimeStamp", userModel.AddedDate.HasValue ? (object)userModel.AddedDate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@CompanyCode", userModel.CompanyCode);
                cmd.Parameters.AddWithValue("@IsActive", false);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                string encryptedUserCode = CommonServices.EncryptPassword(userModel.UserId.ToString());
                string role = userModel.IsAdmin == true ? "admin" :
                              userModel.IsSeller == true ? "seller" :
                              userModel.IsBuyer == true ? "buyer" :
                              "";
                string token = CreateToken(role);
                var newRefreshToken = CreateRefreshToken(encryptedUserCode);
                return new
                {
                    message = "User created successfully wait for admin approval",
                    encryptedUserCode,
                    role,
                    //token,  // Include the token in the response object
                    //newRefreshToken
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error on User Create",ex.Message);
                return new
                {
                    message = "User Not created successfully wait for admin approval",
                    //token,  // Include the token in the response object
                    //newRefreshToken
                };
            }

            //}
        }


        // =================================================== Login ===================================

        public async Task<object> LoginUser(UserModel user)
        {
            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                string query = @"SELECT UR.UserId, UR.IsBuyer, UR.IsAdmin, UR.IsSeller, UR.PasswordHash, UR.PasswordSalt,CR.CompanyAdminId,CR.CompanyCode,UR.IsActive  FROM  UserRegistration UR
                                    LEFT JOIN CompanyRegistration CR ON CR.CompanyCode = UR.CompanyCode OR CR.CompanyAdminId = UR.UserId
                                    WHERE PhoneNumber = @PhoneNumber";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);

                await con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    bool isActive = (bool)reader["IsActive"];
                    int userId = (int)reader["UserId"];
                    bool IsBuyer = (bool)reader["IsBuyer"];
                    bool IsSeller = (bool)reader["IsSeller"];
                    bool IsAdmin = (bool)reader["IsAdmin"];
                    object companyCodeObject = reader["companyCode"];
                    string companyCode = (companyCodeObject != DBNull.Value) ? companyCodeObject.ToString() : null;

                    bool IsSellerAdmin = false;
                    object adminIdObject = reader["CompanyAdminId"];
                    int adminId;

                    if (adminIdObject != DBNull.Value)
                    {
                        adminId = (int)adminIdObject;
                    }
                    else
                    {
                        adminId = 0;
                    }
                    if (userId == adminId)
                    {
                        IsSellerAdmin = true;
                    }
                    byte[] storedPasswordHash = (byte[])reader["PasswordHash"];
                    byte[] storedPasswordSalt = (byte[])reader["PasswordSalt"];

                    await con.CloseAsync();
                    string role = IsAdmin ? "admin" : IsSeller ? "seller" : IsBuyer ? "buyer" : "";
                    if (role == "admin")
                    {
                        companyCode = "admin";
                    }
                    if (!CommonServices.VerifyPasswordHash(user.Password, storedPasswordHash, storedPasswordSalt))
                    {
                        return new { message = "Invalid password", IsSuccess = false };
                    }
                    if (!isActive)
                    {
                        return new { message = "Please waiting for admin approval", userId, role, IsSellerAdmin, companyCode, IsSuccess = false };
                    }


                    string token = CreateToken(role);
                    var newRefreshToken = CreateRefreshToken(userId.ToString());
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(10)

                    };
                    var cookieOptions2 = new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(3)

                    };



                    //string UserId = userId.ToString();
                    if(companyCode != null)
                    {
                        companyCode = CommonServices.EncryptPassword(companyCode);
                    }
                    string UserId = CommonServices.EncryptPassword(userId.ToString());

                    return (new { message = "Login successful",IsSuccess = true, UserId, role, IsSellerAdmin, companyCode, token, newRefreshToken, cookieOptions , cookieOptions2 });
                }
                else
                {
                    return new { message = "Invalid phone number or Password", IsSuccess = false };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new { message = "An error occurred while processing the request.", IsSuccess = false };
            }
        }

        public async Task<object> GenerateRefreshToken(string token)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            var handler = new JwtSecurityTokenHandler();
            bool tokenRefreshed = true;
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null) throw new ArgumentException("Invalid token");
            }
            catch (ArgumentException)
            {
                tokenRefreshed = false;
                return (new
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
                return (new
                {
                    message = "Token is forbidden.",
                });
                //return Ok(new
                //{
                //    message = "reFreshToken Expired",
                //    tokenRefreshed

                //});
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
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

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    await con.OpenAsync();
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
                    Console.WriteLine("Internal server error: " + ex.Message);
                    return (new
                    {
                        message = "Internal server error."
                    });
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
                return (new
                {
                    message = "Token has been invalidated.",
                });
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
            return (new
            {

                accessToken = "newAccessToken",
                refreshToken = "newRefreshToken",
                tokenRefreshed, newAccessToken, cookieOptions, newRefreshToken, cookieOptions2
            });
        }









        public async Task<DataTable> getSingleUser(string userId)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            try
            {
                DataTable dataTable = new DataTable();

                //byte[] userCodeBytes = Encoding.UTF8.GetBytes(userCode);
                string query = @"SELECT UR.UserId, UR.UserCode, UR.FullName, UR.IsAdmin, UR.IsBuyer, UR.IsSeller, UR.PhoneNumber, UR.Email, UR.Address,
                            CR.CompanyName, DATEDIFF(YEAR, CR.CompanyFoundationDate, GETDATE()) as YearsInBusiness, CR.BusinessRegistrationNumber, 
                            CR.TaxIdentificationNumber, CR.PreferredPaymentMethodID, PM.PMName, CR.BankNameID, PD.PMBankName, CR.AccountNumber,
                            CR.AccountHolderName FROM UserRegistration UR LEFT JOIN CompanyRegistration CR ON UR.CompanyCode = CR.CompanyCode 
                            LEFT JOIN HK_PaymentMethodMaster PM ON CR.PreferredPaymentMethodID = PM.PMMasterID 
                            LEFT JOIN HK_PaymentMethodDetails PD ON CR.BankNameID = PD.PMDetailsID WHERE UR.UserId = @UserId;";

                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                // Return the user object as a response
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                await con.CloseAsync();
            }
        }



        public async Task<object> UpdatePasss(UserModel user)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserRegistration WHERE UserId = @UserId", con);
                cmd.Parameters.AddWithValue("@UserId", user.UserId);

                CommonServices.createPasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    byte[] storedPasswordHash = (byte[])reader["PasswordHash"];
                    byte[] storedPasswordSalt = (byte[])reader["PasswordSalt"];

                    reader.CloseAsync();

                    if (!CommonServices.VerifyPasswordHash(user.OldPassword, storedPasswordHash, storedPasswordSalt))
                    {
                        return (new { message = "Password did not match!" });
                    }

                    SqlCommand cmd2 = new SqlCommand("UPDATE UserRegistration SET [PasswordHash] = @passwordHash, [PasswordSalt] = @passwordSalt WHERE UserId = @userId", con);
                    cmd2.Parameters.AddWithValue("@userId", user.UserId);
                    cmd2.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd2.Parameters.AddWithValue("@passwordSalt", passwordSalt);

                    int rowsAffected = cmd2.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        con.CloseAsync();
                        return (new { message = "Password updated successfully!" });
                    }
                }

                con.CloseAsync();
                return (new { message = "Password did not match!" });
            }
            catch (Exception ex)
            {
                // Handle the exception here. You can log the exception or perform any other necessary actions.
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to return a specific error response or customize as needed.
                return (new { message = "Internal Server Error" });
            }
        }



        public async Task<object> UpdateUserProfileAsync(UserModel userModel)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            try
            {
                string query = @"UPDATE UserRegistration SET Email = @email, Address = @address WHERE UserId = @userID";
                if (userModel.Email == null || userModel.Email == "")
                {
                    return (new { message = $"User Email is not Provided." });
                }
                if (userModel.Address == null || userModel.Address == "")
                {
                    return (new { message = $"User Address is not Provided." });
                }
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@email", userModel.Email);
                    command.Parameters.AddWithValue("@address", userModel.Address);
                    command.Parameters.AddWithValue("@userID", userModel.UserId);

                    await con.OpenAsync();
                    // Execute the command
                    int Res = await command.ExecuteNonQueryAsync();
                    if (Res == 0)
                    {
                        return (new { message = $"User didnot found." });
                    }
                    await con.CloseAsync();
                }
                return (new { message = $"User Profile Updated." });
            }
            catch (Exception ex)
            {
                return (new { message = $"User profile update error: {ex.Message}" });
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

    }
}
