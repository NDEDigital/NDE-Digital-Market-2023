using System.Data;
using System.Data.SqlClient;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Data_Access_Layer
{
	public class AddBanner_DAL
	{
		
        private readonly string foldername;
        private readonly string filename = "banner";
        private readonly string _healthCareConnection;

        public AddBanner_DAL(IConfiguration configuration)
		{
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
            foldername = commonServices.FilesPath + "banner";
		}


        public async Task<object> AddBanners(AdsAndBannerModel banner)
		{
            try
            {
                string bannerImage = CommonServices.UploadFiles(foldername, filename, banner.BannerImageFile);

                string query = @"INSERT INTO AdBanner (UserId, IsActive, IsAds, AddedDate, AddedBy, UpdatedDate, UpdatedBy, AddedPC, UpdatedPC, CompanyCode, BannerDescription, BannerImage, StartDate, EndDate, IsPayment, PaymentRemarks)
							 VALUES (@UserId, @IsActive, @IsAds, @AddedDate, @AddedBy, @UpdatedDate, @UpdatedBy, @AddedPC, @UpdatedPC, @CompanyCode, @BannerDescription, @BannerImage, @StartDate, @EndDate, @IsPayment, @PaymentRemarks);";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId", banner.UserId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsAds", banner.IsAds ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AddedBy", banner.AddedBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedDate", banner.UpdateDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddedPC", banner.AddedPC ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CompanyCode", banner.CompanyCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BannerDescription", banner.BannerDescription ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BannerImage", bannerImage ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsPayment", banner.IsPayment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PaymentRemarks", banner.PaymentRemarks ?? (object)DBNull.Value);


                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                    }
                }
                return (new { message = "Banner Added Successful!" });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (new { message = "Banner Added Unsuccessful!" });
            }









		}

        //public async Task<List<BannerDto>> GetBanners()
        //{
        //    List<BannerDto> banners = new List<BannerDto>();

        //    string query = "SELECT * FROM AdBanner";

        //    using (SqlCommand cmd = new SqlCommand(query, _connection))
        //    {
        //        await _connection.OpenAsync();

        //        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                BannerDto banner = new BannerDto
        //                {

        //                    UserId = reader["UserId"] != DBNull.Value ? (int)reader["UserId"] : (int?)null,
        //                    IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null,
        //                    AddedDate = reader["AddedDate"] != DBNull.Value ? (DateTime)reader["AddedDate"] : (DateTime?)null,
        //                    AddedBy = reader["AddedBy"].ToString(),
        //                    UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime)reader["UpdatedDate"] : (DateTime?)null,
        //                    UpdatedBy = reader["UpdatedBy"].ToString(),
        //                    AddedPC = reader["AddedPC"].ToString(),
        //                    UpdatedPC = reader["UpdatedPC"].ToString(),
        //                    CompanyCode = reader["CompanyCode"].ToString(),
        //                    BannerDescription = reader["BannerDescription"].ToString(),
        //                    BannerImage = reader["BannerImage"].ToString(),
        //                    StartDate = reader["StartDate"] != DBNull.Value ? (DateTime)reader["StartDate"] : (DateTime?)null,
        //                    EndDate = reader["EndDate"] != DBNull.Value ? (DateTime)reader["EndDate"] : (DateTime?)null,
        //                    IsPayment = reader["IsPayment"] != DBNull.Value ? (bool)reader["IsPayment"] : (bool?)null,
        //                    PaymentRemarks = reader["PaymentRemarks"].ToString()
        //                    // Map other properties similarly
        //                };

        //                banners.Add(banner);
        //            }
        //        }

        //        await _connection.CloseAsync();
        //    }

        //    return banners;
        //}







        public async Task<DataTable> GetAddBannerForSeller(string CompanyCode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT AB.BannerID, AB.BannerDescription, AB.BannerImage, CR.CompanyName, AB.AddedDate , AB.IsBannerStatus, AB. IsActive, AB. IsAds
                                  FROM AdBanner AB
                                  Left join CompanyRegistration CR on CR.CompanyCode = Ab.CompanyCode
                                  where AB.CompanyCode = @CompanyCode;";





                using(SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }







                //while (await reader.ReadAsync())
                //{
                //    ImageBanner banner = new ImageBanner();
                //    //banner.UserId = reader.GetInt32(0);

                //    banner.BannerID = reader.GetInt32(0);
                //    banner.BannerDescription = reader["BannerDescription"].ToString();
                //    banner.BannerImage = reader["BannerImage"].ToString();
                //    banner.CompanyName = reader["CompanyName"].ToString();
                //    banner.IsBannerStatus = reader.IsDBNull(reader.GetOrdinal("IsBannerStatus")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsBannerStatus"));
                //    banner.IsAds = reader.IsDBNull(reader.GetOrdinal("IsAds")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsAds"));
                //    banner.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

                //    if (!(reader["AddedDate"] is DBNull))
                //    {
                //        banner.AddedDate = reader.GetDateTime(4);
                //    }

                //    banners.Add(banner);
                //}

                return dataTable;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }



        public async Task<DataTable> GetAddBannerForAdmin(bool? status)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string query = @"GetAddBannerForAdmin";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Status", status);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                return dataTable;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }





        public async Task<DataTable> GetBannerForShowingInHomePage()
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT * FROM AdBanner WHERE IsBannerStatus = 1 
                              AND IsActive = 1 
                              AND IsAds =0
                              AND StartDate <= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)
							  and EndDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0);";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                return dataTable;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }





        }




        public async Task<DataTable> GetAddForShowingInHomePage()
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT *
                            FROM AdBanner 
                            WHERE IsBannerStatus = 1 
                              AND IsActive = 1 
                              AND IsAds =1
                              AND StartDate <= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)
							  and EndDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0);";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                return dataTable;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }







        //public async Task<ImageBanner> GetBannerById(int bannerId)
        //{
        //    ImageBanner banner = null;

        //    try
        //    {
        //        await _connection.OpenAsync();

        //        SqlCommand command = new SqlCommand("SELECT BannerDescription, BannerImage FROM AdBanner WHERE BannerID = @BannerID", _connection);
        //        command.Parameters.AddWithValue("@BannerID", bannerId);

        //        SqlDataReader reader = await command.ExecuteReaderAsync();

        //        if (await reader.ReadAsync())
        //        {
        //            banner = new ImageBanner
        //            {
        //                BannerID = bannerId,
        //                BannerDescription = reader["BannerDescription"].ToString(),
        //                BannerImage = reader["BannerImage"].ToString()
        //            };
        //        }

        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception, log, or throw as needed
        //    }
        //    finally
        //    {
        //        if (_connection.State == ConnectionState.Open)
        //            _connection.Close();
        //    }

        //    return banner;
        //}



        public async Task<object> DeleteBanner(int bannerId)
        {
            try
            {
                // Create the SQL command to delete the banner with the specified bannerId
                string query = "DELETE FROM AdBanner WHERE BannerId = @BannerID";


                using(SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    // Create the SQL command object
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("@BannerID", bannerId);

                        // Execute the command
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        // If rowsAffected is greater than 0, deletion was successful
                        return rowsAffected > 0;
                    }
                }
                return (new { message = "Banner Deleted Successful!" });

            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Error deleting banner: {ex.Message}");
                return (new { message = "Banner Deleted Unsuccessful!" });
            }
        }







        //public async Task<bool> UpdateBanner(BannerDto banner)
        //{

        //    string bannerImage = CommonServices.UploadFiles(_folderName, _fileName, banner.BannerImageFile);
        //    try
        //    {
        //        // Create the SQL command to update the banner
        //        string query = @"
        //    UPDATE AdBanner 
        //    SET 
        //        IsActive = @IsActive,
        //        AddedDate = @AddedDate
        //        AddedBy = @ AddedBy
        //        UpdatedDate = @UpdatedDate,
        //        UpdatedBy = @UpdatedBy,
        //        AddedPC   = @AddedPC
        //        UpdatedPC = @UpdatedPC,
        //        BannerDescription = @BannerDescription,
        //        BannerImage = @BannerImage,
        //        StartDate = @StartDate,
        //        EndDate = @EndDate,
        //        IsPayment = @IsPayment,
        //        PaymentRemarks = @PaymentRemarks
        //    WHERE BannerId = @BannerId";

        //        // Open the connection
        //        await _connection.OpenAsync();

        //        // Create the SQL command object
        //        using (SqlCommand cmd = new SqlCommand(query, _connection))
        //        {
        //            // Add parameters to the command


        //            cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@AddedBy", banner.AddedBy ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@UpdatedDate", banner.UpdatedDate ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@AddedPC", banner.AddedPC ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@BannerDescription", banner.BannerDescription ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@BannerImage", bannerImage ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@IsPayment", banner.IsPayment ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@PaymentRemarks", banner.PaymentRemarks ?? (object)DBNull.Value);
        //            cmd.Parameters.AddWithValue("@BannerId", banner.BannerID);

        //            // Execute the command
        //            int rowsAffected = await cmd.ExecuteNonQueryAsync();

        //            // If rowsAffected is greater than 0, update was successful
        //            return rowsAffected > 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log any exceptions
        //        Console.WriteLine($"Error updating banner: {ex.Message}");
        //        return false;
        //    }
        //    finally
        //    {
        //        // Close the connection
        //        if (_connection.State == ConnectionState.Open)
        //        {
        //            _connection.Close();
        //        }
        //    }
        //}

        //public async Task<string> UpdateBanner(BannerDto banner)
        //{
        //    string bannerImage = CommonServices.UploadFiles(_folderName, _fileName, banner.BannerImageFile);

        //    string query = @"UPDATE AdBanner 
        //             SET UserId = @UserId,
        //                 IsActive = @IsActive,
        //                 UpdatedDate = @UpdatedDate,
        //                 UpdatedBy = @UpdatedBy,
        //                 UpdatedPC = @UpdatedPC,
        //                 CompanyCode = @CompanyCode,
        //                 BannerDescription = @BannerDescription,
        //                 BannerImage = @BannerImage,
        //                 StartDate = @StartDate,
        //                 EndDate = @EndDate,
        //                 IsPayment = @IsPayment,
        //                 PaymentRemarks = @PaymentRemarks
        //             WHERE BannerID = @BannerID;";

        //    using (SqlCommand cmd = new SqlCommand(query, _connection))
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@BannerID", banner.BannerID );
        //        cmd.Parameters.AddWithValue("@UserId", banner.UserId ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@CompanyCode", banner.CompanyCode ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@BannerDescription", banner.BannerDescription ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@BannerImage", bannerImage ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@IsPayment", banner.IsPayment ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@PaymentRemarks", banner.PaymentRemarks ?? (object)DBNull.Value);

        //        await _connection.OpenAsync();
        //        await cmd.ExecuteNonQueryAsync();
        //        await _connection.CloseAsync();
        //    }

        //    return "Banner Updated successfully!";
        //}




        //        public async Task<string> UpdateBanner(BannerDto banner)
        //{
        //    // Check if a new banner image file is provided
        //    string bannerImage = banner.BannerImageFile != null ?
        //        CommonServices.UploadFiles(_folderName, _fileName, banner.BannerImageFile) :
        //        null;

        //    string query = @"UPDATE AdBanner 
        //                     SET UserId = COALESCE(@UserId, UserId),
        //                         IsActive = COALESCE(@IsActive, IsActive),
        //                         UpdatedDate = @UpdatedDate,
        //                         UpdatedBy = COALESCE(@UpdatedBy, UpdatedBy),
        //                         UpdatedPC = COALESCE(@UpdatedPC, UpdatedPC),
        //                         CompanyCode = COALESCE(@CompanyCode, CompanyCode),
        //                         BannerDescription = COALESCE(@BannerDescription, BannerDescription),
        //                         BannerImage = COALESCE(@BannerImage, BannerImage),
        //                         StartDate = COALESCE(@StartDate, StartDate),
        //                         EndDate = COALESCE(@EndDate, EndDate),
        //                         IsPayment = COALESCE(@IsPayment, IsPayment),
        //                         PaymentRemarks = COALESCE(@PaymentRemarks, PaymentRemarks)
        //                     WHERE BannerID = @BannerID;";

        //    using (SqlCommand cmd = new SqlCommand(query, _connection))
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@BannerID", banner.BannerID);
        //        cmd.Parameters.AddWithValue("@UserId", banner.UserId ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@CompanyCode", banner.CompanyCode ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@BannerDescription", banner.BannerDescription ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@BannerImage", bannerImage ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@IsPayment", banner.IsPayment ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@PaymentRemarks", banner.PaymentRemarks ?? (object)DBNull.Value);

        //        await _connection.OpenAsync();
        //        await cmd.ExecuteNonQueryAsync();
        //        await _connection.CloseAsync();
        //    }

        //    return "Banner Updated successfully!";
        //}





        public async Task<object> UpdateBanner(AdsAndBannerModel banner)
        {
            try
            {
                // Check if a new banner image file is provided
                string bannerImage = banner.BannerImageFile != null ? CommonServices.UploadFiles(foldername, filename, banner.BannerImageFile) : null;

                // Check if a new banner description is provided
                string bannerDescription = !string.IsNullOrEmpty(banner.BannerDescription) ? banner.BannerDescription : null;

                string query = @"UPDATE AdBanner 
                             SET UserId = COALESCE(@UserId, UserId),
                                 IsActive = COALESCE(@IsActive, IsActive),
                                 IsAds = COALESCE(@IsAds, IsAds),
                                 UpdatedDate = @UpdatedDate,
                                 UpdatedBy = COALESCE(@UpdatedBy, UpdatedBy),
                                 UpdatedPC = COALESCE(@UpdatedPC, UpdatedPC),
                                 CompanyCode = COALESCE(@CompanyCode, CompanyCode),
                                 BannerDescription = COALESCE(@BannerDescription, BannerDescription),
                                 BannerImage = COALESCE(@BannerImage, BannerImage),
                                 StartDate = COALESCE(@StartDate, StartDate),
                                 EndDate = COALESCE(@EndDate, EndDate),
                                 IsPayment = COALESCE(@IsPayment, IsPayment),
                                 PaymentRemarks = COALESCE(@PaymentRemarks, PaymentRemarks)
                             WHERE BannerID = @BannerID;";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@BannerID", banner.BannerID);
                        cmd.Parameters.AddWithValue("@UserId", banner.UserId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsAds", banner.IsAds ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CompanyCode", banner.CompanyCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BannerDescription", bannerDescription ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BannerImage", bannerImage ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsPayment", banner.IsPayment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PaymentRemarks", banner.PaymentRemarks ?? (object)DBNull.Value);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                    }
                }


                return (new { message = "Banner Updated Successful!" });
            }
            catch(Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Error deleting banner: {ex.Message}");
                return (new { message = "Banner Updated Unsuccessful!" });
            }

        }





        public async Task<object> UpdateBannerStatus(AdsAndBannerModel banner)
        {

            SqlConnection connection = new SqlConnection(_healthCareConnection);

            try
            {


                await connection.OpenAsync();


                if (banner.IsActive == true)
                {
                    string checkquery = @"SELECT  Count(BannerID) as count FROM [NDE_Digital_Development].[dbo].[AdBanner]
                                    where IsAds = 1 and IsActive = 1 and IsBannerStatus = 1 and EndDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and StartDate <= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0);";

                    SqlCommand checkcmd = new SqlCommand(checkquery, connection);
                    using (SqlDataReader reader = await checkcmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int res = Convert.ToInt32(reader["count"]);
                            if (res == 6)
                            {
                                return (new
                                {
                                    Message = "Ads Can't be Approved. Max Size Reached."
                                });
                            }
                        }
                    }

                }
                string query = @"UPDATE AdBanner 
                     SET IsActive = @IsActive,
                         UpdatedDate = @UpdatedDate,
                         UpdatedBy = @UpdatedBy,
                         UpdatedPC = @UpdatedPC,
                         StartDate = @StartDate,
                         EndDate = @EndDate,
                         IsBannerStatus = @IsBannerStatus
                     WHERE BannerID = @BannerID;";


                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@BannerID", banner.BannerID);
                    cmd.Parameters.AddWithValue("@IsActive", banner.IsActive ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", banner.UpdatedBy ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedPC", banner.UpdatedPC ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@StartDate", banner.StartDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", banner.EndDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsBannerStatus", banner.IsBannerStatus ?? (object)DBNull.Value);

                    var result = await cmd.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        return (new
                        {
                            Message = "Banner updated successfully."
                        });
                    }
                    else
                    {
                        return (new
                        {
                            Message = "Failed to update banner."
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new
                {
                    Message = "Failed to update banner/Ads."
                });
            }
            finally
            {
                connection.CloseAsync();
            }


        }
















        //public async Task<int> UpdateBanners(ImageBanner banner)
        //{
        //    string query = @"UPDATE AdBanner 
        //             SET BannerDescription = @BannerDescription, 
        //                 BannerImage = @BannerImage 
        //             WHERE BannerID = @BannerID";

        //    SqlCommand command = new SqlCommand(query, _connection);
        //    command.CommandType = CommandType.Text;
        //    command.Parameters.AddWithValue("@BannerID", banner.BannerID);
        //    command.Parameters.AddWithValue("@BannerDescription", (object)banner.BannerDescription ?? DBNull.Value);
        //    command.Parameters.AddWithValue("@BannerImage", (object)banner.BannerImage ?? DBNull.Value);

        //    await _connection.OpenAsync();
        //    int rowsAffected = await command.ExecuteNonQueryAsync();
        //    _connection.Close();

        //    return rowsAffected;
        //}


    }
}
