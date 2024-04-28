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






        [HttpPut("UpdateBanner")]
        public async Task<IActionResult> UpdateBanner([FromForm] BannerDto banner)
        {
            var mssg = await _AddBanner.UpdateBanner(banner);
            return Ok(new { message = mssg });
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