using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.HK_GetsServices;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;
using NDE_Digital_Market.SharedServices;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class HK_GetsController : ControllerBase
    {

        private readonly IHK_Gets _HKGets;
        public HK_GetsController(IHK_Gets hK_Gets)
        {
            this._HKGets = hK_Gets;
        }

        [HttpGet("PreferredPaymentMethods")]
        public async Task<IActionResult> PaymentMethodGetAsync()
        {
            try
            {
                object res = await _HKGets.PaymentMethodGetAsync();
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


        [HttpGet("PreferredBankNames")]
        public async Task<IActionResult> BankNameGetAsync(string preferredPM)
        {
            try
            {
                if (preferredPM == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _HKGets.BankNameGetAsync(preferredPM);
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


        //[HttpGet]
        //[Route("GetUnitList")]
        //public async Task<List<UnitModel>> GetUnitListAsync()
        //{
        //    List<UnitModel> lst = new List<UnitModel>();

        //    try
        //    {
        //        await con.OpenAsync();
        //        string query = "select UnitId, Name from Units;";

        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    UnitModel modelObj = new UnitModel();
        //                    modelObj.UnitId = Convert.ToInt32(reader["UnitId"]);
        //                    modelObj.Name = reader["Name"].ToString();

        //                    lst.Add(modelObj);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        throw;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            await con.CloseAsync();
        //        }
        //    }
        //    return lst;
        //}


        [HttpGet("GetReturnList")]
        public async Task<IActionResult> GetReturnListAsync()
        {
            try
            {
                object res = await _HKGets.GetReturnListAsync();
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
