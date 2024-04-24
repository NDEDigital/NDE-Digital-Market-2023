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

   



        [HttpGet("GetAddBanner")]
        public async Task<ActionResult<List<ImageBanner>>> GetAddBanner(bool? status)
        {
            var banner = await _AddBanner.GetAddBanner(status);
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
                return BadRequest("Invalid banner data.");
            }

            // Corrected SQL query with 'SET'
            string query = @"UPDATE AdBanner 
                     SET IsActive = @IsActive,
                         UpdatedDate = @UpdatedDate,
                         UpdatedBy = @UpdatedBy,
                         UpdatedPC = @UpdatedPC,
                         StartDate = @StartDate,
                         EndDate = @EndDate,
                         IsBannerStatus = @IsBannerStatus
                     WHERE BannerID = @BannerID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
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

                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok("Banner updated successfully.");
        }



        [HttpDelete("DeleteBanner/{bannerId}")]
        public async Task<IActionResult> DeleteBanner(int bannerId)
        {

            bool deleted = await _AddBanner.DeleteBanner(bannerId);


            if (deleted)
            {
                // Return a success response
                return Ok("Banner deleted successfully.");
            }
            else
            {
                // Return an error response
                return BadRequest("Failed to delete banner.");
            }
        }






    }


}