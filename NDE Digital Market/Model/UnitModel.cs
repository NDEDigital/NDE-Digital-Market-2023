namespace NDE_Digital_Market.Model
{
    public class UnitModel
    {
        public int UnitId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? AddedPC { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedPC { get; set; }
        public bool IsActive { get; set; }
        public bool IsConversion { get; set; }

    }
}
