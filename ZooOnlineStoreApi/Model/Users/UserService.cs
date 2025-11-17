using Microsoft.AspNetCore.Identity.Data;
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
        public async Task<User?> RegisterAsync(User user)//первая регистрация
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
            
                return await _userRepository.GetByPhoneAsync(user.Phone);
            }
            else
            {
                throw new DuplicationException("user is duplicated", user.Phone);
            }
        }
        public async Task<User> AuthenticateAsync(string login, string password)
        {
            if (!Regex.IsMatch(login, phonePattern))
            {
                throw new ValidationException("phone", "phone is inavalid", login);
            }
            User? userFromDb = await _userRepository.GetByPhoneAsync(login);
            if (userFromDb == null) {
                throw new UnauthorizedAccessException("user not found");
            }
            if (_encoder.Encode(password) != userFromDb.Password)
            {
                throw new UnauthorizedAccessException("error password");
            }
            return userFromDb;
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
     
    }
}
