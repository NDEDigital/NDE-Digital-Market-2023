using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection con;

        public BannerController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = configuration.GetConnectionString("HealthCare");
            con = new SqlConnection(connectionString);
        }


        [HttpGet]
        [Route("GetBanners")]
        public async Task<IActionResult> GetBanners()
        {
            try
            {
                await con.OpenAsync();
                string query = "SELECT BannerID, UserId, IsActive, AddedDate, AddedBy, UpdatedDate, UpdatedBy, AddedPC, UpdatedPC, CompanyCode, BannerDescription, StartDate, EndDate, IsPayment, PaymentRemarks FROM AdBanner";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        List<AddBanner> banners = new List<AddBanner>();

                        while (await reader.ReadAsync())
                        {
                            AddBanner banner = new AddBanner
                            {
                                BannerID = Convert.ToInt32(reader["BannerID"]),
                                UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : (int?)null,
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                AddedDate = reader["AddedDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddedDate"]) : (DateTime?)null,
                                AddedBy = reader["AddedBy"].ToString(),
                                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : (DateTime?)null,
                                UpdatedBy = reader["UpdatedBy"].ToString(),
                                AddedPC = reader["AddedPC"].ToString(),
                                UpdatedPC = reader["UpdatedPC"].ToString(),
                                CompanyCode = reader["CompanyCode"] != DBNull.Value ? reader["CompanyCode"].ToString() : null,
                                BannerDescription = reader["BannerDescription"].ToString(),
                                StartDate = reader["StartDate"] != DBNull.Value ? Convert.ToDateTime(reader["StartDate"]) : (DateTime?)null,
                                EndDate = reader["EndDate"] != DBNull.Value ? Convert.ToDateTime(reader["EndDate"]) : (DateTime?)null,
                                IsPayment = reader["IsPayment"] != DBNull.Value ? Convert.ToBoolean(reader["IsPayment"]) : false,
                                PaymentRemarks = reader["PaymentRemarks"] != DBNull.Value ? reader["PaymentRemarks"].ToString() : null
                            };

                            banners.Add(banner);
                        }

                        return Ok(banners);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving banners" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }





        [HttpGet]
        [Route("GetBanner/{id}")]
        public async Task<IActionResult> GetBanner(int id)
        {
            try
            {
                await con.OpenAsync();
                string query = "SELECT * FROM AdBanner WHERE BannerID = @BannerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BannerID", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            AddBanner banner = new AddBanner
                            {
                                BannerID = Convert.ToInt32(reader["BannerID"]),
                                // Populate other properties similarly
                            };

                            return Ok(banner);
                        }
                        else
                        {
                            return NotFound(new { message = "Banner not found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving the banner" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }



        [HttpPost]
        [Route("AddBanner")]
        public async Task<IActionResult> AddBanner([FromForm] AddBanner banner)
        {
            try
            {
                await con.OpenAsync();
                string query = @"INSERT INTO AdBanner (UserId, IsActive, AddedDate, AddedBy, UpdatedDate, UpdatedBy, AddedPC, UpdatedPC, CompanyCode, BannerDescription, BannerImage, StartDate, EndDate, IsPayment, PaymentRemarks)
                     VALUES (@UserId, @IsActive, @AddedDate, @AddedBy, @UpdatedDate, @UpdatedBy, @AddedPC, @UpdatedPC, @CompanyCode, @BannerDescription, @BannerImage, @StartDate, @EndDate, @IsPayment, @PaymentRemarks);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", banner.UserId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", banner.IsActive);
                    cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AddedBy", banner.AddedBy);
                    cmd.Parameters.AddWithValue("@UpdatedDate", banner.UpdatedDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy);
                    cmd.Parameters.AddWithValue("@AddedPC", banner.AddedPC);
                    cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC);
                    cmd.Parameters.AddWithValue("@CompanyCode", banner.CompanyCode);
                    cmd.Parameters.AddWithValue("@BannerDescription", banner.BannerDescription);
                    // Check if BannerImage is not null
                 
                    cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPayment", banner.IsPayment);
                    cmd.Parameters.AddWithValue("@PaymentRemarks", banner.PaymentRemarks);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Banner added successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error adding the banner" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }



        [HttpPut]
        [Route("UpdateBanner/{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromForm] AddBanner updatedBanner)
        {
            try
            {
                await con.OpenAsync();
                string query = @"UPDATE AdBanner
                         SET UserId = @UserId, IsActive = @IsActive, AddedDate = @AddedDate, AddedBy = @AddedBy, 
                             UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy, AddedPC = @AddedPC, UpdatedPC = @UpdatedPC, 
                             CompanyCode = @CompanyCode, BannerDescription = @BannerDescription, BannerImage = @BannerImage, 
                             StartDate = @StartDate, EndDate = @EndDate
                         WHERE BannerID = @BannerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", updatedBanner.UserId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", updatedBanner.IsActive);
                    cmd.Parameters.AddWithValue("@AddedDate", updatedBanner.AddedDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AddedBy", updatedBanner.AddedBy);
                    cmd.Parameters.AddWithValue("@UpdatedDate", updatedBanner.UpdatedDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedBanner.UpdatedBy);
                    cmd.Parameters.AddWithValue("@AddedPC", updatedBanner.AddedPC);
                    cmd.Parameters.AddWithValue("@UpdatedPC", updatedBanner.UpdatedPC);
                    cmd.Parameters.AddWithValue("@CompanyCode", updatedBanner.CompanyCode);
                    cmd.Parameters.AddWithValue("@BannerDescription", updatedBanner.BannerDescription);
                    cmd.Parameters.AddWithValue("@BannerImage", updatedBanner.BannerImage);
                    cmd.Parameters.AddWithValue("@StartDate", updatedBanner.StartDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", updatedBanner.EndDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPayment", updatedBanner.IsPayment);
                    cmd.Parameters.AddWithValue("@PaymentRemarks", updatedBanner.PaymentRemarks);
                    cmd.Parameters.AddWithValue("@BannerID", id);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Banner updated successfully" });
                    }
                    else
                    {
                        return NotFound(new { message = "Banner not found" });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating the banner" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }



        [HttpDelete]
        [Route("DeleteBanner/{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            try
            {
                await con.OpenAsync();

                // Check if the banner exists
                string checkQuery = "SELECT COUNT(*) FROM AdBanner WHERE BannerID = @BannerID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@BannerID", id);
                    int bannerCount = (int)await checkCmd.ExecuteScalarAsync();

                    if (bannerCount == 0)
                    {
                        return NotFound(new { message = "Banner not found" });
                    }
                }

                // Delete the banner
                string deleteQuery = "DELETE FROM AdBanner WHERE BannerID = @BannerID";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BannerID", id);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Banner deleted successfully" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deleting the banner" });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deleting the banner" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }
    }
}
