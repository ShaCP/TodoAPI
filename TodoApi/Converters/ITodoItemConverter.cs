using TodoApi.Models;

namespace TodoApi.Converters
{
    public interface ITodoItemConverter
    {
        List<TodoItemDto> ConvertToDto(List<TodoItem> items);
        TodoItemDto ConvertToDto(TodoItem item);
    }
}