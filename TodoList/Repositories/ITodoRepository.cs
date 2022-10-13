using TodoList.Models;

namespace TodoList.Repositories
{
    public interface ITodoRepository
    {
        Task<IList<Todo>> GetAll();
        Task<Todo?> GetById(int id);
        Task SaveTodo(Todo todo);
        Task EditTodo(Todo todo);
        Task DeleteTodo(Todo todo);
    }
}
