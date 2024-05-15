using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Services.AddBanner;
using NDE_Digital_Market.Services.CompanyRegistrationServices;
using NDE_Digital_Market.SharedServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Services.AddBanner;
using Google.Api.Gax.ResourceNames;
using static Org.BouncyCastle.Math.EC.ECCurve;




namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddBannerController : ControllerBase
    {

        private readonly IAddBanner _AddBanner;
        private readonly string _connectionString;

       
        

        public AddBannerController(IAddBanner addBanner, IConfiguration config) 
        {
            this._AddBanner = addBanner;
            CommonServices commonServices = new CommonServices(config);
            _connectionString = commonServices.HealthCareConnection;
        }



        [HttpPost("AddBanner")]
        public async Task<IActionResult> AddBanners([FromForm] BannerDto bannerDto)
        {
            var res = await _AddBanner.AddBanners(bannerDto);
            if (res != null)
            {
                return Ok(new { message = res });

            }

            return BadRequest(new { message = "Company already exists!" });

        }

   



        [HttpGet("GetAddBannerForAdmin")]
        public async Task<ActionResult<List<ImageBanner>>> GetAddBannerForAdmin(bool? status)
        {
            var banner = await _AddBanner.GetAddBannerForAdmin(status);
            return banner;
        }

        [HttpGet("GetAddBannerForSeller")]
        public async Task<ActionResult<List<ImageBanner>>> GetAddBannerForSeller(string ComapnayCode)
        {
            var banner = await _AddBanner.GetAddBannerForSeller(ComapnayCode);
            return banner;
        }

        [HttpGet("GetBannerForShowingInHomePage")]
        public async Task<ActionResult<List<ImageBanner>>> GetBannerForShowingInHomePage()
        {
            var banner = await _AddBanner.GetBannerForShowingInHomePage();
            return banner;
        }


        [HttpGet(" GetAddForShowingInHomePage")]
        public async Task<ActionResult<List<ImageBanner>>> GetAddForShowingInHomePage()
        {
            var banner = await _AddBanner.GetAddForShowingInHomePage();
            return banner;
        }





        [HttpPut("UpdateBanner")]
        public async Task<IActionResult> UpdateBanner([FromForm] BannerDto banner)
        {
            var mssg = await _AddBanner.UpdateBanner(banner);
            return Ok(new { message = mssg });
        }



        [HttpPut("UpdateBannerStatus")]
        public async Task<IActionResult> UpdateBannerStatus([FromBody] BannerDto banner)
        {
            if (banner == null || banner.BannerID <= 0)
            {
                return BadRequest(new
                {
                    Message = "Invalid banner data."
                });
            }
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                

                await connection.OpenAsync();







                if(banner.IsActive == true)
                {
                    string checkquery = @"SELECT  Count(BannerID) as count FROM [NDE_Digital_Development].[dbo].[AdBanner]
                                    where IsAds = 1 and IsActive = 1 and IsBannerStatus = 1 and EndDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and StartDate <= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0);";

                    SqlCommand checkcmd = new SqlCommand(checkquery, connection);
                    using (SqlDataReader reader = await checkcmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int res = Convert.ToInt32(reader["count"]);
                            if (res == 6)
                            {
                                return BadRequest(new
                                {
                                    Message = "Ads Can't be Approved. Max Size Reached."
                                });
                            }
                        }
                    }

                }
                string query = @"UPDATE AdBanner 
                     SET IsActive = @IsActive,
                         UpdatedDate = @UpdatedDate,
                         UpdatedBy = @UpdatedBy,
                         UpdatedPC = @UpdatedPC,
                         StartDate = @StartDate,
                         EndDate = @EndDate,
                         IsBannerStatus = @IsBannerStatus
                     WHERE BannerID = @BannerID;";


                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                        cmd.Parameters.AddWithValue("@BannerID", banner.BannerID);
                        cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsBannerStatus", banner.IsBannerStatus ?? (object)DBNull.Value);

                        var result = await cmd.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            return Ok(new
                            {
                                Message = "Banner updated successfully."
                            });
                        }
                        else
                        {
                            return BadRequest(new
                            {
                                Message = "Failed to update banner."
                            });
                        }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new
                {
                    Message = "Failed to update banner/Ads."
                });
            }
            finally
            {
                connection.CloseAsync();
            }


        }

        //[HttpPut("UpdateBanners")]
        //public async Task<IActionResult> UpdateBanners([FromForm] ImageBanner banner)
        //{
        //    var upd = await _AddBanner. UpdateBanners(banner);
        //    return Ok(new { message = upd });
        //}






        [HttpDelete("DeleteBanner/{bannerId}")]
        public async Task<IActionResult> DeleteBanner(int bannerId)
        {

            bool deleted = await _AddBanner.DeleteBanner(bannerId);


            if (deleted)
            {
                // Return a success response
            
                // If needed, you can return the inserted ItemID
                return Ok(new
                {
                    Message = "Banner deleted successfully.",

                });
            }
            else
            {

        
                return BadRequest(new
                {
                    Message = "Failed to delete banner.",

                });
            }
        }






    }


}