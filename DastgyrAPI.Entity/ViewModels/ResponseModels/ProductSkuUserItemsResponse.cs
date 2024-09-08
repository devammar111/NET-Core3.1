using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ProductSkuUserItemsResponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? SkuId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? ReminderQuantity { get; set; }
        public int? OrderedQuantity { get; set; }
        public int RemainingInventory
        {
            get 
            {
                var remainingInventory = (Quantity ?? 0) - (OrderedQuantity ?? 0);
                return remainingInventory;
            }
        }
        public int? MinimumQuantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExpiryDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StockAvailabilityDate { get; set; }
        public bool? IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ListingDate { get; set; }
    }

    public class ProductSkuReponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? SkuId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
