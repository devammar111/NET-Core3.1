using AutoMapper;
using DastgyrAPI.Entity;
using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using DastgyrAPI.Entity.HelperModels;

namespace DastgyrAPI.Repositories
{
    public class ProductSkuUsersRepository : BaseRepository<ProductSkuUsers>, IProductSkuUsersRepository
    {
        #region Initialization
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ProductSkuUsersRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dbContext, httpContextAccessor)

        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        #endregion

        #region Implementation Methods
        public async Task<int> AddProductSkuUsersAsync(ProductSkuUsersRequest userInput)
        {
            //var productSku = _dbContext.ProductSkus.FirstOrDefault(u => u.ProductId == product.ProductId);
            //if (productSku == null)
            //{

            //}
            var productSkuEntity = _dbContext.ProductSkus.FirstOrDefault(u => u.Id == userInput.SkuId);
            ProductSku newProductSku = _mapper.Map<ProductSku>(productSkuEntity);
            newProductSku.Id = 0;
            //productSku.ProductId = product.ProductId;
            newProductSku.MaxLimit = userInput.Quantity;
            newProductSku.Price = userInput.Price;
            newProductSku.Stock = userInput.Quantity;
            newProductSku.Discount = 0;
            newProductSku.IsActive = false;
            newProductSku.CreatedDate = DateTime.Now;
            newProductSku.ModifiedDate = DateTime.Now;
            newProductSku.DeletedDate = null;
            
            _dbContext.ProductSkus.Add(newProductSku);
            var productSkuUsers = _mapper.Map<ProductSkuUsers>(userInput);
            productSkuUsers.Id = newProductSku.ProductId??0;
            productSkuUsers.ProductSku = newProductSku;
            productSkuUsers.UserId = LoggedInUserId;
            productSkuUsers.IsActive = false;
            _dbContext.Entry(newProductSku).State = EntityState.Added;
            _dbContext.Entry(productSkuUsers).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
            if (!string.IsNullOrEmpty(productSkuEntity.Code))
            {
                newProductSku.Code = GetNewCode(productSkuEntity.Code, newProductSku.Id);
                await _dbContext.SaveChangesAsync();
            }
            return productSkuUsers.SkuId??0;
            //CopyViewModelToEntity(product, productEntity);


        }
        public string GetNewCode(string oldCode,int newSkuId)
        {
            string code = oldCode;
            string[] textSplit = code.Split('-');
            if (textSplit.Count() > 0)
            {
                textSplit[textSplit.Length - 1] = newSkuId.ToString();
                string joined = string.Join("-", textSplit);
                return joined;
            }
            return "";
        }
        public async Task<int> UpdateListedQuantityAsync(UpdateListedQuantityRequest product)
        {

            var productSkuUsers = _dbContext.ProductSkuUsers.FirstOrDefault(u => u.SkuId == product.SkuId && u.UserId == LoggedInUserId);
            //var productSkuUsers = _dbContext.ProductSkuUsers.FirstOrDefault(u => u.id == product.ProductId);
            if (productSkuUsers != null)
            {
                SellerChangeRequests sellerChangeRequests = new SellerChangeRequests();
                sellerChangeRequests.SkuId = productSkuUsers.SkuId ?? 0;
                //sellerChangeRequests.AllowedQuantity = productSkuUsers.MinimumQuantity ?? 0;
                sellerChangeRequests.RequestedQuantity = product.Quantity;
                sellerChangeRequests.SellerId = LoggedInUserId;
                _dbContext.SellerChangeRequests.Add(sellerChangeRequests);
                await _dbContext.SaveChangesAsync();
                return productSkuUsers.Id;
            }
            //CopyViewModelToEntity(product, productEntity);
            return 0;
        }
        public async Task<int> SetMinimumQuantityNotificationAsync(SetMinimumQuantityNotificationRequest product)
        {
            var productSkuUsers = _dbContext.ProductSkuUsers.OrderByDescending(oi=>oi.CreatedDate).FirstOrDefault(u => u.SkuId == product.SkuId && u.UserId == LoggedInUserId);
            //var productSkuUsers = _dbContext.ProductSkuUsers.FirstOrDefault(u => u.id == product.ProductId);
            if (productSkuUsers != null)
            {
                productSkuUsers.ReminderQuantity = product.ReminderQuantity;
                productSkuUsers.SendLowInventoryNotification = product.SendLowInventoryNotification;
                await _dbContext.SaveChangesAsync();
                return productSkuUsers.Id;
            }
            //CopyViewModelToEntity(product, productEntity);
            return 0;
        }
        public async Task<List<ProductSkuUserItemsResponse>> GetAllProductSkuUsersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                startDate = startDate.Value.Date;
                endDate = endDate.Value.Date;
                endDate = endDate.Value.Add(DateTime.MaxValue.TimeOfDay);
            }

            return await _dbContext.ProductSkuUsers.Include(p=>p.ProductSku).Where(x => (
                                                                ((!numberOfDays.HasValue && !startDate.HasValue && !endDate.HasValue) ||
                                                                (startDate.HasValue && endDate.HasValue && startDate.Value <= x.CreatedDate && endDate.Value >= x.CreatedDate) ||
                                                                 (numberOfDays.HasValue && DateTime.UtcNow.Date.AddDays(-1 * numberOfDays.Value) <= x.CreatedDate && DateTime.Now >= x.CreatedDate)))
                                          && x.UserId == LoggedInUserId)
                          .OrderByDescending(p => p.CreatedDate).Select(item => new ProductSkuUserItemsResponse()
                          {
                              UserId = LoggedInUserId,
                              SkuId = item.SkuId,
                              Code = item.ProductSku.Code,
                              Name = item.ProductSku.Name,
                              Image = item.ProductSku.Image,
                              Quantity = item.Quantity,
                              OrderedQuantity = item.ProductSku.OrderItems.Where(o => o.SellerStatus != Convert.ToInt32(OrderSellerStatus.Returned) && o.SellerStatus != Convert.ToInt32(OrderSellerStatus.Pending)).Sum(o => o.Quantity),
                              Price = item.Price,
                              ProductId = item.Id,
                              ExpiryDate = item.ExpiryDate ?? DateTime.Now,
                              MinimumQuantity = item.MinimumQuantity,
                              StockAvailabilityDate = item.StockAvailabilityDate ?? DateTime.Now,
                              IsActive = item.IsActive,
                              ListingDate = item.CreatedDate,
                              ReminderQuantity = item.ReminderQuantity

                          }).Take(50).ToListAsync();
        }
        public async Task<List<ProductSkuReponse>> GetProductSkusAsync(int? id,string productName)
        {

            return await _dbContext.ProductSkus.Where(x => (( !id.HasValue || id.Value == 0) && String.IsNullOrEmpty(productName)) ||
                                                           ((id > 0 && x.Id == id)) ||
                                                           (!String.IsNullOrEmpty(productName) && x.Name.ToLower().StartsWith(productName.ToLower())))
                          .ProjectTo<ProductSkuReponse>(_mapper.ConfigurationProvider).OrderBy(p=>p.Name).Take(100).ToListAsync();
        }
        #endregion

    }
}
