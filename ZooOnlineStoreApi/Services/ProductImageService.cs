using AutoMapper;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services
{
    public class ProductImageService
    {
        private readonly IProductImageRepository _productImages;
        private readonly IMapper _mapper;
        public ProductImageService(IProductImageRepository productImages, IMapper mapper)
        {
            _productImages = productImages;
            _mapper = mapper;
        }
        public async Task InsertAsync(ProductImageRequest request)
        {
            ProductImage? productImageFromDb = await _productImages.GetByNameAsync(request.ImageName);
            if (productImageFromDb!= null)
            {
                throw new DuplicationException("image name", request.ImageName);
            }
            ProductImage imageInsert = _mapper.Map<ProductImage>(request);
            await _productImages.InsertAsync(imageInsert);
        }
        public async Task DeleteByIdAsync(int id)
        {
            ProductImage? imageDeleted = await _productImages.GetByIdAsync(id);
            if ( imageDeleted == null)
            {
                throw new NotFoundException("image not found");
            }
            await _productImages.DeleteAsync(imageDeleted);
        }
        public async Task DeleteByNameAsync(string name)
        {
            ProductImage? imageDeleted = await _productImages.GetByNameAsync(name);
            if (imageDeleted == null)
            {
                throw new NotFoundException("image not found");
            }
            await _productImages.DeleteAsync(imageDeleted);
        }

        public async Task<List<ProductImageResponse>> ListAllAsync()
        {
            List<ProductImage>? images = await _productImages.SelectAllAsync();
            return _mapper.Map<List<ProductImageResponse>>(images);
        }
    }
}
