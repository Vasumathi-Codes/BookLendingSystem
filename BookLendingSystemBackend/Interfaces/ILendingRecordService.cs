using BookLendingSystem.DTOs;

namespace BookLendingSystem.Interfaces
{
    public interface ILendingRecordService
    {
        Task<LendingRecordReadDto> BorrowBook(LendingRecordCreateDto dto);
        Task<LendingRecordReadDto> ReturnBook(int userId, int bookId);
        Task<PagedResult<LendingRecordReadDto>> GetUserBorrowedBooks(LendingRecordQueryDto dto);
        Task<PagedResult<LendingRecordReadDto>> GetOverdueBooks(LendingRecordQueryDto dto);
        Task<PagedResult<LendingRecordReadDto>> GetAllLendingHistory(LendingRecordQueryDto dto);
    }
}
