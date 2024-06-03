using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.Services.ProductListService;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductListController : ControllerBase
{

    private readonly IProductList_Service _productList_Service;
    public ProductListController(IProductList_Service productList_Service)
    {
        _productList_Service = productList_Service;
    }



    [HttpPost("CreateProductList")]
    public async Task<IActionResult> CreateProductGroupsAsync([FromForm] InsertProductListDTO productListDto)
    {
        if (productListDto == null)
        {
            return BadRequest(new { message = "Give Proper Product Data." });
        }
        return Ok(await _productList_Service.CreateProductGroupsAsync(productListDto));

    }

    //======================================================================

    [HttpPut("UpdateProductList")]
    public async Task<IActionResult> UpdateProductListAsync([FromForm] UpdateProductListDTO productDto)
    {
        if (productDto == null || productDto.UnitId == null)
        {
            return BadRequest(new { message = "Invalid Product data." });
        }
        return Ok(await _productList_Service.UpdateProductListAsync(productDto));
    }

    //======================================================================

    [HttpGet]
    [Route("GetProductList")]
    public async Task<IActionResult> GetProductGroupsListAsync()
    {
        object res = await _productList_Service.GetProductGroupsListAsync();
        if (res == null)
        {
            return NotFound(new { message = "Products not Found." });
        }
        return Ok(res);

    }


    [HttpGet]
    [Route("GetProductListByStatus")]
    public async Task<IActionResult> GetProductListByStatus(bool? status = null)
    {
        object res = await _productList_Service.GetProductListByStatus(status);
        if (res == null)
        {
            return NotFound(new { message = "Products not Found." });
        }
        return Ok(res);

    }




    // ==============================productName by productGroupId===================

    [HttpGet]
    //[Authorize(Roles = "seller")]
    [Route("GetProductNameByProductGroupId")]
    public async Task<IActionResult> GetProductNameByProductGroupId(string ProductGroupId)
    {
        object res = await _productList_Service.GetProductNameByProductGroupId(ProductGroupId);
        if (res == null)
        {
            return NotFound(new { message = "Product not Found." });
        }
        return Ok(res);
    }


    //========================tushar=========================

    [HttpPut("MakeProductActiveOrInactive")]
    public async Task<IActionResult> MakeProductActiveOrInactiveAsync(List<string> productIds, bool? IsActive)
    {
        try
        {
            if (productIds == null)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }
            return Ok(await _productList_Service.MakeProductActiveOrInactiveAsync(productIds, IsActive));
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = "Server Error. Try Again!!!" });
        }

    }
}


