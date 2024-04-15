using AutoMapper;
using ShoppingAPI.Models;
using ShoppingApp.Entities;

namespace ShoppingAPI.Mappings
{
    /// <summary>
    /// This class is responsible for the model to domain mapping profile.
    /// </summary>
    public class ModelToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Initializes the model domain mapping profile.
        /// </summary>
        public ModelToDomainMappingProfile()
        {
            CreateMap<User, UserEntity>();
            CreateMap<Category, CategoryEntity>()
                .ForMember(s => s.ID, d => d.MapFrom(e => e.ID))
                .ForMember(s => s.Name, d => d.MapFrom(e => e.Name))
                .ForMember(s => s.Icon, d => d.MapFrom(e => e.Icon))
                .ForMember(s => s.SubCategories, d => d.MapFrom(e => e.SubCategories));
            CreateMap<SubCategory, SubCategoryEntity>();
            CreateMap<PreviewImage, PreviewImageEntity>()
                .ForMember(s => s.PreviewImage, d => d.MapFrom(e => e.Image));
            CreateMap<Product, ProductEntity>();
            CreateMap<Brand, BrandEntity>();
            CreateMap<UserWishlist, UserWishlistEntity>();
        }
    }
}