using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DastgyrAPI.Common;
using DastgyrAPI.Interfaces.Services;
using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using DastgyrAPI.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DastgyrAPI.Controllers
{

    public class ProductSkuUsersController : BaseController
    {
        private LinkGenerator _linkGenerator;
        private IProductSkuUsersService _productSkuUsersService;

        public ProductSkuUsersController(LinkGenerator linkGenerator, IProductSkuUsersService productSkuUsersService)
        {
            _linkGenerator = linkGenerator;
            _productSkuUsersService = productSkuUsersService;
        }

        [HttpGet("GetAllProductSkuUsers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductSkuUsersResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAllProductSkuUsers(int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            //if (!numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue)
            //{
            //    return StatusCode(500, new { status = 400, message = "Invalid Request" });
            //}
            var orderItems = await _productSkuUsersService.GetAllProductSkuUsersAsync(null, numberOfDays, startDate, endDate);
            //if(orderItems==null || orderItems.Count==0)
            //    return NotFound(new { Status = 404, Message = "Product being searched not found" });
            return Ok(new ProductSkuUsersResponse() { Status = 200, Message = "Success", SKU = orderItems.ToList() });
        }

        // POST: api/Product
        [HttpPost("AddProductSkuUser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddProductSkuUsersResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Post([FromBody] ProductSkuUsersRequest product)
        {
            var type = typeof(BadRequestObjectResult);
            if (ModelState.IsValid)
            {
                var productId = await _productSkuUsersService.AddProductSkuUsersAsync(product);
                if (productId > 0)
                {
                    var getNewlyAddedSensorInfoResponse = await _productSkuUsersService.GetAllProductSkuUsersAsync(productId, null, null, null);
                    return Ok(new AddProductSkuUsersResponse() { Status = 200, Message = "Success", ProductSkuUser = getNewlyAddedSensorInfoResponse.FirstOrDefault() });
                }
                ModelState.AddModelError("Server Error!", "Server Error Occur While Adding Product.");
            }
            return await HelperMethods.ResponseBasedOnType(type, ModelState);
        }


        [HttpGet("Products")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductsListResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Products(int? id,string productName)
        {

            //if (id == 0)
            //{
            //    return StatusCode(500, new { status = 400, message = "Invalid Request" });
            //}
            var orderItems = await _productSkuUsersService.GetProductSkusAsync(id,productName);
            //if (orderItems == null || orderItems.Count == 0)
            //    return NotFound(new ProductSkuUsersResponse() { Status = 404, Message = "Product being searched not found", SKU = new List<ProductSkuUserItemsResponse>() });
            return Ok(new ProductsListResponse() { Status = 200, Message = "Success", Products = orderItems.ToList() });
        }

        [HttpPut("SetMinimumQuantityNotification")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> SetMinimumQuantityNotification([FromBody] SetMinimumQuantityNotificationRequest product)
        {
            var type = typeof(BadRequestObjectResult);
            if (product.SkuId == 0)
            {
                return StatusCode(500, new { status = 400, message = "Invalid Request" });
            }
            if (ModelState.IsValid)
            {
                var updatedProduct = await _productSkuUsersService.SetMinimumQuantityNotificationAsync(product);
                if (updatedProduct > 0)
                {
                    return Ok(new ServiceResponse() { StatusCode = 200, Message = "Record updated ", IsValid = true });
                }
                ModelState.AddModelError("Server Error!", "Server Error Occur While Adding Product.");
            }
            return await HelperMethods.ResponseBasedOnType(type, ModelState);


        }

        [HttpPut("UpdateListedQuantity")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateListedQuantityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateListedQuantity(int skuId, int quantity)
        {
            UpdateListedQuantityRequest product = new UpdateListedQuantityRequest() { SkuId = skuId, Quantity = quantity };
            var type = typeof(BadRequestObjectResult);
            if (product.SkuId == 0)
            {
                return StatusCode(500, new { status = 400, message = "Invalid Request" });
            }
            if (ModelState.IsValid)
            {
                var updatedProduct = await _productSkuUsersService.UpdateListedQuantityAsync(product);
                if (updatedProduct > 0)
                {
                    return Ok(new UpdateListedQuantityResponse() { Status = 200, Message = "Request sent for approval "});
                }
                ModelState.AddModelError("Server Error!", "Server Error Occur While Adding Product. Product is not associated with user.");
            }
            return await HelperMethods.ResponseBasedOnType(type, ModelState);


        }
    }
}