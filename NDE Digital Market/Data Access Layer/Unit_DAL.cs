using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class Unit_DAL
    {


        private readonly string _healthCareConnection;

        public Unit_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        public async Task<DataTable> GetUnitListAsync(bool? isActive)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            DataTable dataTable = new DataTable();

            try
            {
                await con.OpenAsync();
                string query;
                if (isActive.HasValue)
                {
                    query = "SELECT * FROM Units WHERE isActive = @IsActive;";
                }
                else
                {
                    query = "SELECT * FROM Units WHERE DATEDIFF(hour, DateAdded, GETDATE()) <= 24;";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    if (isActive.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", isActive.Value);
                    }


                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
            return dataTable;
        }


        public async Task<object> PostUnit(UnitModel unit)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            try
            {
                await con.OpenAsync();
                string query = @"INSERT INTO Units (Name, Description, isActive, isConversion, AddedBy, AddedPC, DateAdded)
                         VALUES (@Name, @Description, @IsActive, @IsConversion, @AddedBy, @AddedPC, @DateAdded);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", unit.Name);
                    cmd.Parameters.AddWithValue("@Description", unit.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@IsConversion", true);
                    cmd.Parameters.AddWithValue("@AddedBy", unit.AddedBy);
                    cmd.Parameters.AddWithValue("@AddedPC", unit.AddedPC);
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);

                    await cmd.ExecuteNonQueryAsync();
                }

                return (new { message = "Unit added successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return (new { message = "Error adding the unit" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }


        public async Task<object> PutUnit(UnitModel unit)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            if (unit == null || unit.UnitId == null)
            {
                return (new { message = "Invalid unit data." });
            }

            try
            {
                await con.OpenAsync();
                string query = @"UPDATE Units 
                         SET Name = @Name, 
                             Description = @Description, 

                             isConversion = @IsConversion,
                             UpdatedBy = @UpdatedBy,
                             UpdatedPC = @UpdatedPC, 
                             DateUpdated = @DateUpdated
                         WHERE UnitId = @UnitId;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UnitId", unit.UnitId);
                    cmd.Parameters.AddWithValue("@Name", unit.Name);
                    cmd.Parameters.AddWithValue("@Description", unit.Description ?? (object)DBNull.Value);
                    // cmd.Parameters.AddWithValue("@IsActive", unit.IsActive);
                    cmd.Parameters.AddWithValue("@IsConversion", unit.IsConversion);
                    cmd.Parameters.AddWithValue("@UpdatedBy", unit.UpdatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedPC", unit.UpdatedPC);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return (new { message = "Unit updated successfully." });
                    }
                    else
                    {
                        return (new { message = "Unit not found." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return ( new { message = "Error updating the unit." });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }


        public async Task<object> UpdateUnitByUnitID(int unitID, bool isActive)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            if (unitID == null)
            {
                return (new { message = "Invalid unit data." });
            }

            try
            {
                await con.OpenAsync();
                string query = @"UPDATE Units 
                         SET
                         isActive = @IsActive
                         WHERE UnitId = @UnitId;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UnitId", unitID);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);


                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return (new { message = "Unit updated successfully." });
                    }
                    else
                    {
                        return (new { message = "Unit not found." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return ( new { message = "Error updating the unit." });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }


        public async Task<object> UpdateUnitsByUnitID(int[] unitIDs, bool isActive)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);


            try
            {
                await con.OpenAsync();

                string query = @"UPDATE Units 
                        SET isActive = @IsActive 
                        WHERE UnitId IN ({0});";

                // Construct the SQL parameter placeholders dynamically
                string parameterPlaceholders = string.Join(",", unitIDs.Select((s, i) => $"@UnitId{i}"));
                query = string.Format(query, parameterPlaceholders);

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameters for each unit ID
                    for (int i = 0; i < unitIDs.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"@UnitId{i}", unitIDs[i]);
                    }
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return (new { message = "Units updated successfully." });
                    }
                    else
                    {
                        return (new { message = "No units found." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return ( new { message = "Error updating the units." });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }

    }
}
