using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoDbContext : IdentityDbContext<ApplicationUser>
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
         TodoItems = Set<TodoItem>();
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
