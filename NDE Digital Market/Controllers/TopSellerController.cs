using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopSellerController : ControllerBase
    {
        private readonly string _healthCareConnection;
        public TopSellerController(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }
        [HttpGet]
        [Route("GetTopSeller")]
        public async Task<ActionResult<List<TopSellerModel>>> getForDropDown()
        {
            List<TopSellerModel> lst = new List<TopSellerModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    string query = "GetTopSellerCompanies";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                TopSellerModel modelObj = new TopSellerModel
                                {
                                    CompanyCode = reader["CompanyCode"].ToString(),
                                    CompanyName = reader["CompanyName"].ToString(),
                                    CompanyImage = reader["CompanyImage"].ToString(),
                                    ProductGroupID = Convert.ToInt32(reader["ProductGroupID"]),
                                    TotalQty = Convert.ToInt32(reader["TotalQty"]),
                                    ProductGroupCode= reader["ProductGroupCode"].ToString(),
                                };
                                lst.Add(modelObj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return lst;
        }
    }
}
