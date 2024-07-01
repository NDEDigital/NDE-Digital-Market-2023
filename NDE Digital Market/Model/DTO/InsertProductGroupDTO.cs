namespace NDE_Digital_Market.Model.DTO
{
    public class InsertProductGroupDTO
    {
        public string? ProductGroupName { get; set; }
        public string? ProductGroupPrefix { get; set; }
        public string? ProductGroupDetails { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
    }
}
