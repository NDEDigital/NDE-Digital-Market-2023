namespace NDE_Digital_Market.Model.DTO
{
    public class GetSellerProductListForAddQtyDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductGroupId { get; set; }
        public string Specification { get; set; }
        public string UnitId { get; set; }
        public string Unit { get; set; }
        public Decimal Price { get; set; }
        public Decimal AvailableQty { get; set; }
    }
}
