using System;
using System.ComponentModel.DataAnnotations;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ReturnItemsResponse
    {
        public int? ProductId { get; set; }
        public int? SkuId { get; set; }
        public string ProductName { get; set; }
        public int? OrderedQuantity { get; set; }
        public int? DeliveredQuantity { get; set; }
        public int? ReturnedQuantity { get; set; }
        public double Amount { get; set; }
        public string ProductImage { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DeliveredAt { get; set; }
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime PickupDate { get; set; }

    }
}
