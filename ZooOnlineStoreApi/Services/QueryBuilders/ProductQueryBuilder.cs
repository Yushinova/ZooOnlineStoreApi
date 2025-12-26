using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;

namespace ZooOnlineStoreApi.Services.QueryBuilders
{
    public class ProductQueryBuilder
    {
        public static IQueryable<Product> BuildQuery(IQueryable<Product> query, ProductQueryParameters parameters)
        {
            query = ApplyFilters(query, parameters);
            query = ApplySorting(query, parameters);
            return query;
        }
        public static IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductQueryParameters parameters)
        {
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
        public static IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductQueryParameters parameters)
        {
            if (parameters.Rating.HasValue)
                query.OrderByDescending(p => p.Rating);
            return query;
        }
    }
}
