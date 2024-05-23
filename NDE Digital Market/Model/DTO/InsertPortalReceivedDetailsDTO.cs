namespace NDE_Digital_Market.Model.DTO
{
    public class InsertPortalReceivedDetailsDTO
    {

        public string? ProductGroupId { get; set; }
        public string? ProductId { get; set; }
        public string? Specification { get; set; }
        public int? ReceivedQty { get; set; }
        public string? UnitId { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? UserId { get; set; }
        public string? Remarks { get; set; }
        //public string? ApprovedBy { get; set; }
        //public DateTime? ApproveDate { get; set; }
        //public string? ApproveStatus { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
    }
}
