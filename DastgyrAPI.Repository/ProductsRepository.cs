using AutoMapper;
using DastgyrAPI.Entity;
using DastgyrAPI.Entity.HelperModels;
using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DastgyrAPI.Repositories
{
    public class ProductsRepository : BaseRepository<Products>, IProductsRepository
    {
        #region Initialization
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ProductsRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dbContext, httpContextAccessor)

        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        #endregion

        #region Implementation Methods

        

        #endregion

    }
}
