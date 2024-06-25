using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class ProductGroup_DAL
    {
        private readonly string _healthCareConnection;
        private readonly string foldername;
        private readonly string filename = "SellerProductGroup";
        public ProductGroup_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
            foldername = commonServices.FilesPath + "SellerProductGroupFiles";
        }


        private async Task<Boolean> ProductGroupsNameCheck(string productgoodsname)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"SELECT COUNT(*) FROM  [ProductGroups] WHERE ProductGroupName = @productgoodsname";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@productgoodsname", productgoodsname);
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
            }
            catch(Exception ex)
            {
                return true;
            }

        }


        private async Task<Boolean> ProductGroupsExist(int? productGroupID)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    if (productGroupID.HasValue)
                    {
                        string query = @"SELECT COUNT(*) FROM ProductGroups WHERE ProductGroupID = @ProductGroupID";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ProductGroupID", productGroupID.Value);
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
                    else
                    {
                        return false;
                    }
                }

            }
            catch(Exception ex)
            {
                return true;
            }

        }



        public async Task<object> CreateProductGroupsAsync(ProductGroupModel productGroupsDto)
        {
            try
            {
                Boolean check = await ProductGroupsNameCheck(productGroupsDto.ProductGroupName);
                if (check)
                {
                    return (new { message = "Product GroupName already exists!" });
                }
                else
                {

                    using(SqlConnection con = new SqlConnection(_healthCareConnection))
                    {
                        string systemCode = string.Empty;

                        // Execute the stored procedure to generate the system code
                        SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con);
                        {
                            cmdSP.CommandType = CommandType.StoredProcedure;
                            cmdSP.Parameters.AddWithValue("@TableName", "ProductGroups");
                            cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            cmdSP.Parameters.AddWithValue("@AddNumber", 1);

                            await con.OpenAsync();
                            var tempSystem = await cmdSP.ExecuteScalarAsync();
                            systemCode = tempSystem?.ToString() ?? string.Empty;
                            await con.CloseAsync();
                        }

                        int ProductGroupsID = int.Parse(systemCode.Split('%')[0]);
                        string ProductGroupsCode = systemCode.Split('%')[1];

                        //SP END

                        string ImagePath = CommonServices.UploadFiles(foldername, filename, productGroupsDto.ImageFile);
                        if (ImagePath == null)
                        {
                            return (new { message = "Image Problem" });
                        }
                        string query = @"INSERT INTO ProductGroups (ProductGroupID, ProductGroupCode, ProductGroupName,ImagePath, ProductGroupPrefix, ProductGroupDetails, IsActive, AddedBy, DateAdded, AddedPC)
                        VALUES(@ProductGroupID, @ProductGroupCode, @ProductGroupName, @ImagePath, @ProductGroupPrefix, @ProductGroupDetails, @IsActive, @AddedBy, @DateAdded, @AddedPC);";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ProductGroupID", ProductGroupsID);
                        cmd.Parameters.AddWithValue("@ProductGroupCode", ProductGroupsCode);
                        cmd.Parameters.AddWithValue("@ProductGroupName", productGroupsDto.ProductGroupName);
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                        cmd.Parameters.AddWithValue("@ProductGroupPrefix", productGroupsDto.ProductGroupPrefix);
                        cmd.Parameters.AddWithValue("@ProductGroupDetails", productGroupsDto.ProductGroupDetails ?? string.Empty);
                        cmd.Parameters.AddWithValue("@IsActive", 1);
                        cmd.Parameters.AddWithValue("@AddedBy", productGroupsDto.AddedBy);
                        cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AddedPC", productGroupsDto.AddedPC);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();

                    }

                    return (new { message = "Product Group Create successfully." });
                }


            }
            catch (Exception ex)
            {
                return (new { message = "Product Group doesn't Created successfully." });
            }

        }

        ///========================================================================================

        public async Task<object> UpdateProductGroupsAsync(ProductGroupModel productGroupsDto)
        {
            try
            {
                Boolean check = await ProductGroupsExist(productGroupsDto.ProductGroupID);

                if (check)
                {
                    using(SqlConnection con = new SqlConnection(_healthCareConnection))
                    {
                        await con.OpenAsync();

                        using (SqlTransaction transaction = con.BeginTransaction())
                        {
                            try
                            {
                                string ImagePath = CommonServices.UploadFiles(foldername, filename, productGroupsDto.ImageFile);

                                if (ImagePath != null)
                                {


                                    if (string.IsNullOrEmpty(productGroupsDto.ExistingImageFileName))
                                    {
                                        string query = "UpdateProductGroupWithImage";
                                        SqlCommand cmd = new SqlCommand(query, con, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ProductGroupID", productGroupsDto.ProductGroupID);
                                        cmd.Parameters.AddWithValue("@ProductGroupName", productGroupsDto.ProductGroupName);
                                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                                        cmd.Parameters.AddWithValue("@ProductGroupPrefix", productGroupsDto.ProductGroupPrefix);
                                        cmd.Parameters.AddWithValue("@ProductGroupDetails", productGroupsDto.ProductGroupDetails ?? string.Empty);
                                        cmd.Parameters.AddWithValue("@UpdatedBy", productGroupsDto.UpdatedBy ?? string.Empty);
                                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@UpdatedPC", productGroupsDto.UpdatedPC ?? string.Empty);

                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                                else
                                {
                                    string query1 = "UpdateProductGroupWithOutImage";
                                    SqlCommand cmdd = new SqlCommand(query1, con, transaction);
                                    cmdd.CommandType = CommandType.StoredProcedure;
                                    cmdd.Parameters.AddWithValue("@ProductGroupID", productGroupsDto.ProductGroupID);
                                    cmdd.Parameters.AddWithValue("@ProductGroupName", productGroupsDto.ProductGroupName);
                                    cmdd.Parameters.AddWithValue("@ProductGroupPrefix", productGroupsDto.ProductGroupPrefix);
                                    cmdd.Parameters.AddWithValue("@ProductGroupDetails", productGroupsDto.ProductGroupDetails ?? string.Empty);
                                    cmdd.Parameters.AddWithValue("@UpdatedBy", productGroupsDto.UpdatedBy ?? string.Empty);
                                    cmdd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                                    cmdd.Parameters.AddWithValue("@UpdatedPC", productGroupsDto.UpdatedPC ?? string.Empty);

                                    await cmdd.ExecuteNonQueryAsync();
                                }

                                transaction.Commit();
                                return (new { message = "Product Group updated successfully." });
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                return (new { message = $"Error updating product group: {ex.Message}" });
                            }
                            finally
                            {
                                con.Close();  // Close the connection in the finally block
                            }
                        }
                    }

                }
                else
                {
                    return (new { message = "Product Group not found!" });
                }
            }
            catch (Exception ex)
            {
                return (new { message = $"Error updating product group: {ex.Message}" });
            }
        }

        /// =====================================================================



        public async Task<DataTable> GetProductGroupsListAsync()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using(SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"SELECT [ProductGroupID],[ProductGroupCode],[ProductGroupName],[ProductGroupPrefix],[ProductGroupDetails],
                                    [IsActive] FROM ProductGroups WHERE IsActive = 1 ORDER BY [ProductGroupID] DESC;";

                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    await con.CloseAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to throw the exception again if you cannot handle it at this level.
                return null;
            }

            return dataTable;
        }




        public async Task<DataTable> GetProductGroupsListByStatus(Int32? status = null)
        {
            

            try
            {
                DataTable dataTable = new DataTable();
                string query = "";
                if (status != null)
                {
                    query = @"SELECT * FROM ProductGroups WHERE IsActive= @IsActive ORDER BY ProductGroupID  DESC;";
                }
                else
                {
                    query = @"SELECT * FROM ProductGroups WHERE CONVERT(DATE, DateAdded) = CONVERT(DATE, GETDATE()) ORDER BY ProductGroupID  DESC";
                }
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (status != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@IsActive", status));
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    await con.CloseAsync();
                }
                return dataTable;

            }
            catch (Exception ex)
            {
                // Handle the exception here. You can log the exception or perform any other necessary actions.
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to throw the exception again if you cannot handle it at this level.
                return null;
            }
        }


        //========================tushar=========================


        public async Task<object> MakeGroupActiveOrInactiveAsync(string groupIds, bool? IsActive)
        {
            try
            {
                string query = $"UPDATE ProductGroups  SET IsActive = @IsActive WHERE ProductGroupID IN ({groupIds})";
                using (SqlConnection con = new SqlConnection(_healthCareConnection)) 
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@groupId", groupIds);

                        await con.OpenAsync();
                        // Execute the command
                        int Res = await command.ExecuteNonQueryAsync();
                        if (Res == 0)
                        {
                            return (new { message = $"Group didnot found." });
                        }
                        await con.CloseAsync();
                    }
                }

                return (new { message = $"Group IsActive status changed." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new { message = $"Group IsActive status not change." });
            }
        }

    }
}
