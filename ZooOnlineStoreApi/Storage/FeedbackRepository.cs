using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class FeedbackRepository : IRepository<Feedback>
    {
        private readonly ApplicationDbContext _context;
        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsynk(Feedback entity)
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

        public async Task UpdateAsync(Feedback entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
