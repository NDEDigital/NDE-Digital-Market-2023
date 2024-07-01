namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderInvoiceDataForSellerByOrderMasterIdDetailsDTO
    {
        public string OrderNo { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductName { get; set; }
        public string Specification { get; set; }
        public decimal StockQty { get; set; }
        public int SaleQty { get; set; }
        public string Unit { get; set; }
        public decimal NetPrice { get; set; }
        public string SSLRemarks { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhone { get; set; }
        public string Address { get; set; }
    }
}
