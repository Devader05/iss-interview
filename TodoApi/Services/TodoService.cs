using Microsoft.Data.Sqlite;
using TodoApi.DTOs;
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

        public TodoResponse CreateTodo(CreateTodoRequest createTodoRequest)
        {
            var todo = new Todo
            {
                Title = createTodoRequest.Title,
                Description = createTodoRequest.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var created = _repository.Create(todo);

            return MapToResponse(created);
        }

        public List<TodoResponse> GetAllTodos()
        {
            var todos = _repository.GetAll();

            return todos.Select(MapToResponse).ToList();
        }

        public TodoResponse GetTodoById(int id)
        {
            var todo = _repository.GetById(id);

            return todo == null ? null : MapToResponse(todo);
        }

        public TodoResponse UpdateTodo(int id, UpdateTodoRequest updateTodoRequest)
        {
            var existing = _repository.GetById(id);

            if (existing == null)
                return null;

            if (updateTodoRequest.Title != null)
                existing.Title = updateTodoRequest.Title;

            if (updateTodoRequest.Description != null)
                existing.Description = updateTodoRequest.Description;

            if (updateTodoRequest.IsCompleted.HasValue)
                existing.IsCompleted = updateTodoRequest.IsCompleted.Value;

            var updated = _repository.Update(id, existing);

            return MapToResponse(updated);
        }

        public bool DeleteTodo(int id)
        {
            return _repository.Delete(id);
        }



        #region Private Methods
        private TodoResponse MapToResponse(Todo todo)
        {
            return new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt
            };
        }

        #endregion
    }
}
