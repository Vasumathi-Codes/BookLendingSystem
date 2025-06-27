using BookLendingSystem.DTOs;
using BookLendingSystem.Models;

namespace BookLendingSystem.Interfaces
{
    public interface IBookService
    {
        Task<PagedResult<BookReadDto>> GetBooks(BookQueryParameters parameters);
        Task<BookReadDto> GetBookById(int id);
        Task<BookReadDto> AddBook(BookCreateDto dto);
        Task<BookReadDto> UpdateBook(int id, BookUpdateDto dto);
        Task<BookReadDto> DeleteBook(int id);
    }
}
