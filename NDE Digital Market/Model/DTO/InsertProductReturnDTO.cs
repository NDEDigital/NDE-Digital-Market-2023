namespace NDE_Digital_Market.Model.DTO
{
    public class InsertProductReturnDTO
    {
        public string? ReturnTypeId { get; set; }
        public string? ProductGroupId { get; set; }
        public string? ProductId { get; set; }
        public string? OrderNo { get; set; }
        public decimal? Price { get; set; }
        public string? OrderDetailsId { get; set; }
        public string? SellerId { get; set; }
        //public DateTime? ApplyDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Remarks { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPc { get; set; }
    }
}
