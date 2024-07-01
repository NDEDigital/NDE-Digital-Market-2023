namespace NDE_Digital_Market.Model.DTO
{
    public class InsertOrderMasterDTO
    {

        public DateTime? OrderDate { get; set; }
        public string? Address { get; set; }
        public string? UserId { get; set; }
        public string? PaymentMethod { get; set; }
        public int? NumberOfItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }


        public List<InsertOrderDetailsDTO>? OrderDetailsList { get; set; }

        public InsertOrderMasterDTO()
        {
            OrderDetailsList = new List<InsertOrderDetailsDTO>();
        }
    }
}
