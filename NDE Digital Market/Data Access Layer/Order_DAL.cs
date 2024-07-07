using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class Order_DAL
    {

        private readonly string _healthCareConnection;
        public Order_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }


        public async Task<object> InsertOrderDateAsync(OrderMasterModel orderdata)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            SqlTransaction transaction = null;

            try
            {
                string systemCode = string.Empty;
                await con.OpenAsync();
                transaction = con.BeginTransaction();
                SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con, transaction);
                {
                    cmdSP.CommandType = CommandType.StoredProcedure;
                    cmdSP.Parameters.AddWithValue("@TableName", "OrderMaster");
                    cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSP.Parameters.AddWithValue("@AddNumber", 1);
                    var tempSystem = await cmdSP.ExecuteScalarAsync();
                    systemCode = tempSystem?.ToString() ?? string.Empty;
                }
                int OrderMasterId = int.Parse(systemCode.Split('%')[0]);
                string OrderNo = systemCode.Split('%')[1];

                SqlCommand cmd = new SqlCommand("InsertOrderMaster", con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderMasterId", OrderMasterId);
                cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Address", orderdata.Address);
                cmd.Parameters.AddWithValue("@UserId", orderdata.UserId);
                cmd.Parameters.AddWithValue("@PaymentMethod", orderdata.PaymentMethod ?? String.Empty);
                cmd.Parameters.AddWithValue("@NumberOfItem", orderdata.NumberOfItem);
                cmd.Parameters.AddWithValue("@TotalPrice", orderdata.TotalPrice);
                cmd.Parameters.AddWithValue("@PhoneNumber", orderdata.PhoneNumber);
                cmd.Parameters.AddWithValue("@DeliveryCharge", orderdata.DeliveryCharge);
                cmd.Parameters.AddWithValue("@Status", "Pending");

                cmd.Parameters.AddWithValue("@AddedBy", orderdata.AddedBy);
                cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@AddedPC", orderdata.AddedPC);

                int a = await cmd.ExecuteNonQueryAsync();
                if (a > 0)
                {
                    bool detailsResult = await InsertOrderDateDetailsAsync(OrderMasterId, orderdata.OrderDetailsList, transaction, con);
                    if (detailsResult is false)
                    {
                        throw new Exception("Order Details data isn't Inserted Successfully");
                    }

                }

                else
                {
                    return (new { message = "Order Master data isn't Inserted Successfully." });
                }


                for (int i = 0; i < orderdata.OrderDetailsList.Count; i++)
                {
                    string query = "DELETE FROM AddToCart WHERE CompanyCode = @CompanyCode AND ProductID = @ProductId AND BuyerUserID = @UserId;";
                    SqlCommand deleteAddTocart = new SqlCommand(query, con, transaction);
                    deleteAddTocart.CommandType = CommandType.Text;
                    deleteAddTocart.Parameters.Clear();

                    deleteAddTocart.Parameters.AddWithValue("@CompanyCode", orderdata.OrderDetailsList[i].CompanyCode);
                    deleteAddTocart.Parameters.AddWithValue("@UserId", orderdata.UserId);
                    deleteAddTocart.Parameters.AddWithValue("@ProductId", orderdata.OrderDetailsList[i].ProductId);

                    await deleteAddTocart.ExecuteNonQueryAsync();
                }



                transaction.Commit();
                return (new { message = "Order data Inserted Successfully." });
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return (new { message = ex.Message });
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
        }


        private async Task<bool> InsertOrderDateDetailsAsync(int OrderMasterId, List<OrderDetailsModel> OrderDetailsList, SqlTransaction transaction, SqlConnection con)
        {
            try
            {
                for (int i = 0; i < OrderDetailsList.Count; i++)
                {
                    string query = "InsertOrderDetails";
                    SqlCommand CheckCMD = new SqlCommand(query, con, transaction);
                    CheckCMD.CommandType = CommandType.StoredProcedure;

                    CheckCMD.Parameters.Clear();
                    CheckCMD.Parameters.AddWithValue("@OrderMasterId", OrderMasterId);
                    CheckCMD.Parameters.AddWithValue("@CompanyCode", OrderDetailsList[i].CompanyCode);
                    CheckCMD.Parameters.AddWithValue("@UserId", 0);
                    CheckCMD.Parameters.AddWithValue("@ProductId", OrderDetailsList[i].ProductId);
                    CheckCMD.Parameters.AddWithValue("@ProductGroupID", OrderDetailsList[i].ProductGroupID);
                    CheckCMD.Parameters.AddWithValue("@Specification", OrderDetailsList[i].Specification);
                    CheckCMD.Parameters.AddWithValue("@Qty", OrderDetailsList[i].Qty);
                    CheckCMD.Parameters.AddWithValue("@UnitId", OrderDetailsList[i].UnitId);
                    CheckCMD.Parameters.AddWithValue("@DiscountAmount", OrderDetailsList[i].DiscountAmount != null ? (object)OrderDetailsList[i].DiscountPct : DBNull.Value);
                    CheckCMD.Parameters.AddWithValue("@Price", OrderDetailsList[i].Price);
                    CheckCMD.Parameters.AddWithValue("@Status", "Pending");
                    CheckCMD.Parameters.AddWithValue("@DeliveryCharge", OrderDetailsList[i].DeliveryCharge);
                    CheckCMD.Parameters.AddWithValue("@DeliveryDate", OrderDetailsList[i].DeliveryDate);
                    CheckCMD.Parameters.AddWithValue("@DiscountPct", OrderDetailsList[i].DiscountPct != null ? (object)OrderDetailsList[i].DiscountPct : DBNull.Value);
                    CheckCMD.Parameters.AddWithValue("@NetPrice", OrderDetailsList[i].NetPrice);

                    CheckCMD.Parameters.AddWithValue("@AddedBy", OrderDetailsList[i].AddedBy);
                    CheckCMD.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    CheckCMD.Parameters.AddWithValue("@AddedPC", OrderDetailsList[i].AddedPC);


                    await CheckCMD.ExecuteNonQueryAsync();

                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        
        public async Task<DataTable> GetOrderMasterData(string? status)
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = "GetOrderMasterByStatus";
                if (status == "Cancelled")
                {
                    query = "GetOrderMasterByCancelled";
                }
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (status != null && status != "Cancelled")
                        {
                            cmd.Parameters.Add(new SqlParameter("@Status", status));
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



        public async Task<DataTable> GetOrderDetailData(int? OrderMasterId, string? status = null)
        {

            try
            {

                DataTable dataTable = new DataTable();
                string query = "GetOrderDetailStatus";
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (status != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@Status", status));
                        }
                        cmd.Parameters.Add(new SqlParameter("@OrderMasterId", OrderMasterId));

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



        public async Task<DataTable> GetDatailsData( int OrderMasterId)
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT [OrderMasterId],[OrderDetailId],[SellerCode] ,[GoodsId],[GoodsName],[GroupCode],[Specification],[Quantity],[Discount],[Price],[Status]
                            ,[DeliveryCharge],[DeliveryDate], UR.[FullName] AS SellerName,  UR.[CompanyName] AS SellerCompanyName,UR.Address AS SellerAddress,UR.[PhoneNumber] as SellerPhone FROM  OrderDetails
                            LEFT JOIN UserRegistration UR ON  SellerCode = UR.[UserCode] where OrderMasterId = @OrderMasterId ";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@OrderMasterId", OrderMasterId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    await con.CloseAsync();
                }
                return dataTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }



        public async Task<object> UpdateOrderStatusAsync(string orderMasterId, string? detailsCancelledId, string status)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            SqlTransaction transaction = null;
            try
            {
                
                
                await con.OpenAsync();
                transaction = con.BeginTransaction();



                if (!string.IsNullOrEmpty(orderMasterId))
                {
                    string MasterIdString = "''";

                    List<int> MasterIds = orderMasterId.Split(',').Select(int.Parse).ToList();
                    MasterIdString = string.Join(",", MasterIds);
                    string masterStatusChangeQuery = "UPDATE OrderMaster SET Status = @value  WHERE OrderMasterId IN (" + MasterIdString + ") ;";

                    SqlCommand cmd1 = new SqlCommand(masterStatusChangeQuery, con, transaction);
                    cmd1.Parameters.AddWithValue("@value", status);

                    int masteRES = await cmd1.ExecuteNonQueryAsync();
                    if (masteRES > 0)
                    {
                        string detailsStatusChangeQuery = "UPDATE OrderDetails SET Status = @value WHERE OrderMasterId  IN (" + MasterIdString + "); ";
                        SqlCommand cmd2 = new SqlCommand(detailsStatusChangeQuery, con, transaction);
                        cmd2.Parameters.AddWithValue("@value", status);

                        int DetailRES = await cmd2.ExecuteNonQueryAsync();
                        if (masteRES > 0)
                        {

                        }
                        else
                        {
                            if (transaction != null)
                            {
                                transaction.Rollback();
                            }
                            return (new { message = "Order Details Status is not Changed." });
                        }
                    }
                    else
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        return (new { message = "Order Master Status is not Changed." });
                    }

                }
                else
                {
                    return (new { message = "Send A Valid Order Id." });

                }



                string CancelledString = "''";
                string detailsStatus = "Cancelled";
                if (!string.IsNullOrEmpty(detailsCancelledId))
                {
                    List<int> CanncelledIds = detailsCancelledId.Split(',').Select(int.Parse).ToList();
                    CancelledString = string.Join(",", CanncelledIds);
                    string detailStatusChangeToCancelQuery = "UPDATE OrderDetails SET Status = 'Rejected' WHERE OrderDetailId IN (" + CancelledString + "); ";
                    SqlCommand cmd3 = new SqlCommand(detailStatusChangeToCancelQuery, con, transaction);
                    int cancelRES = await cmd3.ExecuteNonQueryAsync();
                    if (cancelRES > 0)
                    {

                    }
                    else
                    {
                        // If there is any error, rollback the transaction
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        return (new { message = "Order Details Status cancel is not Inserted." });
                    }
                }
                else
                {

                }

                // If everything is fine, commit the transaction
                transaction.Commit();
                return (new { message = "Order Status Changed Successfully." });
            }
            catch (Exception ex)
            {
                // If there is any error, rollback the transaction
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return (new { message = ex.Message });
            }
            finally
            {
                // Finally block to ensure the connection is always closed
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }

        }




        public class updateOrderClass
        {
            public string? orderdetailsIds { get; set; }
            public string? status { get; set; }
            public InsertSellerSalesMasterDTO? sellerSalesMasterModel { get; set; }
        }


        public async Task<object> SellerOrderDetailsStatusChangedAsync(updateOrderClass updateOrder)
        {

            // Start a transaction
            SqlTransaction transaction = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    if (!string.IsNullOrEmpty(updateOrder.orderdetailsIds))
                    {
                        string orderdetailsIdString = "''";

                        List<int> DetailsIds = updateOrder.orderdetailsIds
                                                                        .Split(',')
                                                                        .Select(id => int.Parse(CommonServices.DecryptPassword(id)))
                                                                        .ToList();

                        orderdetailsIdString = string.Join(",", DetailsIds);

                        for (int i = 0; i < DetailsIds.Count; i++)
                        {

                            await con.OpenAsync();
                            SqlCommand cmdcheck = new SqlCommand("CheckAvailableQuantity", con);
                            cmdcheck.CommandType = CommandType.StoredProcedure;
                            //cmd1.Parameters.AddWithValue("@orderMasterId", orderMasterId);
                            cmdcheck.Parameters.AddWithValue("@OrderDetailId", DetailsIds[i]);

                            SqlDataReader reader = cmdcheck.ExecuteReader();
                            bool result = false;
                            if (reader.Read())
                            {
                                result = Convert.ToBoolean(reader["Result"]);

                            }
                            await con.CloseAsync();
                            if (!result)
                            {
                                return (new { message = "You don't have enough quantity." });
                            }

                        }

                        string masterStatusChangeQuery = "UPDATE OrderDetails SET Status = @value  WHERE OrderDetailId IN (" + orderdetailsIdString + ") ;";


                        await con.OpenAsync();
                        transaction = con.BeginTransaction();
                        SqlCommand cmd1 = new SqlCommand(masterStatusChangeQuery, con, transaction);
                        //cmd1.Parameters.AddWithValue("@orderMasterId", orderMasterId);
                        cmd1.Parameters.AddWithValue("@value", updateOrder.status);

                        int masteRES = await cmd1.ExecuteNonQueryAsync();
                        if (masteRES > 0)
                        {
                            if (updateOrder.status == "Processing")
                            {
                                bool detailsResult = await InsertSellerSalesDataAsync(updateOrder.sellerSalesMasterModel, con, transaction);
                                if (detailsResult is false)
                                {
                                    return (new { message = "SellerSales data Insertion Unsuccessfully." });
                                }
                            }

                            // If everything is fine, commit the transaction
                            transaction.Commit();
                            return (new { message = "Order Status Changed Successfully." });
                        }
                        else
                        {
                            return (new { message = "Order Details not found." });
                        }

                    }
                    else
                    {
                        return (new { message = "Send A Valid OrderDetail Id." });

                    }
                }

            }
            catch (Exception ex)
            {
                // If there is any error, rollback the transaction
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return (new { message = ex.Message });
            }

        }

        private async Task<bool> InsertSellerSalesDataAsync(InsertSellerSalesMasterDTO sellerSalesMasterDto, SqlConnection con, SqlTransaction transaction)
        {


            try
            {
                string systemCode = string.Empty;

                // Execute the stored procedure to generate the system code
                SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con, transaction);
                {
                    cmdSP.CommandType = CommandType.StoredProcedure;
                    cmdSP.Parameters.AddWithValue("@TableName", "SellerSalesMaster");
                    cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdSP.Parameters.AddWithValue("@AddNumber", 1);
                    var tempSystem = await cmdSP.ExecuteScalarAsync();
                    systemCode = tempSystem?.ToString() ?? string.Empty;
                }
                int SSMId = int.Parse(systemCode.Split('%')[0]);
                string SSMCode = systemCode.Split('%')[1];
                // SP END

                SqlCommand cmdMaster = new SqlCommand("InsertSellerSalesMaster", con, transaction);
                cmdMaster.CommandType = CommandType.StoredProcedure;

                cmdMaster.Parameters.AddWithValue("@SSMId", SSMId);
                cmdMaster.Parameters.AddWithValue("@SSMCode", SSMCode);
                cmdMaster.Parameters.AddWithValue("@SSMDate", DateTime.Now);
                cmdMaster.Parameters.AddWithValue("@UserId", int.Parse(CommonServices.DecryptPassword(sellerSalesMasterDto.UserId)));
                cmdMaster.Parameters.AddWithValue("@TotalPrice", sellerSalesMasterDto.TotalPrice);
                cmdMaster.Parameters.AddWithValue("@Challan", sellerSalesMasterDto.Challan ?? (object)DBNull.Value);
                cmdMaster.Parameters.AddWithValue("@Remarks", sellerSalesMasterDto.Remarks ?? (object)DBNull.Value);
                cmdMaster.Parameters.AddWithValue("@BUserId", int.Parse(CommonServices.DecryptPassword(sellerSalesMasterDto.BUserId)));
                cmdMaster.Parameters.AddWithValue("@AddedBy", sellerSalesMasterDto.AddedBy);
                cmdMaster.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                cmdMaster.Parameters.AddWithValue("@AddedPC", sellerSalesMasterDto.AddedPC);

                int a = await cmdMaster.ExecuteNonQueryAsync();
                if (a > 0)
                {
                    for (int i = 0; i < sellerSalesMasterDto.SellerSalesDetailsList.Count; i++)
                    {
                        string detailsQuery = "InsertSellerSalesDetail";
                        //checking if user already exect for not.
                        SqlCommand cmdDetails = new SqlCommand(detailsQuery, con, transaction);
                        cmdDetails.CommandType = CommandType.StoredProcedure;

                        cmdDetails.Parameters.Clear();

                        cmdDetails.Parameters.AddWithValue("@SSMId", SSMId);
                        cmdDetails.Parameters.AddWithValue("@OrderNo", sellerSalesMasterDto.SellerSalesDetailsList[i].OrderNo);
                        cmdDetails.Parameters.AddWithValue("@ProductId", int.Parse(CommonServices.DecryptPassword(sellerSalesMasterDto.SellerSalesDetailsList[i].ProductId)));
                        cmdDetails.Parameters.AddWithValue("@Specification", sellerSalesMasterDto.SellerSalesDetailsList[i].Specification);
                        cmdDetails.Parameters.AddWithValue("@StockQty", sellerSalesMasterDto.SellerSalesDetailsList[i].StockQty);
                        cmdDetails.Parameters.AddWithValue("@SaleQty", sellerSalesMasterDto.SellerSalesDetailsList[i].SaleQty);
                        cmdDetails.Parameters.AddWithValue("@UnitId", int.Parse(CommonServices.DecryptPassword(sellerSalesMasterDto.SellerSalesDetailsList[i].UnitId)));
                        cmdDetails.Parameters.AddWithValue("@NetPrice", sellerSalesMasterDto.SellerSalesDetailsList[i].NetPrice);
                        cmdDetails.Parameters.AddWithValue("@Address", sellerSalesMasterDto.SellerSalesDetailsList[i].Address);
                        cmdDetails.Parameters.AddWithValue("@ProductGroupID", int.Parse(CommonServices.DecryptPassword(sellerSalesMasterDto.SellerSalesDetailsList[i].ProductGroupID)));
                        cmdDetails.Parameters.AddWithValue("@Remarks", sellerSalesMasterDto.SellerSalesDetailsList[i].Remarks ?? (object)DBNull.Value);

                        cmdDetails.Parameters.AddWithValue("@AddedBy", sellerSalesMasterDto.SellerSalesDetailsList[i].AddedBy);
                        cmdDetails.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                        cmdDetails.Parameters.AddWithValue("@AddedPC", sellerSalesMasterDto.SellerSalesDetailsList[i].AddedPC);

                        int detailsRes = await cmdDetails.ExecuteNonQueryAsync();
                        if (detailsRes <= 0)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }







        public async Task<DataTable> getUserInfo(int UserId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT * FROM UserRegistration WHERE UserId = @UserId ";
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
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
            catch(Exception ex)
            {
                return null;
            }
        }



        public async Task<DataTable> GetSellerOrderBasedOnUserCodeAsync(int userid, string? status)
        {
            try
            {

                DataTable dataTable = new DataTable();
                string query = "GetSellerSelesBySellerId";
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SellerId", userid);
                        if (status != null)
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
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



        public async Task<DataTable> GetBuyerOrderBasedOnUserIDAsync(int userid, string? status)
        {
            try
            {

                DataTable dataTable = new DataTable();
                string query = "GetBuyerOrderByUserId";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userid);
                        if (status != null)
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
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




        public async Task<DataTable> getAllOrderForBuyerAsync(int userid, string? status)
        {

            try
            {

                DataTable dataTable = new DataTable();
                string query = "";
                if (status != null)
                {
                    query = @"select OM.OrderMasterId, OM.OrderNo, OM.OrderDate, OM.TotalPrice, OD.OrderDetailId, OD.ProductId, PL.ProductName, OD.CompanyCode,
                                  SPPO.ImagePath, OD.Qty, OD.Price, OD.Status  from OrderMaster OM
                                  join OrderDetails OD on OM.OrderMasterId = OD.OrderMasterId
                                  join ProductList PL on PL.ProductId = OD.ProductId
                                  join SellerProductPriceAndOffer  SPPO on OD.ProductId = SPPO.ProductId and OD.CompanyCode = SPPO.CompanyCode
                                  where OM.UserId = @UserId and OD.Status = @Status ORDER BY OM.OrderMasterId DESC;";
                }
                else
                {
                    query = @"select OM.OrderMasterId, OM.OrderNo, OM.OrderDate, OM.TotalPrice, OD.OrderDetailId, OD.ProductId, PL.ProductName, OD.CompanyCode,
                                  SPPO.ImagePath, OD.Qty, OD.Price, OD.Status  from OrderMaster OM
                                  join OrderDetails OD on OM.OrderMasterId = OD.OrderMasterId
                                  join ProductList PL on PL.ProductId = OD.ProductId
                                  join SellerProductPriceAndOffer  SPPO on OD.ProductId = SPPO.ProductId and OD.CompanyCode = SPPO.CompanyCode
                                  where OM.UserId = @UserId ORDER BY OM.OrderMasterId DESC;";
                }
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (status != null)
                        {
                            cmd.Parameters.AddWithValue("@UserId", userid);
                            cmd.Parameters.AddWithValue("@Status", status);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@UserId", userid);
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
                // Handle the exception
                return null;
            }

        }




        public async Task<DataTable> getOrderDetailsForBuyerBasedOnOrderNoAsync(string OrderNo)
        {

            try
            {

                DataTable dataTable = new DataTable();
                string query = @"SELECT OM.OrderMasterId, OM.OrderNo, OM.OrderDate, OD.UserId AS SellerID, UR.FullName AS SellerName, OD.DeliveryDate, OD.ProductId, PL.ProductName, PL.ImagePath, OD.Status, 
                                SPP.TotalPrice AS Price, OD.Qty As TotalQty, OD.DeliveryCharge, OD.NetPrice As ProductSubtotal, SUM(OD.NetPrice - OD.DeliveryCharge) as ProductTotalPrice, OM.PaymentMethod, 
                                UR2.FullName BuyerName, OM.Address As ShippingAddress, OM.PhoneNumber As ShippingPhoneNumber, UR2.Address As BillingAddress, UR2.PhoneNumber As BillingPhoneNumber, OM.TotalPrice 
                                FROM OrderMaster OM 
                                LEFT JOIN OrderDetails OD ON OD.OrderMasterId = OM.OrderMasterId 
                                LEFT JOIN UserRegistration UR ON UR.UserId = OD.UserId
                                LEFT JOIN ProductList PL ON PL.ProductId = OD.ProductId 
                                LEFT JOIN SellerProductPriceAndOffer SPP ON SPP.ProductId = OD.ProductId AND SPP.CompanyCode = OD.CompanyCode
                                LEFT JOIN UserRegistration UR2 ON UR2.UserId = OM.UserId 
                                WHERE OM.OrderNo = @OrderNo 
                                GROUP BY 
                                OM.OrderMasterId, OM.OrderNo, OM.OrderDate, OD.UserId, UR.FullName, OD.DeliveryDate, OD.ProductId, PL.ProductName, PL.ImagePath, OD.Status, SPP.TotalPrice, 
                                OD.Qty, OD.DeliveryCharge, OD.NetPrice, OM.Address, OM.PhoneNumber, OM.PaymentMethod, UR2.FullName, UR2.Address, UR2.PhoneNumber, OM.TotalPrice";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@OrderNo", OrderNo);

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



        public async Task<DataTable> getAllOrderForSellerAsync(string CompanyCode, string? status)
        {


            try
            {

                DataTable dataTable = new DataTable();
                string query = @"GetALLOrderForSellerBySellerId";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (status != null)
                        {
                            cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);
                            cmd.Parameters.AddWithValue("@Status", status);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);
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
                // Handle the exception
                return null;
            }

        }


    }
}
