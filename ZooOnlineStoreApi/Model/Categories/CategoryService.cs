using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Model.Categories
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categories;
        public CategoryService(ICategoryRepository categories)
        {
            _categories = categories;
        }
        public async Task<List<Category>> ListAllAsync()
        {
            return await _categories.SelectAllAsync();
        }
        public async Task<List<Category>> ListAllByPetTypeIdAsync(int petTypeId)
        {
            List<Category> categories = await _categories.SelectAllByPetTypeIdAsync(petTypeId);
            if (categories != null)
            {
                return categories;
            }
            throw new NotFoundException();
        }
        public async Task InsertAsync(string name)
        {
            Category? category = await _categories.SelectByName(name);
            if (category != null)
            {
                throw new DuplicationException("categoryName", name);
                
            }
            await _categories.InsertAsync(new Category { Name = name });
        }
        public async Task<Category> GetByNameAsynk(string name)
        {
            return await _categories.SelectByName(name);
        }
        public async Task UpdateAsync(Category category)
        {
            Category? categoryUpdated = await _categories.GetByIdAsync(category.Id);
            if (category == null)
            {
                throw new NotFoundException();

            }
            categoryUpdated.Name = category.Name;
            await _categories.UpdateAsync(categoryUpdated);
        }
    }
}
