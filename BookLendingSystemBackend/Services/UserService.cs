using AutoMapper;
using BookLendingSystem.DTOs;
using BookLendingSystem.Exceptions;
using BookLendingSystem.Interfaces;
using BookLendingSystem.Models;
using BookLendingSystem.Repositories;

namespace BookLendingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<int, User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserReadDto> AddUser(UserCreateDto userDto)
        {
            var existingUsers = await _userRepository.GetAll();
            if (existingUsers.Any(u => u.Name.Equals(userDto.Name, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateUsernameException(userDto.Name);

            var user = _mapper.Map<User>(userDto);
            var created = await _userRepository.Add(user);
            return _mapper.Map<UserReadDto>(created);
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<UserReadDto> GetUserById(int id)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new UserNotFoundException(id);

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto> UpdateUser(int id, UserUpdateDto updateDto)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new UserNotFoundException(id);

            // Check if another user has the same username
            var allUsers = await _userRepository.GetAll();
            if (allUsers.Any(u => u.Id != id && u.Name.Equals(updateDto.Name, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateUsernameException(updateDto.Name);

            _mapper.Map(updateDto, user);
            var updated = await _userRepository.Update(id, user);
            return _mapper.Map<UserReadDto>(updated);
        }

        public async Task<UserReadDto> DeleteUser(int id)
        {
            var deleted = await _userRepository.Delete(id);
            if (deleted == null)
                throw new UserNotFoundException(id);

            return _mapper.Map<UserReadDto>(deleted);
        }

        public async Task<UserReadDto> Login(string name, string role)
        {
            var allUsers = await _userRepository.GetAll();
            var user = allUsers.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                                    && u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new UserNotFoundException($"No user found with name '{name}' and role '{role}'");

            return _mapper.Map<UserReadDto>(user);
        }
    }
}
