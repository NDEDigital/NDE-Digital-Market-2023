namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderInvoiceDataForSellerByOrderMasterIdDTO
    {
        public string SSMCode { get; set; }
        public DateTime SSMDate { get; set; }
        public string SelesPerson { get; set; }
        public string Company { get; set; }
        public string SelesAddress { get; set; }
        public string Phone { get; set; }
        public string Challan { get; set; }
        public string Remarks { get; set; }

        public List<GetOrderInvoiceDataForSellerByOrderMasterIdDetailsDTO> OrderInvoiceDetailList { get; set; }

        public GetOrderInvoiceDataForSellerByOrderMasterIdDTO()
        {
            OrderInvoiceDetailList = new List<GetOrderInvoiceDataForSellerByOrderMasterIdDetailsDTO>();
        }
    }
}
