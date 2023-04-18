using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

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
