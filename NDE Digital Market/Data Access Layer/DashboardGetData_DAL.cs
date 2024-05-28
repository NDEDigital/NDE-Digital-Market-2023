

using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class DashboardGetData_DAL
    {

        private readonly string _healthCareConnection;
        public DashboardGetData_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        public async Task<DataTable> GetPermissionData(int UserId, int Status1)
        {
            try
            {
                DataTable dataTable = new DataTable();  
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"
                        DECLARE @Status INT = @Status1;
                        DECLARE @UserId INT = @UserId1;

                        IF @Status = 0
                        BEGIN
                            SELECT P.UserId, P.MenuId, M.IsActive, M.MenuName 
                            FROM Permission P 
                            JOIN MenuList M ON P.MenuId = M.MenuId
                            WHERE P.UserId = @UserId AND M.IsActive = 1;
                        END
                        ELSE IF @Status = 1
                        BEGIN
                            SELECT MenuId, MenuName
                            FROM MenuList
                            WHERE IsAdmin != 1 AND IsActive = 1;
                        END
                    ";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId1", UserId);
                        cmd.Parameters.AddWithValue("@Status1", Status1); // Fix: Use @Status instead of status

                            if (Status1 == 1)
                            {
                                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                {
                                    adapter.Fill(dataTable);
                                }
                            }
                            else
                            {
                                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                {
                                    adapter.Fill(dataTable);
                                }
                            }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
