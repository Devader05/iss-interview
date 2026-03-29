using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class TodoController : ControllerBase
    {
        public TodoController()
        {
        }

        [HttpPost("createTodo")]
        public IActionResult CreateTodo([FromBody] CreateTodoRequest createTodoRequest)
        {
            try
            {
                var todoService = new TodoService();
                var todo = new Todo
                {
                    Title = createTodoRequest.Title,
                    Description = createTodoRequest.Description,
                    IsCompleted = false
                };

                var result = todoService.CreateTodo(todo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getTodo")]
        public IActionResult GetAllTodos()
        {
            try
            {
                var todoService = new TodoService();
                var todos = todoService.GetAllTodos();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getTodo/{id}")]
        public IActionResult GetTodoById(int id)
        {
            try
            {
                var todoService = new TodoService();
                var todo = todoService.GetTodoById(id);

                if (todo == null)
                {
                    return NotFound();
                }

                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateTodo/{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
        {
            try
            {
                var todoService = new TodoService();

                var existingTodo = todoService.GetTodoById(id);
                if (existingTodo == null)
                {
                    return NotFound();
                }

                var todo = new Todo
                {
                    Title = request.Title,
                    Description = request.Description,
                    IsCompleted = request.IsCompleted
                };

                var result = todoService.UpdateTodo(id, todo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deleteTodo/{id}")]
        public IActionResult DeleteTodo(int id)
        {
            try
            {
                var todoService = new TodoService();
                var result = todoService.DeleteTodo(id);
                if (result)
                {
                    return Ok(new { message = "Todo deleted successfully" });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
