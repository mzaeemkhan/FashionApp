using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.DataService
{
    public interface ICatalogDataService
    {
       // Task<List<Product>> GetProductBySubCategoryIdAsync(int subCategoryId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<Status> AddRecentProduct(int userId, int productId);
        Task<List<Product>> GetRecentProductsAsync();
        Task<List<Product>> GetRecommendedProductsAsync();
        Task<CatalogDataService.PagedList> GetProductBySubCategoryIdAsync(int subCategoryId, int index, int count,
            string sort, string filterBrandIds);

        Task<List<Brand>> GetBrandsAsync();
    }
}