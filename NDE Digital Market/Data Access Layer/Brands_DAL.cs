using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class Brands_DAL
    {

        private readonly string _healthCareConnection;
        public Brands_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }



        public async Task<DataTable> GetBrandListAsync(bool? isActive)
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = string.Empty;

                if (isActive.HasValue)
                {
                    query = "SELECT * FROM Brands WHERE isActive = @IsActive;";
                }
                else
                {
                    query = "SELECT * FROM Brands WHERE DATEDIFF(hour, AddedDate, GETDATE()) <= 24;";
                }
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (isActive.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@IsActive", isActive.Value);
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
                return null;
            }
        }



        public async Task<object> PostBrandAsync(BrandsModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    string query = @"INSERT INTO Brands (BrandName, ShortName, Description, IsActive, AddedBy, AddedPC, AddedDate)
                         VALUES (@BrandName, @ShortName, @Description, @IsActive, @AddedBy, @AddedPC, @AddedDate);";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BrandName", model.BrandName);
                        cmd.Parameters.AddWithValue("@ShortName", model.ShortName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        cmd.Parameters.AddWithValue("@AddedBy", model.AddedBy);
                        cmd.Parameters.AddWithValue("@AddedPC", model.AddedPC);
                        cmd.Parameters.AddWithValue("@AddedDate", DateTime.UtcNow);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    return (new { message = "Brand added successfully" });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return (new { message = "Brand didn't added successfully" });
            }
        }


        public async Task<object> PutBrand(BrandsModel model)
        {
            if (model == null || model.BrandId == null)
            {
                return (new { message = "Invalid Brand data." });
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = "UPDATE Brands SET ";
                    List<string> setClauses = new List<string>();

                    if (model.BrandName != null)
                    {
                        setClauses.Add("BrandName = @BrandName");
                    }

                    if (model.ShortName != null)
                    {
                        setClauses.Add("ShortName = @short");
                    }

                    if (model.Description != null)
                    {
                        setClauses.Add("Description = @Description");
                    }

                    // Add other parameters here...

                    setClauses.Add("UpdatedBy = @UpdatedBy");
                    setClauses.Add("UpdatedPC = @UpdatedPC");
                    setClauses.Add("UpdatedDate = @DateUpdated");

                    query += string.Join(", ", setClauses);
                    query += " WHERE BrandId = @BrandId;";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BrandId", model.BrandId);

                        if (model.BrandName != null)
                        {
                            cmd.Parameters.AddWithValue("@BrandName", model.BrandName);
                        }

                        if (model.ShortName != null)
                        {
                            cmd.Parameters.AddWithValue("@short", model.ShortName);
                        }

                        if (model.Description != null)
                        {
                            cmd.Parameters.AddWithValue("@Description", model.Description);
                        }

                        // Add other parameters here...

                        cmd.Parameters.AddWithValue("@UpdatedBy", model.UpdatedBy);
                        cmd.Parameters.AddWithValue("@UpdatedPC", model.UpdatedPC);
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return (new { message = "Brand updated successfully." });
                        }
                        else
                        {
                            return (new { message = "Brand not found." });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return (new { message = "Error updating the Brand." });
            }
        }


        //[HttpPut]
        //[Route("ChangeBrandStatus")]
        //public async Task<IActionResult> ChangeBrandStatus(string BrandId, bool isActive)
        //{
        //    if (BrandId == null)
        //    {
        //        return BadRequest(new { message = "Invalid Brand data." });
        //    }

        //    try
        //    {
        //        await con.OpenAsync();
        //        string query = @"UPDATE Brands 
        //                 SET
        //                     IsActive = @IsActive
        //                 WHERE BrandId = @BrandId;";

        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@BrandId", BrandId);
        //            cmd.Parameters.AddWithValue("@IsActive", isActive);


        //            int rowsAffected = await cmd.ExecuteNonQueryAsync();
        //            if (rowsAffected > 0)
        //            {
        //                return Ok(new { message = "Brand updated successfully." });
        //            }
        //            else
        //            {
        //                return NotFound(new { message = "Brand not found." });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        // Log the exception
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating the Brand." });
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            await con.CloseAsync();
        //        }
        //    }
        //}



        public async Task<object> ChangeBrandsStatus(string[] ids, bool isActive)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();

                    string query = @"UPDATE Brands 
                        SET IsActive = @IsActive 
                        WHERE BrandId IN ({0});";

                    // Construct the SQL parameter placeholders dynamically
                    string parameterPlaceholders = string.Join(",", ids.Select((s, i) => $"@BrandId{i}"));
                    query = string.Format(query, parameterPlaceholders);

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters for each unit ID
                        for (int i = 0; i < ids.Length; i++)
                        {
                            cmd.Parameters.AddWithValue($"@BrandId{i}", ids[i]);
                        }
                        cmd.Parameters.AddWithValue("@IsActive", isActive);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return (new { message = "Brands updated successfully." });
                        }
                        else
                        {
                            return (new { message = "No Brand found." });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return (new { message = "Error updating the Brand." });
            }
        }


    }
}
