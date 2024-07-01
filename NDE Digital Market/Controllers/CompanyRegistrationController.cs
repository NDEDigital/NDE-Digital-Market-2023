using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.CompanyRegistrationServices;
using Microsoft.AspNetCore.Authorization;

using System.Data;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;

namespace NDE_Digital_Market.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
  
    public class CompanyRegistrationController : ControllerBase
    {
        private readonly ICompanyRegistration _CompanyRegistration;
        public CompanyRegistrationController(ICompanyRegistration companyRegistration)
        {
            this._CompanyRegistration = companyRegistration;
        }


        [HttpPost("Companyexists")]
        public async Task<IActionResult> CompanyexistsCheckAsync(CompanyModel companyDto)
        {
            try
            {
                var res = await _CompanyRegistration.CompanyexistsCheckAsync(companyDto);
                //return Ok(res);
                return Ok(new { message = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CompanyRegistrationPostAsync([FromForm] CompanyModel companyDto)
        {
            try
            {
                var res = await _CompanyRegistration.CompanyRegistrationPostAsync(companyDto);
                if (res != null)
                {
                    return Ok(new { message = res });

                }
                return BadRequest(new { message = "Company already exists!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

        [HttpGet("GetCompaniesBasedOnStatus")]
        public async Task<IActionResult> GetCompaniesAsync(int status)
        {
            try
            {
                var res = await _CompanyRegistration.GetCompaniesAsync(status);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

        [HttpPut("UpdateCompany")]
        [Authorize(Roles = "seller,admin")]
        public async Task<IActionResult> UpdateCompany(CompanyModel companyDto)
        {
            try
            {
                //CompanyModel companyModel = JsonConvert.DeserializeObject<CompanyModel>(data);
                var res = await _CompanyRegistration.UpdateCompanyAsync(companyDto);
                return Ok(new { message = res });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

    }
}
