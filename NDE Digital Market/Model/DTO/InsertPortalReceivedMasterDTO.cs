namespace NDE_Digital_Market.Model.DTO
{
    public class InsertPortalReceivedMasterDTO
    {

        public DateTime? MaterialReceivedDate { get; set; }
        public string? ChallanNo { get; set; }
        public DateTime? ChallanDate { get; set; }
        public string? Remarks { get; set; }
        public string? UserId { get; set; }
        //public string? CompanyCode { get; set; }
        //public DateTime AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
        public List<InsertPortalReceivedDetailsDTO>? PortalReceivedDetailslist { get; set; }

        public InsertPortalReceivedMasterDTO()
        {
            PortalReceivedDetailslist = new List<InsertPortalReceivedDetailsDTO>();
        }
    }
}
