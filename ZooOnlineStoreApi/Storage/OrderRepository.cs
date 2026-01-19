using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task InsertAsync(Order entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public Task<Order?> GetByIdAsync(int id)
        {
            return _context.Orders.Include(o => o.User)
                .Include(o => o.OrderItems)
                .Include(o=>o.Payment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order?> InsertReturnEntityAsync(Order entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<List<Order>> SelectAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<List<Order>?> GetAllWithPagination(int skipItems, int countItems)
        {
            return await _context.Orders.Include(o => o.User)
                .Include(o => o.OrderItems).Include(o => o.Payment)
                .OrderByDescending(o => o.Id).Skip(skipItems)
                .Take(countItems).ToListAsync();
        }
        public async Task<List<Order>?> SelectAllByUserIdAsync(int userId)
        {
            return await _context.Orders.Include(o=>o.OrderItems).Include(o=>o.User).Include(o=>o.Payment).Where(o=>o.UserId== userId).OrderByDescending(o=>o.Id).ToListAsync();
        }

        public async Task UpdateAsync(Order entity)
        {
             _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> UpdateReturnEntityAsync(Order entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(Order entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}
