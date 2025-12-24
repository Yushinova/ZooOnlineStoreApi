using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(Address entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task InsertAsync(Address entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Address>> SelectAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<List<Address>?> SelectByUserIdAsync(int id)
        {
            return await _context.Addresses.Where(a => a.UserId == id).ToListAsync();
        }

        public async Task UpdateAsync(Address entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
