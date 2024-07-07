using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;
using static NDE_Digital_Market.Data_Access_Layer.Order_DAL;

namespace NDE_Digital_Market.Services.OrderService
{
    public class Order_Service : IOrder_Service
    {
        private readonly Order_DAL _order_DAL;
        public Order_Service(Order_DAL order_DAL)
        {
            _order_DAL = order_DAL;
        }


        public async Task<object> InsertOrderDateAsync(InsertOrderMasterDTO data)
        {
            OrderMasterModel Model = new OrderMasterModel();

            Model.OrderDate = data.OrderDate;
            Model.Address = data.Address;
            Model.UserId = int.Parse(CommonServices.DecryptPassword(data.UserId));
            Model.PaymentMethod = data.PaymentMethod;
            Model.NumberOfItem = data.NumberOfItem;
            Model.TotalPrice = data.TotalPrice;
            Model.PhoneNumber = data.PhoneNumber;
            Model.DeliveryCharge = data.DeliveryCharge;
            Model.AddedBy = data.AddedBy;
            Model.AddedPC = data.AddedPC;

            // Initialize the list if it's not already
            if (Model.OrderDetailsList == null)
            {
                Model.OrderDetailsList = new List<OrderDetailsModel>();
            }

            // Populate the list with the same number of elements as data.OrderDetailsList
            for (int i = 0; i < data.OrderDetailsList.Count; i++)
            {
                var orderDetails = new OrderDetailsModel();

                orderDetails.CompanyCode = data.OrderDetailsList[i].CompanyCode;
                //orderDetails.OrderMasterId = int.Parse(CommonServices.DecryptPassword(data.OrderDetailsList[i].OrderMasterId));
                // orderDetails.UserId = int.Parse(CommonServices.DecryptPassword(data.OrderDetailsList[i].UserId));
                orderDetails.ProductId = int.Parse(CommonServices.DecryptPassword(data.OrderDetailsList[i].ProductId));
                orderDetails.ProductGroupID = int.Parse(CommonServices.DecryptPassword(data.OrderDetailsList[i].ProductGroupID));
                orderDetails.Specification = data.OrderDetailsList[i].Specification;
                orderDetails.Qty = data.OrderDetailsList[i].Qty;
                orderDetails.UnitId = int.Parse(CommonServices.DecryptPassword(data.OrderDetailsList[i].UnitId));
                orderDetails.DiscountAmount = data.OrderDetailsList[i].DiscountAmount;
                orderDetails.Price = data.OrderDetailsList[i].Price;
                orderDetails.DeliveryCharge = data.OrderDetailsList[i].DeliveryCharge;
                orderDetails.DeliveryDate = data.OrderDetailsList[i].DeliveryDate;
                orderDetails.DiscountPct = data.OrderDetailsList[i].DiscountPct;
                orderDetails.NetPrice = data.OrderDetailsList[i].NetPrice;
                orderDetails.AddedBy = data.OrderDetailsList[i].AddedBy;
                orderDetails.AddedPC = data.OrderDetailsList[i].AddedPC;

                // Add the populated orderDetails to the Model's OrderDetailsList
                Model.OrderDetailsList.Add(orderDetails);
            }

            return await _order_DAL.InsertOrderDateAsync(Model);
        }





        public async Task<List<GetOrderMasterDataListByStatusDTO>> GetOrderMasterData(string? status)
        {

            DataTable dataTable = await _order_DAL.GetOrderMasterData( status);

            var list = new List<GetOrderMasterDataListByStatusDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new GetOrderMasterDataListByStatusDTO();

                obj.OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());
                obj.OrderNo = row["OrderNo"].ToString();
                obj.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                obj.Address = row["Address"].ToString();
                obj.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
                obj.PaymentMethod = row["PaymentMethod"].ToString();
                obj.NumberOfItem = Convert.ToInt32(row["NumberOfItem"]);
                obj.TotalPrice = Convert.ToInt32(row["TotalPrice"]);
                obj.PhoneNumber = row["PhoneNumber"].ToString();
                obj.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                obj.Status = row["Status"].ToString();
                list.Add(obj);

            }
            return list;
        }



        public async Task<List<GetOrderDetailsDataListByStatusDTO>> GetOrderDetailData(string? OrderMasterId, string? status)
        {

            int DecryptOrderMasterId = int.Parse(CommonServices.DecryptPassword(OrderMasterId));
            DataTable dataTable = await _order_DAL.GetOrderDetailData(DecryptOrderMasterId, status);

            var list = new List<GetOrderDetailsDataListByStatusDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new GetOrderDetailsDataListByStatusDTO();


                obj.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                obj.OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());
                obj.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
                obj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                //orderDetail.ProductGroupCode = reader.IsDBNull(reader.GetOrdinal("ProductGroupCode")) ? null : reader.GetString(reader.GetOrdinal("ProductGroupCode"));
                obj.FullName = row["FullName"].ToString();
                obj.ProductName = row["ProductName"].ToString();
                obj.Specification = row["Specification"].ToString();
                obj.Unit = row["Unit"].ToString();
                obj.Status = row["Status"].ToString();
                obj.Qty = Convert.ToInt32(row["Qty"]);
                obj.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                obj.DiscountAmount = Convert.ToDecimal(row["DiscountAmount"]);
                obj.Price = Convert.ToDecimal(row["Price"]);
                obj.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                obj.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"]);
                obj.DiscountPct = Convert.ToDecimal(row["DiscountPct"]);
                obj.NetPrice = Convert.ToDecimal(row["NetPrice"]);

                list.Add(obj);

            }
            return list;
        }



        //public async Task<DataTable> GetDatailsData(string OrderMasterId)
        //{

        //    int DecryptOrderMasterId = int.Parse(CommonServices.DecryptPassword(OrderMasterId));
        //    DataTable dataTable = await _order_DAL.GetDatailsData(DecryptOrderMasterId);

        //    var list = new List<GetOrderDetailsDataListByStatusDTO>();
        //    // Check if dataTable is null
        //    if (dataTable == null)
        //    {
        //        return null;
        //    }

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        var obj = new GetOrderDetailsDataListByStatusDTO();


        //        obj.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
        //        obj.OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());
        //        obj.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
        //        obj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
        //        //orderDetail.ProductGroupCode = reader.IsDBNull(reader.GetOrdinal("ProductGroupCode")) ? null : reader.GetString(reader.GetOrdinal("ProductGroupCode"));
        //        obj.FullName = row["FullName"].ToString();
        //        obj.ProductName = row["ProductName"].ToString();
        //        obj.Specification = row["Specification"].ToString();
        //        obj.Unit = row["Unit"].ToString();
        //        obj.Status = row["Status"].ToString();
        //        obj.Qty = Convert.ToInt32(row["Qty"]);
        //        obj.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
        //        obj.DiscountAmount = Convert.ToDecimal(row["DiscountAmount"]);
        //        obj.Price = Convert.ToDecimal(row["Price"]);
        //        obj.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
        //        obj.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"]);
        //        obj.DiscountPct = Convert.ToDecimal(row["DiscountPct"]);
        //        obj.NetPrice = Convert.ToDecimal(row["NetPrice"]);

        //        list.Add(obj);

        //    }
        //    return list;
        //}



        public async Task<object> UpdateOrderStatusAsync(string orderMasterId, string? detailsCancelledId, string status)
        {
            List<string> decryptedorderMasterIds = orderMasterId.Split(',').ToList();
            string DecryptOrderMasterIds = string.Empty;
            for (int i = 0; i < decryptedorderMasterIds.Count; i++)
            {
                DecryptOrderMasterIds = string.Join(",", CommonServices.DecryptPassword(decryptedorderMasterIds[i]));
            }

            string decryptdetailsCancelledId = string.Empty;
            if (detailsCancelledId is not null)
            {
                List<string> decryptedIds = detailsCancelledId.Split(',').ToList();
                for (int i = 0; i < decryptedIds.Count; i++)
                {
                    decryptdetailsCancelledId = string.Join(",", CommonServices.DecryptPassword(decryptedIds[i]));
                }
            }


            return await _order_DAL.UpdateOrderStatusAsync(DecryptOrderMasterIds, decryptdetailsCancelledId, status);

        }




        public async Task<object> SellerOrderDetailsStatusChangedAsync(updateOrderClass updateOrder)
        {
            return await _order_DAL.SellerOrderDetailsStatusChangedAsync(updateOrder);
        }


        //public async Task<object> SellerOrderDetailsStatusChangedAsync(UpdateOrderDTO updateOrder)
        //{

        //    SqlConnection con = new SqlConnection(_healthCareConnection);
        //    // Start a transaction
        //    SqlTransaction transaction = null;

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(updateOrder.orderdetailsIds))
        //        {
        //            string orderdetailsIdString = "''";

        //            List<int> DetailsIds = updateOrder.orderdetailsIds.Split(',').Select(int.Parse).ToList();
        //            orderdetailsIdString = string.Join(",", DetailsIds);

        //            for (int i = 0; i < DetailsIds.Count; i++)
        //            {

        //                await con.OpenAsync();
        //                SqlCommand cmdcheck = new SqlCommand("CheckAvailableQuantity", con);
        //                cmdcheck.CommandType = CommandType.StoredProcedure;
        //                //cmd1.Parameters.AddWithValue("@orderMasterId", orderMasterId);
        //                cmdcheck.Parameters.AddWithValue("@OrderDetailId", DetailsIds[i]);

        //                SqlDataReader reader = cmdcheck.ExecuteReader();
        //                bool result = false;
        //                if (reader.Read())
        //                {
        //                    result = Convert.ToBoolean(reader["Result"]);

        //                }
        //                await con.CloseAsync();
        //                if (!result)
        //                {
        //                    return (new { message = "You don't have enough quantity." });
        //                }

        //            }

        //            string masterStatusChangeQuery = "UPDATE OrderDetails SET Status = @value  WHERE OrderDetailId IN (" + orderdetailsIdString + ") ;";


        //            await con.OpenAsync();
        //            transaction = con.BeginTransaction();
        //            SqlCommand cmd1 = new SqlCommand(masterStatusChangeQuery, con, transaction);
        //            //cmd1.Parameters.AddWithValue("@orderMasterId", orderMasterId);
        //            cmd1.Parameters.AddWithValue("@value", updateOrder.status);

        //            int masteRES = await cmd1.ExecuteNonQueryAsync();
        //            if (masteRES > 0)
        //            {
        //                if (updateOrder.status == "Processing")
        //                {
        //                    var detailsResult = await InsertSellerSalesDataAsync(updateOrder.sellerSalesMasterModel, con, transaction);
        //                    if (detailsResult is BadRequestObjectResult)
        //                    {
        //                        throw new Exception((detailsResult as BadRequestObjectResult).Value.ToString());
        //                    }
        //                }

        //                // If everything is fine, commit the transaction
        //                transaction.Commit();
        //                return (new { message = "Order Status Changed Successfully." });
        //            }
        //            else
        //            {
        //                return (new { message = "Order Details not found." });
        //            }

        //        }
        //        else
        //        {
        //            return (new { message = "Send A Valid OrderDetail Id." });

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // If there is any error, rollback the transaction
        //        if (transaction != null)
        //        {
        //            transaction.Rollback();
        //        }
        //        return (new { message = ex.Message });
        //    }
        //    finally
        //    {
        //        // Finally block to ensure the connection is always closed
        //        if (con.State == ConnectionState.Open)
        //        {
        //            await con.CloseAsync();
        //        }
        //    }

        //}


        //private async Task<object> InsertSellerSalesDataAsync(SellerSalesMasterModel sellerSalesMasterDto, SqlConnection con, SqlTransaction transaction)
        //{


        //    try
        //    {
        //        string systemCode = string.Empty;

        //        // Execute the stored procedure to generate the system code
        //        SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con, transaction);
        //        {
        //            cmdSP.CommandType = CommandType.StoredProcedure;
        //            cmdSP.Parameters.AddWithValue("@TableName", "SellerSalesMaster");
        //            cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
        //            cmdSP.Parameters.AddWithValue("@AddNumber", 1);
        //            var tempSystem = await cmdSP.ExecuteScalarAsync();
        //            systemCode = tempSystem?.ToString() ?? string.Empty;
        //        }
        //        int SSMId = int.Parse(systemCode.Split('%')[0]);
        //        string SSMCode = systemCode.Split('%')[1];
        //        // SP END

        //        SqlCommand cmdMaster = new SqlCommand("InsertSellerSalesMaster", con, transaction);
        //        cmdMaster.CommandType = CommandType.StoredProcedure;

        //        cmdMaster.Parameters.AddWithValue("@SSMId", SSMId);
        //        cmdMaster.Parameters.AddWithValue("@SSMCode", SSMCode);
        //        cmdMaster.Parameters.AddWithValue("@SSMDate", DateTime.Now);
        //        cmdMaster.Parameters.AddWithValue("@UserId", sellerSalesMasterDto.UserId);
        //        cmdMaster.Parameters.AddWithValue("@TotalPrice", sellerSalesMasterDto.TotalPrice);
        //        cmdMaster.Parameters.AddWithValue("@Challan", sellerSalesMasterDto.Challan ?? (object)DBNull.Value);
        //        cmdMaster.Parameters.AddWithValue("@Remarks", sellerSalesMasterDto.Remarks ?? (object)DBNull.Value);
        //        cmdMaster.Parameters.AddWithValue("@BUserId", sellerSalesMasterDto.BUserId);
        //        cmdMaster.Parameters.AddWithValue("@AddedBy", sellerSalesMasterDto.AddedBy);
        //        cmdMaster.Parameters.AddWithValue("@DateAdded", DateTime.Now);
        //        cmdMaster.Parameters.AddWithValue("@AddedPC", sellerSalesMasterDto.AddedPC);

        //        int a = await cmdMaster.ExecuteNonQueryAsync();
        //        if (a > 0)
        //        {
        //            for (int i = 0; i < sellerSalesMasterDto.SellerSalesDetailsList.Count; i++)
        //            {
        //                string detailsQuery = "InsertSellerSalesDetail";
        //                //checking if user already exect for not.
        //                SqlCommand cmdDetails = new SqlCommand(detailsQuery, con, transaction);
        //                cmdDetails.CommandType = CommandType.StoredProcedure;

        //                cmdDetails.Parameters.Clear();

        //                cmdDetails.Parameters.AddWithValue("@SSMId", SSMId);
        //                cmdDetails.Parameters.AddWithValue("@OrderNo", sellerSalesMasterDto.SellerSalesDetailsList[i].OrderNo);
        //                cmdDetails.Parameters.AddWithValue("@ProductId", sellerSalesMasterDto.SellerSalesDetailsList[i].ProductId);
        //                cmdDetails.Parameters.AddWithValue("@Specification", sellerSalesMasterDto.SellerSalesDetailsList[i].Specification);
        //                cmdDetails.Parameters.AddWithValue("@StockQty", sellerSalesMasterDto.SellerSalesDetailsList[i].StockQty);
        //                cmdDetails.Parameters.AddWithValue("@SaleQty", sellerSalesMasterDto.SellerSalesDetailsList[i].SaleQty);
        //                cmdDetails.Parameters.AddWithValue("@UnitId", sellerSalesMasterDto.SellerSalesDetailsList[i].UnitId);
        //                cmdDetails.Parameters.AddWithValue("@NetPrice", sellerSalesMasterDto.SellerSalesDetailsList[i].NetPrice);
        //                cmdDetails.Parameters.AddWithValue("@Address", sellerSalesMasterDto.SellerSalesDetailsList[i].Address);
        //                cmdDetails.Parameters.AddWithValue("@ProductGroupID", sellerSalesMasterDto.SellerSalesDetailsList[i].ProductGroupID);
        //                cmdDetails.Parameters.AddWithValue("@Remarks", sellerSalesMasterDto.SellerSalesDetailsList[i].Remarks ?? (object)DBNull.Value);

        //                cmdDetails.Parameters.AddWithValue("@AddedBy", sellerSalesMasterDto.SellerSalesDetailsList[i].AddedBy);
        //                cmdDetails.Parameters.AddWithValue("@AddedDate", DateTime.Now);
        //                cmdDetails.Parameters.AddWithValue("@AddedPC", sellerSalesMasterDto.SellerSalesDetailsList[i].AddedPC);

        //                int detailsRes = await cmdDetails.ExecuteNonQueryAsync();
        //                if (detailsRes <= 0)
        //                {
        //                    return (new { message = "SellerSales details data isn't Inserted." });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return (new { message = "SellerSales Master data isn't Inserted Successfully." });
        //        }
        //        return (new { message = "SellerSale data Inserted Successfully." });

        //    }
        //    catch (Exception ex)
        //    {
        //        return (ex.Message);
        //    }
        //}




        public async Task<GetSingleUserInfoDTO> getUserInfo(string UserId)
        {
            int DecryptUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            DataTable dataTable = await _order_DAL.getUserInfo(DecryptUserId);

            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            var obj = new GetSingleUserInfoDTO();
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0]; // Assuming there's only one row of data

                

                obj.FullName = row["FullName"].ToString();
                obj.PhoneNumber = row["PhoneNumber"].ToString();
                obj.Email = row["Email"].ToString();
                obj.Address = row["Address"].ToString();
            }
            return obj;
        }



        public async Task<List<GetSellerOrderDataByUserIdAndStatusDTO>> GetSellerOrderBasedOnUserCodeAsync(string userid, string? status)
        {
            int Decryptuserid = int.Parse(CommonServices.DecryptPassword(userid));
            DataTable dataTable = await _order_DAL.GetSellerOrderBasedOnUserCodeAsync(Decryptuserid, status);

            var list = new List<GetSellerOrderDataByUserIdAndStatusDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var details = new GetSellerOrderDataByUserIdAndStatusDTO();


                details.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                details.OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());

                details.OrderNo = row["OrderNo"].ToString();
                details.Address = row["Address"].ToString();
                details.BUserId = CommonServices.EncryptPassword(row["BUserId"].ToString());
                details.BuyerName = row["BuyerName"].ToString();
                details.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                details.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                details.ProductName = row["ProductName"].ToString();
                details.Specification = row["Specification"].ToString();
                details.StockQty = Convert.ToDecimal(row["StockQty"]);
                details.SaleQty = Convert.ToInt32(row["SaleQty"]);
                details.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                details.Unit = row["Unit"].ToString();
                details.NetPrice = Convert.ToDecimal(row["NetPrice"]);
                details.Status = row["Status"].ToString();
                details.ReturnTypeName = row["ReturnTypeName"].ToString();

                list.Add(details);

            }
            return list;
        }



        public async Task<List<GetBuyerOrderDataByUserIdAndStatusDTO>> GetBuyerOrderBasedOnUserIDAsync(string userid, string? status)
        {
            int Decryptuserid = int.Parse(CommonServices.DecryptPassword(userid));
            DataTable dataTable = await _order_DAL.GetBuyerOrderBasedOnUserIDAsync(Decryptuserid, status);

            var list = new List<GetBuyerOrderDataByUserIdAndStatusDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var details = new GetBuyerOrderDataByUserIdAndStatusDTO();


                details.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                details.OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());

                details.OrderNo = row["OrderNo"].ToString();
                details.Address = row["Address"].ToString();

                details.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                details.BuyerName = row["BuyerName"].ToString();
                details.PaymentMethod = row["PaymentMethod"].ToString();
                details.NumberOfItem = Convert.ToInt32(row["NumberOfItem"]);
                details.TotalPrice = Convert.ToInt32(row["TotalPrice"]);
                details.PhoneNumber = row["PhoneNumber"].ToString();
                details.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                details.ProductName = row["ProductName"].ToString();
                details.Specification = row["Specification"].ToString();
                details.Qty = Convert.ToInt32(row["Qty"].ToString());
                details.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                details.Unit = row["Unit"].ToString();
                details.DiscountAmount = Convert.ToDecimal(row["DiscountAmount"]);
                details.Price = Convert.ToDecimal(row["Price"]);
                details.DetailDeliveryCharge = Convert.ToDecimal(row["DetailDeliveryCharge"]);
                details.DetailDeliveryDate = Convert.ToDateTime(row["DetailDeliveryDate"]);
                details.DiscountPct = Convert.ToDecimal(row["DiscountPct"]);
                details.NetPrice = Convert.ToDecimal(row["NetPrice"]);
                details.OrderStatus = row["OrderStatus"].ToString();
                details.SellerStatus = row["SellerStatus"].ToString();

                list.Add(details);

            }
            return list;
        }




        public async Task<List<GetOrderMasterDataForBuyerByUserIdDTO>> getAllOrderForBuyerAsync(string userid, string? status)
        {

            int Decryptuserid = int.Parse(CommonServices.DecryptPassword(userid));
            DataTable dataTable = await _order_DAL.getAllOrderForBuyerAsync(Decryptuserid, status);

            List<GetOrderMasterDataForBuyerByUserIdDTO> MasterList = new List<GetOrderMasterDataForBuyerByUserIdDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetOrderMasterDataForBuyerByUserIdDTO Master = null;


                string OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());

                if (Master == null || Master.OrderMasterId != OrderMasterId)
                {
                    Master = new GetOrderMasterDataForBuyerByUserIdDTO();
                    Master.OrderMasterId = OrderMasterId;
                    Master.OrderNo = row["OrderNo"].ToString();
                    Master.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                    Master.TotalPrice = Convert.ToDecimal(row["TotalPrice"]);

                    MasterList.Add(Master);

                    GetOrderDetailsDataForBuyerByUserIdDTO Detail = new GetOrderDetailsDataForBuyerByUserIdDTO();
                    {

                        Detail.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                        Detail.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                        Detail.ProductName = row["ProductName"].ToString();
                        Detail.ImagePath = row["ImagePath"].ToString();
                        Detail.Qty = Convert.ToInt32(row["Qty"]);
                        Detail.Price = Convert.ToDecimal(row["Price"]) * Detail.Qty;
                        Detail.Status = row["Status"].ToString();
                        Detail.CompanyCode = row["CompanyCode"].ToString();
                    }
                    Master.OrderDetailsListForBuyer.Add(Detail);

                }
                else
                {
                    GetOrderDetailsDataForBuyerByUserIdDTO Detail = new GetOrderDetailsDataForBuyerByUserIdDTO();
                    {

                        Detail.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                        Detail.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                        Detail.ProductName = row["ProductName"].ToString();
                        Detail.ImagePath = row["ImagePath"].ToString();
                        Detail.Qty = Convert.ToInt32(row["Qty"]);
                        Detail.Price = Convert.ToDecimal(row["Price"]) * Detail.Qty;
                        Detail.Status = row["Status"].ToString();
                    }
                    Master.OrderDetailsListForBuyer.Add(Detail);
                }

            }
            return MasterList;

        }




        public async Task<GetOrderFullDetailsForBuyerMasterDTO> getOrderDetailsForBuyerBasedOnOrderNoAsync(string OrderNo)
        {

            DataTable dataTable = await _order_DAL.getOrderDetailsForBuyerBasedOnOrderNoAsync(OrderNo);

            GetOrderFullDetailsForBuyerMasterDTO Master = null;
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            GetOrderFullDetailsForBuyer2ndMasterDTO SecondMaster = null;
            string? sellermaster = null;
            foreach (DataRow row in dataTable.Rows)
            {
                string OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());

                if (Master == null || Master.OrderMasterId != OrderMasterId)
                {
                    SecondMaster = new GetOrderFullDetailsForBuyer2ndMasterDTO();
                    Master = new GetOrderFullDetailsForBuyerMasterDTO();
                    Master.OrderMasterId = OrderMasterId;
                    Master.OrderNo = row["OrderNo"].ToString();
                    Master.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                    Master.PaymentMethod = row["PaymentMethod"].ToString();
                    Master.BuyerName = row["BuyerName"].ToString();
                    Master.ShippingAddress = row["ShippingAddress"].ToString();
                    Master.ShippingPhoneNumber = row["ShippingPhoneNumber"].ToString();
                    Master.BillingAddress = row["BillingAddress"].ToString();
                    Master.BillingPhoneNumber = row["BillingPhoneNumber"].ToString();
                    Master.TotalAmount = Convert.ToDecimal(row["TotalPrice"]);

                    sellermaster = CommonServices.EncryptPassword(row["SellerId"].ToString());
                    SecondMaster.SellerName = row["SellerName"].ToString();
                    SecondMaster.SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                    SecondMaster.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"]);
                    SecondMaster.Status = row["Status"].ToString();

                    GetOrderFullDetailsForBuyer2ndMDetailsDTO SecondMasterDetails = new GetOrderFullDetailsForBuyer2ndMDetailsDTO();

                    SecondMasterDetails.Imagepath = row["Imagepath"].ToString();
                    SecondMasterDetails.ProductName = row["ProductName"].ToString();
                    //SecondMasterDetails.Price = Convert.ToDecimal(row["Price"]);
                    SecondMasterDetails.Price = row["Price"] != DBNull.Value
                    ? Convert.ToDecimal(row["Price"])
                    : (decimal?)null;

                    SecondMasterDetails.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                    SecondMasterDetails.ProductTotalPrice = Convert.ToDecimal(row["ProductTotalPrice"]);
                    SecondMasterDetails.TotalQty = Convert.ToInt32(row["TotalQty"]);
                    SecondMasterDetails.ProductSubtotal = Convert.ToDecimal(row["ProductSubtotal"]);

                    SecondMaster.PackageDeliveryCharge += SecondMasterDetails.DeliveryCharge;
                    SecondMaster.PackageSubtotal += SecondMasterDetails.ProductTotalPrice;
                    Master.TotalDeliveryCharge += SecondMasterDetails.DeliveryCharge;
                    Master.SubTotal += SecondMasterDetails.ProductTotalPrice;

                    SecondMaster.OrderDetails2ndMDetailsListForBuyer.Add(SecondMasterDetails);
                    Master.OrderDetails2ndMasterListForBuyer.Add(SecondMaster);
                }
                else
                {
                    GetOrderFullDetailsForBuyer2ndMasterDTO SenMaster = new GetOrderFullDetailsForBuyer2ndMasterDTO();

                    string? SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                    if (SellerId == sellermaster)
                    {
                        GetOrderFullDetailsForBuyer2ndMDetailsDTO SecondMasterDetails = new GetOrderFullDetailsForBuyer2ndMDetailsDTO();

                        SecondMasterDetails.Imagepath = row["Imagepath"].ToString();
                        SecondMasterDetails.ProductName = row["ProductName"].ToString();
                        //SecondMasterDetails.Price = Convert.ToDecimal(row["Price"]);
                        SecondMasterDetails.Price = row["Price"] != DBNull.Value
                        ? Convert.ToDecimal(row["Price"])
                        : (decimal?)null;
                        SecondMasterDetails.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                        SecondMasterDetails.ProductTotalPrice = Convert.ToDecimal(row["ProductTotalPrice"]);
                        SecondMasterDetails.TotalQty = Convert.ToInt32(row["TotalQty"]);
                        SecondMasterDetails.ProductSubtotal = Convert.ToDecimal(row["ProductSubtotal"]);

                        SecondMaster.PackageDeliveryCharge += SecondMasterDetails.DeliveryCharge;
                        SecondMaster.PackageSubtotal += SecondMasterDetails.ProductTotalPrice;
                        Master.TotalDeliveryCharge += SecondMasterDetails.DeliveryCharge;
                        Master.SubTotal += SecondMasterDetails.ProductTotalPrice;

                        SecondMaster.OrderDetails2ndMDetailsListForBuyer.Add(SecondMasterDetails);
                    }
                    else
                    {
                        SenMaster.SellerName = row["SellerName"].ToString();
                        SenMaster.SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                        SenMaster.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"]);
                        SenMaster.Status = row["Status"].ToString();

                        GetOrderFullDetailsForBuyer2ndMDetailsDTO SecondMasterDetails = new GetOrderFullDetailsForBuyer2ndMDetailsDTO();

                        SecondMasterDetails.Imagepath = row["Imagepath"].ToString();
                        SecondMasterDetails.ProductName = row["ProductName"].ToString();
                        //SecondMasterDetails.Price = Convert.ToDecimal(row["Price"]);
                        SecondMasterDetails.Price = row["Price"] != DBNull.Value
                        ? Convert.ToDecimal(row["Price"])
                        : (decimal?)null;
                        SecondMasterDetails.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                        SecondMasterDetails.ProductTotalPrice = Convert.ToDecimal(row["ProductTotalPrice"]);
                        SecondMasterDetails.TotalQty = Convert.ToInt32(row["TotalQty"]);
                        SecondMasterDetails.ProductSubtotal = Convert.ToDecimal(row["ProductSubtotal"]);

                        SenMaster.PackageDeliveryCharge += SecondMasterDetails.DeliveryCharge;
                        SenMaster.PackageSubtotal += SecondMasterDetails.ProductTotalPrice;
                        Master.TotalDeliveryCharge += SecondMasterDetails.DeliveryCharge;
                        Master.SubTotal += SecondMasterDetails.ProductTotalPrice;

                        SenMaster.OrderDetails2ndMDetailsListForBuyer.Add(SecondMasterDetails);
                        Master.OrderDetails2ndMasterListForBuyer.Add(SenMaster);
                    }
                }


            }
            return Master;
        }



        public async Task<List<GetOrderMasterDataForSellerByCompanyCodeDTO>> getAllOrderForSellerAsync(string CompanyCode, string? status)
        {

            //string DecryptCompanyCode = CommonServices.DecryptPassword(CompanyCode);
            DataTable dataTable = await _order_DAL.getAllOrderForSellerAsync(CompanyCode, status);

            List<GetOrderMasterDataForSellerByCompanyCodeDTO> MasterList = new List<GetOrderMasterDataForSellerByCompanyCodeDTO>();

            // Check if dataTable is null
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            GetOrderMasterDataForSellerByCompanyCodeDTO Master = null;
            foreach (DataRow row in dataTable.Rows)
            {
                string OrderMasterId = CommonServices.EncryptPassword(row["OrderMasterId"].ToString());

                if (Master == null || Master.OrderMasterId != OrderMasterId)
                {
                    if (Master != null && Master.TotalPrice != null && Master.TotalDeliveryCharge != null)
                    {
                        Master.TotalAmount += (Master.TotalPrice ?? 0) + (Master.TotalDeliveryCharge ?? 0);
                    }



                    Master = new GetOrderMasterDataForSellerByCompanyCodeDTO();
                    Master.OrderMasterId = OrderMasterId;
                    Master.OrderNo = row["OrderNo"].ToString();
                    Master.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                    Master.SellerUserId = CommonServices.EncryptPassword(row["SellerUserId"].ToString());
                    Master.BuyerUserId = CommonServices.EncryptPassword(row["BuyerUserId"].ToString());
                    Master.BuyerAddress = row["Address"].ToString();


                    MasterList.Add(Master);

                    GetOrderDetailsDataForSellerByCompanyCodeDTO Detail = new GetOrderDetailsDataForSellerByCompanyCodeDTO();
                    {

                        Detail.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                        Detail.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                        Detail.ProductName = row["ProductName"].ToString();
                        Detail.Specification = row["Specification"].ToString();
                        Detail.ImagePath = row["ImagePath"].ToString();
                        Detail.Qty = Convert.ToInt32(row["Qty"]);
                        Detail.Price = Convert.ToDecimal(row["Price"]);
                        Detail.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                        Detail.StockQty = Convert.ToDecimal(row["StockQty"]);
                        Detail.SaleQty = Convert.ToInt32(row["SaleQty"]);
                        Detail.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                        Detail.NetPrice = Convert.ToDecimal(row["NetPrice"]);
                        Detail.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());


                        Detail.ReturnTypeName = row["ReturnTypeName"].ToString();
                        Detail.Status = row["Status"].ToString();

                        Master.TotalPrice += Detail.Price * Detail.Qty;

                        Master.TotalDeliveryCharge += Convert.ToDecimal(row["DeliveryCharge"]);

                    }
                    Master.OrderDetailsListForSeller.Add(Detail);

                }
                else
                {
                    GetOrderDetailsDataForSellerByCompanyCodeDTO Detail = new GetOrderDetailsDataForSellerByCompanyCodeDTO();
                    {

                        Detail.OrderDetailId = CommonServices.EncryptPassword(row["OrderDetailId"].ToString());
                        Detail.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                        Detail.ProductName = row["ProductName"].ToString();
                        Detail.ImagePath = row["ImagePath"].ToString();
                        Detail.Qty = Convert.ToInt32(row["Qty"]);
                        Detail.Specification = row["Specification"].ToString();
                        Detail.NetPrice = Convert.ToDecimal(row["NetPrice"]);
                        Detail.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                        Detail.SaleQty = Convert.ToInt32(row["SaleQty"]);
                        Detail.StockQty = Convert.ToDecimal(row["StockQty"]);
                        Detail.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());





                        //Price = reader["Price"] is DBNull ? (decimal?)null : Convert.ToDecimal(reader["Price"]),
                        Detail.Price = Convert.ToDecimal(row["Price"]);
                        Detail.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);

                        Detail.ReturnTypeName = row["ReturnTypeName"].ToString();

                        Detail.Status = row["Status"].ToString();

                        Master.TotalPrice += Detail.Price * Detail.Qty;

                        Master.TotalDeliveryCharge += Convert.ToDecimal(row["DeliveryCharge"]);
                        // Master.TotalAmount += Master.TotalPrice + Detail.DeliveryCharge;
                    }
                    Master.OrderDetailsListForSeller.Add(Detail);
                }

            }
            return MasterList;

        }

    }
}
