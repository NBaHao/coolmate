﻿using CoolMate.DTO;
using CoolMate.Models;
using AutoMapper;

namespace CoolMate.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDTO, SiteUser>()
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.Split(new[] { '@' })[0]));
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductItem, ProductItemDTO>();
            CreateMap<ProductItemImage, ProductItemImageDTO>();
            CreateMap<AddCategoryDTO, ProductCategory>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
}
