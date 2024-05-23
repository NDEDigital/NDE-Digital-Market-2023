namespace NDE_Digital_Market.Model
{
    public class ProductGroupModel: CommonFieldsModel
    {
        public int? ProductGroupID { get; set; }
        public string? ProductGroupName { get; set; }
        public string? ProductGroupPrefix { get; set; }
        public string? ProductGroupDetails { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImageFileName { get; set; }
    }
}
