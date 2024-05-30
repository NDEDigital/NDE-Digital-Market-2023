using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class WishList_DAL
    {
        private readonly string _healthCareConnection;

        public WishList_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        //public async Task<List<WishListDTO>> GetWishList(int UserId)
        //{
        //    List<WishListDTO> lst = new List<WishListDTO>();

        //    //try
        //    //{
        //        using (SqlConnection con = new SqlConnection(_healthCareConnection))
        //        {
        //            await con.OpenAsync();
        //            string query = "getWishListForBuyer";

        //            using (SqlCommand cmd = new SqlCommand(query, con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.AddWithValue("@UserId", UserId);
        //                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        WishListDTO modelObj = new WishListDTO();
        //                        modelObj.UserId = Convert.ToInt32(reader["SellerId"]);
        //                        modelObj.CompanyCode = reader["CompanyCode"].ToString();
        //                        modelObj.CompanyName = reader["CompanyName"].ToString();
        //                        modelObj.ProductGroupName = reader["ProductGroupName"].ToString();
        //                        modelObj.ProductId = Convert.ToInt32(reader["ProductId"]);
        //                        modelObj.ProductName = reader["ProductName"].ToString();
        //                        modelObj.GroupCode = reader["ProductGroupCode"].ToString();
        //                        modelObj.SellerId = Convert.ToInt32(reader["SellerId"]);
        //                        modelObj.ProductGroupID = Convert.ToInt32(reader["ProductGroupID"]);
        //                        modelObj.Specification = reader["Specification"].ToString();
        //                        modelObj.UnitId = Convert.ToInt32(reader["UnitId"]);
        //                        modelObj.Unit = reader["Unit"].ToString();
        //                        modelObj.Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0;
        //                        modelObj.DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(reader["DiscountAmount"]) : 0;
        //                        modelObj.DiscountPct = reader["DiscountPct"] != DBNull.Value ? Convert.ToDecimal(reader["DiscountPct"]) : 0;
        //                        modelObj.ImagePath = reader["ImagePath"].ToString();
        //                        modelObj.TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0;

        //                        modelObj.AvailableQty = Convert.ToInt32(reader["AvailableQty"]);
        //                        DateTime? endDate = null;
        //                        if (reader["EndDate"] != DBNull.Value)
        //                        {
        //                            endDate = Convert.ToDateTime(reader["EndDate"]);
        //                            if (endDate <= DateTime.Now)
        //                            {

        //                                modelObj.TotalPrice = modelObj.Price;
        //                                modelObj.DiscountAmount = 0;
        //                                modelObj.DiscountPct = 0;
        //                            }
        //                        }


        //                        lst.Add(modelObj);
        //                    }
        //                }
        //            }
        //        }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine($"An error occurred: {ex.Message}");
        //    //    return null;
        //    //}

        //    return lst;
        //}

        public async Task<DataTable> GetWishList(int UserId)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    string query = "getWishListForBuyer";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred on GetWishList: {ex.Message}");
                return null;
            }

            return dataTable;
        }




        public async Task<object> InsertWishList(int UserId, int ProductId, string CompanyCode)
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
                        if (insertedItemId > 0)
                        {
                            // If needed, you can return the number of rows affected
                            return new
                            {

                                Message = "Item inserted successfully",
                                RowsAffected = insertedItemId
                            };
                        }
                        else
                        {
                            // If needed, you can return the number of rows affected
                            return new
                            {

                                Message = "Item didnot inserted successfully",
                                RowsAffected = insertedItemId
                            };
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // If needed, you can return the number of rows affected
                return new
                {

                    Message = ex.Message,
                };
            }
        }


        public async Task<object> DeleteWishList(int UserId, int ProductId, string CompanyCode)
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

                        if(deletedRowCount > 0)
                        {
                            return new
                            {
                                Message = "Item deleted successfully",
                                RowsAffected = deletedRowCount
                            };
                        }
                        else
                        {
                            return new
                            {
                                Message = "Item Not Found.",
                                RowsAffected = deletedRowCount
                            };
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new
                {
                    Message = ex.Message
                };
            }
        }

    }
}
