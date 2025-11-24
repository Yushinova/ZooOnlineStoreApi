using System.Text.RegularExpressions;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.Users;
using ZooOnlineStoreApi.Storage;
namespace ZooOnlineStoreApi.Model.Admins
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IEncoder _encoder;
        public AdminService(IAdminRepository adminRepository, IEncoder encoder  )
        {
            _adminRepository = adminRepository;
            _encoder = encoder;
        }
        public async Task InsertAsync(Admin admin)
        {
            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(admin.Login);
            if (adminFromDb != null)
            {
                throw new DuplicationException("login", admin.Login);
            }
            Admin adminInsert = new Admin
            {
                Login = admin.Login,
                Password = admin.Password,
                Name = admin.Name,
                Role = admin.Role,
                RegisteredAt = admin.RegisteredAt
            };
            await _adminRepository.InsertAsync(adminInsert);
        }
        public async Task UpdateAsync(Admin admin)
        {
            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(admin.Login);
            if (adminFromDb == null)
            {
                throw new NotFoundException();
            }
            adminFromDb.Name=admin.Name;
            adminFromDb.Role=admin.Role;
            await _adminRepository.UpdateAsync(adminFromDb);
        }
        public async Task<Admin?> GetByLoginAsync(string login)
        {
            return await _adminRepository.GetByLoginAsync(login);
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
        public async Task<Admin> GetAdminAsync(string apiKey)
        {
            List<Admin> adminsFromDb = await _adminRepository.SelectAllAsync();
            foreach (var item in adminsFromDb)
            {
                string generatedKey = generateApiKey(item);
                if (generatedKey == apiKey)
                {
                    return item;
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
