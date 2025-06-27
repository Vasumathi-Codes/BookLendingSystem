using BookLendingSystem.DTOs;
using BookLendingSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace BookLendingSystem.Controllers
{
    [ApiController]
    [Route("api/lending-records")]
    public class LendingRecordController : ControllerBase
    {
        private readonly ILendingRecordService _lendingService;

        public LendingRecordController(ILendingRecordService lendingService)
        {
            _lendingService = lendingService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("borrowbook")]
        public async Task<ActionResult<LendingRecordReadDto>> BorrowBook([FromBody] LendingRecordCreateDto dto)
        {
            var record = await _lendingService.BorrowBook(dto);
            return CreatedAtAction(nameof(GetUserBorrowedBooks), new { userId = dto.UserId }, record);
        }

        [Authorize(Roles = "User")]
        [HttpPut("returnbook")]
        public async Task<ActionResult<LendingRecordReadDto>> ReturnBook([FromQuery] int userId, [FromQuery] int bookId)
        {
            var record = await _lendingService.ReturnBook(userId, bookId);
            return Ok(record);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("user/borrowed")]
        public async Task<ActionResult<PagedResult<LendingRecordReadDto>>> GetUserBorrowedBooks([FromQuery] LendingRecordQueryDto dto)
        {
            var result = await _lendingService.GetUserBorrowedBooks(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("overdue")]
        public async Task<ActionResult<PagedResult<LendingRecordReadDto>>> GetOverdueBooks([FromQuery] LendingRecordQueryDto dto)
        {
            var result = await _lendingService.GetOverdueBooks(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("history")]
        public async Task<ActionResult<PagedResult<LendingRecordReadDto>>> GetAllLendingHistory([FromQuery] LendingRecordQueryDto dto)
        {
            var result = await _lendingService.GetAllLendingHistory(dto);
            return Ok(result);
        }
    }
}
