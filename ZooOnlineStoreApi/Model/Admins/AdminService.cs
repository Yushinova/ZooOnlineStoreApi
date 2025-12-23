using AutoMapper;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
namespace ZooOnlineStoreApi.Model.Admins
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IEncoder _encoder;
        private readonly IMapper _mapper;
        public AdminService(IAdminRepository adminRepository, IEncoder encoder, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _encoder = encoder;
            _mapper = mapper;
        }
        public async Task InsertAsync(AdminRequest request)
        {
            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(request.Login);
            if (adminFromDb != null)
            {
                throw new DuplicationException("login", request.Login);
            }
            Admin adminInsert = _mapper.Map<Admin>(request);
            adminInsert.Password = _encoder.Encode(request.Password);
            adminInsert.RegisteredAt = DateTime.UtcNow;

            await _adminRepository.InsertAsync(adminInsert);
        }
        public async Task UpdateAsync(AdminUpdateRequest request)
        {
            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(request.Login);
            if (adminFromDb == null)
            {
                throw new NotFoundException();
            }
            adminFromDb.Name=request.Name;
            adminFromDb.Role=request.Role;
            await _adminRepository.UpdateAsync(adminFromDb);
        }
        public async Task<AdminResponse> GetByLoginAsync(string login)
        {
            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(login);
            if (adminFromDb == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<AdminResponse>(adminFromDb);
        }
        public async Task DeleteAsync(Admin admin)
        {
            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(admin.Login);
            if (adminFromDb == null)
            {
                throw new NotFoundException();
            }
            await _adminRepository.DeleteAsync(admin);
        }
        public async Task<string> AuthenticateAsync(string login, string password)
        {

            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(login);
            if (adminFromDb == null)
            {
                throw new UnauthorizedAccessException("admin not found");
            }
            if (!_encoder.Verify(password, adminFromDb.Password))
            {
                throw new UnauthorizedAccessException("error password");
            }
            return generateApiKey(adminFromDb);
        }

        // GetUserAsync - получение данных о пользователе по ключу
        // вход: api-ключ пользователя
        // выход: объект с информацией о пользователе
        // иключения: UserNotFoundException
        public async Task<AdminResponse> GetAdminAsync(string apiKey)
        {
            List<Admin> adminsFromDb = await _adminRepository.SelectAllAsync();
            foreach (var item in adminsFromDb)
            {
                string generatedKey = generateApiKey(item);
                if (generatedKey == apiKey)
                {
                    return  _mapper.Map<AdminResponse>(item);
                }
            }
            throw new NotFoundException();
        }

        private string generateApiKey(Admin admin)
        {
            return _encoder.Encode($"{admin.Name} - {admin.Login} - {admin.RegisteredAt}");
        }
    }
}
