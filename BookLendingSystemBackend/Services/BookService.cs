using AutoMapper;
using BookLendingSystem.DTOs;
using BookLendingSystem.Exceptions;
using BookLendingSystem.Interfaces;
using BookLendingSystem.Models;
using Microsoft.Extensions.Logging;

namespace BookLendingSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<int, Book> _bookRepo;
        private readonly ILogger<BookService> _logger;
        private readonly IMapper _mapper;

        public BookService(IRepository<int, Book> bookRepo, ILogger<BookService> logger, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedResult<BookReadDto>> GetBooks(BookQueryParameters parameters)
        {
            try
            {
                var allBooks = await _bookRepo.GetAll();
                var query = allBooks.AsQueryable();

                if (!string.IsNullOrWhiteSpace(parameters.Search))
                {
                    var keyword = parameters.Search.ToLower();
                    query = query.Where(b =>
                        b.Title.ToLower().Contains(keyword) ||
                        b.Author.ToLower().Contains(keyword));
                }

                if (!string.IsNullOrWhiteSpace(parameters.Author))
                    query = query.Where(b => b.Author.ToLower().Contains(parameters.Author.ToLower()));

                if (!string.IsNullOrWhiteSpace(parameters.Title))
                    query = query.Where(b => b.Title.ToLower().Contains(parameters.Title.ToLower()));

                if (!string.IsNullOrWhiteSpace(parameters.SortBy))
                {
                    query = parameters.SortBy.ToLower() switch
                    {
                        "title" => parameters.SortOrder == "desc"
                            ? query.OrderByDescending(b => b.Title)
                            : query.OrderBy(b => b.Title),

                        "author" => parameters.SortOrder == "desc"
                            ? query.OrderByDescending(b => b.Author)
                            : query.OrderBy(b => b.Author),

                        "createdat" => parameters.SortOrder == "desc"
                            ? query.OrderByDescending(b => b.CreatedAt)
                            : query.OrderBy(b => b.CreatedAt),

                        _ => query
                    };
                }

                var totalCount = query.Count();
                var items = query
                    .Skip((parameters.Page - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .ToList();

                var mappedItems = _mapper.Map<List<BookReadDto>>(items);

                return new PagedResult<BookReadDto>
                {
                    Items = mappedItems,
                    TotalCount = totalCount,
                    Page = parameters.Page,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching paged book list.");
                throw;
            }
        }

        public async Task<BookReadDto> GetBookById(int id)
        {
            try
            {
                var book = await _bookRepo.Get(id);
                if (book == null || book.IsDeleted)
                    throw new BookNotFoundException(id);

                return _mapper.Map<BookReadDto>(book);
            }
            catch (BookNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving book with ID {id}");
                throw;
            }
        }

        public async Task<BookReadDto> AddBook(BookCreateDto createDto)
        {
            try
            {
                var book = _mapper.Map<Book>(createDto);
                book.CreatedAt = DateTime.UtcNow;
                var added = await _bookRepo.Add(book);
                return _mapper.Map<BookReadDto>(added);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book.");
                throw;
            }
        }

        public async Task<BookReadDto> UpdateBook(int id, BookUpdateDto updateDto)
        {
            try
            {
                var existing = await _bookRepo.Get(id);
                if (existing == null || existing.IsDeleted)
                    throw new BookNotFoundException(id);

                if (!string.IsNullOrWhiteSpace(updateDto.Description))
                    existing.Description = updateDto.Description;

                if (updateDto.TotalCopies.HasValue)
                {
                    int difference = updateDto.TotalCopies.Value - existing.TotalCopies;

                    if (difference < 0 && existing.AvailableCopies < -difference)
                        throw new InvalidOperationException("Cannot reduce total copies below currently borrowed count.");

                    existing.TotalCopies = updateDto.TotalCopies.Value;
                    existing.AvailableCopies += difference;
                }

                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _bookRepo.Update(id, existing);
                return _mapper.Map<BookReadDto>(updated);
            }
            catch (BookNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book with ID {id}");
                throw;
            }
        }

        public async Task<BookReadDto> DeleteBook(int id)
        {
            try
            {
                var book = await _bookRepo.Get(id);
                if (book == null || book.IsDeleted)
                    throw new BookNotFoundException(id);

                var deleted = await _bookRepo.Delete(id);
                return _mapper.Map<BookReadDto>(deleted);
            }
            catch (BookNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book with ID {id}");
                throw;
            }
        }
    }
}
