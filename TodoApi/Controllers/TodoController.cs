using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.DTOs;
using TodoApi.Interfaces;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class TodoController : ControllerBase
    {

        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost("createTodo")]
        public IActionResult CreateTodo([FromBody] CreateTodoRequest createTodoRequest)
        {
            try
            {
                var todo = new Todo
                {
                    Title = createTodoRequest.Title,
                    Description = createTodoRequest.Description,
                    IsCompleted = false
                };

                var result = _todoService.CreateTodo(todo);
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
                var todos = _todoService.GetAllTodos();
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
                var todo = _todoService.GetTodoById(id);

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

                var existingTodo = _todoService.GetTodoById(id);
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

                var result = _todoService.UpdateTodo(id, todo);
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
                var result = _todoService.DeleteTodo(id);
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
