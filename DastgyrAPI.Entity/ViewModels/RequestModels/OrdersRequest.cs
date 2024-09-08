using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class OrdersRequest
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public int WalletId { get; set; }
        public int CouponId { get; set; }
        public int PreviousStatus { get; set; }
        public int SpecialDiscount { get; set; }
        public int DeliveredAt { get; set; }
    }
}
