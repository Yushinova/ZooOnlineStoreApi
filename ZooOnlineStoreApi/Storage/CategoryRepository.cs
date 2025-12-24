using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(Category entity)
        {
           _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
           return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task InsertAsync(Category entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> SelectAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Category>> SelectAllByPetTypeIdAsync(int petTypeId)
        {
            return await _context.Categories.Where(c => c.PetTypes.Any(p => p.Id == petTypeId)).ToListAsync();
        }

        public async Task<Category?> SelectByName(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task UpdateAsync(Category entity)
        {
            _context.Update(entity);
           await _context.SaveChangesAsync();
            
        }
    }
}
