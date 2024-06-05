
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Services.AddBanner;
using NDE_Digital_Market.SharedServices;
using System.Data.SqlClient;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model.DTO;

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
        public async Task<IActionResult> AddBanners([FromForm] InsertAdsAndBannerDTO bannerDto)
        {
            try
            {
                if (bannerDto == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddBanner.AddBanners(bannerDto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

   



        [HttpGet("GetAddBannerForAdmin")]
        public async Task<IActionResult> GetAddBannerForAdmin(bool? status)
        {
            try
            {

                object res = await _AddBanner.GetAddBannerForAdmin(status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }

        [HttpGet("GetAddBannerForSeller")]
        public async Task<IActionResult> GetAddBannerForSeller(string ComapnayCode)
        {
            try
            {
                if (ComapnayCode == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddBanner.GetAddBannerForSeller(ComapnayCode);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }

        [HttpGet("GetBannerForShowingInHomePage")]
        public async Task<IActionResult> GetBannerForShowingInHomePage()
        {
            try
            {

                object res = await _AddBanner.GetBannerForShowingInHomePage();
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        [HttpGet(" GetAddForShowingInHomePage")]
        public async Task<ActionResult> GetAddForShowingInHomePage()
        {
            try
            {
                object res = await _AddBanner.GetAddForShowingInHomePage();
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }





        [HttpPut("UpdateBanner")]
        public async Task<IActionResult> UpdateBanner([FromForm] UpdateAdsAndBannerDTO banner)
        {
            try
            {
                if (banner == null || banner.BannerID == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddBanner.UpdateBanner(banner);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



        [HttpPut("UpdateBannerStatus")]
        public async Task<IActionResult> UpdateBannerStatus([FromBody] UpdateAdsAndBannerDTO banner)
        {

            try
            {
                if (banner == null || banner.BannerID == null)
                {
                    return BadRequest(new { Message = "Invalid banner data."});
                }
                object res = await _AddBanner.UpdateBannerStatus(banner);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }


        }



        //[HttpPut("UpdateBanners")]
        //public async Task<IActionResult> UpdateBanners([FromForm] ImageBanner banner)
        //{
        //    var upd = await _AddBanner. UpdateBanners(banner);
        //    return Ok(new { message = upd });
        //}






        [HttpDelete("DeleteBanner/{bannerId}")]
        public async Task<IActionResult> DeleteBanner(string bannerId)
        {

            try
            {
                if (bannerId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddBanner.DeleteBanner(bannerId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }






    }


}