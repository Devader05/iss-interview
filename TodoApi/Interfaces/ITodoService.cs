using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoService
    {
        Todo CreateTodo(Todo todo);
        List<Todo> GetAllTodos();
        Todo GetTodoById(int id);
        Todo UpdateTodo(int id, Todo todo);
        bool DeleteTodo(int id);
    }
}
