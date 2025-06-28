using BookLendingSystem.DTOs;
using BookLendingSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookLendingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserReadDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserReadDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserReadDto>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(UserReadDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserReadDto>> CreateUser([FromBody] UserCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AddUser(createDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(UserReadDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserReadDto>> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _userService.UpdateUser(id, updateDto);
            return Ok(updated);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(UserReadDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserReadDto>> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUser(id);
            return Ok(deleted);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserReadDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserReadDto>> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.Login(loginDto.Name, loginDto.Role);
            return Ok(user);
        }
    }
}
