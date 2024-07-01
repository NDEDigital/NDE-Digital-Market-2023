using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;
using System.Data;
using NDE_Digital_Market.SharedServices;
using NDE_Digital_Market.Services.UnitService;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnit_Service _unit_Service;
        public UnitController( IUnit_Service unit_Service)
        {
            _unit_Service = unit_Service;
        }

        [HttpGet]
        [Route("GetUnitList")]
        public async Task<IActionResult> GetUnitListAsync(bool? isActive)
        {
            try
            {
                object res = await _unit_Service.GetUnitListAsync(isActive);
                if (res == null)
                {
                    return NotFound(new { message = "Unit not Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }


        }

        [HttpPost]
        [Route("AddUnit")]
        public async Task<IActionResult> PostUnit([FromForm] UnitCreationDTO unit)
        {
            try
            {
                if (unit == null)
                {
                    return BadRequest(new { message = "Give Proper Unit Data." });
                }
                return Ok(await _unit_Service.PostUnit(unit));
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

        [HttpPut]
        [Route("UpdateUnit")]
        public async Task<IActionResult> PutUnit([FromForm] UpdateUnitDTO unit)
        {
            try
            {
                if (unit == null || unit.UnitId == null)
                {
                    return BadRequest(new { message = "Invalid unit data." });
                }
                return Ok(await _unit_Service.PutUnit(unit));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

        [HttpPut]
        [Route("UpdateUnitByID")]
        public async Task<IActionResult> UpdateUnitByUnitID(string unitID, bool isActive)
        {
            try
            {
                if (unitID == null || isActive == null)
                {
                    return BadRequest(new { message = "Invalid unit data." });
                }
                return Ok(await _unit_Service.UpdateUnitByUnitID(unitID, isActive));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


        [HttpPut]
        [Route("UpdateUnitsByID")]
        public async Task<IActionResult> UpdateUnitsByUnitID(string unitIDs, bool isActive)
        {
            try
            {
                if (string.IsNullOrEmpty(unitIDs))
                {
                    return BadRequest(new { message = "No unit IDs provided." });
                }
                return Ok(await _unit_Service.UpdateUnitsByUnitID(unitIDs, isActive));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


    }
}
