using System.Collections.Generic;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class InventoryResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<InventoryItemsResponse> InventoryData { get; set; }
    }
}
