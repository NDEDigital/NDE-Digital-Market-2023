namespace NDE_Digital_Market.Model.DTO
{
    public class GetSellerInventoryInfoDTO
    {
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int TotalQty { get; set; }
        public int AvailableQty { get; set; }
        public int SaleQty { get; set; }
    }
}
