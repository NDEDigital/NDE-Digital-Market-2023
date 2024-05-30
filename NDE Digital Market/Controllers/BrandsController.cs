using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.Services.BrandsService;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly IBrands_Service _Brands_Service;
        public BrandsController(IBrands_Service Brands_Service)
        {
            _Brands_Service = Brands_Service;
        }

        [HttpGet]
        [Route("GetBrandList")]
        public async Task<IActionResult> GetBrandListAsync(bool? isActive)
        {
            try
            {

                object res = await _Brands_Service.GetBrandListAsync(isActive);
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


        [HttpPost]
        [Route("AddBrand")]
        public async Task<IActionResult> PostBrandAsync([FromForm] InsertBrandDataDTO model)
        {
            try
            {
                if (model == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                object res = await _Brands_Service.PostBrandAsync(model);
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
        [Route("UpdateBrand")]
        public async Task<IActionResult> PutBrand([FromForm] UpdateBrandsDTO model)
        {


            try
            {
                if (model == null || model.BrandId == null)
                {
                    return BadRequest(new { message = "Invalid Brand data." });
                }

                object res = await _Brands_Service.PutBrand(model);
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


        //[HttpPut]
        //[Route("ChangeBrandStatus")]
        //public async Task<IActionResult> ChangeBrandStatus(string BrandId, bool isActive)
        //{
        //    if (BrandId == null)
        //    {
        //        return BadRequest(new { message = "Invalid Brand data." });
        //    }

        //    try
        //    {
        //        await con.OpenAsync();
        //        string query = @"UPDATE Brands 
        //                 SET
        //                     IsActive = @IsActive
        //                 WHERE BrandId = @BrandId;";

        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@BrandId", BrandId);
        //            cmd.Parameters.AddWithValue("@IsActive", isActive);


        //            int rowsAffected = await cmd.ExecuteNonQueryAsync();
        //            if (rowsAffected > 0)
        //            {
        //                return Ok(new { message = "Brand updated successfully." });
        //            }
        //            else
        //            {
        //                return NotFound(new { message = "Brand not found." });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        // Log the exception
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating the Brand." });
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            await con.CloseAsync();
        //        }
        //    }
        //}


        [HttpPut]
        [Route("ChangeBrandsStatus")]
        public async Task<IActionResult> ChangeBrandsStatus(string BrandIDs, bool isActive)
        {

            try
            {
                if (BrandIDs == null || isActive == null)
                {
                    return BadRequest(new { message = "Invalid Brand data." });
                }

                object res = await _Brands_Service.ChangeBrandsStatus(BrandIDs, isActive);  
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
