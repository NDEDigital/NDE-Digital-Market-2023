using NDE_Digital_Market.SharedServices;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class UserControllOfDashboard_DAL
    {

        private readonly string _healthCareConnection;

        public UserControllOfDashboard_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }



        public async Task<object> DeleteMenuItems(int UserId, List<int> menuIdsToDelete)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();

                    // Create a parameterized query with dynamic number of parameters
                    string query = $"DELETE FROM Permission WHERE UserId = @UserId AND MenuId IN ({string.Join(",", menuIdsToDelete.Select((id, index) => $"@MenuId{index}"))})";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        // Add parameters for each menu ID
                        for (int i = 0; i < menuIdsToDelete.Count; i++)
                        {
                            cmd.Parameters.AddWithValue($"@MenuId{i}", menuIdsToDelete[i]);
                        }

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return (new { message = "Menu items deleted successfully." });
                        }
                        else
                        {
                            return (new { message = "User or menu items not found." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // logger.LogError(ex, "An error occurred while processing the request.");
                return (new { message = "User or menu items not found." });
            }
        }


    }
}
