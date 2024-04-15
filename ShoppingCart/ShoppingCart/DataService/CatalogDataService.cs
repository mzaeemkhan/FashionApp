using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using ShoppingApp.Entities;
using ShoppingCart.Models;
using Xamarin.Forms.Internals;

namespace ShoppingCart.DataService
{
    /// <summary>
    /// Data service to load the data from database using Web API.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class CatalogDataService : ICatalogDataService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Creates an instance for the <see cref="CatalogDataService" /> class.
        /// </summary>
        public CatalogDataService()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// This method is to add the recent product.
        /// </summary>
        public async Task<Status> AddRecentProduct(int userId, int productId)
        {
            var status = new Status();
            try
            {
                var uri = new UriBuilder($"{App.BaseUri}products/addrecent?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT&userId={userId}&productId={productId}");
                var response = await httpClient.PostAsync(uri.ToString(), null);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null) status = JsonConvert.DeserializeObject<Status>(result);
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                status.IsSuccess = false;
                status.Message = ex.Message;
            }

            return status;
        }

        /// <summary>
        /// This method is to get the recenet/viewed products.
        /// </summary>
        public async Task<List<Product>> GetRecentProductsAsync()
        {
            var Products = new List<Product>();
            try
            {
                var uri = new UriBuilder($"{App.BaseUri}products/recent?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT");
                var response = await httpClient.GetAsync(uri.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        var productsEntity = JsonConvert.DeserializeObject<List<ProductEntity>>(result);
                        if (productsEntity != null)
                            Products = Mapper.Map<List<ProductEntity>, List<Product>>(productsEntity);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }

            return Products;
        }

        public async Task<List<Product>> GetRecommendedProductsAsync()
        {
            var Products = new List<Product>();
            try
            {
                var uri = new UriBuilder($"{App.BaseUri}products/recommended?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT");
                var response = await httpClient.GetAsync(uri.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        var productsEntity = JsonConvert.DeserializeObject<List<ProductEntity>>(result);
                        if (productsEntity != null)
                            Products = Mapper.Map<List<ProductEntity>, List<Product>>(productsEntity);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }

            return Products;
        }

       


        /// <summary>
        /// This method is to get the product using id.
        /// </summary>
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var Product = new Product();
            try
            {
                var uri = new UriBuilder($"{App.BaseUri}products?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT&productId={productId}");
                var response = await httpClient.GetAsync(uri.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result != null)
                    {
                        var productEntity = JsonConvert.DeserializeObject<ProductEntity>(result);
                        if (productEntity != null) Product = Mapper.Map<ProductEntity, Product>(productEntity);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }

            return Product;
        }
        public class PagedList
        {
            public List<ProductEntity> products { get; }
            public int Totalcounts { get; }

            public PagedList(List<ProductEntity> products, int totalcounts)
            {
                this.products = products;
                Totalcounts = totalcounts;
            }
        }

        /// <summary>
        /// To get product using subcategory id.
        /// </summary>
        public async Task<PagedList> GetProductBySubCategoryIdAsync(int subCategoryId, int index, int count,
            string sort, string filterBrandIds)
    {
            var Products = new PagedList(new List<ProductEntity>(), 0);

            try
            {
                var uri = new UriBuilder($"{App.BaseUri}products?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT&subCategoryId={subCategoryId}&index={index}&count={count}&sort={sort}&filter={filterBrandIds}");
                var response = await httpClient.GetAsync(uri.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        var pagedList = JsonConvert.DeserializeObject<PagedList>(result);
                        if (pagedList != null && pagedList.products != null)
                            return pagedList;
                           
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }

            return Products;
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            var Brands = new List<Brand>();

            try
            {
                var uri = new UriBuilder($"{App.BaseUri}products/brands?key=zmacs3jmjprNC9gba7xf5nZZDg87NM6dLYdaTwzT");
                var response = await httpClient.GetAsync(uri.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        var brands = JsonConvert.DeserializeObject<List<BrandEntity>>(result);
                        if (brands != null) Brands = Mapper.Map<List<BrandEntity>, List<Brand>>(brands);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                throw ex;
            }

            return Brands;
        }
    }
}