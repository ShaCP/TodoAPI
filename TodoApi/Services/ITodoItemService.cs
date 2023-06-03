using TodoApi.Enums;

namespace TodoApi.Services
{
    public interface ITodoItemService
    {
        Task<TodoItemValidationResult> ValidateTodoItem(int id, string userId);
    }
}
