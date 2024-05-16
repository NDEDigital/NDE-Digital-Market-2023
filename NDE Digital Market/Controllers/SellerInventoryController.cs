using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Services.SellerInventoryService;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;
namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Authorize]
    public class SellerInventoryController : ControllerBase
    {
        private readonly ISellerInventory_Service _sellerInventory_Service;
        public SellerInventoryController(ISellerInventory_Service sellerInventory_Service)
        {
            _sellerInventory_Service = sellerInventory_Service;
        }



        [HttpGet]

        [Route("GetSellerInventoryDataBySellerId/{UserId}")]
        public async Task<IActionResult> GetSellerInventoryDataBySellerId(string UserId)
        {
            if(UserId == null)
            {
                return BadRequest(new { message = "Give Valid Data." });
            }
            object res = await _sellerInventory_Service.GetSellerInventoryDataBySellerId(UserId);
            if (res == null)
            {
                return NotFound(new { message = "Unit not Found." });
            }
            return Ok(res);
        }

    }
}