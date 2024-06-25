namespace NDE_Digital_Market.Model
{
    public class SellerSalesMasterModel : CommonFieldsModel
    {

        public int? SSMId { get; set; }
        public string? SSMCode { get; set; }
        public DateTime? SSMDate { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Challan { get; set; }
        public string? Remarks { get; set; }
        public int? BUserId { get; set; }
        public string? CompanyCode { get; set; }

        public List<SellerSalesDetailsModel>? SellerSalesDetailsList { get; set; }

        public SellerSalesMasterModel()
        {
            SellerSalesDetailsList = new List<SellerSalesDetailsModel>();
        }
    }
}
