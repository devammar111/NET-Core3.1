using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class OrderItemsRequest
    {
        public int OrderId { get; set; }

        public int SkuId { get; set; }

        public double? Price { get; set; }

        public double Discount { get; set; }

        public int Quantity { get; set; }

        public int Status { get; set; }

        public int Tag { get; set; }

        public int ReturnQuantity { get; set; }

        public string Reason { get; set; }

        public int DealId { get; set; }

        public int DeltaReturn { get; set; }

    }
}
