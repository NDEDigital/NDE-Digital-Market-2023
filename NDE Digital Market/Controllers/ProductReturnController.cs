﻿using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using NDE_Digital_Market.DTOs;

namespace NDE_Digital_Market.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ProductReturnController : Controller
    {



        private readonly string _connectionHealthCare;
        public ProductReturnController(IConfiguration config)
        {
            _connectionHealthCare = config.GetConnectionString("HealthCare");
        }



        //[HttpPost, Authorize(Roles = "buyer")]
        [HttpPost]
        [Route("InsertReturnedData")]
        public async Task<IActionResult> InsertProductReturn([FromForm] ProductReturnDto returnData)
        {
            SqlTransaction transaction = null;
            SqlConnection con = new SqlConnection(_connectionHealthCare);

            try
            {
                await con.OpenAsync();
                transaction = con.BeginTransaction();
                string systemCode = string.Empty;

                SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con, transaction);
                {
                    cmdSP.CommandType = CommandType.StoredProcedure;
                    cmdSP.Parameters.AddWithValue("@TableName", "ProductReturn");
                    cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSP.Parameters.AddWithValue("@AddNumber", 1);
                    var tempSystem = await cmdSP.ExecuteScalarAsync();

                    systemCode = tempSystem?.ToString() ?? string.Empty;
                }
                int ProductReturnId = int.Parse(systemCode.Split('%')[0]);
                string ProductReturnCode = systemCode.Split('%')[1];

                string query = @"INSERT INTO ProductReturn(ProductReturnId,ProductReturnCode,ReturnTypeId,ProductGroupId,ProductId,OrderNo,Price,
                                    OrderDetailsId,SellerId,ApplyDate,DeliveryDate,Remarks,AddedDate,AddedBy,AddedPc)
                                VALUES(@ProductReturnId,@ProductReturnCode,@ReturnTypeId,@ProductGroupId,@ProductId,@OrderNo,@Price,
                                    @OrderDetailsId,@SellerId,@ApplyDate,@DeliveryDate,@Remarks,@AddedDate,@AddedBy,@AddedPc);";


                SqlCommand cmd = new SqlCommand(query, con, transaction);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@ProductReturnId", ProductReturnId);
                cmd.Parameters.AddWithValue("@ProductReturnCode", ProductReturnCode);
                cmd.Parameters.AddWithValue("@ReturnTypeId", returnData.ReturnTypeId);
                cmd.Parameters.AddWithValue("@ProductGroupId", returnData.ProductGroupId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProductId", returnData.ProductId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@OrderNo", returnData.OrderNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", returnData.Price ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@OrderDetailsId", returnData.OrderDetailsId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SellerId", returnData.SellerId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ApplyDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@DeliveryDate", returnData.DeliveryDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Remarks", returnData.Remarks ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@AddedBy", returnData.AddedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AddedPc", returnData.AddedPc ?? (object)DBNull.Value);

                int a = await cmd.ExecuteNonQueryAsync();
                if (a > 0)
                {
                    SqlCommand command = new SqlCommand("UPDATE OrderDetails SET Status = 'ToReturn' WHERE OrderDetailId = " + returnData.OrderDetailsId + "", con, transaction);

                    int updateResult = await command.ExecuteNonQueryAsync();

                    if (updateResult <= 0)
                    {

                        transaction.Rollback();
                        return BadRequest(new { message = "Order Details status isn't change or not found." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "ProductReturn data isn't Inserted Successfully." });
                }

                transaction.Commit();
                return Ok(new { message = "ProductReturn data Inserted Successfully." });

            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }

        }


        //private readonly string _connectionDigitalMarket;
        //public ProductReturnController(IConfiguration config)
        //{


        //    _connectionDigitalMarket = config.GetConnectionString("DigitalMarketConnection");
        //}



        //[HttpPost, Authorize(Roles = "buyer")]
        //[Route("InsertReturnedData")]
        //public IActionResult InsertProductReturn([FromForm] ProductReturnModel returnData)
        //{
        //    try
        //    {
        //        int returnId = 0;
        //        SqlConnection con = new SqlConnection(_connectionDigitalMarket);

        //        SqlCommand getLastReturnId = new SqlCommand("SELECT ISNULL(MAX(ReturnId), 0) FROM ProductReturn;", con);
        //        con.Open();

        //        returnId = Convert.ToInt32(getLastReturnId.ExecuteScalar()) + 1;
        //        SqlCommand cmd = new SqlCommand("INSERT INTO  [ProductReturn] ([ReturnId], [GroupName],GoodsName, [GroupCode], [GoodsId], [TypeId], [Remarks],[Price],[DetailsId],[SellerCode],[ApplyDate],[OrderNo], [DeliveryDate]) VALUES (@ReturnId, @GroupName,@GoodsName, @GroupCode, @GoodsId, @TypeId, @Remarks , @Price, @DetailsId, @SellerCode,GETDATE(),@OrderNo,@DeliveryDate);", con);

        //        using (cmd)
        //        {
        //            cmd.Parameters.AddWithValue("@returnId", returnId);
        //            cmd.Parameters.AddWithValue("@GroupName", returnData.GroupName);
        //            cmd.Parameters.AddWithValue("@GroupCode", returnData.GroupCode);
        //            cmd.Parameters.AddWithValue("@GoodsId", returnData.GoodsId);
        //            cmd.Parameters.AddWithValue("@TypeId", returnData.TypeId);
        //            cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrEmpty(returnData.Remarks) ? (object)DBNull.Value : returnData.Remarks);
        //            cmd.Parameters.AddWithValue("@Price", returnData.Price);
        //            cmd.Parameters.AddWithValue("@DetailsId", returnData.DetailsId);
        //            cmd.Parameters.AddWithValue("@SellerCode", returnData.SellerCode);
        //            cmd.Parameters.AddWithValue("@OrderNo", returnData.OrderNo);
        //            cmd.Parameters.AddWithValue("@GoodsName", returnData.GoodsName ?? " ");
        //            cmd.Parameters.AddWithValue("@DeliveryDate", returnData.DeliveryDate);

        //            cmd.ExecuteNonQuery();
        //        }

        //        con.Close();

        //        SqlCommand command = new SqlCommand("UPDATE OrderDetails SET Status = 'to Return' WHERE OrderDetailId = " + returnData.DetailsId + "", con);

        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception here. You can log the exception or perform any other necessary actions.
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        // You might want to return a specific error response or customize as needed.
        //        return StatusCode(500, new { message = "Internal Server Error" });
        //    }
        //}


        //[HttpGet, Authorize(Roles = "buyer")]
        //[Route("GetReturnType")]
        //public IActionResult GetReturnTypeData()
        //{
        //    List<ProductReturnModel> returnTypeList = new List<ProductReturnModel>();

        //    string sqlSelect = "SELECT [TypeId], [ReturnType] FROM  [ReturnType]";


        //    using (SqlConnection connection = new SqlConnection(_connectionDigitalMarket))
        //    {

        //        using (SqlCommand cmd = new SqlCommand(sqlSelect, connection))
        //        {
        //            try
        //            {             
        //                connection.Open();
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    ProductReturnModel returnType = new ProductReturnModel
        //                    {
        //                        TypeId = (int)reader["TypeId"],
        //                        ReturnType = reader["ReturnType"].ToString()
        //                    };

        //                    returnTypeList.Add(returnType);
        //                }
        //                reader.Close();

        //                return Ok(returnTypeList); 
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest($"Error: {ex.Message}");
        //            }
        //        }
        //    }
        //}
    }
}




 
