using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class PetTypeRepository : IPetTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public PetTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(PetType entity)
        {
            _context.Remove(entity);
           await _context.SaveChangesAsync();
        }

        public async Task<PetType?> GetByIdAsync(int id)
        {
            return await _context.PetTypes.FirstOrDefaultAsync(p=> p.Id == id);
        }

        public async Task InsertAsync(PetType entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PetType>> SelectAllAsync()
        {
            return await _context.PetTypes.OrderBy(P=>P.Id).ToListAsync();
        }

        public async Task<List<PetType>> SelectAllByProductIdAsync(int productId)
        {
            return await _context.PetTypes.Where(p => p.Products.Any(p => p.Id == productId)).ToListAsync();
        }

        public async Task<List<PetType>> SelectAllWithCategoies()
        {
            return await _context.PetTypes.Include(p => p.Categories).OrderBy(P => P.Id).ToListAsync();
        }

        public async Task<PetType?> SelectByIdWithCategories(int id)
        {
            return await _context.PetTypes.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PetType?> SelectByNameAsync(string name)
        {
            return await _context.PetTypes.FirstOrDefaultAsync(p => p.Name == name);
        }


        public async Task UpdateAsync(PetType entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
