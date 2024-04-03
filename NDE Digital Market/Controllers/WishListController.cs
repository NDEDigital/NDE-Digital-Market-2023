using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WishListController : ControllerBase
    {
        private readonly string _healthCareConnection;

        public WishListController(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        [HttpPost("InsertWishList/{UserId}/{ProductId}/{CompanyCode}")]
        public async Task<IActionResult> InsertWishList(int UserId, string ProductId,  string CompanyCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"INSERT INTO WishList (UserId,ProductId, CompanyCode) VALUES (@UserId,@ProductId, @CompanyCode);";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        cmd.Parameters.AddWithValue("@ProductId", ProductId);
                        cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);


                        // ExecuteScalarAsync is used for queries that return a single value
                        var insertedItemId = await cmd.ExecuteNonQueryAsync();

                        // If needed, you can return the number of rows affected
                        return Ok(new
                        {
                            Message = "Item inserted successfully",
                            RowsAffected = insertedItemId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteWishList/{UserId}/{ProductId}/{CompanyCode}")]
        public async Task<IActionResult> DeleteWishList(int UserId, string ProductId, string CompanyCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"DELETE FROM WishList WHERE UserId=@UserId AND ProductId=@ProductId AND CompanyCode=@CompanyCode;";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.Parameters.AddWithValue("@ProductId", ProductId);
                        cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);

                        var deletedRowCount = await cmd.ExecuteNonQueryAsync();

                        return Ok(new
                        {
                            Message = "Item deleted successfully",
                            RowsAffected = deletedRowCount
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



    }
}
