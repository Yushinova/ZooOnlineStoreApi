using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(ProductImage entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsynk(List<ProductImage> images)
        {
            _context.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductImage?> GetByIdAsync(int id)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task InsertAsync(ProductImage entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImage>> SelectAllAsync()
        {
           return await _context.ProductImages.ToListAsync();
        }

        public async Task<List<ProductImage>> SelectAllByProductIdAsync(int productId)
        {
            return await _context.ProductImages.Where(c => c.ProductId == productId).ToListAsync();
        }

        public async Task<ProductImage?> GetByNameAsync(string name)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(p => p.ImageName == name);
        }

        public Task UpdateAsync(ProductImage entity)//не нужно
        {
            throw new NotImplementedException();
        }
    }
}
