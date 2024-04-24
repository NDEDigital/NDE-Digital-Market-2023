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




namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddBannerController : ControllerBase
    {

        private readonly IAddBanner _AddBanner;

        public AddBannerController(IAddBanner addBanner) 
        {
            this._AddBanner = addBanner;
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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BannerDto>>> GetBanners()
        //{
        //    try
        //    {
        //        var banners = await _AddBanner.GetBanners();
        //        return Ok(banners);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BannerDto>>> GetBanners()
        //{
        //    try
        //    {
        //        var banners = await _AddBanner.GetBanners();
        //        return Ok(banners);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}



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