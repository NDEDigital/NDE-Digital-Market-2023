namespace NDE_Digital_Market.Model.DTO
{
    public class InsertSellerSalesMasterDTO
    {
        public DateTime? SSMDate { get; set; }
        public string? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Challan { get; set; }
        public string? Remarks { get; set; }
        public string? BUserId { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? AddedPC { get; set; }
        public string? CompanyCode { get; set; }

        public List<InsertSellerSalesDetailsDTO>? SellerSalesDetailsList { get; set; }

        public InsertSellerSalesMasterDTO()
        {
            SellerSalesDetailsList = new List<InsertSellerSalesDetailsDTO>();
        }
    }
}
