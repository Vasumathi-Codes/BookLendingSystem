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

        // Unique constraint on ISBN
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique();
    }

}
