using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using TodoApi.Enums;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoRepository todoRepository, ITodoItemService todoItemService)
        {
            _todoRepository = todoRepository;
            _todoItemService = todoItemService;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(await _todoRepository.GetAllAsync(userId));
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _todoRepository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] TodoItem todoItem)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validationResult = await _todoItemService.ValidateTodoItem(id, userId);

            switch (validationResult)
            {
                case TodoItemValidationResult.NotFound:
                    return NotFound();
                case TodoItemValidationResult.Forbidden:
                    return Forbid();
                default:
                    bool isDeleted = await _todoRepository.DeleteAsync(id);

                    if (!isDeleted)
                    {
                        return NotFound(new { message = "Todo item with the specified id was not found" });
                    }
                    await _todoRepository.UpdateAsync(todoItem);
                    return NoContent();
            }
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] TodoItem todoItem)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            todoItem.UserId = userId;
            await _todoRepository.AddAsync(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validationResult = await _todoItemService.ValidateTodoItem(id, userId);

            switch (validationResult)
            {
                case TodoItemValidationResult.NotFound:
                    return NotFound();
                case TodoItemValidationResult.Forbidden:
                    return Forbid();
                default:
                    bool isDeleted = await _todoRepository.DeleteAsync(id);

                    if (!isDeleted)
                    {
                        return NotFound(new { message = "Todo item with the specified id was not found" });
                    }

                    return NoContent();
            }
        }
    }
}