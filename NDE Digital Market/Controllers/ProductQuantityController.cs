using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NDE_Digital_Market.Services.ProductQuantityService;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductQuantityController : Controller
    {

        private readonly IProductQuantity_Service _productQuantity_Service;
        public ProductQuantityController(IProductQuantity_Service productQuantity_Service)
        {
            _productQuantity_Service = productQuantity_Service;
        }


        [HttpGet]
        [Authorize(Roles = "seller")]
        [Route("ProductGroupsDropdownByUserId/{userID}")]
        public async Task<IActionResult> ProductGroupsDropdownByUserId(string userID)
        {
            if (userID == null)
            {
                return BadRequest(new { message = "Give Proper Data." });
            }
            object res = await _productQuantity_Service.ProductGroupsDropdownByUserId(userID);
            if (res == null)
            {
                return NotFound(new { message = "Data not Found." });
            }
            return Ok(res);
        }


        [HttpGet("GetProductForAddQtyByUserId/{UserId}/{productGroupId}")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> GetProductForAddQtyByUserId(string UserId, string productGroupId)
        {
            try
            {
                if (UserId == null || productGroupId == null)
                {
                    return BadRequest(new { message = "Give Proper Data." });
                }
                object res = await _productQuantity_Service.GetProductForAddQtyByUserId(UserId, productGroupId);
                if (res == null)
                {
                    return NotFound(new { message = "Data not Found." });
                }
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


        [HttpPost("PortalReceivedPost")]
        [Authorize(Roles ="seller")]
        public async Task<IActionResult> InsertPortalReceivedAsync(InsertPortalReceivedMasterDTO portaldata)
        {
            try
            {
                if (portaldata == null)
                {
                    return BadRequest(new { message = "Give Proper portalreceived Data." });
                }
                return Ok(await _productQuantity_Service.InsertPortalReceivedAsync(portaldata));
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


        [HttpPost("CreateSellerProductPriceAndOffer")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> CreateSellerProductPriceAndOfferAsync([FromForm] InsertSellerProductPriceAndOfferDTO sellerproductdata)
        {
            try
            {
                if (sellerproductdata == null)
                {
                    return BadRequest(new { message = "Give Proper price and offer Data." });
                }
                return Ok(await _productQuantity_Service.CreateSellerProductPriceAndOfferAsync(sellerproductdata));
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


        [HttpPut("UpdateSellerProductPriceAndOffer")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> UpdateSellerProductPriceAndOffer([FromForm] UpdateSellerProductPriceAndOfferDTO sellerproductdata)
        {
            try
            {
                if (sellerproductdata == null)
                {
                    return BadRequest(new { message = "No product IDs provided." });
                }
                return Ok(await _productQuantity_Service.UpdateSellerProductPriceAndOffer(sellerproductdata));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        [HttpGet]
        [Authorize(Roles = "seller")]
        [Route("GetSellerProductsByCompanyCode")]
        public async Task<IActionResult> GetSellerProductsByCompanyCode(string userID, Int32? status = null)
        {

            if (userID == null)
            {
                return BadRequest(new { message = "Give Proper Data." });
            }
            object res = await _productQuantity_Service.GetSellerProductsForPriceAndOfferByUserId(userID, status);
            if (res == null)
            {
                return NotFound(new { message = "Data not Found." });
            }
            return Ok(res);
        }


        [HttpGet("GetPortalReceivedByUserId")]
        [Authorize(Roles = "seller")]
        public async Task<ActionResult> GetPortalReceivedByUserId(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest(new { message = "Give Proper Data." });
                }
                object res = await _productQuantity_Service.GetPortalReceivedByUserId(userId);
                if (res == null)
                {
                    return NotFound(new { message = "Data not Found." });
                }
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


        [HttpGet]
        [Authorize(Roles = "seller")]
        [Route("GetPortalData")]
        public async Task<IActionResult> GetPortalData(string PortalReceivedId)
        {
            try
            {
                if (PortalReceivedId == null)
                {
                    return BadRequest(new { message = "Give Proper Data." });
                }
                object res = await _productQuantity_Service.GetPortalData(PortalReceivedId);
                if (res == null)
                {
                    return NotFound(new { message = "Data not Found." });
                }
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }



    }
}
