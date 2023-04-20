using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
         TodoItems = Set<TodoItem>();
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
