namespace NDE_Digital_Market.Model.DTO
{
    public class GetSellerProductForPriceAndOfferByUserIdDTO
    {
        public string SellerProductId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPct { get; set; }
        public DateTime EffectivateDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AddedDate { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPrice { get; set; }

        public string UnitName { get; set; }

        public string ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
    }
}
