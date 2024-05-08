using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Services.WishListService;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WishListController : ControllerBase
    {
        private readonly IWishList _wishlist;

        public WishListController(IWishList wishList)
        {
            _wishlist = wishList;
        }


        [HttpGet]
        [Route("GetWishList/{UserId}")]
        public async Task<IActionResult> GetWishList(int UserId)
        {

            try
            {
                if(UserId > 0)
                {
                    List<WishListDTO> result = await _wishlist.GetWishList(UserId);
                    if(result == null)
                    {
                        return NotFound(new { message = "No WishList Data Found." });
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { message = "Give Proper UserId." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return BadRequest(new { message = "There is problem In retriving the WishList." });
            }

        }



        [HttpPost("InsertWishList/{UserId}/{ProductId}/{CompanyCode}")]
        public async Task<IActionResult> InsertWishList(int UserId, string ProductId,  string CompanyCode)
        {
            try
            {
                if (UserId is not 0 || ProductId is not null || CompanyCode is not null)
                {
                    return Ok(await _wishlist.InsertWishList(UserId, ProductId, CompanyCode));
                }
                else
                {
                    return BadRequest(new { message = "Give Proper Input" });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }

        }



        [HttpDelete("DeleteWishList/{UserId}/{ProductId}/{CompanyCode}")]
        public async Task<IActionResult> DeleteWishList(int UserId, string ProductId, string CompanyCode)
        {
            try
            {
                if (UserId is not 0 || ProductId is not null || CompanyCode is not null)
                {
                    return Ok(await _wishlist.InsertWishList(UserId, ProductId, CompanyCode));
                }
                else
                {
                    return BadRequest(new { message = "Give Proper Input" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



    }
}
