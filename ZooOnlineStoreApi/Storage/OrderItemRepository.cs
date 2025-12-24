using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertRangeAsync(List<OrderItem> items)
        {
            await _context.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderItem entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRangeAsync(List<OrderItem> items)
        {
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
        public async Task<OrderItem?> GetByIdAsync(int id)
        {
          return await _context.OrderItems.FirstOrDefaultAsync(o=>o.Id== id);
        }

        public async Task InsertAsync(OrderItem entity)
        {
             await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<OrderItem>?> SelectAllByOrderIdAsync(int orderId)
        {
           return await _context.OrderItems.Where(o=>o.OrderId== orderId).ToListAsync();
        }
        public Task<List<OrderItem>> SelectAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OrderItem entity)
        {
            throw new NotImplementedException();
        }

     
    }
}
