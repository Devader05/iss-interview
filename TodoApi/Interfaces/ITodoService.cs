using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoService
    {
        TodoResponse CreateTodo(CreateTodoRequest request);
        List<TodoResponse> GetAllTodos();
        TodoResponse GetTodoById(int id);
        TodoResponse UpdateTodo(int id, UpdateTodoRequest request);
        bool DeleteTodo(int id);
    }
}
