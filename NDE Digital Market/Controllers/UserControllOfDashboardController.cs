using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.UserControllOfDashboardService;
using NDE_Digital_Market.SharedServices;
using System.Data.SqlClient;

[ApiController]
[Authorize]
public class UserControllOfDashboardController : ControllerBase
{

    private readonly IUserControllOfDashboard_Service _UserControllOfDashboard_Service;
    public UserControllOfDashboardController(IUserControllOfDashboard_Service UserControllOfDashboard_Service)
    {
        _UserControllOfDashboard_Service = UserControllOfDashboard_Service;
    }



    [HttpDelete("deleteMenuItems")]
    [Authorize (Roles ="seller")]
    public async Task<IActionResult> DeleteMenuItems(string UserId, [FromBody] List<string> menuIdsToDelete)
    {
        try
        {
            if (UserId == null || menuIdsToDelete ==  null)
            {
                return NotFound(new { message = "Give Valid Data." });
            }
            object res = await _UserControllOfDashboard_Service.DeleteMenuItems(UserId, menuIdsToDelete);
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
