using BookLendingSystem.DTOs;

namespace BookLendingSystem.Interfaces
{
    public interface IUserService
    {
        Task<UserReadDto> AddUser(UserCreateDto userDto);
        Task<IEnumerable<UserReadDto>> GetAllUsers();
        Task<UserReadDto> GetUserById(int id);
        Task<UserReadDto> UpdateUser(int id, UserUpdateDto updateDto);
        Task<UserReadDto> DeleteUser(int id);
        Task<UserReadDto> Login(string name, string role);
    }
}
