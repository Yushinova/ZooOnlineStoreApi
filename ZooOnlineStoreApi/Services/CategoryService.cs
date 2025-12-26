using AutoMapper;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categories;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categories, IMapper mapper)
        {
            _categories = categories;
            _mapper = mapper;
        }
        public async Task<List<CategoryResponse>> ListAllAsync()
        {
            List<Category> categoriesFromDb = await _categories.SelectAllAsync();
            return _mapper.Map<List<CategoryResponse>>(categoriesFromDb);
        }
        public async Task<List<CategoryResponse>> ListAllByPetTypeIdAsync(int petTypeId)
        {
            List<Category> categories = await _categories.SelectAllByPetTypeIdAsync(petTypeId);
            return _mapper.Map<List<CategoryResponse>>(categories);
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
        public async Task<CategoryResponse> GetByNameAsync(string name)
        {
            Category? categoryFromDb = await _categories.SelectByName(name);
            if (categoryFromDb == null)
            {
                throw new NotFoundException("category not found");
            }
            return _mapper.Map<CategoryResponse>(categoryFromDb);
        }
        public async Task<CategoryResponse> GetByIdAsync(int id)
        {
            Category? category = await _categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("category not found");
            }
            return _mapper.Map<CategoryResponse>(category);
        }
        public async Task DeleteAsync(int id)
        {
            Category? categoryDeleted = await _categories.GetByIdAsync(id);
            if (categoryDeleted == null)
            {
                throw new NotFoundException("category not found");

            }

            await _categories.DeleteAsync(categoryDeleted);
        }
    }
}
