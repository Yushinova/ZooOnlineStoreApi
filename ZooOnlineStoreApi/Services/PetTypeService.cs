using AutoMapper;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services
{
    public class PetTypeService
    {
        private readonly IPetTypeRepository _petTypes;
        private readonly ICategoryRepository _category;
        private readonly IMapper _mapper;
        public PetTypeService(IPetTypeRepository petType, ICategoryRepository category, IMapper mapper)
        {
            _petTypes = petType;
            _category = category;
            _mapper = mapper;
        }
        public async Task<List<PetTypeResponse>> ListAllAsync()
        {
            List<PetType> petTypes = await _petTypes.SelectAllAsync();
            return _mapper.Map<List<PetTypeResponse>>(petTypes);
        }
        public async Task<PetTypeResponse> GetByNameAsync(string name)
        {
            PetType? petType = await _petTypes.SelectByNameAsync(name);
            if (petType == null)
            {
                throw new NotFoundException("pet type not found");
            }
            return _mapper.Map<PetTypeResponse>(petType);
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
        public async Task UpdateAsync(PetTypeUpdate request)
        {
            PetType? petTypeUpdate = await _petTypes.SelectByIdWithCategories(request.Id);
            if (petTypeUpdate == null)
            {
                throw new NotFoundException("pet type not found");
            }
            if (request.CategoriesIds != null && request.CategoriesIds.Any())
            {
                List<Category> categoriesFromDb = await _category.SelectAllAsync();
                petTypeUpdate.Categories = new HashSet<Category>();
                foreach (var item in categoriesFromDb)
                {
                    if (request.CategoriesIds.Contains(item.Id))
                    {
                        petTypeUpdate.Categories.Add(item);
                    }
                }
            }
            petTypeUpdate.Name = request.Name;
            petTypeUpdate.ImageName = request.ImageName;
         
            await _petTypes.UpdateAsync(petTypeUpdate);
        }
        public async Task DeleteByIdAsync(int id)
        {
            PetType? petTypeDeleted = await _petTypes.GetByIdAsync(id);
            if (petTypeDeleted == null)
            {
                throw new NotFoundException("pet type not found");
            }
            await _petTypes.DeleteAsync(petTypeDeleted);
        }
        public async Task<List<PetTypeResponse>> ListAllWithCategories()
        {
            List<PetType> petTypes = await _petTypes.SelectAllWithCategoies();
            return _mapper.Map<List<PetTypeResponse>>(petTypes);
        }
        public async Task<PetTypeResponse> SelectByIdWithCategoties(int id)
        {
            PetType? petType = await _petTypes.SelectByIdWithCategories(id);
            if (petType == null)
            {
                throw new NotFoundException("pet type not found");
            }
            return _mapper.Map<PetTypeResponse>(petType);
        }
    }
}
