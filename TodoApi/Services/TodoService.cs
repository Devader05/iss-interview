using Microsoft.Data.Sqlite;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Todo CreateTodo(Todo todo)
        {
            return _repository.Create(todo);
        }

        public List<Todo> GetAllTodos()
        {
            return _repository.GetAll();
        }

        public Todo GetTodoById(int id)
        {
            return _repository.GetById(id);
        }

        public Todo UpdateTodo(int id, Todo todo)
        {
            return _repository.Update(id, todo);
        }

        public bool DeleteTodo(int id)
        {
            return _repository.Delete(id);
        }
    }
}
