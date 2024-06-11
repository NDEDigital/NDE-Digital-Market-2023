using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Services.OrderService;
using NDE_Digital_Market.Model.DTO;
using static NDE_Digital_Market.Data_Access_Layer.Order_DAL;

namespace NDE_Digital_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrder_Service _order_Service;
        public OrderController(IOrder_Service order_Service)
        {
            _order_Service = order_Service;
        }

    

        [HttpPost("InsertOrderData")]
        public async Task<IActionResult> InsertOrderDateAsync(InsertOrderMasterDTO orderdata)
        {

            try
            {
                if (orderdata == null)
                {
                    return BadRequest(new { message = "Give Proper Data." });
                }
                return Ok(await _order_Service.InsertOrderDateAsync(orderdata));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        //admin order getdata

        [HttpGet("GetOrderMasterData")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetOrderMasterData(string? status)
        {

            try
            {
                object res = await _order_Service.GetOrderMasterData(status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



        [HttpGet("GetOrderDetailData")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetOrderDetailData(string? OrderMasterId, string? status = null)
        {
            try
            {
                object res = await _order_Service.GetOrderDetailData(OrderMasterId,status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



        //[HttpPost("GetDatailsData")]
        //[Authorize(Roles = "admin")]
        //public IActionResult GetDatailsData([FromForm] string OrderMasterId)
        //{
        //    try
        //    {
        //        object res = await _order_Service.GetDatailsData(OrderMasterId);
        //        if (res == null)
        //        {
        //            return NotFound(new { message = "No Data Found." });
        //        }
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Server Error. Try Again!!!" });
        //    }
        //}



        [HttpPut("AdminOrderUpdateStatus")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateOrderStatusAsync(string orderMasterId, string? detailsCancelledId, string status)
        {
            try
            {
                if (orderMasterId == null || status == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                object res = await _order_Service.UpdateOrderStatusAsync(orderMasterId, detailsCancelledId, status);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }



        ////Under Construction tushar


        [HttpPut("UpdateSellerOrderDetailsStatus")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> SellerOrderDetailsStatusChangedAsync(updateOrderClass updateOrder)
        {

            try
            {
                if (updateOrder == null || updateOrder.orderdetailsIds == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                object res = await _order_Service.SellerOrderDetailsStatusChangedAsync(updateOrder);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }

        }







        //    [HttpPost]
        //    [Authorize(Roles = "admin")]
        //    [Route("getReturnDataForAdmin/{pageNumber}/{pageSize}")]

        //    public IActionResult getReturnDataForAdmin([FromForm] string status, int pageNumber, int pageSize, [FromForm] string searchby, [FromForm] string searchValue, [FromForm] string? fromDate = null, [FromForm] string? toDate = null)
        //    {
        //        int PendingCount = 0, ApprovedCount = 0, DeliveredCount = 0, ReturnedCount = 0, CancelledCount = 0, TotalRowCount = 0, ToReturnCount = 0;
        //        List<ProductReturnModel> returnData = new List<ProductReturnModel>();
        //        using SqlConnection con = new SqlConnection(_prominentConnection);
        //        con.Open();
        //        string condition = "FROM  [ProductReturn] r  LEFT JOIN  [ReturnType] t ON r.[TypeId] = t.[TypeId]" +
        //                     " JOIN  OrderDetails od ON r.[DetailsId] = od.[OrderDetailId] AND od.[Status] = @status";


        //        if (searchValue != "All")
        //        {
        //            condition += " AND ";

        //            if (searchby == "OrderNo")
        //            {
        //                condition += " r.[OrderNo] LIKE @searchValue";
        //            }
        //            else if (searchby == "GroupName")
        //            {
        //                condition += "  r.[GroupName] LIKE @searchValue";
        //            }
        //            else if (searchby == "GoodsName")
        //            {
        //                condition += "r.[GoodsName] LIKE @searchValue";
        //            }
        //            else if (searchby == "ReturnType")
        //            {
        //                condition += " t.[ReturnType] LIKE @searchValue";
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(fromDate))
        //        {


        //            condition += " And r.[ApplyDate] BETWEEN  @fromDate AND  @toDate";
        //        }

        //        string query = $@"
        //    DECLARE @TotalRow AS INT;
        //    SET @TotalRow = (SELECT COUNT(*) FROM  OrderMaster);

        //    SELECT 
        //        @TotalRow AS TotalRowCount,
        //        (SELECT COUNT(*) FROM  OrderMaster WHERE Status = 'Pending') AS PendingCount,
        //        (SELECT COUNT(*) FROM  OrderMaster WHERE Status = 'Approved') AS ApprovedCount,
        //        (SELECT COUNT(*) FROM  OrderDetails WHERE Status = 'Returned') AS ReturnedCount,
        //(SELECT COUNT(*) FROM  OrderDetails WHERE Status = 'to Return') AS ToReturnCount,
        //        (SELECT COUNT(*) FROM  OrderMaster WHERE Status = 'Cancelled') AS CancelledCount,
        //        (SELECT COUNT(*) FROM  OrderMaster WHERE Status = 'Delivered') AS DeliveredCount

        //    FROM  OrderMaster;"+
        //            " SELECT r.[ReturnId], r.[GroupName],r.[GoodsName], r.[GroupCode], r.[GoodsId],r.[TypeId],r.[Remarks],r.[OrderNo],r.[DeliveryDate],r.[Price],r.[DetailsId],r.[SellerCode],r.[ApplyDate] ,t.[TypeId]," +
        //            "t.[ReturnType], od.[OrderDetailId],od.[Status] , ( SELECT COUNT(*) " + @condition + ") AS TotalRowCount " + condition + " ORDER BY OrderNo DESC" +
        //            " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";


        //        SqlCommand cmd = new SqlCommand(query, con);

        //        cmd.Parameters.AddWithValue("@status", status);
        //        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //        cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            cmd.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
        //        }
        //        if (!string.IsNullOrEmpty(fromDate))
        //        {
        //            cmd.Parameters.AddWithValue("@FromDate", fromDate);
        //            cmd.Parameters.AddWithValue("@ToDate", toDate);
        //        }

        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        adapter.Fill(ds);


        //        // Check if the dataset contains the tables you need
        //        if (ds.Tables.Count >= 1)
        //        {
        //            DataTable dataTable1st = ds.Tables[0]; // Get the 1st table from the dataset
        //            DataTable dataTable = ds.Tables[1]; // Get the 2nd table from the dataset
        //            foreach (DataRow row in dataTable1st.Rows)
        //            {

        //                PendingCount = int.Parse(row["PendingCount"].ToString());
        //                ApprovedCount = int.Parse(row["ApprovedCount"].ToString());
        //                DeliveredCount = int.Parse(row["DeliveredCount"].ToString());
        //                ReturnedCount = int.Parse(row["ReturnedCount"].ToString());
        //                TotalRowCount = int.Parse(row["TotalRowCount"].ToString());
        //                CancelledCount = int.Parse(row["CancelledCount"].ToString());
        //                ToReturnCount = int.Parse(row["ToReturnCount"].ToString());
        //                // Other status counts...
        //            }
        //            List<ProductReturnModel> ordersData = new List<ProductReturnModel>();
        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                ProductReturnModel modelObj = new ProductReturnModel();
        //                // int
        //                modelObj.TypeId = int.Parse(row["TypeId"].ToString());
        //                modelObj.Price = int.Parse(row["Price"].ToString());
        //                modelObj.ReturnId = int.Parse(row["ReturnId"].ToString());
        //                modelObj.DetailsId = int.Parse(row["DetailsId"].ToString());
        //                modelObj.totalRowsCount = int.Parse(row["TotalRowCount"].ToString());
        //                // string
        //                modelObj.ReturnType = row["ReturnType"].ToString();
        //                modelObj.OrderNo = row["OrderNo"].ToString();
        //                modelObj.GroupName = row["GroupName"].ToString();
        //                modelObj.GoodsName = row["GoodsName"].ToString();
        //                modelObj.ApplyDate = DateTime.Parse(row["ApplyDate"].ToString());
        //                modelObj.DeliveryDate = DateTime.Parse(row["DeliveryDate"].ToString());
        //                modelObj.Remarks = row["Remarks"].ToString();
        //                modelObj.Status = row["Status"].ToString();

        //                // Add other properties here...
        //                ordersData.Add(modelObj);
        //            }
        //            // Create an anonymous object to hold the data in the desired format
        //            var result = new
        //            {
        //                statusCount = new
        //                {
        //                    PendingCount,
        //                    ApprovedCount,
        //                    CancelledCount,
        //                    ReturnedCount,
        //                    DeliveredCount,
        //                    TotalRowCount,
        //                    ToReturnCount
        //                },
        //                ordersData
        //            };
        //            return Ok(result);
        //        }

        //        return null;

        //    }


        ////------------ get return data for SELLER --------

        //[HttpPost]
        //[Authorize(Roles = "seller")]
        //[Route("GetReturnData/{pageNumber}/{pageSize}")]
        //public IActionResult getReturnData([FromForm] string status, int pageNumber, int pageSize)
        //{
        //    List<ProductReturnModel> returnData = new List<ProductReturnModel>();
        //    string condition = "FROM  [ProductReturn] r " +
        //"LEFT JOIN  [ReturnType] t ON r.[TypeId] = t.[TypeId]" +
        //"JOIN  OrderDetails od ON r.[DetailsId] = od.[OrderDetailId] AND od.[Status] = @status";
        // string sqlSelect = "SELECT r.[ReturnId],r.[GoodsName], r.[GroupName], r.[GroupCode], r.[GoodsId],r.[TypeId],r.[Remarks],r.[OrderNo],r.[DeliveryDate],r.[Price],r.[DetailsId],r.[SellerCode],r.[ApplyDate] ,t.[TypeId]," +
        //        "t.[ReturnType], od.[OrderDetailId],od.[Status] , ( SELECT COUNT(*) " + @condition + ") AS TotalRowCount " + condition + " ORDER BY [ApplyDate] DESC" +
        //        " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        //    using (SqlConnection connection = new SqlConnection(_prominentConnection))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlSelect, connection))
        //        {
        //            try
        //            {
        //                connection.Open();
        //                 cmd.Parameters.AddWithValue("@status", status);
        //                cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    ProductReturnModel returnType = new ProductReturnModel
        //                    {
        //                        TypeId = (int)reader["TypeId"],
        //                        ReturnType = reader["ReturnType"].ToString(),
        //                        Price = (double)reader["Price"],

        //                        Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
        //                        Remarks = reader["Remarks"] == DBNull.Value ? null : reader["Remarks"].ToString(),
        //                        GroupName = reader["GroupName"].ToString(),
        //                        GoodsName = reader["GoodsName"].ToString(),
        //                        ReturnId = (int)reader["ReturnId"],
        //                        DetailsId = (int)reader["DetailsId"],
        //                        TotalRowCount = (int)reader["TotalRowCount"],
        //                        ApplyDate = reader.GetDateTime(reader.GetOrdinal("ApplyDate")),
        //                        OrderNo = reader["OrderNo"].ToString(),
        //                        DeliveryDate = reader.GetDateTime(reader.GetOrdinal("DeliveryDate")),
        //                    };
        //                    returnData.Add(returnType);
        //                }
        //                reader.Close();
        //                return Ok(returnData);
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest($"Error: {ex.Message}");
        //            }
        //        }
        //    }
        //    return Ok();
        //}



        //================================== Added By Rey ==============================



        [HttpGet("getOrderUserInfo")]
        public async Task<IActionResult> getUserInfo(string UserId)
        {
            try
            {
                object res = await _order_Service.getUserInfo(UserId);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        //================================== Added By Tushar ==============================
        [HttpGet("GetSellerOrderBasedOnUserID")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> GetSellerOrderBasedOnUserCodeAsync(string userid, string? status)
        {
            try
            {
                object res = await _order_Service.GetSellerOrderBasedOnUserCodeAsync(userid, status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        [HttpGet("GetBuyerOrderBasedOnUserID")]
        public async Task<IActionResult> GetBuyerOrderBasedOnUserIDAsync(string userid, string? status)
        {
            try
            {
                object res = await _order_Service.GetBuyerOrderBasedOnUserIDAsync(userid, status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        [HttpGet("getAllOrderForBuyer")]
        [Authorize(Roles = "buyer")]
        public async Task<IActionResult> getAllOrderForBuyerAsync(string userid, string? status)
        {
            try
            {
                object res = await _order_Service.getAllOrderForBuyerAsync(userid, status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



        //[HttpGet("getOrderDetailsForBuyerBasedOnOrderNo")]
        //public async Task<IActionResult> getOrderDetailsForBuyerBasedOnOrderNoAsync(string OrderNo)
        //{
        //    //OrderDetailsMasterForBuyerDto Master = new OrderDetailsMasterForBuyerDto();
        //    OrderDetailsMasterForBuyerDto Master = null;
        //    try
        //    {
        //                              string query = @"SELECT
        //                              OM.OrderMasterId,
        //                              OM.OrderNo,
        //                              OM.OrderDate,
        //                              OD.UserId AS SellerID,
        //                              UR.FullName AS SellerName,
        //                              OD.DeliveryDate,
        //                              OD.ProductId,
        //                              PL.ProductName,
        //                              PL.ImagePath,
        //                              OD.Status,
        //                              SPP.TotalPrice AS Price,
        //                              OD.Qty As TotalQty,
        //                              OD.DeliveryCharge,
        //                              OD.NetPrice  As ProductSubtotal,
        //                              OM.PaymentMethod,
        //                              UR2.FullName BuyerName,
        //                              OM.Address As ShippingAddress,
        //                              OM.PhoneNumber As ShippingPhoneNumber,
        //                              UR2.Address As BillingAddress,
        //                              UR2.PhoneNumber As BillingPhoneNumber,
        //                              OM.TotalPrice

        //                            FROM
        //                              OrderMaster OM
        //                              LEFT JOIN OrderDetails OD ON OD.OrderMasterId = OM.OrderMasterId
        //                              LEFT JOIN UserRegistration UR ON UR.UserId = OD.UserId
        //                              LEFT JOIN ProductList PL ON PL.ProductId = OD.ProductId
        //                              LEFT JOIN SellerProductPriceAndOffer SPP ON SPP.ProductId = OD.ProductId AND SPP.UserId = OD.UserId
        //                              LEFT JOIN UserRegistration UR2 ON UR2.UserId= OM.UserId
        //                            WHERE
        //                              OM.OrderNo = @OrderNo
        //                            GROUP BY
        //                              OM.OrderMasterId,
        //                              OM.OrderNo,
        //                              OM.OrderDate,
        //                              OD.UserId,
        //                              UR.FullName,
        //                              OD.DeliveryDate,
        //                              OD.ProductId,
        //                              PL.ProductName,
        //                              PL.ImagePath,
        //                              OD.Status,
        //                              SPP.TotalPrice,
        //                              OD.Qty,
        //                              OD.DeliveryCharge,
        //                              OD.NetPrice,
        //                              OM.Address,
        //                              OM.PhoneNumber,
        //                              OM.PaymentMethod,
        //                              UR2.FullName,
        //                              UR2.Address,
        //                              UR2.PhoneNumber,
        //                              OM.TotalPrice";


        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Parameters.AddWithValue("@OrderNo", OrderNo);


        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {

        //                OrderDetails2ndMasterForBuyerDto SecondMaster = null;
        //                int? sellermaster = null;
        //                while (reader.Read())
        //                {

        //                    decimal? Packagesubtotal = 0;
        //                    int OrderMasterId = reader.IsDBNull("OrderMasterId") ? -1 : Convert.ToInt32(reader["OrderMasterId"]);
        //                    //OrderDetails2ndMasterForBuyerDto SecondMaster = new OrderDetails2ndMasterForBuyerDto();

        //                    if (Master == null || Master.OrderMasterId != OrderMasterId)
        //                    {
        //                        SecondMaster = new OrderDetails2ndMasterForBuyerDto();
        //                        Master = new OrderDetailsMasterForBuyerDto();
        //                        Master.OrderMasterId = OrderMasterId;
        //                        Master.OrderNo = reader.IsDBNull("OrderNo") ? (string?)null : reader["OrderNo"].ToString();
        //                        Master.OrderDate = reader.IsDBNull(reader.GetOrdinal("OrderDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("OrderDate"));
        //                        Master.PaymentMethod = reader.IsDBNull("PaymentMethod") ? (string?)null : reader["PaymentMethod"].ToString();
        //                        Master.BuyerName = reader.IsDBNull("BuyerName") ? (string?)null : reader["BuyerName"].ToString();
        //                        Master.ShippingAddress = reader.IsDBNull("ShippingAddress") ? (string?)null : reader["ShippingAddress"].ToString();
        //                        Master.ShippingPhoneNumber = reader.IsDBNull("ShippingPhoneNumber") ? (string?)null : reader["ShippingPhoneNumber"].ToString();
        //                        Master.BillingAddress = reader.IsDBNull("BillingAddress") ? (string?)null : reader["BillingAddress"].ToString();
        //                        Master.BillingPhoneNumber = reader.IsDBNull("BillingPhoneNumber") ? (string?)null : reader["BillingPhoneNumber"].ToString();



        //                        sellermaster = reader.IsDBNull("SellerId") ? (int?)null : Convert.ToInt32(reader["SellerId"]);
        //                        SecondMaster.SellerId = reader.IsDBNull("SellerId") ? (int?)null : Convert.ToInt32(reader["SellerId"]);
        //                        SecondMaster.DeliveryDate = reader.IsDBNull(reader.GetOrdinal("DeliveryDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("DeliveryDate"));
        //                        SecondMaster.Status = reader.IsDBNull("Status") ? null : reader["Status"].ToString();




        //                        OrderDetails2ndMDetailsForBuyerDto SecondMasterDetails = new OrderDetails2ndMDetailsForBuyerDto();

        //                        SecondMasterDetails.Imagepath = reader.IsDBNull("Imagepath") ? null : reader["Imagepath"].ToString();
        //                        SecondMasterDetails.ProductName = reader.IsDBNull("ProductName") ? null : reader["ProductName"].ToString();
        //                        SecondMasterDetails.Price = reader.IsDBNull("Price") ? (int?)null : Convert.ToDecimal(reader["Price"]);
        //                        SecondMasterDetails.DeliveryCharge = reader.IsDBNull("DeliveryCharge") ? (int?)null : Convert.ToDecimal(reader["DeliveryCharge"]);
        //                        SecondMasterDetails.TotalQty = reader.IsDBNull("TotalQty") ? (int?)null : Convert.ToInt32(reader["TotalQty"]);

        //                        SecondMaster.OrderDetails2ndMDetailsListForBuyer.Add(SecondMasterDetails);

        //                        Master.OrderDetails2ndMasterListForBuyer.Add(SecondMaster);

        //                    }
        //                    else
        //                    {
        //                        OrderDetails2ndMasterForBuyerDto SenMaster = new OrderDetails2ndMasterForBuyerDto();

        //                        int? SellerId = reader.IsDBNull("SellerId") ? (int?)null : Convert.ToInt32(reader["SellerId"]);
        //                        if (SellerId == sellermaster)
        //                        {
        //                            OrderDetails2ndMDetailsForBuyerDto SecondMasterDetails = new OrderDetails2ndMDetailsForBuyerDto();

        //                            SecondMasterDetails.Imagepath = reader.IsDBNull("Imagepath") ? null : reader["Imagepath"].ToString();
        //                            SecondMasterDetails.ProductName = reader.IsDBNull("ProductName") ? null : reader["ProductName"].ToString();
        //                            SecondMasterDetails.Price = reader.IsDBNull("Price") ? (int?)null : Convert.ToDecimal(reader["Price"]);
        //                            SecondMasterDetails.DeliveryCharge = reader.IsDBNull("DeliveryCharge") ? (int?)null : Convert.ToDecimal(reader["DeliveryCharge"]);
        //                            SecondMasterDetails.TotalQty = reader.IsDBNull("TotalQty") ? (int?)null : Convert.ToInt32(reader["TotalQty"]);
        //                            SecondMaster.OrderDetails2ndMDetailsListForBuyer.Add(SecondMasterDetails);
        //                            SenMaster.PackageDeliveryCharge += SecondMasterDetails.DeliveryCharge;

        //                        }
        //                        else
        //                        {
        //                            SenMaster.SellerId = reader.IsDBNull("SellerId") ? (int?)null : Convert.ToInt32(reader["SellerId"]);
        //                            SenMaster.DeliveryDate = reader.IsDBNull(reader.GetOrdinal("DeliveryDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("DeliveryDate"));
        //                            SenMaster.Status = reader.IsDBNull("Status") ? null : reader["Status"].ToString();






        //                            OrderDetails2ndMDetailsForBuyerDto SecondMasterDetails = new OrderDetails2ndMDetailsForBuyerDto();

        //                            SecondMasterDetails.Imagepath = reader.IsDBNull("Imagepath") ? null : reader["Imagepath"].ToString();
        //                            SecondMasterDetails.ProductName = reader.IsDBNull("ProductName") ? null : reader["ProductName"].ToString();
        //                            SecondMasterDetails.Price = reader.IsDBNull("Price") ? (int?)null : Convert.ToInt32(reader["Price"]);
        //                            SecondMasterDetails.TotalQty = reader.IsDBNull("TotalQty") ? (int?)null : Convert.ToInt32(reader["TotalQty"]);
        //                            SecondMasterDetails.DeliveryCharge = reader.IsDBNull("DeliveryCharge") ? (int?)null : Convert.ToDecimal(reader["DeliveryCharge"]);
        //                            SenMaster.PackageDeliveryCharge += SecondMasterDetails.DeliveryCharge;

        //                            SenMaster.OrderDetails2ndMDetailsListForBuyer.Add(SecondMasterDetails);
        //                            Master.OrderDetails2ndMasterListForBuyer.Add(SenMaster);
        //                        }



        //                    }
        //                }
        //            }
        //        }
        //        return Ok(Master);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception
        //        return null;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }

        //}



        [HttpGet("getOrderDetailsForBuyerBasedOnOrderNo")]
        [Authorize(Roles ="buyer")]
        public async Task<IActionResult> getOrderDetailsForBuyerBasedOnOrderNoAsync(string OrderNo)
        {
            try
            {
                object res = await _order_Service.getOrderDetailsForBuyerBasedOnOrderNoAsync(OrderNo);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



        [HttpGet("getAllOrderForSeller")]
        [Authorize(Roles ="seller")]
        public async Task<IActionResult> getAllOrderForSellerAsync( string CompanyCode, string? status)
        {
            try
            {
                object res = await _order_Service.getAllOrderForSellerAsync(CompanyCode, status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }








        //public class CountsList
        //{
        //    public int PendingCount { get; set; }
        //    public int ProcessingCount { get; set; }
        //    public int ReadyToShipCount { get; set; }
        //    public int ShippedCount { get; set; }
        //    public int DeliveredCount { get; set; }
        //    public int CancelledCount { get; set; }
        //    public int AllCount { get; set; }
        //    public int ToReturnCount { get; set; }
        //    public int ReturnedCount { get; set; }



        //}



        //[HttpGet, Authorize(Roles = "seller")]
        //[Route("getSearchedAllOrderForSeller")]
        //public IActionResult getSearchedAllOrderForSeller(string sellerCode, int PageNumber, int PageSize, String? status = null, String? SearchedOrderNo = null, String? SearchedPaymentMethod = null, String? SearchedStatus = null)
        //{
        //    Console.WriteLine(sellerCode, "sellerCode");
        //    string decryptedSupplierCode = CommonServices.DecryptPassword(sellerCode);
        //    List<SellerOrderMaster> orderLst = new List<SellerOrderMaster>();
        //    List<CountsList> countsList = new List<CountsList>();

        //    SqlConnection con = new SqlConnection(_prominentConnection);
        //    string queryForSeller = "sp_OrderMasterDataForSeller";
        //    con.Open();
        //    SqlCommand cmdForSeller = new SqlCommand(queryForSeller, con);
        //    cmdForSeller.CommandType = CommandType.StoredProcedure;
        //    cmdForSeller.Parameters.AddWithValue("@SellerCode", decryptedSupplierCode);
        //    cmdForSeller.Parameters.AddWithValue("@PageNumber", PageNumber);
        //    cmdForSeller.Parameters.AddWithValue("@PageSize", PageSize);
        //    if (status != null) { cmdForSeller.Parameters.AddWithValue("@Status", status); }
        //    if (SearchedOrderNo != null) { cmdForSeller.Parameters.AddWithValue("@SearchedOrderNo", SearchedOrderNo); }
        //    if (SearchedPaymentMethod != null) { cmdForSeller.Parameters.AddWithValue("@SearchedPaymentMethod", SearchedPaymentMethod); }
        //    if (SearchedStatus != null) { cmdForSeller.Parameters.AddWithValue("@SearchedStatus", SearchedStatus); }
        //    SqlDataAdapter adapter = new SqlDataAdapter(cmdForSeller);
        //    DataSet ds = new DataSet();
        //    adapter.Fill(ds);
        //    DataTable dt = ds.Tables[0];
        //    DataTable dt1 = ds.Tables[1];
        //    con.Close();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        SellerOrderMaster order = new SellerOrderMaster();
        //        order.OrderMasterId = Convert.ToInt32(dt.Rows[i]["OrderMasterId"]);
        //        order.OrderNo = dt.Rows[i]["OrderNo"].ToString();
        //        order.OrderDate = dt.Rows[i]["OrderDate"].ToString() ;
        //        order.Address = dt.Rows[i]["Address"].ToString();
        //        order.Status = dt.Rows[i]["Status"].ToString();
        //        order.PaymentMethod = dt.Rows[i]["PaymentMethod"].ToString();
        //        order.NumberofItem = Convert.ToInt32(dt.Rows[i]["NumberOfItem"]);
        //        order.TotalPrice = Convert.ToDecimal(dt.Rows[i]["TotalPrice"]);
        //        order.TotalRowCount = Convert.ToInt32(dt.Rows[i]["TotalRowCount"]);
        //        orderLst.Add(order);
        //    }
        //    for (int i = 0; i < dt1.Rows.Count; i++)
        //    {
        //        CountsList counts = new CountsList();
        //        counts.PendingCount = Convert.ToInt32(dt1.Rows[i]["PendingCount"]);
        //        counts.ProcessingCount = Convert.ToInt32(dt1.Rows[i]["ProcessingCount"]);
        //        counts.ReadyToShipCount = Convert.ToInt32(dt1.Rows[i]["ReadyToShipCount"]);
        //        counts.ShippedCount = Convert.ToInt32(dt1.Rows[i]["ShippedCount"]);
        //        counts.DeliveredCount = Convert.ToInt32(dt1.Rows[i]["DeliveredCount"]);
        //        counts.CancelledCount = Convert.ToInt32(dt1.Rows[i]["CancelledCount"]);
        //        counts.AllCount = Convert.ToInt32(dt1.Rows[i]["AllCount"]);
        //        counts.ToReturnCount = Convert.ToInt32(dt1.Rows[i]["ToReturnCount"]);
        //        counts.ReturnedCount = Convert.ToInt32(dt1.Rows[i]["ReturnedCount"]);
        //        countsList.Add(counts);
        //    }
        //    return Ok(new { message = "content get successfully", orderLst, countsList });
        //}

        // update order status for seller

        // ============================== Added By Rey ==========================


        // by Marufa





    }
}




