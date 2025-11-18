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
        public async Task<Admin> AuthenticateAsync(string login, string password)
        {

            Admin? adminFromDb = await _adminRepository.GetByLoginAsync(login);
            if (adminFromDb == null)
            {
                throw new UnauthorizedAccessException("admin not found");
            }
            if (_encoder.Encode(password) != adminFromDb.Password)
            {
                throw new UnauthorizedAccessException("error password");
            }
            return adminFromDb;
        }
    }
}
