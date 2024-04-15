using System.Linq;
using AutoMapper;
using ShoppingApp.Entities;
using ShoppingCart.Models;

namespace ShoppingCart.Mapping
{
    public class DomainToModelMappingProfile : Profile
    {
        public DomainToModelMappingProfile()
        {
            CreateMap<UserEntity, User>();
            CreateMap<CategoryEntity, Category>();
            CreateMap<SubCategoryEntity, SubCategory>();
            CreateMap<ProductEntity, Product>()
                .ForMember(s => s.Category, d => d.MapFrom(e => e.SubCategory.Name));
            //     .ForMember(s => s.PreviewImage, d => d.MapFrom(e => e.PreviewImages.FirstOrDefault().PreviewImage));
            //   CreateMap<PreviewImageEntity, PreviewImage>()
            //     .ForMember(s => s.Image, d => d.MapFrom(e => e.PreviewImage));

            CreateMap<BannerEntity, Banner>();

        }
    }
}