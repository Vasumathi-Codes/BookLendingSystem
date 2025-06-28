using BookLendingSystem.DTOs;
using BookLendingSystem.Interfaces;
using BookLendingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookLendingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BookReadDto>), 200)]
        public async Task<ActionResult<PagedResult<BookReadDto>>> GetBooks([FromQuery] BookQueryParameters parameters)
        {
            var result = await _bookService.GetBooks(parameters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookReadDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookReadDto>> GetBook(int id)
        {
            var book = await _bookService.GetBookById(id);
            return Ok(book);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(BookReadDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BookReadDto>> CreateBook([FromBody] BookCreateDto createDto)
        {
            var book = await _bookService.AddBook(createDto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(BookReadDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookReadDto>> UpdateBook(int id, [FromBody] BookUpdateDto updateDto)
        {
            var updated = await _bookService.UpdateBook(id, updateDto);
            return Ok(updated);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(BookReadDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookReadDto>> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteBook(id);
            return Ok(deleted);
        }
    }
}
