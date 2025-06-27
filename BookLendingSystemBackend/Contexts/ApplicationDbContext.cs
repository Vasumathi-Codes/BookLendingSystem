using BookLendingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLendingSystem.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<LendingRecord> LendingRecords => Set<LendingRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft delete filters (exclude IsDeleted = true)
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Book>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<LendingRecord>().HasQueryFilter(e => !e.IsDeleted);

        // Required role for User
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .IsRequired();

        // Unique username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Name)
            .IsUnique();

        // Unique constraint on ISBN
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique();

        modelBuilder.Entity<LendingRecord>()
            .HasIndex(lr => new { lr.UserId, lr.BookId, lr.ReturnDate })
            .IsUnique()
            .HasFilter("\"ReturnDate\" IS NULL"); 


    }

}
