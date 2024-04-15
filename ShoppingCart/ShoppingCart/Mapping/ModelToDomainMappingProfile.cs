using AutoMapper;
using ShoppingApp.Entities;
using ShoppingCart.Models;

namespace ShoppingCart.Mapping
{
    public class ModelToDomainMappingProfile : Profile
    {
        public ModelToDomainMappingProfile()
        {
            CreateMap<User, UserEntity>();
            CreateMap<Category, CategoryEntity>();
            CreateMap<SubCategory, SubCategoryEntity>();
            CreateMap<Product, ProductEntity>();
            CreateMap<PreviewImage, PreviewImageEntity>()
                .ForMember(s => s.PreviewImage, d => d.MapFrom(e => e.Image));
            CreateMap<Banner, BannerEntity>();
        }
    }
}