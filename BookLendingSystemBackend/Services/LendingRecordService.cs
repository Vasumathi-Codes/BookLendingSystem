using AutoMapper;
using BookLendingSystem.DTOs;
using BookLendingSystem.Exceptions;
using BookLendingSystem.Interfaces;
using BookLendingSystem.Models;
using BookLendingSystem.Repositories;

namespace BookLendingSystem.Services
{
    public class LendingRecordService : ILendingRecordService
    {
        private readonly IRepository<int, LendingRecord> _lendingRepo;
        private readonly IRepository<int, Book> _bookRepo;
        private readonly IRepository<int, User> _userRepo;
        private readonly IMapper _mapper;

        public LendingRecordService(
            IRepository<int, LendingRecord> lendingRepo,
            IRepository<int, Book> bookRepo,
            IRepository<int, User> userRepo,
            IMapper mapper)
        {
            _lendingRepo = lendingRepo;
            _bookRepo = bookRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<LendingRecordReadDto> BorrowBook(LendingRecordCreateDto dto)
        {
            var user = await _userRepo.Get(dto.UserId) ?? throw new UserNotFoundException(dto.UserId);
            var book = await _bookRepo.Get(dto.BookId) ?? throw new BookNotFoundException(dto.BookId);

            if (book.AvailableCopies <= 0)
                throw new NoAvailableCopiesException(book.Title);

            var allRecords = await _lendingRepo.GetAll();
            bool alreadyBorrowed = allRecords.Any(r =>
                r.UserId == dto.UserId && r.BookId == dto.BookId && r.ReturnDate == null);

            if (alreadyBorrowed)
                throw new DuplicateBorrowingException(user.Name, book.Title);

            var newRecord = _mapper.Map<LendingRecord>(dto);
            var saved = await _lendingRepo.Add(newRecord);

            book.AvailableCopies--;
            await _bookRepo.Update(book.Id, book);

            return _mapper.Map<LendingRecordReadDto>(saved);
        }

        public async Task<LendingRecordReadDto> ReturnBook(int userId, int bookId)
        {
            var user = await _userRepo.Get(userId) ?? throw new UserNotFoundException(userId);
            var book = await _bookRepo.Get(bookId) ?? throw new BookNotFoundException(bookId);

            var allRecords = await _lendingRepo.GetAll();
            var record = allRecords.FirstOrDefault(r =>
                r.UserId == userId && r.BookId == bookId && r.ReturnDate == null);

            if (record == null)
                throw new LendingRecordNotFoundException(userId, bookId);

            record.ReturnDate = DateTime.UtcNow;
            var updated = await _lendingRepo.Update(record.Id, record);

            book.AvailableCopies++;
            await _bookRepo.Update(book.Id, book);

            return _mapper.Map<LendingRecordReadDto>(updated);
        }

        public async Task<PagedResult<LendingRecordReadDto>> GetUserBorrowedBooks(LendingRecordQueryDto dto)
        {
            if (!dto.UserId.HasValue)
                throw new ArgumentException("UserId is required.");

            var allRecords = await _lendingRepo.GetAll();
            var filtered = allRecords.Where(r => r.UserId == dto.UserId.Value).ToList();

            return ApplyFilters(filtered, dto);
        }

        public async Task<PagedResult<LendingRecordReadDto>> GetOverdueBooks(LendingRecordQueryDto dto)
        {
            var now = DateTime.UtcNow;
            var allRecords = await _lendingRepo.GetAll();
            var filtered = allRecords
                .Where(r => r.ReturnDate == null && r.DueDate < now)
                .ToList();

            return ApplyFilters(filtered, dto);
        }

        public async Task<PagedResult<LendingRecordReadDto>> GetAllLendingHistory(LendingRecordQueryDto dto)
        {
            var allRecords = await _lendingRepo.GetAll();
            return ApplyFilters(allRecords.ToList(), dto);
        }

        private PagedResult<LendingRecordReadDto> ApplyFilters(List<LendingRecord> records, LendingRecordQueryDto dto)
        {
            // Filter by BookId
            if (dto.BookId.HasValue)
            {
                records = records.Where(r => r.BookId == dto.BookId.Value).ToList();
            }

            // Search by BookTitle or UserName
            // Searching (safe null handling)
            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = dto.Search.ToLower();
                records = records.Where(r =>
                    r.Book?.Title?.ToLower().Contains(search) == true ||
                    r.User?.Name?.ToLower().Contains(search) == true
                ).ToList();
            }


            // Sorting
            records = dto.SortBy?.ToLower() switch
            {
                "duedate" => dto.SortOrder.ToLower() == "asc"
                    ? records.OrderBy(r => r.DueDate).ToList()
                    : records.OrderByDescending(r => r.DueDate).ToList(),

                "returndate" => dto.SortOrder.ToLower() == "asc"
                    ? records.OrderBy(r => r.ReturnDate).ToList()
                    : records.OrderByDescending(r => r.ReturnDate).ToList(),

                _ => dto.SortOrder.ToLower() == "asc"
                    ? records.OrderBy(r => r.BorrowDate).ToList()
                    : records.OrderByDescending(r => r.BorrowDate).ToList(),
            };

            // Pagination
            var totalCount = records.Count;
            var paged = records
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToList();

            return new PagedResult<LendingRecordReadDto>
            {
                Items = _mapper.Map<IEnumerable<LendingRecordReadDto>>(paged),
                TotalCount = totalCount,
                Page = dto.Page,
                PageSize = dto.PageSize
            };
        }
    }
}
