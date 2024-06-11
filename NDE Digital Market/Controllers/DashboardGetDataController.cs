using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.DashboardGetDataService;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "seller,admin")]
    public class DashboardGetDataController : ControllerBase
    {
        private readonly IDashboardGetData_Service _dashboardGetData_Service;

        public DashboardGetDataController(IDashboardGetData_Service dashboardGetData_Service)
        {
            _dashboardGetData_Service = dashboardGetData_Service;
        }

        [HttpGet]
        [Route("SellerPermissionData/{UserId}/{Status1}")]
        public async Task<IActionResult> GetPermissionData(string UserId, int Status1)
        {
            try
            {
                if (UserId == null || Status1 == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _dashboardGetData_Service.GetPermissionData(UserId, Status1);
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
