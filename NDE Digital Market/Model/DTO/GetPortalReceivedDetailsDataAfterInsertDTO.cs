namespace NDE_Digital_Market.Model.DTO
{
    public class GetPortalReceivedDetailsDataAfterInsertDTO
    {
        public string PortalReceivedId { get; set; }
        public string PortalDetailsId { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Specification { get; set; }
        public decimal ReceivedQty { get; set; }
        public string UnitId { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AvailableQty { get; set; }
        public string Remarks { get; set; }
    }
}
