using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NDE_Digital_Market.Services.ProductGroupService;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductGroupsController : ControllerBase
    {


        private readonly IProductGroup_Service _productGroup_Service;
        public ProductGroupsController(IProductGroup_Service productGroup_Service)
        {
            _productGroup_Service = productGroup_Service;
        }





        [HttpPost("CreateProductGroups")]
        public async Task<IActionResult> CreateProductGroupsAsync([FromForm] InsertProductGroupDTO productGroupsDto)
        {
            try
            {
                if (productGroupsDto == null)
                {
                    return BadRequest(new { message = "Give Proper Product Data." });
                }
                return Ok(await _productGroup_Service.CreateProductGroupsAsync(productGroupsDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }


        }

        ///========================================================================================

        [HttpPut("UpdateProductGroups")]
        public async Task<IActionResult> UpdateProductGroupsAsync([FromForm] UpdateProductGroupDTO productGroupsDto)
        {
            try
            {
                if (productGroupsDto == null || productGroupsDto.ProductGroupID == null)
                {
                    return BadRequest(new { message = "Invalid Product data." });
                }
                return Ok(await _productGroup_Service.UpdateProductGroupsAsync(productGroupsDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }

        /// =====================================================================

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("GetProductGroupsList")]
        public async Task<IActionResult> GetProductGroupsListAsync()
        {
            try
            {
                object res = await _productGroup_Service.GetProductGroupsListAsync();
                if (res == null)
                {
                    return NotFound(new { message = "Products not Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }




        [HttpGet]
        [Route("GetProductGroupsListByStatus")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProductGroupsListByStatus(Int32? status = null)
        {
            try
            {
                object res = await _productGroup_Service.GetProductGroupsListByStatus(status);
                if (res == null)
                {
                    return NotFound(new { message = "Products not Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


        //========================tushar=========================

        [HttpPut("MakeGroupActiveOrInactive")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MakeGroupActiveOrInactiveAsync(string groupIds, bool? IsActive)
        {
            try
            {
                if (groupIds == null)
                {
                    return BadRequest(new { message = "No product IDs provided." });
                }
                return Ok(await _productGroup_Service.MakeGroupActiveOrInactiveAsync(groupIds, IsActive));
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }


    }
}
