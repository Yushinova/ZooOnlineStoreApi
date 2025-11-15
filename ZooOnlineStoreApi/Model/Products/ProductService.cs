using System.Xml.Linq;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.ProductImages;
using static System.Net.Mime.MediaTypeNames;

namespace ZooOnlineStoreApi.Model.Products
{
    public class ProductService
    {
        private readonly IProductRepository _products;
        private readonly IProductImageRepository _images;
        private readonly IPetTypeRepository _petTypes;
        public ProductService(IProductRepository products, IProductImageRepository images, IPetTypeRepository petTypes)
        {
            _products = products;
            _images = images;
            _petTypes = petTypes;
        }
        public Task InsertAsync(Product product)
        {
            return _products.InsertAsync(product);
        }
        public async Task<Product?> SelectByIdWithAllInfoAsync(int id)
        {
            return await _products.SelectByIdWithAllInfo(id);
        }
        public async Task<List<Product>> ListAllAsync()
        {
            return await _products.SelectAllAsync();
        }
        public async Task<List<Product>?> ListAllByPetTypeIdAsync(int petTypeId)
        {
            return await _products.SelectAllByPetTypeIdAsync(petTypeId);
        }
        public async Task<List<Product>> ListAllByCategoryIdAsync(int categoryId)
        {
            return await _products.SelectAllByCategoryIdAsync(categoryId);
        }
        public async Task<List<Product>> ListAllByCategoryAndPetTypeIdAsync(int categoryId, int petTypeId)
        {
            return await _products.SelectAllByCategoryAndPetTypeIdAsync(categoryId, petTypeId);
        }
        public async Task UpdateAsync(Product product)
        {
            Product? productFromDb = await _products.SelectByIdWithAllInfo(product.Id);
            if (productFromDb == null)
            {
                throw new NotFoundException();
            }
            productFromDb.Name = product.Name;
            productFromDb.Brand = product.Brand;
            productFromDb.Description = product.Description;
            productFromDb.CostPrice = product.CostPrice;
            productFromDb.Price = product.Price;
            productFromDb.Quantity = product.Quantity;
            productFromDb.isPromotion = product.isPromotion;
            productFromDb.isActive = product.isActive;
            productFromDb.CategoryId = product.CategoryId;
            if (productFromDb.PetTypes!=null) {
                productFromDb.PetTypes.Clear();
            }
            productFromDb.PetTypes = product.PetTypes;
            await _products.UpdateAsync(productFromDb);
        }
        public async Task AddPetTypeAsync(int productId, int petTypeId)
        {
            Product? productFromDb = await _products.SelectByIdWithAllInfo(productId);
            PetType? petType = await _petTypes.GetByIdAsync(petTypeId);
            if (productFromDb == null || petType == null)
            {
                throw new NotFoundException();
            }
            productFromDb.PetTypes ??= new HashSet<PetType>();
            if (productFromDb.PetTypes.Any(p => p.Name == petType.Name))
            {
                throw new DuplicationException("pet type", petType.Name);
            }
            productFromDb.PetTypes.Add(petType);
            await _products.UpdateAsync(productFromDb);
        }
        public async Task DeletePetTypeFromProductAsync(int productId, int petTypeId)
        {
            Product? productFromDb = await _products.SelectByIdWithAllInfo(productId);
            PetType? petType = await _petTypes.GetByIdAsync(petTypeId);
            if (productFromDb == null || petType == null || productFromDb.PetTypes == null)
            {
                throw new NotFoundException();
            }
            productFromDb.PetTypes.Remove(petType);
            await _products.UpdateAsync(productFromDb);
        }

    }
}
