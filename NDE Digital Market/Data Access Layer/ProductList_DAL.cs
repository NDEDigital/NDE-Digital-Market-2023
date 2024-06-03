using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer;

public class ProductList_DAL
{

    private readonly string _healthCareConnection;
    private readonly string foldername;
    private readonly string filename = "Productfile";
    public ProductList_DAL(IConfiguration configuration)
    {
        CommonServices commonServices = new CommonServices(configuration);
        _healthCareConnection = commonServices.HealthCareConnection;
        foldername = commonServices.FilesPath + "Productfiles";
    }


    private async Task<Boolean> ProductNameCheck(string ProductName, string Specification)
    {
        SqlConnection con = new SqlConnection(_healthCareConnection);

        string query = @"SELECT COUNT(*) FROM ProductList WHERE ProductName = @ProductName AND Specification = @Specification;";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductName", ProductName);
        cmd.Parameters.AddWithValue("@Specification", Specification);
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

    private async Task<Boolean> ProductListExist(int? ProductId)
    {
        SqlConnection con = new SqlConnection(_healthCareConnection);
        if (ProductId.HasValue)
        {
            string query = @"SELECT COUNT(*) FROM ProductList WHERE ProductId = @ProductId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ProductId", ProductId.Value);
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


    public async Task<object> CreateProductGroupsAsync(ProductListModel productListDto)
    {
        SqlConnection con = new SqlConnection(_healthCareConnection);
        try
        {
            Boolean check = await ProductNameCheck(productListDto.ProductName, productListDto.Specification);
            if (check)
            {
                return (new { message = "ProductName and Specification is same!" });
                //return Ok("ProductName and Specification is same.");
            }
            else
            {
                string systemCode = string.Empty;

                // Execute the stored procedure to generate the system code
                SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con);
                {
                    cmdSP.CommandType = CommandType.StoredProcedure;
                    cmdSP.Parameters.AddWithValue("@TableName", "ProductList");
                    cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSP.Parameters.AddWithValue("@AddNumber", 1);

                    await con.OpenAsync();
                    var tempSystem = await cmdSP.ExecuteScalarAsync();
                    systemCode = tempSystem?.ToString() ?? string.Empty;
                    await con.CloseAsync();
                }
                string ImagePath = CommonServices.UploadFiles(foldername, filename, productListDto.ImageFile);
                if (ImagePath == null)
                {
                    return (new { message = "Image Problem" });
                }

                int ProductID = int.Parse(systemCode.Split('%')[0]);
                //string ProductGroupsCode = systemCode.Split('%')[1];

                //SP END
                string query = "INSERT INTO ProductList (ProductId, ProductName, ProductGroupID,Specification, BrandId, UnitId, ImagePath, ProductSubName, IsActive, AddedDate, AddedBy, AddedPC)" +
                    "VALUES (@ProductId, @ProductName, @ProductGroupID, @Specification, @BrandId, @UnitId, @ImagePath, @ProductSubName, @IsActive, @AddedDate, @AddedBy, @AddedPC)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductId", ProductID);
                cmd.Parameters.AddWithValue("@ProductName", productListDto.ProductName);
                cmd.Parameters.AddWithValue("@ProductGroupID", productListDto.ProductGroupID);
                cmd.Parameters.AddWithValue("@Specification", productListDto.Specification);
                cmd.Parameters.AddWithValue("@BrandId", productListDto.BrandId);
                cmd.Parameters.AddWithValue("@UnitId", productListDto.UnitId);
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                cmd.Parameters.AddWithValue("@ProductSubName", productListDto.ProductSubName ?? string.Empty);
                cmd.Parameters.AddWithValue("@IsActive", 1);
                cmd.Parameters.AddWithValue("@AddedBy", productListDto.AddedBy);
                cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@AddedPC", productListDto.AddedPC);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();

                return (new { message = "Product Added Successfully." });
            }


        }
        catch (Exception ex)
        {

            return (new { message = "Product didn't added Successfully." });
        }

    }

    //======================================================================

    public async Task<object> UpdateProductListAsync(ProductListModel productListDto)
    {
        try
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            Boolean check = await ProductListExist(productListDto.ProductId);

            if (check)
            {
                await con.OpenAsync();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string ImagePath = CommonServices.UploadFiles(foldername, filename, productListDto.ImageFile);

                        if (ImagePath != null)
                        {


                            if (string.IsNullOrEmpty(productListDto.ExistingImageFileName))
                            {
                                string query = "UpdateProductListWithImage";
                                SqlCommand cmd = new SqlCommand(query, con, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ProductId", productListDto.ProductId);
                                cmd.Parameters.AddWithValue("@ProductName", productListDto.ProductName);
                                cmd.Parameters.AddWithValue("@ProductGroupID", productListDto.ProductGroupID);
                                cmd.Parameters.AddWithValue("@Specification", productListDto.Specification);
                                cmd.Parameters.AddWithValue("@BrandId", productListDto.BrandId);
                                cmd.Parameters.AddWithValue("@UnitId", productListDto.UnitId);

                                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                                cmd.Parameters.AddWithValue("@ProductSubName", productListDto.ProductSubName ?? string.Empty);

                                cmd.Parameters.AddWithValue("@UpdatedBy", productListDto.UpdatedBy ?? string.Empty);
                                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@UpdatedPC", productListDto.UpdatedPC ?? string.Empty);

                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        else
                        {
                            string query1 = "UpdateProductListWithoutImage";
                            SqlCommand cmdd = new SqlCommand(query1, con, transaction);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.Parameters.AddWithValue("@ProductId", productListDto.ProductId);
                            cmdd.Parameters.AddWithValue("@ProductName", productListDto.ProductName);
                            cmdd.Parameters.AddWithValue("@ProductGroupID", productListDto.ProductGroupID);
                            cmdd.Parameters.AddWithValue("@Specification", productListDto.Specification);
                            cmdd.Parameters.AddWithValue("@UnitId", productListDto.UnitId);
                            cmdd.Parameters.AddWithValue("@BrandId", productListDto.BrandId);
                            cmdd.Parameters.AddWithValue("@ProductSubName", productListDto.ProductSubName ?? string.Empty);
                            cmdd.Parameters.AddWithValue("@UpdatedBy", productListDto.UpdatedBy ?? string.Empty);
                            cmdd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                            cmdd.Parameters.AddWithValue("@UpdatedPC", productListDto.UpdatedPC ?? string.Empty);

                            await cmdd.ExecuteNonQueryAsync();


                        }

                        transaction.Commit();
                        return (new { message = "Product updated successfully." });
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
            else
            {
                return (new { message = "Product not found!" });
            }
        }
        catch (Exception ex)
        {
            return (new { message = $"Error updating product: {ex.Message}" });
        }
    }

    //======================================================================
    public async Task<DataTable> GetProductGroupsListAsync()
    {
        try
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            DataTable dataTable = new DataTable();
            await con.OpenAsync();
            string query = @"SELECT
                                PL.ProductId, 
                                PL.ProductName, 
                                PL.UnitId,
                                U.Name as UnitName, 
                                PL.ProductGroupID,
                                PG.ProductGroupName
                                FROM ProductList PL
                                JOIN Units U ON U.UnitId = PL.UnitId
                                JOIN ProductGroups PG ON PG.ProductGroupID = PL.ProductGroupID 
                                WHERE PL.IsActive = 1  
                                ORDER BY PL.ProductId DESC;  ";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }

            return (dataTable);
        }
        catch (Exception ex)
        {
            return null;
        }

    }


    public async Task<DataTable> GetProductListByStatus(bool? status = null)
    {
        try
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            DataTable dt = new DataTable();
            await con.OpenAsync();
            string query = "";
            if (status != null)
            {
                query = @"  SELECT PL.ProductId,PL.ProductName,PL.ProductGroupID,PG.ProductGroupName,PL.Specification, PL.BrandId , B.BrandName,
                                PL.UnitId,U.Name Unit,PL.IsActive,PL.AddedDate,PL.UpdatedDate,PL.AddedBy,PL.UpdatedBy,
                                PL.AddedPC,PL.UpdatedPC,PL.ImagePath,PL.Status,ProductSubName FROM ProductList PL 
                                LEFT JOIN ProductGroups PG ON PL.ProductGroupID=PG.ProductGroupID
								LEFT JOIN Brands B ON PL.BrandId = B.BrandId
                                LEFT JOIN Units U ON PL.UnitId = U.UnitId WHERE PL.IsActive= @IsActive ORDER BY PL.ProductId  DESC;";
            }
            else
            {
                query = @"SELECT PL.ProductId,PL.ProductName,PL.ProductGroupID,PG.ProductGroupName,PL.Specification, PL.BrandId , B.BrandName,
                            PL.UnitId,U.Name Unit,PL.IsActive,PL.AddedDate,PL.UpdatedDate,PL.AddedBy,PL.UpdatedBy,
                            PL.AddedPC,PL.UpdatedPC,PL.ImagePath,PL.Status,PL.ProductSubName FROM ProductList PL 
                            LEFT JOIN ProductGroups PG ON PL.ProductGroupID=PG.ProductGroupID
                            LEFT JOIN Brands B ON PL.BrandId = B.BrandId
                            LEFT JOIN Units U ON PL.UnitId = U.UnitId WHERE CONVERT(DATE, PL.AddedDate) = CONVERT(DATE, GETDATE()) ORDER BY PL.ProductId  DESC";
            }
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                if (status != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@IsActive", status));
                }


                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return (dt);
        }
        catch (Exception ex)
        {
            return null;
        }

    }




    // ==============================productName by productGroupId===================

    public async Task<DataTable> GetProductNameByProductGroupId(int ProductGroupId)
    {
        SqlConnection con = new SqlConnection(_healthCareConnection);
        DataTable dataTable = new DataTable();

        try
        {

            await con.OpenAsync();
            string query = @"ProductNameDropDown";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // Add the parameter and its value to the command
                cmd.Parameters.AddWithValue("@ProductGroupId", ProductGroupId);


                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // You might want to throw the exception again if you cannot handle it at this level.
            throw;
        }
        finally
        {
            // Ensure the connection is closed, even in case of an exception.
            if (con.State == ConnectionState.Open)
            {
                await con.CloseAsync();
            }
        }

        return dataTable;
    }


    //========================tushar=========================
    public async Task<object> MakeProductActiveOrInactiveAsync(List<int> productIds, bool? IsActive)
    {
        SqlConnection con = new SqlConnection(_healthCareConnection);
        try
        {
            string query = @"UPDATE ProductList
                          SET IsActive = @IsActive
                          WHERE ProductId IN ({0})";

            // Create a parameterized list of parameters for the IN clause
            string parameterList = string.Join(",", productIds.Select((_, index) => $"@ProductId{index}"));
            query = string.Format(query, parameterList);

            using (SqlCommand command = new SqlCommand(query, con))
            {
                // Add parameters for productIds
                for (int i = 0; i < productIds.Count; i++)
                {
                    command.Parameters.AddWithValue($"@ProductId{i}", productIds[i]);
                }

                command.Parameters.AddWithValue("@IsActive", IsActive);

                await con.OpenAsync();

                // Log the final query and parameters
                Console.WriteLine("Executing query: " + query);
                foreach (SqlParameter param in command.Parameters)
                {
                    Console.WriteLine($"{param.ParameterName} = {param.Value}");
                }

                // Execute the command
                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No products found.");
                    return new { message = "No products found." };
                }

                Console.WriteLine($"{rowsAffected} rows affected.");
                await con.CloseAsync();
            }

            return new { message = "Products' IsActive status changed." };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new { message = $"Products' IsActive status not changed: {ex.Message}" };
        }
    }

}
