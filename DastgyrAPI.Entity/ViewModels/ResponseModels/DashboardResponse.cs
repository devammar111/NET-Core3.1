namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class DashboardResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public double? TotalMoneyEarned { get; set; }
        public int? PendingOrders { get; set; }
        public int? ReadyToPickupOrders { get; set; }
        public int? ActivityListings { get; set; }
        public decimal? PercentagePendingOrders { get; set; }
        public decimal? PercentageReadyToPickupOrders { get; set; }
        public decimal? PercentageActivityListings { get; set; }
    }
}
