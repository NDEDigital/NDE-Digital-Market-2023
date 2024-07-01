namespace NDE_Digital_Market.Model.DTO
{
    public class UpdateBrandsDTO
    {
        public string? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? ShortName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

        public string? UpdatedBy { get; set; }
        public string? UpdatedPC { get; set; }
    }
}
