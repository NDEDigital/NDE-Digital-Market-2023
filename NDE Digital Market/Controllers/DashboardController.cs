using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.DashboardService;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Authorize(Roles ="seller")]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboard_Service _Dashboard_Service;
        public DashboardController(IDashboard_Service Dashboard_Service)
        {
            _Dashboard_Service = Dashboard_Service;
        }




        [HttpGet]
        [Authorize (Roles ="seller")]
        [Route("sellerDashboard/{UserId}")]
        public async Task<IActionResult> CompanySellerDetails(string UserId)
        {
            try
            {
                if (UserId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _Dashboard_Service.CompanySellerDetails(UserId);
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
