using NDE_Digital_Market.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using NDE_Digital_Market.SharedServices;
using Microsoft.AspNetCore.Authorization;
using NDE_Digital_Market.Services.GoodsService;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {

        private readonly IGoods_Service _Goods_Service;
        public GoodsController(IGoods_Service Goods_Service)
        {
            _Goods_Service = Goods_Service;
        }

        [HttpGet]
        [Route("GetNavData")]
        public async Task<IActionResult> GetNavData()
        {
            try
            {

                object res = await _Goods_Service.GetNavData();
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
     

        [HttpGet]
        [Authorize(Roles = "seller")]
        [Route("GetDataForDropdown")]
        public async Task<IActionResult> getForDropDown()
        {
            try
            {
                object res = await _Goods_Service.getForDropDown();
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


        [HttpGet]
        [Route("GetGoodsList")]
        public async Task<IActionResult> GetGoodsList()
        {
            try
            {

                object res = await _Goods_Service.GetGoodsList();
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

        //====================== Product Details in url =================

        [HttpGet]
        [Route("GetGoodsDetails/{CompanyCode}/{ProductId}")]
        public async Task<IActionResult> GetGoodsDetails(string CompanyCode,string ProductId)
        {
            try
            {
                if (CompanyCode == null || ProductId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _Goods_Service.GetGoodsDetails(CompanyCode, ProductId);
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
        //====================== ProductCompany =================


        [HttpGet]
        [Route("GetProductCompany")]
        public async Task<IActionResult> GetProductCompany(string ProductGroupCode)
        {
            try
            {
                if (ProductGroupCode == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _Goods_Service.GetProductCompany(ProductGroupCode);
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


        [HttpGet]
        [Route("GetProductList")]
        public async Task<IActionResult> GetProductList(string? CompanyCode, string? ProductGroupCode)
        {
            try
            {


                object res = await _Goods_Service.GetProductList(CompanyCode, ProductGroupCode);
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





        [HttpGet]
        [Route("GetRecommendedProductList/{CompanyCode}/{ProductId}")]
        public async Task<IActionResult> GetRecommendedProductList(string CompanyCode, string ProductId)
        {
            try
            {
                if (CompanyCode == null || CompanyCode == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _Goods_Service.GetRecommendedProductList(CompanyCode, ProductId);
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
