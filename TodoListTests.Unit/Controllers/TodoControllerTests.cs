using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoList.Controllers;
using TodoList.Models;
using TodoList.Repositories;

namespace TodoListTests.Unity.Controllers
{
    public class TodoControllerTests
    {
        private Mock<ITodoRepository> todoRepositoryMock;
        private TodoController todoController;

        [SetUp]
        public void Setup()
        {
            todoRepositoryMock = new Mock<ITodoRepository>();
            todoController = new TodoController(todoRepositoryMock.Object);
            var todoList = new List<Todo>
            {
                new Todo {Title = "Todo 1"},
                new Todo {Title = "Todo 2"}
            };
            todoRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(todoList);
        }


        [Test]
        public async Task GetAllShouldReturnAlistOfTodos()
        {
            var result = await todoController.GetAll();

            var resultObject = (ObjectResult)result;
            var value = (List<Todo>)resultObject.Value;

            result.Should().BeOfType(typeof(OkObjectResult));
            value.Count.Should().Be(2);
        }

        [Test]
        public async Task GetByIdShouldReturnNotFoundObjectWhenDontFound()
        {
            todoRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Todo)null);
            var result = await todoController.GetById(1);

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task GetByIdShouldReturnTodoWhenIdIsNotValid()
        {
            todoRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Todo
            {
                Id = 1,
                Title = "Teste",
                IsDone = true,
            });
            var result = await todoController.GetById(1);

            var resultObject = (ObjectResult)result;
            var value = (Todo)resultObject.Value;

            value.Id.Should().Be(1);
            value.Title.Should().Be("Teste");
            value.IsDone.Should().Be(true);
        }

        [Test]
        public async Task SaveTodoShouldReturnInternalServerErrorStatusWhenThrowsException()
        {
            var todo = new Todo();
            todoRepositoryMock.Setup(x => x.SaveTodo(It.IsAny<Todo>())).ThrowsAsync(new Exception());
            var result = await todoController.SaveTodo(todo);

            var objectResult = (StatusCodeResult)result;

            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }


        [Test]
        public async Task SaveTodoShouldReturnOkStatusWhenTodoIsSave()
        {
            var todo = new Todo();
            var result = await todoController.SaveTodo(todo);

            result.Should().BeOfType(typeof(OkObjectResult));
        }


        [Test]
        public async Task EditTodoStatusShouldReturnNotFoundWhenIdIsNotValid()
        {
            todoRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Todo)null);
            var result = await todoController.EditTodoStatus(1);

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        public async Task EditTodoStatusShouldReturnInternalServerErrorStatusWhenThrowsException()
        {
            var todo = new Todo();
            todoRepositoryMock.Setup(x => x.EditTodo(It.IsAny<Todo>())).ThrowsAsync(new Exception());
            var result = await todoController.EditTodoStatus(1);

            var objectResult = (StatusCodeResult)result;

            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Test]
        public async Task EditTodoStatusShouldReturnNoContentStatusWhenTodoStatusIsEdited()
        {
            todoRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Todo
            {
                Id = 1,
                Title = "Teste",
                IsDone = true,
            });
            var result = await todoController.EditTodoStatus(1);

            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public async Task DeleteTodoShouldReturnNotFoundWhenIdIsNotValid()
        {
            todoRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Todo)null);
            var result = await todoController.DeleteTodo(1);

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        public async Task DeleteTodoShouldReturnInternalServerErrorStatusWhenThrowsException()
        {
            var todo = new Todo();
            todoRepositoryMock.Setup(x => x.EditTodo(It.IsAny<Todo>())).ThrowsAsync(new Exception());
            var result = await todoController.DeleteTodo(1);

            var objectResult = (StatusCodeResult)result;

            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Test]
        public async Task DeleteTodoShouldReturnNoContentStatusWhenTodoStatusIsEdited()
        {
            todoRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Todo
            {
                Id = 1,
                Title = "Teste",
                IsDone = true,
            });
            var result = await todoController.DeleteTodo(1);

            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
