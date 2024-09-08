using DastgyrAPI.Entity.HelperModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class OrderItemsResponse
    {

        public int? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int? ProductId { get; set; }
        public int? SkuId { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public int? Quantity { get; set; }
        public double Amount { get; set; }
        public int? Status { get; set; }
        public string StatusName
        {
            get
            {
                if (Status.HasValue)
                {
                    return ((OrderSellerStatus)Status.Value).ToString();
                }
                return "";
            }
        }
        public string ProductImage { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? PickupDate { get; set; }
        public int? ListedQuantity { get; set; }
    }
}
