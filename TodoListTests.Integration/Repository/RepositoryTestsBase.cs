using Microsoft.EntityFrameworkCore;
using TodoList.Context;

namespace TodoListTests.Integration.Repository
{
    public class RepositoryTestsBase
    {
        protected TodoAppContext _context;

        [TearDown]
        public void Destroy()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [SetUp]
        public void SetupContext()
        {
            var builder = new DbContextOptionsBuilder<TodoAppContext>()
                .UseNpgsql($"User ID=postgres;Password=Veteranos1;Host=Localhost;Port=5432;Database=TodoApp.Test;", opt => opt.EnableRetryOnFailure());

            _context = new TodoAppContext(builder.Options);

            _context.Database.Migrate();
        }
    }
}
