using Microsoft.AspNetCore.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using ZooOnlineStoreApi.Model.Admins;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Storage;
using ValidationException = ZooOnlineStoreApi.Model.Exeptions.ValidationException;

namespace ZooOnlineStoreApi.Model.Users
{
    public class UserService
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        string phonePattern = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";//+7(905)455-22-00

        private readonly IUserRepository _userRepository;
        private readonly IEncoder _encoder;
        public UserService(IUserRepository userRepository, IEncoder encoder)
        {
            _userRepository = userRepository;
            _encoder = encoder;
        }
        public async Task<string> RegisterAsync(User user)//первая регистрация
        {
            // валидация строк
            if (!Regex.IsMatch(user.Phone, phonePattern))
            {
                throw new ValidationException("phon4", "phone is invalid", user.Phone);
            }
            if (!Regex.IsMatch(user.Email, emailPattern))
            {
                throw new ValidationException("email", "email is invalid", user.Email);
            }

            // проверка на дубликацию
            bool isLoginDuplicated = await _userRepository.GetByPhoneAsync(user.Phone) != null;
            if (isLoginDuplicated)
            {
                throw new DuplicationException("login", user.Phone);
            }
            bool isEmailDuplicated = await _userRepository.GetByEmailAsync(user.Email) != null;
            if (isEmailDuplicated)
            {
                throw new DuplicationException("email", user.Email);
            }

            // выполним регистрацию
            User newUser = new User()
            {
                UUID = Guid.NewGuid(), // генерация UUID для пользователя
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                Password = _encoder.Encode(user.Password),
                RegisteredAt = DateTime.UtcNow
            };
            string apiKey = generateApiKey(newUser);
            await _userRepository.InsertAsync(newUser);
            return apiKey;
        }
        public async Task<string> LoginAsync(string login, string password)
        {
            if (!Regex.IsMatch(login, phonePattern))
            {
                throw new ValidationException("phone", "phone is inavalid", login);
            }
            User? userFromDb = await _userRepository.GetByPhoneAsync(login);
            if (userFromDb == null)
            {
                throw new UnauthorizedAccessException("user not found");
            }
            if (!_encoder.Verify(password, userFromDb.Password))
            {
                throw new UnauthorizedAccessException("error password");
            }
            return generateApiKey(userFromDb);

        }
        public async Task<List<User>> ListAllAsync()
        {
            return await _userRepository.SelectAllAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            User? userFromDb = await _userRepository.GetByIdAsync(id);
            if (userFromDb == null)
            {
                throw new NotFoundException();
            }
            await _userRepository.DeleteAsync(userFromDb);
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            User? userFromDb = await _userRepository.GetByIdAsync(id);
            if (userFromDb == null)
            {
                throw new NotFoundException();
            }
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task UpdateAsync(User user)
        {
            User? userFromDb = await _userRepository.GetByIdAsync(user.Id);
            if (userFromDb == null)
            {
                throw new NotFoundException();
            }
            userFromDb.Discont = user.Discont;
            userFromDb.TotalOrders = user.TotalOrders;
            await _userRepository.UpdateAsync(userFromDb);
        }
        // GetUserAsync - получение данных о пользователе по ключу
        // вход: api-ключ пользователя
        // выход: объект с информацией о пользователе
        // иключения: UserNotFoundException
        public async Task<User> GetUserAsync(string apiKey)
        {
            List<User> usersFromDb = await _userRepository.SelectAllAsync();
            foreach (var item in usersFromDb)
            {
                if (generateApiKey(item) == apiKey)
                {
                    return item;
                }
            }
            throw new NotFoundException();
        }
        // генерация api-ключа для пользователя
        private string generateApiKey(User user)
        {
            return _encoder.Encode($"{user.UUID} - {user.Phone} - {user.Email} - {user.RegisteredAt}");
        }

    }
}
