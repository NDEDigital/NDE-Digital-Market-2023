namespace NDE_Digital_Market.Model.DTO
{
    public class InsertProductListDTO
    {
        public string? ProductName { get; set; }
        public string? ProductGroupID { get; set; }
        public string? Specification { get; set; }
        public string? BrandId { get; set; }
        public string? UnitId { get; set; }
        public IFormFile? ImageFile { get; set; }
        //public string? ExistingImageFileName { get; set; }
        public string? ProductSubName { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
    }
}
