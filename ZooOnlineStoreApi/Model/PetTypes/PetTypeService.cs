using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;

namespace ZooOnlineStoreApi.Model.PetTypes
{
    public class PetTypeService
    {
        private readonly IPetTypeRepository _petTypes;
        private readonly ICategoryRepository _category;
        public PetTypeService(IPetTypeRepository petType, ICategoryRepository category)
        {
            _petTypes = petType;
            _category = category;
        }
        public async Task<List<PetType>> ListAllAsync()
        {
            return await _petTypes.SelectAllAsync();
        }
        public async Task<PetType?> GetNyNameAsync(string name)
        {
            return await _petTypes.SelectByNameAsync(name);
        }
        public async Task InsertAsync(string name, string imageName)
        {
            PetType? type = await _petTypes.SelectByNameAsync(name);
            if (type != null)
            {  
                throw new DuplicationException("petType", name);
               
            }
           await _petTypes.InsertAsync(new PetType { Name = name, ImageName = imageName });
        }
        public async Task UpdateAsync(PetType petType)
        {
            PetType? type = await _petTypes.GetByIdAsync(petType.Id);
            if (type == null)
            {
                throw new NotFoundException();
            }
            type.Name = petType.Name;
            type.ImageName = petType.ImageName;
            if (type.Categories != null)
            {
                type.Categories.Clear();
            }
            type.Categories = petType.Categories;
            await _petTypes.UpdateAsync(type);
        }
        public async Task<List<PetType>> ListAllWithCategories()
        {
            return await _petTypes.SelectAllWithCategoies();
        }
        public async Task<PetType?> SelectByIdWithCategoties(int id)
        {
            return await _petTypes.SelectByIdWithCategories(id);
        }
        public async Task RemoveCategoryByIdFromPetType(int petTypeId, int categoryId)
        {
            //находим тип по id с категориями
            PetType? petTypeUpdated = await _petTypes.SelectByIdWithCategories(petTypeId);
            if (petTypeUpdated != null)
            {
                if (petTypeUpdated.Categories != null)
                {
                    Category? categoryDel = petTypeUpdated.Categories.FirstOrDefault(c => c.Id == categoryId);
                    if (categoryDel != null)
                    {
                        petTypeUpdated.Categories.Remove(categoryDel);
                        await _petTypes.UpdateAsync(petTypeUpdated);
                    }
                    else
                    {
                        throw new NotFoundException();
                    }
                }
            }
            else
            {
                throw new NotFoundException();
            }
        }
        public async Task AddCategoryToPetTypeAsync(int petTypeId, int categoryId)
        {
            Category? categoryInsert = await _category.GetByIdAsync(categoryId);
            PetType? petTypeUpdated = await _petTypes.SelectByIdWithCategories(petTypeId);
            if (categoryInsert == null || petTypeUpdated == null)
            {
                throw new NotFoundException();
            }
            petTypeUpdated.Categories ??= new HashSet<Category>();//если null инициализируем
            if (petTypeUpdated.Categories.Any(c => c.Id == categoryId))
            {
                throw new DuplicationException("categoryName", categoryInsert.Name);
            }
            petTypeUpdated.Categories.Add(categoryInsert);
            await _petTypes.UpdateAsync(petTypeUpdated);
        }
    }
}
