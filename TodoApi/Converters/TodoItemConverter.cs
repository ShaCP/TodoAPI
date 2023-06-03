using TodoApi.Models;

namespace TodoApi.Converters
{
    public class TodoItemConverter : ITodoItemConverter
    {
        public TodoItemDto ConvertToDto(TodoItem item)
        {
            return CreateDto(item);
        }

        public List<TodoItemDto> ConvertToDto(List<TodoItem> items)
        {
            return items.Select(CreateDto).ToList();
        }

        private TodoItemDto CreateDto(TodoItem item)
        {
            return new TodoItemDto
            {
                Id = item.Id,
                Description = item.Description,
                Title = item.Title,
                IsCompleted = item.IsCompleted
            };
        }
    }
}
