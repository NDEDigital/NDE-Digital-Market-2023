using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.Services.WishListService;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WishListController : ControllerBase
    {
        private readonly IWishList_Service _wishlist;

        public WishListController(IWishList_Service wishList)
        {
            _wishlist = wishList;
        }


        [HttpGet]
        [Route("GetWishList/{UserId}")]
        public async Task<IActionResult> GetWishList(string UserId)
        {

            try
            {
                if(UserId is not null)
                {
                    List<GetAllWishListDTO> result = await _wishlist.GetWishList(UserId);
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
        public async Task<IActionResult> InsertWishList(string UserId, string ProductId,  string CompanyCode)
        {
            try
            {
                if (UserId is not null || ProductId is not null || CompanyCode is not null)
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
        public async Task<IActionResult> DeleteWishList(string UserId, string ProductId, string CompanyCode)
        {
            try
            {
                if (UserId is not null || ProductId is not null || CompanyCode is not null)
                {
                    return Ok(await _wishlist.DeleteWishList(UserId, ProductId, CompanyCode));
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
