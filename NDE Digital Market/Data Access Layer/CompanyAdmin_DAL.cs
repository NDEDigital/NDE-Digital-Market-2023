using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class CompanyAdmin_DAL
    {
        private readonly string _healthCareConnection;
        public CompanyAdmin_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }



        public async Task<DataTable> CompanySellerDetails(int userId, bool IsActive)
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT UR.UserId, UR.FullName, UR.PhoneNumber, UR.Email, UR.Address, UR.AddedDate, CR.IsActive, CR.CompanyCode, CR.CompanyName, CR.CompanyAdminId
                                FROM UserRegistration UR
                                JOIN CompanyRegistration CR ON UR.CompanyCode = CR.CompanyCode
                                WHERE CR.IsActive = 1 AND UR.IsActive = @IsActive AND UR.UserId!=  CR.CompanyAdminId AND UR.UserId!=@userId
                                AND EXISTS (SELECT * FROM UserRegistration WHERE CompanyCode = CR.CompanyCode AND UserId = @userId);";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@IsActive", IsActive);
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



        public async Task<object> UpdateUserStatus(int userId, bool IsActive)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    con.Open();

                    string query = @"UPDATE UserRegistration SET IsActive = @IsActive WHERE UserId = @UserId;";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@IsActive", IsActive);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + "ekhane Jhamela");
                        if (rowsAffected > 0)
                        {
                            return (new { message = "Updated Successfully" }); // Update successful
                        }
                        else
                        {
                            return (new { message = "User not found." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (new { message = "Updated Unsuccessfull." });
            }
        }

    }
}
