namespace NDE_Digital_Market.Model.DTO
{
    public class UpdateProductListDTO
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductGroupID { get; set; }
        public string? Specification { get; set; }
        public string? BrandId { get; set; }
        public string? UnitId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImageFileName { get; set; }
        public string? ProductSubName { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedPC { get; set; }
    }
}
