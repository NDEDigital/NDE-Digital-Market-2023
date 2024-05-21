namespace NDE_Digital_Market.Model
{
    public class PortalReceivedMasterModel
    {
        public int? PortalReceivedId { get; set; }
        public string? PortalReceivedCode { get; set; }
        public DateTime? MaterialReceivedDate { get; set; }
        public string? ChallanNo { get; set; }
        public DateTime? ChallanDate { get; set; }
        public string? Remarks { get; set; }
        public int? UserId { get; set; }
        public string? CompanyCode { get; set; }
        //public DateTime AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
        public List<PortalReceivedDetailsModel>? PortalReceivedDetailslist { get; set; }

        public PortalReceivedMasterModel()
        {
            PortalReceivedDetailslist = new List<PortalReceivedDetailsModel>();
        }
    }
}
