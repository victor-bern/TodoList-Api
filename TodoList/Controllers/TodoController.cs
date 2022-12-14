using Microsoft.AspNetCore.Mvc;
using TodoList.Models;
using TodoList.Repositories;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("api/v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll() => Ok(await todoRepository.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await todoRepository.GetById(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTodo([FromBody] Todo todo)
        {
            try
            {
                await todoRepository.SaveTodo(todo);
                return Ok(todo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> EditTodoStatus(int id)
        {
            try
            {
                var todo = await todoRepository.GetById(id);
                if (todo == null)
                {
                    return NotFound();
                }

                todo.IsDone = todo.IsDone ? false : true;

                await todoRepository.EditTodo(todo);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditTodo(int id, [FromBody] Todo model)
        {
            try
            {
                var todo = await todoRepository.GetById(id);
                if (todo == null)
                {
                    return NotFound();
                }

                todo.Title = model.Title;

                await todoRepository.EditTodo(todo);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                var todo = await todoRepository.GetById(id);
                if (todo == null)
                {
                    return NotFound();
                }

                await todoRepository.DeleteTodo(todo);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
