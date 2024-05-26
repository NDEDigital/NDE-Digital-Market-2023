using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.DTOs;
using System.Data;
using System.Data.SqlClient;
using NDE_Digital_Market.SharedServices;
using Microsoft.Extensions.Logging;
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
            if (productGroupsDto == null)
            {
                return BadRequest(new { message = "Give Proper Product Data." });
            }
            return Ok(await _productGroup_Service.CreateProductGroupsAsync(productGroupsDto));

        }

        ///========================================================================================

        [HttpPut("UpdateProductGroups")]
        public async Task<IActionResult> UpdateProductGroupsAsync([FromForm] UpdateProductGroupDTO productGroupsDto)
        {
            if (productGroupsDto == null || productGroupsDto.ProductGroupID == null)
            {
                return BadRequest(new { message = "Invalid Product data." });
            }
            return Ok(await _productGroup_Service.UpdateProductGroupsAsync(productGroupsDto));
        }

        /// =====================================================================



        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("GetProductGroupsList")]
        public async Task<IActionResult> GetProductGroupsListAsync()
        {
            object res = await _productGroup_Service.GetProductGroupsListAsync();
            if (res == null)
            {
                return NotFound(new { message = "Products not Found." });
            }
            return Ok(res);
        }




        [HttpGet]
        [Route("GetProductGroupsListByStatus")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProductGroupsListByStatus(Int32? status = null)
        {
            object res = await _productGroup_Service.GetProductGroupsListByStatus(status);
            if (res == null)
            {
                return NotFound(new { message = "Products not Found." });
            }
            return Ok(res);
        }


        //========================tushar=========================

        [HttpPut("MakeGroupActiveOrInactive")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MakeGroupActiveOrInactiveAsync(string groupIds, bool? IsActive)
        {
            if (groupIds == null)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }
            return Ok(await _productGroup_Service.MakeGroupActiveOrInactiveAsync(groupIds, IsActive));
        }


    }
}
