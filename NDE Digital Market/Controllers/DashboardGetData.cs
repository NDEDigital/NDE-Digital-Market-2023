using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.SharedServices;
using System;
using System.Collections.Generic; // Add this for List<T>
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NDE_Digital_Market.Controllers
{
    public class DashboardGetData : Controller
    {
        private readonly string _healthCareConnection;

        public DashboardGetData(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        [HttpGet]
        [Route("SellerPermissionData/{UserId}")]
        public async Task<IActionResult> GetPermissionData(int UserId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"SELECT P.UserId,P.MenuId,M.IsActive,M.MenuName FROM Permission P JOIN MenuList M
                                                                    ON P.MenuId=M.MenuId
                                                                    WHERE UserId=@UserId;";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add the @UserId parameter
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            var result = new List<PermissionToDashDto>(); // Fix the List initialization

                            while (reader.Read())
                            {
                              

                                var permission = new PermissionToDashDto
                                {
                                   UserId = Convert.ToInt32(reader["UserId"]),
                                MenuId = Convert.ToInt32(reader["MenuId"]),
                                    MenuName = reader["MenuName"].ToString(),
                                    // Add the necessary properties in the PermissionToDashDto class
                                    // FullName = reader["FullName"].ToString(),
                                    // PermissionId = reader.GetInt32(reader.GetOrdinal("PermissionId")),
                                };

                                result.Add(permission); // Add the permission to the result list
                            }

                            return Ok(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, returning an error response, etc.)
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
