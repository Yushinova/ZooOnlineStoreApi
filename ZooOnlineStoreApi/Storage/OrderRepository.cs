using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Orders;

namespace ZooOnlineStoreApi.Storage
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsynk(Order entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            return _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task InsertAsync(Order entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> InsertReturnEntityAsync(Order entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Order>> SelectAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>?> SelectAllByDataAsync(DateTime date)
        {
           return await _context.Orders.Where(o=>o.CreatedAt==date).ToListAsync();
        }

        public async Task<List<Order>?> SelectAllByStatusAsync(string status)
        {
            return await _context.Orders.Where(o => o.Status == status).ToListAsync();
        }

        public async Task<List<Order>?> SelectAllByUserIdAsync(int userId)
        {
            return await _context.Orders.Where(o=>o.UserId== userId).ToListAsync();
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
    }
}
