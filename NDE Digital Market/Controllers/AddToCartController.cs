using Microsoft.AspNet.WebHooks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;

using NDE_Digital_Market.SharedServices;
using CommonServices = NDE_Digital_Market.SharedServices.CommonServices;
using System.Data;
using Google.Api.Gax.ResourceNames;
using NDE_Digital_Market.Services.AddToCartService;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddToCartController : ControllerBase
    {

        private readonly IAddToCart_Service _AddToCart_Service;
        public AddToCartController(IAddToCart_Service AddToCart_Service)
        {
            _AddToCart_Service = AddToCart_Service;
        }


        [HttpGet]
        [Route("GetAddToCartData")]
        public async Task<IActionResult> GetAddToCartData()
        {
            try
            {

                object res = await _AddToCart_Service.GetAddToCartData();
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


        [HttpGet]
        [Route("GetAddToCartData/{userId}")]
        public async Task<IActionResult> GetAddToCartDataByUserID(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddToCart_Service.GetAddToCartDataByUserID(userId);
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


        [HttpPost]
        [Route("AddToCartData")]
        public async Task<IActionResult> AddToCartData([FromForm] InsertAddToCartDataDTO addToCart)
        {
            try
            {
                if (addToCart == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddToCart_Service.AddToCartData(addToCart);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        [HttpDelete]
        [Route("DeleteAddToCart/{id}")]
        public async Task<IActionResult> DeleteAddToCart(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _AddToCart_Service.DeleteAddToCart(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



    }
}
