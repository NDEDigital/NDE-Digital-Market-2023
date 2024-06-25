using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class PermissionToDashboard_DAL
    {

        private readonly string _healthCareConnection;
        public PermissionToDashboard_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }


        public async Task<object> InsertPermissionToDashboard(int UserId, int MenuId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"INSERT INTO Permission (UserId, MenuId, IsActive, PermissionId) VALUES (@UserId,@MenuId, 1, (select Max(PermissionId) from Permission)+1);";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.Parameters.AddWithValue("@MenuId", MenuId);


                        // ExecuteScalarAsync is used for queries that return a single value
                        var insertedItemId = await cmd.ExecuteScalarAsync();

                        // If needed, you can return the inserted ItemID
                        return (new
                        {
                            Message = "item insert  successful",

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (new
                {
                    Message = "Item Insert  Unsuccessful.",

                });
            }
        }


        public async Task<DataTable> GetPermissionData(int UserId)
        {
            try
            {
                DataTable dataTable = new DataTable();  
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @" SELECT P.UserId,M.MenuId,P.PermissionId,M.MenuName,U.FullName,U.CompanyCode,U.FUllName
                                      FROM Permission P 
                                      left JOIN MenuList M ON P.MenuId = M.MenuId
                                      left JOIN UserRegistration U ON P.UserId = U.UserId
                                      WHERE p.UserId = @UserId and M.IsAdmin = 0 AND M.IsActive=1 AND U.CompanyCode=(select CompanyCode from UserRegistration WHERE UserId= @UserId);";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId", UserId);


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
                // Handle exceptions appropriately (logging, returning an error response, etc.)
                return null;
            }
        }

    }
}
