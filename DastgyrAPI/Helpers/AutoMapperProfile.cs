using AutoMapper;
using DastgyrAPI.Entity;
using DastgyrAPI.Models.ViewModels.RequestModels;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DastgyrAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductSkuUsers, ProductSkuUsersRequest>().ReverseMap();
            CreateMap<ProductSku, ProductSkuReponse>().ForMember(dest =>
                    dest.SkuId,
                    opt => opt.MapFrom(src => src.Id)); ;
            CreateMap<Products, ProductsRequest>().ReverseMap();
            CreateMap<Products, ProductsResponse>().ReverseMap();
            CreateMap<Products, ProductSku>().ReverseMap();
            CreateMap<Users, LoginResponse>().ReverseMap();
            CreateMap<ProductSkuUserItemsResponse, ProductSku>().ReverseMap()
                .ForMember(dest =>
                    dest.SkuId,
                    opt => opt.MapFrom(src => src.Id));

        }
    }
}
