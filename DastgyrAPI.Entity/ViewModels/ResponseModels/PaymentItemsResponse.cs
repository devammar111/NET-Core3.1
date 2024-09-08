using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class PaymentItemsResponse
    {
        public int? ProductId { get; set; }
        public int? SkuId { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public int? Quantity { get; set; }
        public double Amount { get; set; }
        public string ProductImage { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DeliveredAt { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? PickupDate { get; set; }

    }
}
