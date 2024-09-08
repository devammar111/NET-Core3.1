using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class InventoryItemsResponse
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SkuId { get; set; }

        public int? ListedQuantity { get; set; }
        public int? OrderedQuantity { get; set; }
        public int InventoryRemaining 
        {
            get { return (ListedQuantity??0) - (OrderedQuantity??0); }
        }
        public string ProductImage { get; set; }
        public bool? SendLowInventoryNotification { get; set; }
        public int? MinimumQuantity { get; set; }
        public int? ReminderQuantity { get; set; }
    }

}
