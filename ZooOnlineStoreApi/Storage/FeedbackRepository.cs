using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

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

        public async Task<Feedback?> InsertAndRerunAsync(Feedback entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            Feedback? feedbackWithUser = await _context.Feedbacks.Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == entity.Id);
            return feedbackWithUser;
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
            return await _context.Feedbacks.Where(f=>f.ProductId == productId).OrderByDescending(f => f.Id).ToListAsync();
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

        public async Task<Feedback?> SelectByUserIdAndProductId(int userId, int productId)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
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
