using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.GetBuyerInAdminService;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Authorize]
    public class getBuyerInAdminController : ControllerBase
    {
        private readonly IGetBuyerInAdmin_Service _getBuyerInAdmin_Service;

        public getBuyerInAdminController(IGetBuyerInAdmin_Service getBuyerInAdmin_Service)
        {
            _getBuyerInAdmin_Service = getBuyerInAdmin_Service;
        }


        [HttpGet]
        [Route("getBuyerInAdmin/{IsBuyer}")]
        [Authorize(Roles = "seller,admin")]
        public async Task<IActionResult> CompanySellerDetails( bool IsBuyer, bool IsActive)
        {
            try
            {
                if (IsBuyer == null || IsActive == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _getBuyerInAdmin_Service.CompanySellerDetails(IsBuyer, IsActive);
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
       
        
    }
}

