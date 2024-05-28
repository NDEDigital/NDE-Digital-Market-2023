
using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.InvoiceService
{
    public class Invoice_Service : IInvoice_Service
    {
        private readonly Invoice_DAL _invoice_DAL;
        public Invoice_Service(Invoice_DAL invoice_DAL)
        {
            _invoice_DAL = invoice_DAL;
        }

        public async Task<GetOrderInvoiceDataForBuyerByOrderMasterIdDTO> GetInvoiceDataForBuyer(string OrderMasterId)
        {
            int DecryptOrderMasterId = int.Parse(CommonServices.DecryptPassword(OrderMasterId));
            DataTable dataTable = await _invoice_DAL.GetInvoiceDataForBuyer(DecryptOrderMasterId);

            GetOrderInvoiceDataForBuyerByOrderMasterIdDTO invoice = new GetOrderInvoiceDataForBuyerByOrderMasterIdDTO();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                invoice.InvoiceNumber = row["InvoiceNumber"].ToString();
                invoice.OrderDate = Convert.ToDateTime(row["OrderDate"].ToString());
                invoice.BuyerName = row["BuyerName"].ToString();
                invoice.Address = row["Address"].ToString();
                invoice.Phone = row["PhoneNumber"].ToString();
                invoice.PaymentMethod = row["PaymentMethod"].ToString();
                invoice.NumberOfItem = Convert.ToInt32(row["NumberOfItem"].ToString());
                invoice.TotalPrice = Convert.ToDecimal(row["TotalPrice"].ToString());
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetOrderInvoiceDataForBuyerByOrderMasterIdDetailsDTO orderDetails = new GetOrderInvoiceDataForBuyerByOrderMasterIdDetailsDTO
                {

                    ProductName = row["ProductName"].ToString(),
                    Status = row["Status"].ToString(),
                    Specification = row["Specification"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"].ToString()),
                    SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString()),
                    Unit = row["Unit"].ToString(),
                    Price = Convert.ToDecimal(row["Price"].ToString()),
                    DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"].ToString()),
                    DiscountAmount = Convert.ToDecimal(row["DiscountAmount"].ToString()),
                    DeliveryDate = Convert.ToDateTime(row["DeliveryDate"].ToString()),
                    DiscountPct = Convert.ToDecimal(row["DiscountPct"].ToString()),
                    NetPrice = Convert.ToDecimal(row["NetPrice"].ToString()),
                    DetailDeliveryCharge = Convert.ToDecimal(row["DetailDeliveryCharge"].ToString()),
                    SubTotalPrice = Convert.ToDecimal(row["SubTotalPrice"].ToString()),
                    SelesPerson = row["SelesPerson"].ToString(),
                    SelesAddress = row["SelesAddress"].ToString(),
                    SellerContact = row["SellerContact"].ToString(),
                    Company = row["Company"].ToString(),
                };
                invoice.OrderInvoiceDetailList.Add(orderDetails);
            }
            return invoice;

        }



        public async Task<GetOrderInvoiceDataForSellerByOrderMasterIdDTO> GetInvoiceDataForSeller(string SSMId)
        {

            int DecryptOrderMasterId = int.Parse(CommonServices.DecryptPassword(SSMId));
            DataTable dataTable = await _invoice_DAL.GetInvoiceDataForSeller(DecryptOrderMasterId);

            GetOrderInvoiceDataForSellerByOrderMasterIdDTO invoice = new GetOrderInvoiceDataForSellerByOrderMasterIdDTO();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                invoice.SSMCode = row["SSMCode"].ToString();
                invoice.SSMDate = Convert.ToDateTime(row["SSMDate"].ToString());
                invoice.SelesPerson = row["SelesPerson"].ToString();
                invoice.Company = row["Company"].ToString();
                invoice.SelesAddress = row["SelesAddress"].ToString();
                invoice.Phone = row["Phone"].ToString();
                invoice.Challan = row["Challan"].ToString();
                invoice.Remarks = row["Remarks"].ToString();
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetOrderInvoiceDataForSellerByOrderMasterIdDetailsDTO sellerDetails = new GetOrderInvoiceDataForSellerByOrderMasterIdDetailsDTO
                {
                    OrderNo = row["OrderNo"].ToString(),
                    ProductGroupName = row["ProductGroupName"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    Specification = row["Specification"].ToString(),
                    StockQty = Convert.ToDecimal(row["StockQty"].ToString()),
                    SaleQty = Convert.ToInt32(row["SaleQty"].ToString()),
                    Unit = row["Unit"].ToString(),
                    NetPrice = Convert.ToDecimal(row["NetPrice"].ToString()),
                    SSLRemarks = row["SSLRemarks"].ToString(),
                    BuyerName = row["BuyerName"].ToString(),
                    BuyerPhone = row["BuyerPhone"].ToString(),
                    Address = row["Address"].ToString(),
                };
                invoice.OrderInvoiceDetailList.Add(sellerDetails);
            }
            return invoice;
        }

    }
}
