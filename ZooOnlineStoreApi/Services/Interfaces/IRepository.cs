
namespace ZooOnlineStoreApi.Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        Task<List<T>> SelectAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
