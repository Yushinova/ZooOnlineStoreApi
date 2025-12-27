using AutoMapper;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.QueryBuilders;
using Microsoft.EntityFrameworkCore;

namespace ZooOnlineStoreApi.Services
{
    public class ProductService
    {
        private readonly IProductRepository _products;
        private readonly IPetTypeRepository _petTypes;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository products, IPetTypeRepository petTypes, IMapper mapper)
        {
            _products = products;
            _petTypes = petTypes;
            _mapper = mapper;
        }
        public async Task<ProductResponse> InsertAsync(ProductRequest request)
        {
            Product newProduct = _mapper.Map<Product>(request);
            if (request.PetTypeIds != null && request.PetTypeIds.Any())
            {
                List<PetType> petTypesFromDb = await _petTypes.SelectAllAsync();
                newProduct.PetTypes ??= new HashSet<PetType>();
                foreach (var item in petTypesFromDb)
                {
                    if (request.PetTypeIds.Contains(item.Id))
                    {
                        newProduct.PetTypes.Add(item);
                    }
                }
            }
            Product productFromDb = await _products.InsertAndReturnAsync(newProduct);
            return _mapper.Map<ProductResponse>(productFromDb);
        }
        public async Task<List<ProductResponse>> SuperPagination(ProductQueryParameters parameters)
        {
            if (parameters.Page < 1) parameters.Page = 1;
            if (parameters.PageSize < 1) parameters.PageSize = 10;
            IQueryable<Product> query = _products.SelectAllWithImagesAndPetTypes();
            query = ProductQueryBuilder.BuildQuery(query, parameters);
            int skip = (parameters.Page - 1) * parameters.PageSize;
            //pagination
            List<Product> productsSorted = await query.Skip(skip).Take(parameters.PageSize).ToListAsync();
            return _mapper.Map<List<ProductResponse>>(productsSorted);
        }
        public async Task<ProductResponse> SelectByIdWithAllInfoAsync(int id)
        {
            Product? product = await _products.SelectByIdWithAllInfo(id);
            if (product == null)
            {
                throw new NotFoundException("product not found");
            }
            return _mapper.Map<ProductResponse>(product);

        }
        public async Task DeleteQuantityByIdAsync(int id, int quantity)
        {
            Product? productFromDb = await _products.GetByIdAsync(id);
            if (productFromDb != null)
            {
                if (productFromDb.Quantity >= quantity)
                {
                    productFromDb.Quantity -= quantity;
                }
                if (productFromDb.Quantity <= 0)
                {
                    productFromDb.isActive = false;
                }
                await _products.UpdateAsync(productFromDb);
            }

        }
        public async Task AddQuantityByIdAsync(int id, int quantity)
        {
            Product? productFromDb = await _products.GetByIdAsync(id);
            if (productFromDb != null)
            {
                productFromDb.Quantity += quantity;
                productFromDb.isActive = true;
                await _products.UpdateAsync(productFromDb);
            }

        }
        public async Task<ProductResponse> UpdateAsync(int id, ProductRequest request)
        {
            Product? productFromDb = await _products.SelectByIdWithAllInfo(id);
            if (productFromDb == null)
            {
                throw new NotFoundException("product not found");
            }
            productFromDb.Name = request.Name;
            productFromDb.Brand = request.Brand;
            productFromDb.Rating = request.Rating;
            productFromDb.Description = request.Description;
            productFromDb.CostPrice = request.CostPrice;
            productFromDb.Price = request.Price;
            productFromDb.Quantity = request.Quantity;
            productFromDb.isPromotion = request.isPromotion;
            productFromDb.isActive = request.isActive;
            productFromDb.CategoryId = request.CategoryId;
            if (request.PetTypeIds != null && request.PetTypeIds.Any())
            {
                List<PetType> petTypesFromDb = await _petTypes.SelectAllAsync();
                productFromDb.PetTypes = new HashSet<PetType>();
                foreach (var item in petTypesFromDb)
                {
                    if (request.PetTypeIds.Contains(item.Id))
                    {
                        productFromDb.PetTypes.Add(item);
                    }
                }
            }
            await _products.UpdateAsync(productFromDb);
            return _mapper.Map<ProductResponse>(productFromDb);
        }

        public async Task DeleteAsync(int id)
        {
            Product? productFromDb = await _products.GetByIdAsync(id);
            if (productFromDb == null)
            {
                throw new NotFoundException("product not found");
            }
            await _products.DeleteAsync(productFromDb);
        }

    }
}
