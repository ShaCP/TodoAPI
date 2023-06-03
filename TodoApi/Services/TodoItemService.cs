using TodoApi.Enums;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    // Service class
    public class TodoItemService: ITodoItemService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoItemService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoItemValidationResult> ValidateTodoItem(int id, string userId)
        {
            var existingTodoItem = await _todoRepository.GetByIdAsync(id);

            if (existingTodoItem == null)
            {
                return TodoItemValidationResult.NotFound;
            }

            if (existingTodoItem.UserId != userId)
            {
                return TodoItemValidationResult.Forbidden;
            }

            return TodoItemValidationResult.Valid;
        }
    }
}
