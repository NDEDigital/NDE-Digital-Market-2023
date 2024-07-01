namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderInvoiceDataForBuyerByOrderMasterIdDTO
    {
        public string InvoiceNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string BuyerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PaymentMethod { get; set; }
        public int NumberOfItem { get; set; }
        public decimal TotalPrice { get; set; }

        public List<GetOrderInvoiceDataForBuyerByOrderMasterIdDetailsDTO> OrderInvoiceDetailList { get; set; }

        public GetOrderInvoiceDataForBuyerByOrderMasterIdDTO()
        {
            OrderInvoiceDetailList = new List<GetOrderInvoiceDataForBuyerByOrderMasterIdDetailsDTO>();
        }
    }
}
