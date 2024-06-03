
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

            DataSet dataSet = await _invoice_DAL.GetInvoiceDataForBuyer(DecryptOrderMasterId);

            // Check if dataSet is null
            if (dataSet == null || dataSet.Tables.Count < 2)
            {
                return null;
            }

            // Get the invoice information table
            DataTable invoiceTable = dataSet.Tables[0];

            // Get the order details table
            DataTable orderDetailsTable = dataSet.Tables[1];

            // Populate the invoice model
            GetOrderInvoiceDataForBuyerByOrderMasterIdDTO invoice = new GetOrderInvoiceDataForBuyerByOrderMasterIdDTO();
            if (invoiceTable.Rows.Count > 0)
            {
                DataRow invoiceRow = invoiceTable.Rows[0]; // Assuming only one row for invoice info

                invoice.InvoiceNumber = invoiceRow["InvoiceNumber"].ToString();
                invoice.OrderDate = Convert.ToDateTime(invoiceRow["OrderDate"]);
                invoice.BuyerName = invoiceRow["BuyerName"].ToString();
                invoice.Address = invoiceRow["Address"].ToString();
                invoice.Phone = invoiceRow["PhoneNumber"].ToString();
                invoice.PaymentMethod = invoiceRow["PaymentMethod"].ToString();
                invoice.NumberOfItem = Convert.ToInt32(invoiceRow["NumberOfItem"]);
                invoice.TotalPrice = Convert.ToDecimal(invoiceRow["TotalPrice"]);
            }

            // Populate the order details
            foreach (DataRow row in orderDetailsTable.Rows)
            {
                GetOrderInvoiceDataForBuyerByOrderMasterIdDetailsDTO orderDetails = new GetOrderInvoiceDataForBuyerByOrderMasterIdDetailsDTO();

                orderDetails.ProductName = row["ProductName"].ToString();
                orderDetails.Status = row["Status"].ToString();
                orderDetails.Specification = row["Specification"].ToString();
                orderDetails.Quantity = Convert.ToInt32(row["Quantity"]);
                orderDetails.SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                orderDetails.Unit = row["Unit"].ToString();
                orderDetails.Price = Convert.ToDecimal(row["Price"]);
                orderDetails.DeliveryCharge = Convert.ToDecimal(row["DeliveryCharge"]);
                orderDetails.DiscountAmount = Convert.ToDecimal(row["DiscountAmount"]);
                orderDetails.DeliveryDate = Convert.ToDateTime(row["DeliveryDate"]);
                orderDetails.DiscountPct = Convert.ToDecimal(row["DiscountPct"]);
                orderDetails.NetPrice = Convert.ToDecimal(row["NetPrice"]);
                orderDetails.DetailDeliveryCharge = Convert.ToDecimal(row["DetailDeliveryCharge"]);
                orderDetails.SubTotalPrice = Convert.ToDecimal(row["SubTotalPrice"]);
                orderDetails.SelesPerson = row["SelesPerson"].ToString();
                orderDetails.SelesAddress = row["SelesAddress"].ToString();
                orderDetails.SellerContact = row["SellerContact"].ToString();
                orderDetails.Company = row["Company"].ToString();

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
