using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Products;

namespace ZooOnlineStoreApi.Storage
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> SelectAllWithImagesAndPetTypesAsync()
        {
            return _context.Products.Include(p => p.ProductImages).Include(p => p.PetTypes).AsQueryable();//тут все приходят
        }

        public IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductQueryParameters parameters)
        {
            var par = parameters;
            if (parameters.IsActive.HasValue)
                query = query.Where(p => p.isActive == parameters.IsActive.Value);

            if (parameters.IsPromotion.HasValue)
                query = query.Where(p => p.isPromotion == parameters.IsPromotion.Value);

            if (parameters.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);

            if (parameters.PetTypeId.HasValue)
                query = query.Where(p => p.PetTypes.Any(pt => pt.Id == parameters.PetTypeId.Value));
            if (parameters.Name != null)
                query = query.Where(p => p.Name.ToLower().Contains(parameters.Name.ToLower()));
            if (parameters.Brand != null)
                query = query.Where(p => p.Brand.ToLower().Contains(parameters.Brand.ToLower()));
            if (parameters.MinPrice.HasValue && parameters.MaxPrice.HasValue)
                query = query.Where(p => p.Price >= parameters.MinPrice && p.Price <= parameters.MaxPrice);
            if (parameters.Rating.HasValue)
                query = query.Where(p => p.Rating >= parameters.Rating);
            return query;
        }

        public IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductQueryParameters parameters)
        {
            if (parameters.Rating.HasValue)
                query.OrderByDescending(p => p.Rating);
            return query;

        }

        public async Task<List<Product>?> SelectAllWithFilters(ProductQueryParameters parameters)
        {
            var query = SelectAllWithImagesAndPetTypesAsync();
            query = ApplyFilters(query, parameters);
            query = ApplySorting(query, parameters);
            int skip = (parameters.Page - 1) * parameters.PageSize;
            //pagination
            List<Product> productsSorted = await query.Skip(skip).Take(parameters.PageSize).ToListAsync();
            return productsSorted;
        }
        ////////////////////////
        public async Task DeleteAsync(Product entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task InsertAsync(Product entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> SelectAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> SelectByIdWithAllInfo(int id)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.PetTypes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Product entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }


    }
}
