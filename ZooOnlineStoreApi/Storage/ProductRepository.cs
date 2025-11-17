using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Storage
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(Product entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
           return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task InsertAsync(Product entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> SelectAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> SelectAllByCategoryAndPetTypeIdAsync(int categoryId, int petTypeId)//получаем с рисунками
        {
            return await _context.Products.Include(p=>p.ProductImages).Where(p => p.CategoryId == categoryId && p.PetTypes.Any(p => p.Id == petTypeId)).ToListAsync();
        }

        public async Task<List<Product>> SelectAllByCategoryIdAsync(int CategoryId)
        {
            return await _context.Products.Include(p => p.ProductImages).Where(p => p.CategoryId == CategoryId).ToListAsync();
        }

        public async Task<List<Product>> SelectAllByPetTypeIdAsync(int petTypeId)
        {
            return await _context.Products.Include(p=>p.ProductImages).Where(p => p.PetTypes.Any(p => p.Id == petTypeId)).ToListAsync();
        }

        public async Task<Product?> SelectByIdWithAllInfo(int id)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.PetTypes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Product entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
