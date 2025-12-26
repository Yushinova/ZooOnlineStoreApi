using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;
using ZooOnlineStoreApi.Services.Exeptions;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Storage;
using ValidationException = ZooOnlineStoreApi.Services.Exeptions.ValidationException;

namespace ZooOnlineStoreApi.Services
{
    public class UserService
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        string phonePattern = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";//+7(905)455-22-00

        private readonly IUserRepository _userRepository;
        private readonly IEncoder _encoder;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IEncoder encoder, IMapper mapper)
        {
            _userRepository = userRepository;
            _encoder = encoder;
            _mapper = mapper;
        }
        public async Task<string> RegisterAsync(UserRequest request)//первая регистрация
        {

            // валидация строк
            if (!Regex.IsMatch(request.Phone, phonePattern))
            {
                throw new ValidationException("phone", "phone is invalid", request.Phone);
            }
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                throw new ValidationException("email", "email is invalid", request.Email);
            }

            // проверка на дубликацию
            bool isLoginDuplicated = await _userRepository.GetByPhoneAsync(request.Phone) != null;
            if (isLoginDuplicated)
            {
                throw new DuplicationException("phone", request.Phone);
            }
            bool isEmailDuplicated = await _userRepository.GetByEmailAsync(request.Email) != null;
            if (isEmailDuplicated)
            {
                throw new DuplicationException("email", request.Email);
            }
            User user = _mapper.Map<User>(request);
            user.Password = _encoder.Encode(user.Password);
            user.UUID = Guid.NewGuid();
            user.RegisteredAt = DateTime.UtcNow;
           
            string apiKey = generateApiKey(user);
            await _userRepository.InsertAsync(user);
            return apiKey;
        }
        public async Task<string> LoginAsync(UserLoginRequest request)
        {
            if (!Regex.IsMatch(request.Phone, phonePattern))
            {
                throw new ValidationException("phone", "phone is inavalid", request.Phone);
            }
            User? userFromDb = await _userRepository.GetByPhoneAsync(request.Phone);
            if (userFromDb == null)
            {
                throw new UnauthorizedException("user not found");
            }
            if (!_encoder.Verify(request.Password, userFromDb.Password))
            {
                throw new UnauthorizedException("error password");
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
                throw new NotFoundException("user not found");
            }
            await _userRepository.DeleteAsync(userFromDb);
        }
        public async Task<UserResponse> GetByIdAsync(int id)
        {
            User? userFromDb = await _userRepository.GetByIdAsync(id);
            if (userFromDb == null)
            {
                throw new NotFoundException("user not found");
            }
            return _mapper.Map<UserResponse>(userFromDb);
        }
        public async Task UpdateAsync(UserResponse user)//order
        {
            User? userFromDb = await _userRepository.GetByIdAsync(user.Id);
            if (userFromDb == null)
            {
                throw new NotFoundException("user not found");
            }
            userFromDb.Discont = user.Discont;
            userFromDb.TotalOrders = user.TotalOrders;
            await _userRepository.UpdateAsync(userFromDb);
        }
        // GetUserAsync - получение данных о пользователе по ключу
        // вход: api-ключ пользователя
        // выход: объект с информацией о пользователе
        // иключения: UserNotFoundException
        public async Task<UserResponse> GetUserAsync(string apiKey)
        {
            List<User> usersFromDb = await _userRepository.SelectAllAsync();
            foreach (var item in usersFromDb)
            {
                if (generateApiKey(item) == apiKey)
                {
                    return _mapper.Map<UserResponse>(item);
                }
            }
            throw new NotFoundException("user not found");
        }
        // генерация api-ключа для пользователя
        private string generateApiKey(User user)
        {
            return _encoder.Encode($"{user.UUID} - {user.Phone} - {user.Email} - {user.RegisteredAt}");
        }

    }
}
