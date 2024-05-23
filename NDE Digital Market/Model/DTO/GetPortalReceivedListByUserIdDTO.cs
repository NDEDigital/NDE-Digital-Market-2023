namespace NDE_Digital_Market.Model.DTO
{
    public class GetPortalReceivedListByUserIdDTO
    {
        public string PortalReceivedId { get; set; }
        public string PortalReceivedCode { get; set; }
        public DateTime? MaterialReceivedDate { get; set; }
        public string UserId { get; set; }
    }
}
