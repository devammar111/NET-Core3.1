using DastgyrAPI.Entity;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DastgyrAPI.Interfaces.Repositories
{
    public interface IOrdersRepository : IBaseRepository<Orders>
    {
        Task<int> AcceptOrderAsync(int orderId, int skuId);
        Task<int> ReadyToPickAsync(int orderId, int skuId);
        Task<int> SendNotification(string fcmToken);
        Task<List<PaymentItemsResponse>> GetPaymentsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<ReturnItemsResponse>> GetReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<ReturnItemsResponse>> GetAwaitingReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<OrderItemsResponse>> GetOrdersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<OrderItemsResponse>> GetOrdersByStatusAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate, int? sellerStatus);
        Task<List<InventoryItemsResponse>> GetInventoryAsync();
        Task<DashboardResponse> GetDashboardResultByUserId();
    }
}
