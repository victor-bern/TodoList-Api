using FluentAssertions;
using Newtonsoft.Json;
using System.Text;
using TodoList.Models;

namespace TodoListTests.Integration.Controllers
{
    [NonParallelizable]
    public class TodoControllerTests : WebFactoryProgram
    {
        private HttpClient _client;


        [SetUp]
        public void Setup()
        {
            _client = CreateClient();
        }


        [Test]
        public async Task GetAll_ShouldReturnStatusCodeOk()
        {
            var response = await _client.GetAsync("api/v1/todos");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task GetById_ShouldReturnStatusNotFoundWhenDontFindTodo()
        {
            var response = await _client.GetAsync("api/v1/todos/2");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        }

        [Test]
        [Order(0)]
        public async Task GetById_ShouldReturnStatusOkWhenFindTodo()
        {
            var response = await _client.GetAsync("api/v1/todos/1");

            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            content.Should().Contain("Teste");
            content.Should().Contain("false");
        }

        [Test]
        public async Task SaveTodo_ShouldReturnNoContentWhenSave()
        {
            var todo = new Todo
            {
                Title = "Teste2"
            };

            var stringContent = JsonConvert.SerializeObject(todo);

            var response = await _client.PostAsync("api/v1/todos", new StringContent(stringContent, Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Test]
        public async Task EditTodoStatus_ShouldReturnStatusNotFoundWhenIdIsNotValid()
        {
            var response = await _client.PutAsync("api/v1/todos/status/2", null);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        [Order(1)]
        public async Task EditTodoStatus_ShouldReturnStatusNoContentWhenIdIsValid()
        {
            var response = await _client.PutAsync("api/v1/todos/status/1", null);
            var res = await _client.GetAsync("api/v1/todos");
            var content = await res.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Test]
        public async Task DeleteTodo_ShouldReturnStatusNotFoundWhenIdIsNotValid()
        {
            var response = await _client.DeleteAsync("api/v1/todos/delete/2");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task DeleteTodo_ShouldReturnStatusNoContentWhenIdIsValid()
        {
            var response = await _client.DeleteAsync("api/v1/todos/delete/1");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
