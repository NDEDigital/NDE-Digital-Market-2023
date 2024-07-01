namespace NDE_Digital_Market.Model
{
    public class AddToCartModal
    {
        public int? Id { get; set; }
        public int BuyerUserID { get; set; }
        public string CompanyCode { get; set; }
        public int ProductID { get; set; }
        public int ProductGroupID { get; set; }
        public int UnitID { get; set; }
        public int ProductCartQuantity { get; set; }
        public DateTime? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedPC { get; set; }
        public string? ImagePath { get; set; }
        public string? ProductName { get; set; }
        public string? Price { get; set; }
        public string? TotalPrice { get; set; }
        public string? AvailableQty { get; set; }
        public string? CompanyName { get; set; }
        public string? Specification { get; set; }
    }
}
