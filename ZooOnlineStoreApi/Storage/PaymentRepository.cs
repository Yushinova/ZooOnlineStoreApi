using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;

namespace ZooOnlineStoreApi.Storage
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task DeleteAsync(Payment entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.Include(p => p.Order).FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task InsertAsync(Payment entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> InsertReturnEntityAsync(Payment entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Payment>> SelectAllAsync()
        {
            return await _context.Payments.Include(p => p.Order).ToListAsync();
        }

        public async Task<List<Payment>> SelectAllByUserIdAsync(int userId)
        {
            return await _context.Payments.Include(p => p.Order).Where(p => p.Order.UserId == userId).ToListAsync();
        }

        public async Task<PaymentPaged> SelectWithPagination(PaymentFilter parameters)
        {
            IQueryable<Payment> query = _context.Payments.Include(p => p.Order).AsQueryable();

            if (parameters.UserId.HasValue)
            {
                query = query.Where(p => p.Order.UserId == parameters.UserId.Value);
            }
            if (!string.IsNullOrEmpty(parameters.Status))
            {
                query = query.Where(p => p.Status == parameters.Status);
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "amount" => parameters.SortDescending
                    ? query.OrderByDescending(p => p.Amount)
                    : query.OrderBy(p => p.Amount),
                _ => parameters.SortDescending
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt)
            };
            int totalCount = await query.CountAsync();
            List<Payment> items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToListAsync();
         
            PaymentPaged response = new PaymentPaged
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize)
            };
            return response;

        }

        public Task UpdateAsync(Payment entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> UpdateReturnEntityAsync(Payment entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
