using AutoMapper;
using DastgyrAPI.Common;
using DastgyrAPI.Entity.HelperModels;
using DastgyrAPI.Interfaces.Services;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using DastgyrAPI.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DastgyrAPI.Controllers
{

    public class OrdersController : BaseController
    {
        private LinkGenerator _linkGenerator;
        private IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrdersController(LinkGenerator linkGenerator, IOrdersService ordersService, IMapper mapper)
        {
            _linkGenerator = linkGenerator;
            _ordersService = ordersService;
        }

        // GET: api/Payments/
        [HttpGet("Payments")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Payments(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            //if (id == 0)
            //{
            //    return StatusCode(500, new { status = 400, message = "Invalid Request" });
            //}
            var orderItems = await _ordersService.GetPaymentsAsync(id, numberOfDays, startDate, endDate);
            double? totalMoneyEarned = 0;
            if (orderItems.Count > 0)
                totalMoneyEarned = orderItems.Sum(o => o.Amount);

            return Ok(new PaymentResponse() { Status = 200, Message = "Success", TotalMoneyEarned = totalMoneyEarned, PaymentsData = orderItems.ToList() });
        }

        // GET: api/Returns/
        [HttpGet("Returns")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Returns(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {

            //if (!id.HasValue && !numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue)
            //{
            //    return StatusCode(500, new { status = 400, message = "Invalid Request" });
            //}
            var returnItems = await _ordersService.GetReturnsAsync(id, numberOfDays, startDate, endDate);
            var awaitingReturnItems = await _ordersService.GetAwaitingReturnsAsync(id, numberOfDays, startDate, endDate);
            double? totalMoneyEarned = 0;
            if (returnItems.Count > 0)
                totalMoneyEarned = returnItems.Sum(o => o.Amount);

            return Ok(new ReturnsResponse() { Status = 200, Message = "", TotalMoneyEarned = totalMoneyEarned, Returns = returnItems.ToList(), AwaitingReturns = awaitingReturnItems.ToList() });
        }

        // GET: api/Payments/
        [HttpGet("Orders")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrdersResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Orders(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {

            //if (id == 0)
            //{
            //    return StatusCode(500, new { status = 400, message = "Invalid Request" });
            //}

            var pendingOrderItems = await _ordersService.GetPendingOrdersAsync(id, numberOfDays, startDate, endDate, Convert.ToInt32(OrderSellerStatus.Pending));
            var awaitingPickup = await _ordersService.GetPendingOrdersAsync(id, numberOfDays, startDate, endDate, Convert.ToInt32(OrderSellerStatus.AwaitingPickup));
            var readyToPick = await _ordersService.GetPendingOrdersAsync(id, numberOfDays, startDate, endDate, Convert.ToInt32(OrderSellerStatus.ReadyToPick));
            var pickup = await _ordersService.GetPendingOrdersAsync(id, numberOfDays, startDate, endDate, Convert.ToInt32(OrderSellerStatus.PickUp));
            //var shiped = await _ordersService.GetPendingOrdersAsync(id, numberOfDays, startDate, endDate, Convert.ToInt32(OrderSellerStatus.Shipped));
            var ready = await _ordersService.GetPendingOrdersAsync(id, numberOfDays, startDate, endDate, Convert.ToInt32(OrderSellerStatus.Ready));


            var aggregatedItems = pendingOrderItems.Concat(awaitingPickup).Concat(readyToPick).Concat(pickup).Concat(ready)
                                .GroupBy(g => new { g.ProductId, g.ProductName, g.SkuId, g.ProductImage })
                                                            .Select(item => new AggregatedItemsResponse()
                                                            {
                                                                ProductId = item.Key.ProductId,
                                                                ProductName = item.Key.ProductName,
                                                                SkuId = item.Key.SkuId,
                                                                ProductImage = item.Key.ProductImage,
                                                                ListedQuantity = item.Sum(i => i.ListedQuantity),
                                                                OrderedQuantity = item.Sum(i => i.Quantity)
                                                            }).OrderBy(p=>p.ProductName).ToList(); 


            return Ok(new OrdersResponse() { Status = 200, Message = "Success", Aggregated = aggregatedItems, Pending = pendingOrderItems, Ready = ready, Shipped = pickup, ReadyToPick = readyToPick, AwaitingPickup = awaitingPickup});

        }

        [HttpGet("Inventory")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InventoryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Inventory()
        {

            var orderItems = await _ordersService.GetInventoryAsync();
            return Ok(new InventoryResponse() { Status = 200, Message = "Success", InventoryData = orderItems.ToList() });

        }

        [HttpPut("AcceptOrder")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Put(int orderId, int skuId)
        {

            var type = typeof(BadRequestObjectResult);
            if (orderId == 0)
            {
                return StatusCode(500, new { status = 400, message = "Invalid Request" });
            }
            if (ModelState.IsValid)
            {
                var updatedProduct = await _ordersService.AcceptOrderAsync(orderId, skuId);
                if (updatedProduct > 0)
                {
                    return Ok(new ServiceResponse() { StatusCode = 200, Message = "Order accepted ", IsValid = true });
                }
                ModelState.AddModelError("Server Error!", "Server Error Occur While Adding Product.");
            }
            return await HelperMethods.ResponseBasedOnType(type, ModelState);


        }

        [HttpPut("SendNotification")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> SendNotification(string fcmToken)
        {

            var type = typeof(BadRequestObjectResult);
            if (string.IsNullOrEmpty(fcmToken))
            {
                return StatusCode(500, new { status = 400, message = "Invalid Request" });
            }
            if (ModelState.IsValid)
            {
                var updatedProduct = await _ordersService.SendNotification(fcmToken);
                if (updatedProduct > 0)
                {
                    return Ok(new ServiceResponse() { StatusCode = 200, Message = "Success ", IsValid = true });
                }
                ModelState.AddModelError("Server Error!", "Server Error Occur While");
            }
            return await HelperMethods.ResponseBasedOnType(type, ModelState);


        }

        [HttpPut("ReadyToPickAsync")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ReadyToPickAsync(int orderId, int skuId)
        {
            var type = typeof(BadRequestObjectResult);
            if (orderId == 0)
            {
                return StatusCode(500, new { status = 400, message = "Invalid Request" });
            }
            if (ModelState.IsValid)
            {
                var updatedProduct = await _ordersService.ReadyToPickAsync(orderId, skuId);
                if (updatedProduct > 0)
                {
                    return Ok(new ServiceResponse() { StatusCode = 200, Message = "Order is ready to pick ", IsValid = true });
                }
                ModelState.AddModelError("Server Error!", "Server Error Occur While Adding Product.");
            }
            return await HelperMethods.ResponseBasedOnType(type, ModelState);


        }
    }

}
