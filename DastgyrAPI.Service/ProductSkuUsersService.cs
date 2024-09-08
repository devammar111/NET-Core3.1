using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Interfaces.Services;
using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DastgyrAPI.Services
{
    public class ProductSkuUsersService : IProductSkuUsersService
    {
        #region Initialization
        private IProductSkuUsersRepository _ProductSkuUsersRepository;
        public ProductSkuUsersService(IProductSkuUsersRepository ProductSkuUsersRepository, IHostingEnvironment hostingEnvironment)
        {
            _ProductSkuUsersRepository = ProductSkuUsersRepository;
        }
        #endregion

        #region Implementation Methods

        public async Task<int> AddProductSkuUsersAsync(ProductSkuUsersRequest product)
        {
            return await _ProductSkuUsersRepository.AddProductSkuUsersAsync(product);
        }
        public async Task<int> UpdateListedQuantityAsync(UpdateListedQuantityRequest product)
        {
            return await _ProductSkuUsersRepository.UpdateListedQuantityAsync(product);
        }
        public async Task<int> SetMinimumQuantityNotificationAsync(SetMinimumQuantityNotificationRequest product)
        {
            return await _ProductSkuUsersRepository.SetMinimumQuantityNotificationAsync(product);
        }
        public async Task<List<ProductSkuUserItemsResponse>> GetAllProductSkuUsersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate)
        {
            return await _ProductSkuUsersRepository.GetAllProductSkuUsersAsync(id,numberOfDays, startDate, endDate);
        }
        public async Task<List<ProductSkuReponse>> GetProductSkusAsync(int? id,string productName)
        {
            return await _ProductSkuUsersRepository.GetProductSkusAsync(id,productName);
        }
        #endregion

    }
}
