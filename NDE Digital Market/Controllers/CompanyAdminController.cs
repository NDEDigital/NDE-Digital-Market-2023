using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Make sure to import this namespace
using NDE_Digital_Market.Services.CompanyAdminService;
using NDE_Digital_Market.SharedServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
  
    public class CompanyAdminController : ControllerBase
    {
        private readonly ICompanyAdmin_Service _CompanyAdmin_Service;
        public CompanyAdminController(ICompanyAdmin_Service CompanyAdmin_Service)
        {
            _CompanyAdmin_Service = CompanyAdmin_Service;
        }

        [HttpGet]
        [Route("CompanySellerDetails/{userId}/{IsActive}")] //gets company seller list without companyAdmin
        public async Task<IActionResult> CompanySellerDetails(string userId,bool IsActive)
        {
            try
            {
                if (userId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _CompanyAdmin_Service.CompanySellerDetails(userId,IsActive);
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
        
        
        [HttpPut]
        // [Authorize(Roles = "seller")]
        [Route("CompanySellerDetailsUpdateUserStatus/{userId}/{IsActive}")]
        public async Task<IActionResult> UpdateUserStatus(string userId, bool IsActive)
        {
            try
            {
                if (userId == null || IsActive == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                object res = await _CompanyAdmin_Service.UpdateUserStatus(userId, IsActive);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }

    }
}
