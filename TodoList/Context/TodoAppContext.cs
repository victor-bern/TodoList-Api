using Microsoft.EntityFrameworkCore;
using TodoList.Context.Mappings;
using TodoList.Models;

namespace TodoList.Context
{
    public class TodoAppContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        3
        public TodoAppContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TodoMapping());
        }
    }
}
