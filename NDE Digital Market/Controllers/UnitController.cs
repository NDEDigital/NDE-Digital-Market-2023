using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;
using System.Data;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection con;
        public UnitController(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _configuration = configuration;
            con = new SqlConnection(commonServices.HealthCareConnection);

        }
        [HttpGet]
        [Route("GetUnitList")]
        public async Task<List<UnitModel>> GetUnitListAsync(bool? isActive)
        {
            List<UnitModel> lst = new List<UnitModel>();

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
                    if (isActive.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", isActive.Value);
                    }

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            UnitModel modelObj = new UnitModel
                            {
                                UnitId = Convert.ToInt32(reader["UnitId"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                IsConversion = reader.GetBoolean(reader.GetOrdinal("IsConversion"))
                            };

                            lst.Add(modelObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
            return lst;
        }
        [HttpPost]
        [Route("AddUnit")]
        public async Task<IActionResult> PostUnit([FromForm] UnitModel unit)
        {
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

                return Ok(new { message = "Unit added successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error adding the unit" });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }
        [HttpPut]
        [Route("UpdateUnit")]
        public async Task<IActionResult> PutUnit([FromForm] UnitModel unit)
        {
            if (unit == null || unit.UnitId == null)
            {
                return BadRequest(new { message = "Invalid unit data." });
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
                        return Ok(new { message = "Unit updated successfully." });
                    }
                    else
                    {
                        return NotFound(new { message = "Unit not found." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating the unit." });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }
        [HttpPut]
        [Route("UpdateUnitByID")]
        public async Task<IActionResult> UpdateUnitByUnitID(string unitID, bool isActive)
        {
            if (unitID == null)
            {
                return BadRequest(new { message = "Invalid unit data." });
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
                        return Ok(new { message = "Unit updated successfully." });
                    }
                    else
                    {
                        return NotFound(new { message = "Unit not found." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating the unit." });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }
        [HttpPut]
        [Route("UpdateUnitsByID")]
        public async Task<IActionResult> UpdateUnitsByUnitID(string unitIDs, bool isActive)
        {
            if (string.IsNullOrEmpty(unitIDs))
            {
                return BadRequest(new { message = "No unit IDs provided." });
            }

            try
            {
                await con.OpenAsync();
                string[] ids = unitIDs.Split(',');

                string query = @"UPDATE Units 
                        SET isActive = @IsActive 
                        WHERE UnitId IN ({0});";

                // Construct the SQL parameter placeholders dynamically
                string parameterPlaceholders = string.Join(",", ids.Select((s, i) => $"@UnitId{i}"));
                query = string.Format(query, parameterPlaceholders);

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameters for each unit ID
                    for (int i = 0; i < ids.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"@UnitId{i}", ids[i]);
                    }
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Units updated successfully." });
                    }
                    else
                    {
                        return NotFound(new { message = "No units found." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating the units." });
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
