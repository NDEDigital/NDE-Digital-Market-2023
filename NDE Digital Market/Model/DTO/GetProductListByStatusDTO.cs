namespace NDE_Digital_Market.Model.DTO
{
    public class GetProductListByStatusDTO
    {
        public string? ProductId { get; set; }
        public string? UnitId { get; set; }
        public string? Unit { get; set; }
        public string? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? ProductGroupID { get; set; }
        public string? ProductGroupName { get; set; }
        public string? ProductName { get; set; }
        public string? Specification { get; set; }
        public string? ImagePath { get; set; }
        public string? ProductSubName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
