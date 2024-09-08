using System.Collections.Generic;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ReturnsResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ReturnItemsResponse> Returns { get; set; }
        public List<ReturnItemsResponse> AwaitingReturns { get; set; }
        public double? TotalMoneyEarned
        {
            get;
            set;
        }
    }
}
