using DastgyrAPI.Models.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DastgyrAPI.Interfaces.Services
{
    public interface IOrdersService
    {
        Task<List<PaymentItemsResponse>> GetPaymentsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<ReturnItemsResponse>> GetReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<ReturnItemsResponse>> GetAwaitingReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<OrderItemsResponse>> GetOrdersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<OrderItemsResponse>> GetPendingOrdersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate, int? sellerStatus);
        Task<List<InventoryItemsResponse>> GetInventoryAsync();
        Task<int> AcceptOrderAsync(int orderId, int skuId);
        Task<int> SendNotification(string fcmToken);
        Task<int> ReadyToPickAsync(int orderId, int skuId);
        Task<DashboardResponse> GetDashboardResultByUserId();
    }
}
