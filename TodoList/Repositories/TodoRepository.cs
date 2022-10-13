using Microsoft.EntityFrameworkCore;
using TodoList.Context;
using TodoList.Models;

namespace TodoList.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoAppContext _context;

        public TodoRepository(TodoAppContext context)
        {
            _context = context;
        }

        public async Task<IList<Todo>> GetAll() => await _context.Todos.AsNoTracking().ToListAsync();

        public async Task<Todo?> GetById(int id) => await _context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        public async Task SaveTodo(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
        }
        public async Task EditTodo(Todo todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodo(Todo todo)
        {
            _context.Remove(todo);
            await _context.SaveChangesAsync();
        }


    }
}
