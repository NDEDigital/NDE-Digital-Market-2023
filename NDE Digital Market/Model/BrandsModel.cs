namespace NDE_Digital_Market.Model
{
    public class BrandsModel
    {
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? ShortName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedPC { get; set; }
    }
}
