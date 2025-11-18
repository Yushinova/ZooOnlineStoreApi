using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Storage
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;
        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(Feedback entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task InsertAsync(Feedback entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Feedback>> SelectAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<List<Feedback>?> SelectByProductIdAsync(int productId)
        {
            return await _context.Feedbacks.Where(f=>f.ProductId == productId).ToListAsync();
        }

        public async Task<List<Feedback>?> SelectByProductIdWithPaginationAsync(int productId, int page, int count)
        {
            return await _context.Feedbacks
            .Include(f => f.User)
            .Where(f => f.ProductId == productId)
            .OrderByDescending(f => f.Id)
            .Skip(page)
            .Take(count)
            .ToListAsync();
        }

        public async Task<List<Feedback>?> SelectByUserIdWithPaginationAsync(int userId, int page, int count)
        {
            return await _context.Feedbacks
           .Include(f => f.User)
           .Where(f => f.UserId == userId)
           .OrderByDescending(f => f.Id)
           .Skip(page)
           .Take(count)
           .ToListAsync();
        }

        public async Task UpdateAsync(Feedback entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
