
namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class AggregatedItemsResponse
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SkuId { get; set; }
        public int? ListedQuantity { get; set; }
        public int? OrderedQuantity { get; set; }
        public string ProductImage { get; set; }
    }
}
