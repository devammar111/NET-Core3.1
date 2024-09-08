using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class OrdersResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<AggregatedItemsResponse> Aggregated { get; set; }
        public List<OrderItemsResponse> Pending { get; set; }
        public List<OrderItemsResponse> Ready { get; set; }
        public List<OrderItemsResponse> Shipped { get; set; }
        public List<OrderItemsResponse> AwaitingPickup { get; set; }
        public List<OrderItemsResponse> ReadyToPick { get; set; }
        public List<OrderItemsResponse> Pickup { get; set; }
        

    }
}
