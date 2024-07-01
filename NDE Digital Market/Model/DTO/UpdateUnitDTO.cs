namespace NDE_Digital_Market.Model.DTO
{
    public class UpdateUnitDTO
    {
        public string UnitId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? UpdatedBy { get; set; }
        public string? UpdatedPC { get; set; }
        public bool IsActive { get; set; }
        public bool IsConversion { get; set; }
    }
}
