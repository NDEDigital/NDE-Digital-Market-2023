using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer;

public class Products_DAL
{
    private readonly string _healthCareConnection;
    private readonly IConfiguration _configuration;

    public Products_DAL(IConfiguration config)
    {
        _configuration = config;
        CommonServices commonServices = new CommonServices(config);
        _healthCareConnection = commonServices.HealthCareConnection;
    }

    public async Task<object> UpdateProduct(GoodsQuantityModel product)
    {

        try
        {
            using(SqlConnection con = new SqlConnection(_healthCareConnection))
            {
                string decryptedSupplierCode = CommonServices.DecryptPassword(product.SellerCode);
                product.UpdatedBy = decryptedSupplierCode;
                product.UpdatedDate = DateTime.Now;
                SqlCommand cmd = new SqlCommand("INSERT INTO EditedProductList (GoodsId, GoodsName, Specification, GroupCode, GroupName,Quantity,Price, QuantityUnit, UpdatedDate, UpdatedBy, UpdatedPc, Status, SellerCode)VALUES( @GoodsId, @GoodsName, @Specification, @GroupCode, @GroupName,@Price, @Quantity, @QuantityUnit, @UpdatedDate, @UpdatedBy, @UpdatedPc, @Status,  @SellerCode);" +
                    "UPDATE ProductList SET Status=@Status WHERE  SellerCode = @SellerCode AND GoodsId = @GoodsId", con);
                cmd.CommandType = CommandType.Text;


                cmd.Parameters.AddWithValue("@GoodsId", CommonServices.DecryptPassword(product.GoodsId));
                cmd.Parameters.AddWithValue("@GoodsName", product.GoodsName);
                cmd.Parameters.AddWithValue("@Specification", product.Specification);
                cmd.Parameters.AddWithValue("@GroupCode", product.GroupCode);
                cmd.Parameters.AddWithValue("@GroupName", product.GroupName);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@QuantityUnit", product.QuantityUnit);
                cmd.Parameters.AddWithValue("@UpdatedDate", product.UpdatedDate);
                cmd.Parameters.AddWithValue("@UpdatedBy", product.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedPc", product.UpdatedPc);
                cmd.Parameters.AddWithValue("@Status", product.Status);
                cmd.Parameters.AddWithValue("@SellerCode", decryptedSupplierCode);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return (new { message = "product update Successfully."}); // If execution is successful
            }

        }
        catch (SqlException ex)
        {
            // Handle any SQL-related errors
            return (new { message = "product update Unsuccessfully." }); // If execution is successful
        }
    }



    // ======================= GET Dashboard Contents ================== 

    public async Task<object> GetDashboardContents(string sellerCode, String? status = null, String? productName = null, String? companyName = null, DateTime? addedDate = null)
    {
        SqlConnection con = new SqlConnection(_healthCareConnection);
        try
        {
            Console.WriteLine(sellerCode, "sellerCode");
            string decryptedSupplierCode = CommonServices.DecryptPassword(sellerCode);

            bool isAdmin = false;
            int newCount = 0, editedCount = 0, approvedCount = 0, rejectedCount = 0;
            string query = "SELECT PhoneNumber FROM UserRegistration WHERE UserCode = @UserCode;";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserCode", decryptedSupplierCode);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string phoneNumber = reader["PhoneNumber"].ToString();

                if (phoneNumber == "admin")
                {
                    isAdmin = true;
                }
            }
            con.Close();
            List<GoodsQuantityModel> Products = new List<GoodsQuantityModel>();

            if (isAdmin && status != null)
            {

                Console.WriteLine(isAdmin);
                Console.WriteLine("isAdmin");

                string queryForAdmin = "sp_ProductListWithCompanyName";

                SqlCommand cmdForAdmin = new SqlCommand(queryForAdmin, con);
                cmdForAdmin.CommandType = CommandType.StoredProcedure;
                cmdForAdmin.Parameters.AddWithValue("@Status", status);
                if (productName != null) { cmdForAdmin.Parameters.AddWithValue("@productName", productName); }
                if (companyName != null) { cmdForAdmin.Parameters.AddWithValue("@CompanyName", companyName); }
                if (addedDate != null) { cmdForAdmin.Parameters.AddWithValue("@AddedDate", addedDate); }
                SqlDataAdapter adapter = new SqlDataAdapter(cmdForAdmin);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                con.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    newCount = Convert.ToInt32(dt.Rows[i]["NewCount"]);
                    editedCount = Convert.ToInt32(dt.Rows[i]["EditedCount"]);
                    approvedCount = Convert.ToInt32(dt.Rows[i]["ApprovedCount"]);
                    rejectedCount = Convert.ToInt32(dt.Rows[i]["RejectedCount"]);
                }

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    GoodsQuantityModel modelObj = new GoodsQuantityModel();
                    modelObj.CompanyName = dt1.Rows[i]["CompanyName"].ToString();
                    modelObj.GroupCode = CommonServices.DecryptPassword(dt1.Rows[i]["GroupCode"].ToString());
                    modelObj.GoodsId = CommonServices.DecryptPassword(dt1.Rows[i]["GoodsID"].ToString());
                    modelObj.GroupName = dt1.Rows[i]["GroupName"].ToString();
                    modelObj.GoodsName = dt1.Rows[i]["GoodsName"].ToString();
                    modelObj.Specification = dt1.Rows[i]["Specification"].ToString();
                    modelObj.ApproveSalesQty = float.Parse(dt1.Rows[i]["Quantity"].ToString());
                    modelObj.SellerCode = dt1.Rows[i]["SellerCode"].ToString();
                    modelObj.Price = float.Parse(dt1.Rows[i]["Price"].ToString());
                    modelObj.QuantityUnit = dt1.Rows[i]["QuantityUnit"].ToString();
                    modelObj.ImagePath = dt1.Rows[i]["ImagePath"].ToString();
                    modelObj.AddedDate = dt1.Rows[i]["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dt1.Rows[i]["AddedDate"]) : (DateTime?)null;
                    Products.Add(modelObj);
                }
            }
            else
            {
                isAdmin = false;
            }

            return (new { message = "content get successfully", Products, isAdmin, newCount, editedCount, approvedCount, rejectedCount });
        }
        catch (Exception ex)
        {
            // Handle the exception here. You can log the exception or perform any other necessary actions.
            Console.WriteLine($"An error occurred: {ex.Message}");
            // You might want to return a specific error response or customize as needed.
            return (500, new { message = "Internal Server Error" });
        }
    }


    public async Task<List<GetSellerProductListForAdminApprovalDTO>> GetSellerProductForAdminApproval(string status)
    {
        //string DecryptId = CommonServices.DecryptPassword(companyCode);
        var products = new List<GetSellerProductListForAdminApprovalDTO>();

        try
        {
            using (var connection = new SqlConnection(_healthCareConnection))
            {
                using (var command = new SqlCommand("SellerProductStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Status", status));
                    //command.Parameters.Add(new SqlParameter("@userID", userId));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            var product = new GetSellerProductListForAdminApprovalDTO
                            {
                                ProductId = CommonServices.EncryptPassword(reader["ProductId"].ToString()),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                UserId = CommonServices.EncryptPassword(reader["UserId"].ToString()),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("Price")),
                                DiscountAmount = reader.IsDBNull(reader.GetOrdinal("DiscountAmount")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                                DiscountPct = reader.IsDBNull(reader.GetOrdinal("DiscountPct")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("DiscountPct")),
                                EffectivateDate = reader.IsDBNull(reader.GetOrdinal("EffectivateDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("EffectivateDate")),
                                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                                TotalPrice = reader.IsDBNull(reader.GetOrdinal("TotalPrice")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                CompanyCode = CommonServices.EncryptPassword(reader["CompanyCode"].ToString()),
                                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                                PreviousPrice = reader.IsDBNull(reader.GetOrdinal("PPrice")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PPrice")),
                                PreviousDiscountAmount = reader.IsDBNull(reader.GetOrdinal("PDiscountAmount")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PDiscountAmount")),
                                PreviousDiscountPct = reader.IsDBNull(reader.GetOrdinal("PDiscountPct")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PDiscountPct")),
                                PreviousTotalPrice = reader.IsDBNull(reader.GetOrdinal("PTotalPrice")) ? (decimal?)null : (decimal?)reader.GetDecimal(reader.GetOrdinal("PTotalPrice"))
                                //UpdatedDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return (products);
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    public async Task<object> DeleteProcuct(string sellerCode, int ProductId)
    {
        try
        {
            using(SqlConnection con = new SqlConnection(_healthCareConnection))
            {
                string decryptedSupplierCode = CommonServices.DecryptPassword(sellerCode);

                SqlCommand cmd = new SqlCommand("DELETE FROM ProductList WHERE  GoodsId = @GoodsId AND SellerCode = @SellerCode", con);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@GoodsId", ProductId);
                cmd.Parameters.AddWithValue("@SellerCode", decryptedSupplierCode);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return (new { message = "Product DELETED successfully" });
            }

        }
        catch (Exception ex)
        {
            // Handle the exception here. You can log the exception or perform any other necessary actions.
            Console.WriteLine($"An error occurred: {ex.Message}");
            // You might want to return a specific error response or customize as needed.
            return (new { message = "Internal Server Error" });
        }
    }


    public async Task<object> UpdateSellerProductStatusAsync(List<SellerProductsModel> productStatusList)
    {
        try
        {
            string query = @"UPDATE SellerProductPriceAndOffer SET Status = @Status, UpdatedDate = @UpdatedDate WHERE ProductId = @ProductId AND UserId = @UserId AND CompanyCode = @CompanyCode ";

            using (var connection = new SqlConnection(_healthCareConnection))
            {
                await connection.OpenAsync();

                foreach (var productStatus in productStatusList)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", productStatus.Status);
                        command.Parameters.AddWithValue("@UserId", productStatus.UserId);
                        command.Parameters.AddWithValue("@CompanyCode", productStatus.CompanyCode);
                        command.Parameters.AddWithValue("@ProductId", productStatus.ProductId);
                        command.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        // Execute the command
                        await command.ExecuteNonQueryAsync();
                    }
                }

                await connection.CloseAsync();
            }

            return (new { message = "SellerProduct status Changed successfully." });
        }
        catch (Exception ex)
        {
            return (new { message = ex.Message });
        }
    }









    //public class EditedUserInfoModel
    //{

    //    public string? FullName { get; set; }
    //    public string? SupplierCode { get; set; }
    //    public string? Email { get; set; }
    //    public string? ProductName { get; set; }
    //}

    public async Task<object> comapreEditedProduct(int productId)
    {
        try
        {
            using (var con = new SqlConnection(_healthCareConnection))
            {
                GoodsQuantityModel oldData = new GoodsQuantityModel();
                GoodsQuantityModel newData = new GoodsQuantityModel();
                string query = "SELECT * FROM ProductList WHERE GoodsId=@ProductId; SELECT * FROM EditedProductList WHERE GoodsId=@ProductId;";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductId", productId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt0 = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];
                con.Close();
                for (int i = 0; i < dt0.Rows.Count; i++)
                {
                    oldData.GoodsId = dt0.Rows[i]["GoodsId"].ToString();
                    oldData.Status = dt0.Rows[i]["Status"].ToString();
                    oldData.GoodsName = dt0.Rows[i]["GoodsName"].ToString();
                    oldData.Specification = dt0.Rows[i]["Specification"].ToString();
                    oldData.GroupCode = dt0.Rows[i]["GroupCode"].ToString();
                    oldData.GroupName = dt0.Rows[i]["GroupName"].ToString();
                    oldData.Price = Convert.ToSingle(dt0.Rows[i]["Price"]);
                    oldData.ImagePath = dt0.Rows[i]["ImagePath"].ToString();
                    oldData.SellerCode = dt0.Rows[i]["SellerCode"].ToString();
                    oldData.Quantity = Convert.ToInt32(dt0.Rows[i]["Quantity"].ToString());
                    oldData.QuantityUnit = dt0.Rows[i]["QuantityUnit"].ToString();
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    newData.GoodsId = dt1.Rows[i]["GoodsId"].ToString();
                    newData.Status = dt1.Rows[i]["Status"].ToString();
                    newData.GoodsName = dt1.Rows[i]["GoodsName"].ToString();
                    newData.Specification = dt1.Rows[i]["Specification"].ToString();
                    newData.GroupCode = dt1.Rows[i]["GroupCode"].ToString();
                    newData.GroupName = dt1.Rows[i]["GroupName"].ToString();
                    newData.Price = Convert.ToSingle(dt1.Rows[i]["Price"]);
                    newData.ImagePath = dt1.Rows[i]["ImagePath"].ToString();
                    newData.SellerCode = dt1.Rows[i]["SellerCode"].ToString();
                    newData.Quantity = Convert.ToInt32(dt1.Rows[i]["Quantity"].ToString());
                    newData.QuantityUnit = dt1.Rows[i]["QuantityUnit"].ToString();
                }
                return (new { message = "GET Products data successful", oldData, newData });
            }
                
        }
        catch (Exception ex)
        {
            // Handle the exception here. You can log the exception or perform any other necessary actions.
            Console.WriteLine($"An error occurred: {ex.Message}");
            // You might want to return a specific error response or customize as needed.
            return (new { message = "Internal Server Error" });
        }
    }


}
