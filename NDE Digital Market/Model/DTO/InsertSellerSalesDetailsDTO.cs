namespace NDE_Digital_Market.Model.DTO
{
    public class InsertSellerSalesDetailsDTO
    {
        public int? SSMId { get; set; }
        public string? OrderNo { get; set; }
        public string? ProductId { get; set; }
        public string? Specification { get; set; }
        public int? StockQty { get; set; }
        public int? SaleQty { get; set; }
        public string? UnitId { get; set; }
        public decimal? NetPrice { get; set; }
        public string? Remarks { get; set; }
        public string? Address { get; set; }
        public string? ProductGroupID { get; set; }


        public DateTime? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
    }
}
