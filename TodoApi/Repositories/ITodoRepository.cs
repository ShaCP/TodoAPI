using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllAsync(string userId);
        Task<TodoItem?> GetByIdAsync(int id);
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task<bool> DeleteAsync(int Id);
        Task<bool> SaveChangesAsync();
    }
}
