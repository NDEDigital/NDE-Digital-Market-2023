using Microsoft.AspNet.WebHooks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;

using NDE_Digital_Market.SharedServices;
using CommonServices = NDE_Digital_Market.SharedServices.CommonServices;
using System.Data;
using Google.Api.Gax.ResourceNames;
namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddToCartController : ControllerBase
    {
        private readonly string _healthCareConnection;

        public AddToCartController(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        [HttpGet]

        [Route("GetAddToCartData")]
        public IActionResult GetAddToCartData()
        {
            List<AddToCartModal> addToCartList = new List<AddToCartModal>();

            using (var con = new SqlConnection(_healthCareConnection))
            {
                var cmd = new SqlCommand("SELECT * FROM AddToCart", con);

                con.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AddToCartModal addToCart = new AddToCartModal
                    {
                        Id = (int)reader["Id"],
                        BuyerUserID = (int)reader["BuyerUserID"],
                        CompanyCode = reader["CompanyCode"].ToString(),
                        ProductID = (int)reader["ProductID"],
                        ProductGroupID = reader["ProductGroupID"].ToString(),
                        UnitID = (int)reader["UnitID"],
                        ProductCartQuantity = (int)reader["ProductCartQuantity"],
                        AddedDate = reader["AddedDate"] as DateTime?,
                        AddedBy = reader["AddedBy"] as string,
                        AddedPC = reader["AddedPC"] as string,
                        UpdatedDate = reader["UpdatedDate"] as DateTime?,
                        UpdatedBy = reader["UpdatedBy"] as string,
                        UpdatedPC = reader["UpdatedPC"] as string
                    };

                    addToCartList.Add(addToCart);
                }
            }

            return Ok(new { message = "Data retrieved successfully.", result = addToCartList });
        }


        [HttpGet]

        [Route("GetAddToCartData/{userId}")]
        public async Task<IActionResult> GetAddToCartDataByUserID(int userId)
        {
            List<AddToCartModal> addToCartList = new List<AddToCartModal>();

            using (var con = new SqlConnection(_healthCareConnection))
            {
                await con.OpenAsync();
                string query = "GetAddToCartDataByBuyerUserId";
                var cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BuyerUserId", userId);

                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    AddToCartModal addToCart = new AddToCartModal
                    {
                        Id = (int)reader["Id"],
                        BuyerUserID = (int)reader["BuyerUserID"],
                        CompanyCode = reader["CompanyCode"].ToString(),
                        ProductID = (int)reader["ProductID"],
                        ProductGroupID = reader["ProductGroupID"].ToString(),
                        UnitID = (int)reader["UnitID"],
                        ProductCartQuantity = (int)reader["ProductCartQuantity"],
                        ImagePath = (string)reader["ImagePath"].ToString(),
                        ProductName = (string)reader["ProductName"].ToString(),
                        Price = (string)reader["Price"].ToString(),
                        TotalPrice = (string)reader["TotalPrice"].ToString(),
                        AvailableQty = (string)reader["AvailableQty"].ToString(),

                        CompanyName = (string)reader["CompanyName"].ToString(),
                        Specification = (string)reader["Specification"].ToString(),
                    };

                    addToCartList.Add(addToCart);
                }
            }

            return Ok(new { message = "Data retrieved successfully.", result = addToCartList });
        }


        [HttpPost]
        [Route("AddToCartData")]
        public IActionResult AddToCartData([FromForm] AddToCartModal addToCart)
        {
            try
            {
                using (var con = new SqlConnection(_healthCareConnection))
                {
                    con.Open();

                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM AddToCart WHERE BuyerUserID = @BuyerUserID AND ProductID = @ProductID AND CompanyCode = @CompanyCode ", con);
                    checkCmd.Parameters.AddWithValue("@BuyerUserID", addToCart.BuyerUserID);
                    checkCmd.Parameters.AddWithValue("@ProductID", addToCart.ProductID);
                    checkCmd.Parameters.AddWithValue("@CompanyCode", addToCart.CompanyCode);


                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        // If the record exists, update only the ProductCartQuantity
                        var updateCmd = new SqlCommand("UPDATE AddToCart SET ProductCartQuantity = @ProductCartQuantity WHERE BuyerUserID = @BuyerUserID AND ProductID = @ProductID AND CompanyCode = @CompanyCode", con);
                        updateCmd.Parameters.AddWithValue("@BuyerUserID", addToCart.BuyerUserID);
                        updateCmd.Parameters.AddWithValue("@ProductID", addToCart.ProductID);
                        updateCmd.Parameters.AddWithValue("@CompanyCode", addToCart.CompanyCode);
                        updateCmd.Parameters.AddWithValue("@ProductCartQuantity", addToCart.ProductCartQuantity);

                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // If the record does not exist, insert the new record
                        var insertCmd = new SqlCommand(@"INSERT INTO AddToCart (BuyerUserID, CompanyCode, ProductID, ProductGroupID, UnitID, ProductCartQuantity, AddedDate, AddedBy, AddedPC, UpdatedDate, UpdatedBy, UpdatedPC)
                                    VALUES (@BuyerUserID, @CompanyCode, @ProductID, @ProductGroupID, @UnitID, @ProductCartQuantity, @AddedDate, @AddedBy, @AddedPC, @UpdatedDate, @UpdatedBy, @UpdatedPC)", con);

                        insertCmd.Parameters.AddWithValue("@BuyerUserID", addToCart.BuyerUserID);
                        insertCmd.Parameters.AddWithValue("@CompanyCode", addToCart.CompanyCode);
                        insertCmd.Parameters.AddWithValue("@ProductID", addToCart.ProductID);
                        insertCmd.Parameters.AddWithValue("@ProductGroupID", addToCart.ProductGroupID);
                        insertCmd.Parameters.AddWithValue("@UnitID", addToCart.UnitID);
                        insertCmd.Parameters.AddWithValue("@ProductCartQuantity", addToCart.ProductCartQuantity);
                        insertCmd.Parameters.AddWithValue("@AddedDate", DateTime.UtcNow);
                        insertCmd.Parameters.AddWithValue("@AddedBy", addToCart.AddedBy);
                        insertCmd.Parameters.AddWithValue("@AddedPC", addToCart.AddedPC);
                        insertCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.UtcNow);
                        insertCmd.Parameters.AddWithValue("@UpdatedBy", addToCart.UpdatedBy);
                        insertCmd.Parameters.AddWithValue("@UpdatedPC", addToCart.UpdatedPC);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "Data added or updated successfully.", result = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error occurred while adding or updating data.", error = ex.Message, result = false });
            }
        }

        [HttpDelete]
        [Route("DeleteAddToCart/{id}")]
        public IActionResult DeleteAddToCart(int id)
        {
            try
            {
                using (var con = new SqlConnection(_healthCareConnection))
                {
                    con.Open();

                    // Check if the record exists
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM AddToCart WHERE Id = @Id", con);
                    checkCmd.Parameters.AddWithValue("@Id", id);

                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        // If the record exists, delete it
                        var deleteCmd = new SqlCommand("DELETE FROM AddToCart WHERE Id = @Id", con);
                        deleteCmd.Parameters.AddWithValue("@Id", id);

                        deleteCmd.ExecuteNonQuery();

                        return Ok(new { message = "Data deleted successfully.", result = true });
                    }
                    else
                    {
                        // If the record does not exist, return a NotFound response
                        return NotFound(new { message = "Record not found.", result = false });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { message = "Error occurred while deleting data.", error = ex.Message, result = false });
            }
        }



    }
}
