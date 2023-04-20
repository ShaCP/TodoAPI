using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int Id);
        Task<bool> SaveChangesAsync();
    }
}
