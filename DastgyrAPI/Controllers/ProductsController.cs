using DastgyrAPI.Interfaces.Services;

namespace DastgyrAPI.Controllers
{
    public class ProductsController : BaseController
    {
        private IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

    }
}