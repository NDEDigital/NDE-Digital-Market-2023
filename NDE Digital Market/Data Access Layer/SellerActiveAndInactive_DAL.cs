using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class SellerActiveAndInactive_DAL
    {
        private readonly string _healthCareConnection;

        public SellerActiveAndInactive_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        public async Task<DataTable> CompanySellerDetails(string CompanyCode, bool IsSeller, bool IsActive)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = @"DECLARE @SpecificCompanyCode NVARCHAR(255);
                                IF EXISTS (SELECT * FROM CompanyRegistration WHERE CompanyCode = @CompanyCode)
                                    SET @SpecificCompanyCode = @CompanyCode;
                                ELSE
                                    SET @SpecificCompanyCode = NULL;

                                                        SELECT
                                                            UR.UserId,
                                                            UR.FullName,
                                                            UR.PhoneNumber,
                                                            UR.Email,
                                                            UR.Address,
                                                            UR.AddedDate,
                                                            UR.IsActive,
                                                            UR.CompanyCode,
                                                            UR.IsBuyer,
                                                            UR.IsSeller,
                                                        
	                                                        CR.CompanyCode,
	                                                        CR.CompanyName,
                                                            CR.CompanyAdminId
                                                        FROM
                                                            UserRegistration UR
                                                        JOIN
                                                            CompanyRegistration CR ON UR.CompanyCode = CR.CompanyCode
                                                        WHERE
                                                            (
                                                                (UR.IsSeller = @IsSeller AND UR.IsActive = @IsActive AND
                                                                    (UR.CompanyCode = @SpecificCompanyCode OR @SpecificCompanyCode IS NULL)
                                                                )
                                                            )
                                                            AND
                                                            (
                                                                CR.CompanyCode = @SpecificCompanyCode OR @SpecificCompanyCode IS NULL
                                );";



                        using (SqlConnection con = new SqlConnection(_healthCareConnection))
                        {
                            await con.OpenAsync();

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);
                                cmd.Parameters.AddWithValue("@IsActive", IsActive);

                                cmd.Parameters.AddWithValue("@IsSeller", IsSeller);

                                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                {
                                    adapter.Fill(dataTable);
                                }
                            }
                        }



                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // You might want to handle errors more gracefully
                return null;
            }
        }


        public async Task<object> UpdateSellerProductStatusAsync(string userIds, bool isActive)
        {
            try
            {
                string query = $" UPDATE UserRegistration SET IsActive = @IsActive WHERE UserId IN ({userIds})";

                using (var connection = new SqlConnection(_healthCareConnection))
                {

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        command.Parameters.AddWithValue("@IsActive", isActive);
                        await command.ExecuteNonQueryAsync();
                        await connection.CloseAsync();
                    }


                }

                return (new { message = "updated seller" });
            }
            catch (Exception ex)
            {
                return (new { message = ex.Message });
            }

        }

    }
}
