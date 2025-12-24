using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(Admin entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _context.Admins.FirstOrDefaultAsync(a=>a.Id == id);
        }

        public async Task<Admin?> GetByLoginAsync(string login)
        {
           return await _context.Admins.FirstOrDefaultAsync(a=>a.Login == login);
        }

        public async Task InsertAsync(Admin entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Admin>> SelectAllAsync()
        {
           return await _context.Admins.ToListAsync();
        }

        public async Task UpdateAsync(Admin entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
