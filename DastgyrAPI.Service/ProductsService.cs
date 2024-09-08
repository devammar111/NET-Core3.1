using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Interfaces.Services;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DastgyrAPI.Services
{
    public class ProductsService : IProductsService
    {
        #region Initialization
        private IProductsRepository _productsRepository;
        public ProductsService(IProductsRepository productsRepository, IHostingEnvironment hostingEnvironment)
        {
            _productsRepository = productsRepository;
        }
        #endregion

        #region Implementation Methods

        
        #endregion

    }
}
