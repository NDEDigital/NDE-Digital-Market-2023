using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.Services.TopSellerService;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopSellerController : ControllerBase
    {
        private readonly ITopSeller_Service _topSeller_Service;
        public TopSellerController(ITopSeller_Service topSeller_Service)
        {
            _topSeller_Service = topSeller_Service;
        }

        [HttpGet]
        [Route("GetTopSeller")]
        public async Task<IActionResult> getForDropDown()
        {
            List<TopSellerListDTO> result = await _topSeller_Service.getForDropDown();
            if(result.Count == 0)
            {
                return NotFound(new {message = "No Top Seller Found."});
            }
            return Ok(result);
        }
    }
}
