using DastgyrAPI.Models.ViewModels.ResponseModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class ProductSkuUsersRequest
    {
        public int ProductId { get; set; }
        public int? SkuId { get; set; }
        public int? Quantity { get; set; }
        public int? MinimumQuantity { get; set; }
        public double? Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExpiryDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StockAvailabilityDate { get; set; }
        //public bool? SendLowInventoryNotification { get; set; }
    }

    public class AddProductSkuUsersResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public ProductSkuUserItemsResponse ProductSkuUser { get; set; }
    }
    public class SetMinimumQuantityNotificationRequest
    {
        public int SkuId { get; set; }
        public int? ReminderQuantity { get; set; }
        public bool? SendLowInventoryNotification { get; set; }
    }
    public class UpdateListedQuantityRequest
    {
        public int SkuId { get; set; }
        public int Quantity { get; set; }
    }
}
