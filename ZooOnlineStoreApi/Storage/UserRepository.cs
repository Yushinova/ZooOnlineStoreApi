using Microsoft.EntityFrameworkCore;
using System.Numerics;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task InsertAsync(User entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task DeleteAsync(User entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetByPhoneAsync(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }
        public async Task<List<User>> SelectAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}
