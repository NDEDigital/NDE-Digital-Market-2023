namespace NDE_Digital_Market.Model.DTO
{
    public class UpdateProductGroupDTO
    {
        public string? ProductGroupID { get; set; }
        public string? ProductGroupName { get; set; }
        public string? ProductGroupPrefix { get; set; }
        public string? ProductGroupDetails { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImageFileName { get; set; }

        public string? UpdatedBy { get; set; }
        public string? UpdatedPC { get; set; }
    }
}
