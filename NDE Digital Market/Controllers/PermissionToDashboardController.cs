using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Services.PermissionToDashboardService;
using NDE_Digital_Market.SharedServices;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Authorize]
    public class PermissionToDashboardController : ControllerBase
    {
        private readonly IPermissionToDashboard_Service _permissionToDashboard_Service;

        public PermissionToDashboardController(IPermissionToDashboard_Service permissionToDashboard_Service)
        {
            _permissionToDashboard_Service = permissionToDashboard_Service;
        }

        [HttpPost]
        [Authorize(Roles = "seller")]
        [Route("GiveAcessDashboard/{UserId}/{MenuId}")]
        public async Task<IActionResult> InsertPermissionToDashboard(string UserId, string MenuId)
        {
            try
            {
                if (UserId == null || MenuId == null)
                {
                    return BadRequest(new { message = "Give Proper Data." });
                }
                return Ok(await _permissionToDashboard_Service.InsertPermissionToDashboard(UserId, MenuId));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { message = "Give Proper Data." });
            }
        }


        [HttpGet]
        [Authorize(Roles = "seller")]
        [Route("GetPermissionData/{UserId}")]
        public async Task<IActionResult> GetPermissionData(string UserId)
        {
            try
            {
                object res = await _permissionToDashboard_Service.GetPermissionData(UserId);
                if (res == null)
                {
                    return NotFound(new { message = "Data not Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, returning an error response, etc.)
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}

