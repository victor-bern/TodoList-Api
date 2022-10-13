using FluentAssertions;
using TodoList.Models;
using TodoList.Repositories;

namespace TodoListTests.Integration.Repository
{
    public class TodoRepositoryTests : RepositoryTestsBase
    {
        private TodoRepository todoRepository;

        [SetUp]
        public void SetUp()
        {
            todoRepository = new TodoRepository(_context);
        }


        [Test]
        public async Task GetAllShouldReturnAListWithTodos()
        {
            await _context.Todos.AddAsync(new Todo
            {
                Title = "Teste"
            });
            await _context.SaveChangesAsync();

            var result = await todoRepository.GetAll();

            result.Count.Should().Be(1);
        }

        [Test]
        public async Task GetByIdShouldReturnNullWhenTodoIsNotFound()
        {
            var result = await todoRepository.GetById(1);

            result.Should().BeNull();
        }

        [Test]
        public async Task GetByIdShouldReturnAValidTodoWhenFound()
        {
            await _context.Todos.AddAsync(new Todo
            {
                Title = "Teste"
            });
            await _context.SaveChangesAsync();

            var result = await todoRepository.GetById(1);

            result.Title.Should().Be("Teste");
            result.IsDone.Should().Be(false);
        }

        [Test]
        public async Task SaveUserShouldSave()
        {
            await todoRepository.SaveTodo(new Todo
            {
                Title = "Teste"
            });

            var result = await todoRepository.GetById(1);

            result.Title.Should().Be("Teste");
            result.IsDone.Should().Be(false);
        }

        [Test]
        public async Task EditTodoShouldEditTodoTitle()
        {
            var todo = new Todo
            {
                Title = "Teste"
            };
            await todoRepository.SaveTodo(todo);
            _context.Entry(todo).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            todo.Title = "TesteEditado";
            await todoRepository.EditTodo(todo);

            var result = await todoRepository.GetById(1);

            result.Title.Should().Be("TesteEditado");
            result.IsDone.Should().Be(false);
        }

        [Test]
        public async Task EditTodoShouldEditTodoStatus()
        {
            var todo = new Todo
            {
                Title = "Teste"
            };
            await todoRepository.SaveTodo(todo);
            _context.Entry(todo).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            todo.IsDone = true;
            await todoRepository.EditTodo(todo);

            var result = await todoRepository.GetById(1);

            result.IsDone.Should().Be(true);
        }

        [Test]
        public async Task DeleteTodoShouldRemoveATodo()
        {
            var todo = new Todo
            {
                Title = "Teste"
            };
            await todoRepository.SaveTodo(todo);
            _context.Entry(todo).State = Microsoft.EntityFrameworkCore.EntityState.Detached;


            await todoRepository.DeleteTodo(todo);

            var result = await todoRepository.GetById(1);
            result.Should().BeNull();
        }
    }
}
