using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> InsertAndReturnAsync(Product entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task UpdateAsync(Product entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        public IQueryable<Product> SelectAllWithImagesAndPetTypes()
        {
            return _context.Products.Include(p => p.ProductImages)
                .Include(p => p.PetTypes).AsQueryable();//тут все приходят
        }
     
        public async Task DeleteAsync(Product entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task InsertAsync(Product entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Product>> SelectAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product?> SelectByIdWithAllInfo(int id)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.PetTypes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task DeleteQuantityByIdAsync(int id, int quantity)
        {
            Product? productFromDb = await GetByIdAsync(id);
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
                await UpdateAsync(productFromDb);
            }
        }
        public async Task AddQuantityByIdAsync(int id, int quantity)
        {
            Product? productFromDb = await GetByIdAsync(id);
            if (productFromDb != null)
            {
                productFromDb.Quantity += quantity;
                productFromDb.isActive = true;
                await UpdateAsync(productFromDb);
            }

        }
    }
}
