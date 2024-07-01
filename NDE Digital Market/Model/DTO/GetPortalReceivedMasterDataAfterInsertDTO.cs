namespace NDE_Digital_Market.Model.DTO
{
    public class GetPortalReceivedMasterDataAfterInsertDTO
    {
        public GetPortalReceivedMasterDataAfterInsertDTO()
        {
            PortalReceivedDetailAfterInsertlList = new List<GetPortalReceivedDetailsDataAfterInsertDTO>();
        }
        public string PortalReceivedId { get; set; }
        public string PortalReceivedCode { get; set; }
        public DateTime MaterialReceivedDate { get; set; }
        public string ChallanNo { get; set; }
        public DateTime? ChallanDate { get; set; }
        public string Remarks { get; set; }
        public List<GetPortalReceivedDetailsDataAfterInsertDTO>? PortalReceivedDetailAfterInsertlList { get; set; }
    }
}
