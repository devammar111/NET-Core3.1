using DastgyrAPI.Common;
using DastgyrAPI.Entity;
using DastgyrAPI.Entity.HelperModels;
using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DastgyrAPI.Repositories
{
    public class OrdersRepository : BaseRepository<Orders>, IOrdersRepository
    {
        #region Initialization
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrdersRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)

        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Implementation Methods
        public async Task<int> AcceptOrderAsync(int orderId, int skuId)
        {

            var orderEntity = _dbContext.OrderItems.Include(o => o.ProductSku).ThenInclude(p => p.ProductSkuUsers).FirstOrDefault(u => u.OrderId == orderId && u.SkuId == skuId);
            if (orderEntity != null)
            {
                orderEntity.SellerStatus = Convert.ToInt32(OrderSellerStatus.AwaitingPickup);
            }
            await _dbContext.SaveChangesAsync();


            var inventory = GetSKUInventory(orderEntity.SkuId ?? 0);
            if (inventory != null && (inventory.SendLowInventoryNotification ?? false) && (inventory.InventoryRemaining) <= inventory.ReminderQuantity)
            {
                var userFcmTokens = _dbContext.UserFcmTokens.FirstOrDefault(f => f.UserId == LoggedInUserId);
                await NotificationHelper.SendNotification(userFcmTokens.FcmToken, "Low Inventory", inventory.ProductName + " has low quantity, please more quantity");
            }

            return orderEntity.OrderId;
            //CopyViewModelToEntity(product, productEntity);
        }
        public async Task<DashboardResponse> GetDashboardResultByUserId()
        {

            var pendingOrderItems = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.Pending));
            var awaitingPickup = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.AwaitingPickup));
            var readyToPick = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.ReadyToPick));
            var pickup = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.PickUp));
            var shiped = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.Shipped__));
            var ready = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.Ready));
            var returned = await GetOrdersByStatusAsync(null, null, null, null, Convert.ToInt32(OrderSellerStatus.Returned));

            var aggregatedItems = pendingOrderItems.Concat(awaitingPickup).Concat(readyToPick).Concat(pickup).Concat(ready).Concat(shiped).Concat(returned)
                .ToList();

            int id = LoggedInUserId;
            DashboardResponse modelResponse = new DashboardResponse();
            //var totalOrders = await _dbContext.OrderItems.Where(ord =>
            //ord.ProductSku.ProductSkuUsers.Any(u => u.UserId == LoggedInUserId) &&
            //ord.SellerStatus != ((int)OrderSellerStatus.Pending)).CountAsync();
            if (aggregatedItems.Count > 0)
            {
                var remainingItems = awaitingPickup.Concat(pickup).Concat(ready).Concat(shiped).Concat(returned)
                    .ToList();
                decimal remainingPercentage = GetPercentageValue(Convert.ToDecimal(remainingItems.Count),Convert.ToDecimal(aggregatedItems.Count));
                modelResponse.TotalMoneyEarned = aggregatedItems.Sum(o => o.Amount);
                modelResponse.PendingOrders = pendingOrderItems.Count();
                modelResponse.ReadyToPickupOrders = readyToPick.Count();
                var totalListings = _dbContext.ProductSkuUsers.Count();
                modelResponse.ActivityListings = _dbContext.ProductSkuUsers.Where(x => x.UserId == id && x.IsActive == true).Count();
                if (modelResponse.PendingOrders != null && modelResponse.PendingOrders > 0)
                {
                    modelResponse.PercentagePendingOrders = GetPercentageValue(modelResponse.PendingOrders.Value, aggregatedItems.Count) + remainingPercentage/2;
                }
                if (modelResponse.ReadyToPickupOrders != null && modelResponse.ReadyToPickupOrders > 0)
                {
                    modelResponse.PercentageReadyToPickupOrders = GetPercentageValue(modelResponse.ReadyToPickupOrders.Value, aggregatedItems.Count) 
                        + remainingPercentage / 2;
                }
            }
            modelResponse.Status = 200;
            modelResponse.Message = "Success";
            return modelResponse;

        }
        public decimal GetPercentageValue(decimal val,decimal totalVal) {
            return (val * 100) / totalVal;
        }
        public async Task<int> SendNotification(string fcmToken)
        {

            await NotificationHelper.SendNotification(fcmToken, "Hi", "How can I help you sir?");
            return 1;
            //CopyViewModelToEntity(product, productEntity);
        }
        public async Task<int> ReadyToPickAsync(int orderId, int skuId)
        {

            var orderEntity = _dbContext.OrderItems.Include(o => o.ProductSku).ThenInclude(p => p.ProductSkuUsers).FirstOrDefault(u => u.OrderId == orderId && u.SkuId == skuId);
            if (orderEntity != null)
            {
                orderEntity.SellerStatus = Convert.ToInt32(OrderSellerStatus.ReadyToPick);

                await _dbContext.SaveChangesAsync();
                return orderEntity.OrderId;
            }
            return 0;
        }
        public async Task<List<PaymentItemsResponse>> GetPaymentsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                startDate = startDate.Value.Date;
                endDate = endDate.Value.Date;
                endDate = endDate.Value.Add(DateTime.MaxValue.TimeOfDay);
            }

            return await _dbContext.OrderItems
                                            .Include(x => x.Orders)
                                            .Include(o => o.ProductSku).ThenInclude(x => x.ProductSkuReturns)
                                            .Where(x => ((!id.HasValue && !numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue) ||
                                                         (id > 0 && x.OrderId == id) ||
                                                         (startDate.HasValue && endDate.HasValue && startDate.Value <= x.Orders.PickupDate && endDate.Value >= x.Orders.PickupDate) ||
                                                         (numberOfDays.HasValue && DateTime.UtcNow.Date.AddDays(-1 * numberOfDays.Value) <= x.PickupDate && DateTime.Now >= x.PickupDate)
                                                        )
                                              && x.SellerStatus != ((int)OrderSellerStatus.Returned)
                                              && x.ProductSku.ProductSkuUsers.Any(u => u.UserId == LoggedInUserId)
                                             )
           .GroupBy(g => new { g.Orders.PickupDate, g.ProductSku.ProductId, g.ProductSku.Name, g.SkuId }).Select(item => new PaymentItemsResponse()
           {
               ProductId = item.Key.ProductId,
               ProductName = item.Key.Name,
               SkuId = item.Key.SkuId,
               PickupDate = item.Key.PickupDate,
               ProductImage = _dbContext.ProductSkus.FirstOrDefault(x => x.Id == item.Key.SkuId).Image,
               Quantity = item.Sum(i => i.Quantity),
               Price = item.Average(i => i.Price),
               Discount = item.Sum(i => i.Discount),
               Amount = item.Sum(i => i.PaidSellerAmount) ?? 0,

           }).Take(50).OrderByDescending(p => p.PickupDate).ToListAsync();

        }
        public async Task<List<ReturnItemsResponse>> GetAwaitingReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                startDate = startDate.Value.Date;
                endDate = endDate.Value.Date;
                endDate = endDate.Value.Add(DateTime.MaxValue.TimeOfDay);
            }

            return await _dbContext.ProductSkuUsersReturns.Include(x => x.Orders).ThenInclude(o => o.OrderItems)
                                              .Include(o => o.ProductSku)
                                              .Where(x => ((!id.HasValue && !numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue) ||
                                                           (id > 0 && x.OrderId == id) ||
                                                           (startDate.HasValue && endDate.HasValue && startDate.Value <= x.Orders.DeliveredAt && endDate.Value >= x.Orders.DeliveredAt) ||
                                                           (numberOfDays.HasValue && DateTime.UtcNow.Date.AddDays(-1 * numberOfDays.Value) <= x.Orders.DeliveredAt && DateTime.Now >= x.Orders.DeliveredAt))
                                                        && !x.ReceivedDate.HasValue
                                                        && x.ProductSku.ProductSkuUsers.Any(u => u.UserId == LoggedInUserId)
                                                        )
            .GroupBy(g => new { g.Orders.DeliveredAt, g.ProductSku.ProductId, g.ProductSku.Name, g.SkuId }).Select(item => new ReturnItemsResponse()
            {
                ProductId = item.Key.ProductId,
                ProductName = item.Key.Name,
                SkuId = item.Key.SkuId,
                DeliveredAt = item.Key.DeliveredAt,
                ProductImage = _dbContext.ProductSkus.FirstOrDefault(x => x.Id == item.Key.SkuId).Image,
                ReturnedQuantity = item.Sum(i => (i.ReturnQuantity ?? 0)),
                OrderedQuantity = item.Sum(i => i.Quantity),
                DeliveredQuantity = item.Sum(i => i.Quantity) - item.Sum(i => i.ReturnQuantity)

            }).Take(50).OrderByDescending(p => p.DeliveredAt).ToListAsync();

        }
        public async Task<List<ReturnItemsResponse>> GetReturnsAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                startDate = startDate.Value.Date;
                endDate = endDate.Value.Date;
                endDate = endDate.Value.Add(DateTime.MaxValue.TimeOfDay);
            }
            return await _dbContext.ProductSkuUsersReturns.Include(x => x.Orders).ThenInclude(o => o.OrderItems)
                                              .Include(o => o.ProductSku)
                                              .Where(x => ((!id.HasValue && !numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue) ||
                                                            (id > 0 && x.OrderId == id) ||
                                                            (startDate.HasValue && endDate.HasValue && startDate.Value <= x.Orders.DeliveredAt && endDate.Value >= x.Orders.DeliveredAt) ||
                                                            (numberOfDays.HasValue && DateTime.UtcNow.Date.AddDays(-1 * numberOfDays.Value) <= x.Orders.DeliveredAt && DateTime.Now >= x.Orders.DeliveredAt))
                                                        && x.ReceivedDate.HasValue
                                                        && x.ProductSku.ProductSkuUsers.Any(u => u.UserId == LoggedInUserId)
                                                        )
            .GroupBy(g => new { g.Orders.DeliveredAt, g.ProductSku.ProductId, g.ProductSku.Name, g.SkuId }).Select(item => new ReturnItemsResponse()
            {
                ProductId = item.Key.ProductId,
                ProductName = item.Key.Name,
                SkuId = item.Key.SkuId,
                DeliveredAt = item.Key.DeliveredAt,
                ProductImage = _dbContext.ProductSkus.FirstOrDefault(x => x.Id == item.Key.SkuId).Image,
                OrderedQuantity = item.Sum(i => i.Quantity),
                ReturnedQuantity = item.Sum(i => (i.ReturnQuantity ?? 0)),
                DeliveredQuantity = item.Sum(i => i.Quantity) - item.Sum(i => i.ReturnQuantity),
                //Amount = item.Sum(i => i.Orders.OrderItems.Where(r=>r.SkuId==item.Key.SkuId).Sum(oi=>oi.PaidSellerAmount))??0
            }).Take(50).OrderByDescending(p => p.DeliveredAt).ToListAsync();

        }
        public async Task<List<OrderItemsResponse>> GetOrdersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            return await _dbContext.OrderItems
                                    .Include(o => o.ProductSku).ThenInclude(x => x.ProductSkuUsers)
                                    .Where(x => ((!id.HasValue && !numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue) ||
                                                (id > 0 && x.OrderId == id) ||
                                                (startDate.HasValue && endDate.HasValue && startDate.Value.Date <= x.PickupDate && endDate.Value.Date >= x.PickupDate) ||
                                                (numberOfDays.HasValue && DateTime.UtcNow.Date.AddDays(-1 * numberOfDays.Value) <= x.PickupDate && DateTime.Now >= x.PickupDate))
                                               && x.ProductSku.ProductSkuUsers.Any(u => u.UserId == LoggedInUserId)
                                               && x.SellerStatus != Convert.ToInt32(OrderSellerStatus.Returned)
                                               )
             .Select(item => new OrderItemsResponse()
             {
                 OrderId = item.OrderId,
                 OrderNumber = item.Orders.OrderNumber,
                 PickupDate = item.PickupDate,
                 ProductId = item.ProductSku.ProductId,
                 ProductName = item.ProductSku.Name,
                 SkuId = item.SkuId,
                 ProductImage = item.ProductSku.Image,
                 Quantity = item.Quantity,
                 Price = item.Price,
                 Discount = item.Discount,
                 Amount = item.PaidSellerAmount ?? 0,
                 Status = item.SellerStatus,
                 ListedQuantity = item.ProductSku.ProductSkuUsers.FirstOrDefault(p => p.UserId == LoggedInUserId).Quantity, //
             }).Take(50).OrderByDescending(p => p.PickupDate).ToListAsync();
        }
        public async Task<List<OrderItemsResponse>> GetOrdersByStatusAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate, int? sellerStatus)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                startDate = startDate.Value.Date;
                endDate = endDate.Value.Date;
                endDate = endDate.Value.Add(DateTime.MaxValue.TimeOfDay);
            }

            return await _dbContext.OrderItems
                .Include(x=>x.ProductSku).ThenInclude(x=>x.ProductSkuUsers)
                .Include(x => x.Orders)
                .Where(x => ((!id.HasValue && !numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue) ||
                             (id > 0 && x.OrderId == id) ||
                             (startDate.HasValue && endDate.HasValue && startDate.Value <= x.Orders.CreatedDate && endDate.Value >= x.Orders.CreatedDate) ||
                             (numberOfDays.HasValue && DateTime.UtcNow.Date.AddDays(-1 * numberOfDays.Value) <= x.Orders.CreatedDate && DateTime.Now >= x.Orders.CreatedDate))
                                                && x.ProductSku.ProductSkuUsers.Any(u => u.UserId == LoggedInUserId)
                                                && x.SellerStatus == sellerStatus
                                               )
             .Take(50).Select(item => new OrderItemsResponse()
             {
                 OrderId = item.OrderId,
                 OrderNumber = item.Orders.OrderNumber,
                 PickupDate = item.PickupDate,
                 ProductId = item.ProductSku.ProductId,
                 ProductName = item.ProductSku.Name,
                 SkuId = item.SkuId,
                 ProductImage = item.ProductSku.Image,
                 Amount = item.PaidSellerAmount ?? 0,
                 Quantity = item.Quantity,
                 Price = item.Price,
                 Discount = item.Discount,
                 Status = item.SellerStatus,
                 ListedQuantity = item.ProductSku.ProductSkuUsers.FirstOrDefault(p => p.UserId == LoggedInUserId).Quantity, //
             }).OrderByDescending(p => p.PickupDate).ToListAsync();
        }

        public async Task<List<InventoryItemsResponse>> GetInventoryAsync()
        {
            return await _dbContext.ProductSkuUsers.Where(x => x.UserId == LoggedInUserId)
            .Select(item => new InventoryItemsResponse()
            {
                ProductId = item.ProductSku.ProductId,
                SkuId = item.SkuId,
                ProductName = item.ProductSku.Name,
                ProductImage = item.ProductSku.Image,
                ListedQuantity = item.Quantity,
                //InventoryRemaining = item.ProductSku.Stock??0,
                OrderedQuantity = item.ProductSku.OrderItems.Where(o => o.SellerStatus != Convert.ToInt32(OrderSellerStatus.Returned) && o.SellerStatus != Convert.ToInt32(OrderSellerStatus.Pending)).Sum(o => o.Quantity),
                SendLowInventoryNotification = item.SendLowInventoryNotification,
                MinimumQuantity = item.MinimumQuantity,
                ReminderQuantity = item.ReminderQuantity

            }).Take(50).ToListAsync();
        }

        public InventoryItemsResponse GetSKUInventory(int skuId)
        {
            return _dbContext.ProductSkuUsers.Where(x => x.SkuId == skuId && x.UserId == LoggedInUserId)
            .Select(item => new InventoryItemsResponse()
            {
                ProductId = item.ProductSku.ProductId,
                SkuId = item.SkuId,
                MinimumQuantity = item.MinimumQuantity,
                ProductName = item.ProductSku.Name,
                ProductImage = item.ProductSku.Image,
                ListedQuantity = item.Quantity,
                //InventoryRemaining = item.ProductSku.Stock ?? 0,
                OrderedQuantity = item.ProductSku.OrderItems.Where(o => o.SellerStatus != Convert.ToInt32(OrderSellerStatus.Returned) && o.SellerStatus != Convert.ToInt32(OrderSellerStatus.Pending)).Sum(o => o.Quantity),
                SendLowInventoryNotification = item.SendLowInventoryNotification
            }).FirstOrDefault();
        }
        #endregion
    }
}
