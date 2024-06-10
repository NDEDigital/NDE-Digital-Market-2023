
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class ProductQuantity_DAL
    {

        private readonly string foldername;
        private readonly string filename = "SellerProductPriceAndOffer";
        private readonly string _healthCareConnection;

        public ProductQuantity_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
            foldername = commonServices.FilesPath + "SellerProductPriceAndOfferFiles";
        }

        public async Task<DataTable> ProductGroupsDropdownByUserId(int userID)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                string query = @"GetProductGroupsDropdownByUserId";
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", userID));
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();
                return (dataTable);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<DataTable> GetProductForAddQtyByUserId(int UserId, int productGroupId)
        {
            DataTable dataTable = new DataTable();

            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                const string query = "GetProductForAddQtyByUserId";

                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                    cmd.Parameters.Add(new SqlParameter("@productGroupId", productGroupId));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();


                return (dataTable);
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public async Task<object> InsertPortalReceivedAsync(PortalReceivedMasterModel portaldata)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            // Start a transaction
            SqlTransaction transaction = null;
            try
            {
                string systemCode = string.Empty;
                await con.OpenAsync();
                transaction = (SqlTransaction)await con.BeginTransactionAsync();

                // Execute the stored procedure to generate the system code
                SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con, transaction);
                {
                    cmdSP.CommandType = CommandType.StoredProcedure;
                    cmdSP.Parameters.AddWithValue("@TableName", "PortalReceivedMaster");
                    cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSP.Parameters.AddWithValue("@AddNumber", 1);

                    var tempSystem = await cmdSP.ExecuteScalarAsync();
                    systemCode = tempSystem?.ToString() ?? string.Empty;
                }

                int PortalReceivedId = int.Parse(systemCode.Split('%')[0]);
                string PortalReceivedCode = systemCode.Split('%')[1];
                //SP END
                portaldata.PortalReceivedId = PortalReceivedId;
                portaldata.PortalReceivedCode = PortalReceivedCode;

                SqlCommand cmd = new SqlCommand("InsertPortalReceivedMaster", con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PortalReceivedId", PortalReceivedId);
                cmd.Parameters.AddWithValue("@PortalReceivedCode", PortalReceivedCode);
                cmd.Parameters.AddWithValue("@MaterialReceivedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@ChallanNo", portaldata.ChallanNo ?? String.Empty);



                cmd.Parameters.AddWithValue("@ChallanDate", portaldata.ChallanDate ?? (object)DBNull.Value);


                cmd.Parameters.AddWithValue("@Remarks", portaldata.Remarks ?? String.Empty);
                cmd.Parameters.AddWithValue("@UserId", portaldata.UserId);


                cmd.Parameters.AddWithValue("@AddedBy", portaldata.AddedBy);
                cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@AddedPC", portaldata.AddedPC);

                int a = await cmd.ExecuteNonQueryAsync();

                if (a > 0)
                {
                    var detailsResult = await InsertPortalReceivedDetailsAsync(PortalReceivedId, portaldata.PortalReceivedDetailslist, transaction, con);
                    if (detailsResult is BadRequestObjectResult)
                    {
                        throw new Exception((detailsResult as BadRequestObjectResult).Value.ToString());
                    }
                }
                else
                {
                    return (new { message = "Portal Master data isn't Inserted Successfully." });
                }
                // If everything is fine, commit the transaction
                await transaction.CommitAsync();
                return (portaldata);
            }
            catch (Exception ex)
            {
                // If there is any error, rollback the transaction
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                return (ex.Message);
            }
            finally
            {
                // Finally block to ensure the connection is always closed
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }


        private async Task<object> InsertPortalReceivedDetailsAsync(int PortalReceivedId, List<PortalReceivedDetailsModel> PortalReceivedDetailsList, SqlTransaction transaction, SqlConnection con)
        {
            try
            {
                for (int i = 0; i < PortalReceivedDetailsList.Count; i++)
                {
                    PortalReceivedDetailsList[i].PortalReceivedId = PortalReceivedId;
                    string query = "InsertPortalReceivedDetails";
                    //checking if user already exect for not.
                    SqlCommand CheckCMD = new SqlCommand(query, con, transaction);
                    CheckCMD.CommandType = CommandType.StoredProcedure;

                    CheckCMD.Parameters.Clear();
                    CheckCMD.Parameters.AddWithValue("@PortalReceivedId", PortalReceivedId);
                    CheckCMD.Parameters.AddWithValue("@ProductGroupID", PortalReceivedDetailsList[i].ProductGroupId);
                    CheckCMD.Parameters.AddWithValue("@ProductId", PortalReceivedDetailsList[i].ProductId);
                    CheckCMD.Parameters.AddWithValue("@Specification", PortalReceivedDetailsList[i].Specification);
                    CheckCMD.Parameters.AddWithValue("@ReceivedQty", PortalReceivedDetailsList[i].ReceivedQty);
                    CheckCMD.Parameters.AddWithValue("@UnitId", PortalReceivedDetailsList[i].UnitId);
                    CheckCMD.Parameters.AddWithValue("@Price", PortalReceivedDetailsList[i].Price);
                    CheckCMD.Parameters.AddWithValue("@TotalPrice", PortalReceivedDetailsList[i].TotalPrice);
                    CheckCMD.Parameters.AddWithValue("@UserId", PortalReceivedDetailsList[i].UserId);
                    CheckCMD.Parameters.AddWithValue("@Remarks", PortalReceivedDetailsList[i].Remarks ?? String.Empty);


                    CheckCMD.Parameters.AddWithValue("@AddedBy", PortalReceivedDetailsList[i].AddedBy);
                    CheckCMD.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    CheckCMD.Parameters.AddWithValue("@AddedPC", PortalReceivedDetailsList[i].AddedPC);

                    await CheckCMD.ExecuteNonQueryAsync();

                }
                return (new { message = "Portal Details data Inserted Successfully." });
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }



        private async Task<Boolean> SellerProductPriceAndOfferCheck(int ProductId, int? userId)
        {
            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                string query = @"SELECT COUNT(*) AS ProductCount FROM SellerProductPriceAndOffer WHERE ProductId = @ProductId AND CompanyCode =(SELECT UPPER(CompanyCode) FROM UserRegistration WHERE UserId = @UserId)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                await con.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();
                await con.CloseAsync();
                Boolean check = false;
                if (count > 0)
                {
                    check = true;
                }
                return check;
            }
            catch(Exception ex)
            {
                return false;
            }

        }


        public async Task<object> CreateSellerProductPriceAndOfferAsync(SellerProductPriceAndOfferModel sellerproductdata)
        {
            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                Boolean ProductPriceAndOfferExist = await SellerProductPriceAndOfferCheck(sellerproductdata.ProductId, sellerproductdata.UserId);
                if (ProductPriceAndOfferExist)
                {
                    return (new { message = "ProductPriceAndOffer Allready Added." });
                }
                else
                {
                    string ImagePath = CommonServices.UploadFiles(foldername, filename, sellerproductdata.ImageFile);

                    //SP END
                    string query = @"INSERT INTO SellerProductPriceAndOffer(ProductId, UserId, Price,DiscountAmount,DiscountPct,EffectivateDate,
                    EndDate,ImagePath,Status,IsActive, AddedDate,AddedBy,AddedPC,TotalPrice,CompanyCode) 
                    VALUES (@ProductId,@UserId,@Price,@DiscountAmount,@DiscountPct,@EffectivateDate,@EndDate, @ImagePath,
                    @Status, @IsActive, @AddedDate,@AddedBy, @AddedPC,@TotalPrice, (SELECT UPPER(CompanyCode) FROM UserRegistration WHERE UserId = @UserId));";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ProductId", sellerproductdata.ProductId);
                    cmd.Parameters.AddWithValue("@UserId", sellerproductdata.UserId);
                    cmd.Parameters.AddWithValue("@Price", sellerproductdata.Price);
                    cmd.Parameters.AddWithValue("@DiscountAmount", sellerproductdata.DiscountAmount ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiscountPct", sellerproductdata.DiscountPct ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EffectivateDate", sellerproductdata.EffectivateDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", sellerproductdata.EndDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@TotalPrice", sellerproductdata.TotalPrice);

                    cmd.Parameters.AddWithValue("@AddedBy", sellerproductdata.AddedBy);
                    cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AddedPC", sellerproductdata.AddedPC);

                    await con.OpenAsync();
                    int res = await cmd.ExecuteNonQueryAsync();
                    await con.CloseAsync();
                    if (res > 0)
                    {
                        return (new { message = "SellerProductPriceAndOffer Added Successfully." });
                    }
                    else
                    {
                        return (new { message = "SellerProductPriceAndOffer Add Unsuccessfull." });
                    }
                }



            }
            catch (Exception ex)
            {
                return (new { message = ex.Message });
            }
        }



        public async Task<object> UpdateSellerProductPriceAndOffer(SellerProductPriceAndOfferModel sellerproductdata)
        {
            try
            {
                SqlConnection con = new SqlConnection(_healthCareConnection);
                // Validation
                if (sellerproductdata == null)
                {
                    return (new { message = "Invalid request data." });
                }

                // Additional validation as needed for required fields, e.g., ProductId, UserId, Price, etc.

                Boolean check = await SellerProductPriceAndOfferCheck(sellerproductdata.ProductId, sellerproductdata.UserId);

                if (check)
                {
                    await con.OpenAsync();

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {

                            string ImagePath = CommonServices.UploadFiles(foldername, filename, sellerproductdata.ImageFile);

                            string query = "UpdateSellerProductPriceAndOffer";
                            SqlCommand cmd = new SqlCommand(query, con, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;


                            if (ImagePath != null)
                            {
                                // Adding parameters with null checks
                                cmd.Parameters.AddWithValue("@ProductId", sellerproductdata.ProductId);
                                cmd.Parameters.AddWithValue("@UserId", sellerproductdata.UserId);
                                cmd.Parameters.AddWithValue("@Price", sellerproductdata.Price ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@DiscountAmount", sellerproductdata.DiscountAmount ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@DiscountPct", sellerproductdata.DiscountPct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@EffectivateDate", sellerproductdata.EffectivateDate ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@EndDate", sellerproductdata.EndDate ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Status", "Pending");
                                cmd.Parameters.AddWithValue("@IsActive", 1);
                                cmd.Parameters.AddWithValue("@UpdatedBy", sellerproductdata.UpdatedBy ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@UpdatedPC", sellerproductdata.UpdatedPC ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@TotalPrice", sellerproductdata.TotalPrice ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);


                                await cmd.ExecuteNonQueryAsync();
                            }
                            else
                            {
                                // Adding parameters with null checks
                                cmd.Parameters.AddWithValue("@ProductId", sellerproductdata.ProductId);
                                cmd.Parameters.AddWithValue("@UserId", sellerproductdata.UserId);
                                cmd.Parameters.AddWithValue("@Price", sellerproductdata.Price ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@DiscountAmount", sellerproductdata.DiscountAmount ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@DiscountPct", sellerproductdata.DiscountPct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@EffectivateDate", sellerproductdata.EffectivateDate ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@EndDate", sellerproductdata.EndDate ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@Status", "Pending");
                                cmd.Parameters.AddWithValue("@IsActive", 1);
                                cmd.Parameters.AddWithValue("@UpdatedBy", sellerproductdata.UpdatedBy ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@UpdatedPC", sellerproductdata.UpdatedPC ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@TotalPrice", sellerproductdata.TotalPrice ?? (object)DBNull.Value);

                                await cmd.ExecuteNonQueryAsync();
                            }



                            transaction.Commit();
                            return (new { message = "Price updated successfully." });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return (new { message = $"Error updating price: {ex.Message}" });
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
                else
                {
                    return (new { message = "Price not found!" });
                }
            }
            catch (Exception ex)
            {
                return (new { message = $"Error updating price: {ex.Message}" });
            }
        }


        public async Task<DataTable> GetSellerProductsForPriceAndOfferByUserId(int userID, Int32? status = null)
        {
            
            try
            {
                DataTable dataTable = new DataTable();
                SqlConnection con = new SqlConnection(_healthCareConnection);
                string query = @"GetSellerProductsByCompanyCode";
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", userID));
                    cmd.Parameters.Add(new SqlParameter("@status", status));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();
                return (dataTable);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<DataTable> GetPortalReceivedByUserId(int userId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                SqlConnection con = new SqlConnection(_healthCareConnection);
                string query = @"SELECT [PortalReceivedId], [PortalReceivedCode], [MaterialReceivedDate], UserId 
                                FROM [PortalReceivedMaster] WHERE UserId = @UserId ORDER BY [PortalReceivedCode] DESC ";

                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();

                return (dataTable);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<DataSet> GetPortalData(int PortalReceivedId)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            try
            {
                DataSet dataSet = new DataSet();
                string query = @"GetPortalDataAfterInsertByPortalReceivedId";
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PortalReceivedId", PortalReceivedId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet);
                    }
                }
                await con.CloseAsync();
                return dataSet;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }



    }
}
