using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Services.SellerActiveAndInactiveService;
using NDE_Digital_Market.SharedServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    //[Authorize(Roles = "seller")]
    public class SellerActive_InactiveController : ControllerBase
    {
        private readonly ISellerActiveAndInactive_Service _sellerActiveAndInactive_Service;

        public SellerActive_InactiveController(ISellerActiveAndInactive_Service sellerActiveAndInactive_Service)
        {
            _sellerActiveAndInactive_Service = sellerActiveAndInactive_Service;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("getSellerActive&Inactive/{IsSeller}")]
        public async Task<IActionResult> CompanySellerDetails(string? CompanyCode, bool IsSeller, bool IsActive)
        {
            object res = await _sellerActiveAndInactive_Service.CompanySellerDetails(CompanyCode, IsSeller, IsActive);
            if (res == null)
            {
                return NotFound(new { message = "Unit not Found." });
            }
            return Ok(res);
        }

        [HttpPut]
        [Route("updateSellerActive&Inactive")]
        public async Task<IActionResult> UpdateSellerProductStatusAsync(string userIds,bool isActive)
        {
            if (string.IsNullOrEmpty(userIds))
            {
                return BadRequest(new { message = "No unit IDs provided." });
            }
            return Ok(await _sellerActiveAndInactive_Service.UpdateSellerProductStatusAsync(userIds, isActive));

        }

    }
}