namespace NDE_Digital_Market.Model.DTO
{
    public class GetAllProductGroupDTO
    {
        public string? ProductGroupID { get; set; }
        public string? ProductGroupCode { get; set; }
        public string? ProductGroupName { get; set; }
        public string? ProductGroupPrefix { get; set; }
        public string? ProductGroupDetails { get; set; }
        public bool? IsActive { get; set; }
    }
}
