﻿namespace NDE_Digital_Market.DTOs
{
    public class GetOrderInvoiceByMasterIdDto
    {
        // Order Master Details
        public int? OrderMasterId { get; set; }
        public string? OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Address { get; set; }
        public string? BuyerName { get; set; }
        public string? PaymentMethod { get; set; }
        public int? NumberOfItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? DeliveryCharge { get; set; }

        // Order Detail Details
        public int? OrderDetailId { get; set; }
        public string? SellerName { get; set; }
        public string? ProductName { get; set; }
        public string? Specification { get; set; }
        public int? Qty { get; set; }
        public int? UnitId { get; set; }
        public string? Unit { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? Price { get; set; }
        public decimal? DetailDeliveryCharge { get; set; }
        public DateTime? DetailDeliveryDate { get; set; }
        public decimal? DiscountPct { get; set; }
        public decimal? NetPrice { get; set; }
    }
}
