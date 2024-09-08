using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Interfaces.Services;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DastgyrAPI.Services
{
    public class OrdersService : IOrdersService
    {
        #region Initialization
        private IOrdersRepository _OrdersRepository;
        public OrdersService(IOrdersRepository OrdersRepository, IHostingEnvironment hostingEnvironment)
        {
            _OrdersRepository = OrdersRepository;
        }
        #endregion

        #region Implementation Methods
        public async Task<int> AcceptOrderAsync(int orderId, int skuId)
        {
            return await _OrdersRepository.AcceptOrderAsync(orderId, skuId);
        }

        public async Task<int> SendNotification(string fcmToken)
        {
            return await _OrdersRepository.SendNotification(fcmToken);
        }
        
        public async Task<int> ReadyToPickAsync(int orderId, int skuId)
        {
            return await _OrdersRepository.ReadyToPickAsync(orderId, skuId);
        }
        public async Task<List<PaymentItemsResponse>> GetPaymentsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            return await _OrdersRepository.GetPaymentsAsync(id, numberOfDays, startDate,endDate);
        }

        public async Task<List<ReturnItemsResponse>> GetReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            return await _OrdersRepository.GetReturnsAsync(id, numberOfDays, startDate, endDate);
        }
        public async Task<List<ReturnItemsResponse>> GetAwaitingReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            return await _OrdersRepository.GetAwaitingReturnsAsync(id, numberOfDays, startDate, endDate);
        }

        public async Task<List<OrderItemsResponse>> GetOrdersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            return await _OrdersRepository.GetOrdersAsync(id, numberOfDays, startDate, endDate);
        }
        public async Task<List<OrderItemsResponse>> GetPendingOrdersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate, int? sellerStatus)
        {
            return await _OrdersRepository.GetOrdersByStatusAsync(id, numberOfDays, startDate, endDate, sellerStatus);
        }
        public async Task<List<InventoryItemsResponse>> GetInventoryAsync()
        {
            return await _OrdersRepository.GetInventoryAsync();
        }

        public async Task<DashboardResponse> GetDashboardResultByUserId()
        {
            return await _OrdersRepository.GetDashboardResultByUserId();

        }

        #endregion

    }
}
