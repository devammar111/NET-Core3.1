using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DastgyrAPI.Interfaces.Services
{
    public interface IProductSkuUsersService
    {
        Task<int> AddProductSkuUsersAsync(ProductSkuUsersRequest product);
        Task<int> UpdateListedQuantityAsync(UpdateListedQuantityRequest product);
        Task<int> SetMinimumQuantityNotificationAsync(SetMinimumQuantityNotificationRequest product);
        Task<List<ProductSkuUserItemsResponse>> GetAllProductSkuUsersAsync(int? id, int? numberOfDays, DateTime? startDate, DateTime? endDate);
        Task<List<ProductSkuReponse>> GetProductSkusAsync(int? id,string productName);
    }
}
