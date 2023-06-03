using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository, IDisposable
    {
        private readonly TodoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoRepository(TodoDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TodoItem>> GetAllAsync(string userId)
        {
            var todoItems = await _context.TodoItems.Where(todoItem => todoItem.UserId == userId).ToListAsync();

            return todoItems;
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            return todoItem;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(TodoItem item)
        {
            var currentItem = await _context.TodoItems.FindAsync(item.Id);
            if (currentItem != null)
            {
                _context.Entry(currentItem).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            //GC.SuppressFinalize(this);
        }
    }
}
