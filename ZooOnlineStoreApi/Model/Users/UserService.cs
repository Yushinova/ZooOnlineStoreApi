using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Interfaces;
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
        public async Task<string> InsertAsync(User user)//первая регистрация
        {
            User? userFromDb = await _userRepository.GetByPhoneAsync(user.Phone);
            if (userFromDb == null) {

                // валидация строк
                if (!Regex.IsMatch(user.Phone, phonePattern))
                {
                    throw new ValidationException("phone", "phone is inavalid", user.Phone);
                }
                if (!Regex.IsMatch(user.Email, emailPattern))
                {
                    throw new ValidationException("email", "email is invalid", user.Email);
                }
                await _userRepository.InsertAsync(user);
                string apiKey = generateApiKey(user);
                return apiKey;//пока нигде не хранится
            }
            else
            {
                throw new DuplicationException("user is duplicated", user.Phone);
            }
        }
        public async Task<List<User>> ListAllAsync()
        {
            return await _userRepository.SelectAllAsync();
        }

        public async Task DeleteByIdAstnc(int id)
        {
            User? userFromDb = await _userRepository.GetByIdAsync(id);
            if (userFromDb == null)
            {
                throw new NotFoundException();
            }
            await _userRepository.DeleteAsynk(userFromDb);
        }

        // генерация api-ключа для пользователя
        private string generateApiKey(User user)
        {
            return _encoder.Encode($"{user.UUID} - {user.Phone} - {user.Email} - {user.RegisteredAt}");
        }
    }
}
