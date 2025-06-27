using BookLendingSystem.Contexts;
using BookLendingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLendingSystem.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        protected Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Add(T item)
        {
            var createdAtProp = typeof(T).GetProperty("CreatedAt");
            if (createdAtProp != null && createdAtProp.PropertyType == typeof(DateTime))
            {
                createdAtProp.SetValue(item, DateTime.UtcNow);
            }

            _context.Set<T>().Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public virtual async Task<T> Update(K key, T item)
        {
            var existingItem = await Get(key);
            if (existingItem == null)
                throw new Exception("No such item found for update");

            _context.Entry(existingItem).CurrentValues.SetValues(item);

            var updatedAtProp = typeof(T).GetProperty("UpdatedAt");
            if (updatedAtProp != null && updatedAtProp.PropertyType == typeof(DateTime?))
            {
                updatedAtProp.SetValue(existingItem, DateTime.UtcNow);
            }

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public virtual async Task<T?> Delete(K key)
        {
            var item = await Get(key);
            if (item == null)
                return null;

            var type = typeof(T);
            var isDeletedProp = type.GetProperty("IsDeleted");
            var deletedAtProp = type.GetProperty("DeletedAt");
            var updatedAtProp = type.GetProperty("UpdatedAt");

            if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
            {
                var isDeleted = (bool)(isDeletedProp.GetValue(item) ?? false);
                if (isDeleted)
                    throw new Exception("Item is already deleted.");

                isDeletedProp.SetValue(item, true);

                if (deletedAtProp != null && deletedAtProp.PropertyType == typeof(DateTime?))
                    deletedAtProp.SetValue(item, DateTime.UtcNow);

                if (updatedAtProp != null && updatedAtProp.PropertyType == typeof(DateTime?))
                    updatedAtProp.SetValue(item, DateTime.UtcNow);

                _context.Update(item);
            }
            else
            {
                _context.Remove(item);
            }

            await _context.SaveChangesAsync();
            return item;
        }

        public abstract Task<T?> Get(K key);
        public abstract Task<IEnumerable<T>> GetAll();
    }
}
