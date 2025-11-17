using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Model.ProductImages
{
    public class ProductImageService
    {
        private readonly IProductImageRepository _productImages;
        public ProductImageService(IProductImageRepository productImages)
        {
            _productImages = productImages;
        }
        public async Task InsertAsync(ProductImage image)
        {
            ProductImage? productImageFromDb = await _productImages.GetByNameAsync(image.ImageName);
            if (productImageFromDb!= null)
            {
                throw new DuplicationException("image name", image.ImageName);
            }
            await _productImages.InsertAsync(image);
        }
        public async Task DeleteByIdAsync(int id)
        {
            ProductImage? imageDeleted = await _productImages.GetByIdAsync(id);
            if ( imageDeleted == null)
            {
                throw new NotFoundException();
            }
            await _productImages.DeleteAsync(imageDeleted);
        }
        public async Task<ProductImage?> GetByNameAsync(string name)
        {
            ProductImage? image = await _productImages.GetByNameAsync(name);
            if (image == null)
            {
                throw new NotFoundException();
            }
            return image;
        }
        public async Task<List<ProductImage>> ListAllAsync()
        {
            return await _productImages.SelectAllAsync();
        }
    }
}
