using BookLendingSystem.Contexts;
using BookLendingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLendingSystem.Repositories
{
    public class LendingRecordRepository : Repository<int, LendingRecord>
    {
        public LendingRecordRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<LendingRecord?> Get(int id)
        {
            return await _context.LendingRecords
                .Include(lr => lr.User)
                .Include(lr => lr.Book)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public override async Task<IEnumerable<LendingRecord>> GetAll()
        {
            return await _context.LendingRecords
                .Include(lr => lr.User)
                .Include(lr => lr.Book)
                .ToListAsync();
        }
    }
}
