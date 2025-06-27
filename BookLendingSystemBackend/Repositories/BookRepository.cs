using BookLendingSystem.Contexts;
using BookLendingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLendingSystem.Repositories
{
    public class BookRepository : Repository<int, Book>
    {
        public BookRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Book?> Get(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public override async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }
    }
}
