using BookLendingSystem.Contexts;
using BookLendingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLendingSystem.Repositories
{
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<User?> Get(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
