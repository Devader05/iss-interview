using Microsoft.Data.Sqlite;
using TodoApi.DTOs;
using TodoApi.Exceptions;
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

        public TodoResponse CreateTodo(CreateTodoRequest request)
        {
            var todo = new Todo
            {
                Title = request.Title,
                Description = request.Description,
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

            if (todo == null)
                throw new NotFoundException($"Todo with id {id} not found");

            return MapToResponse(todo);
        }

        public TodoResponse UpdateTodo(int id, UpdateTodoRequest request)
        {
            var existing = _repository.GetById(id);

            if (existing == null)
                throw new NotFoundException($"Todo with id {id} not found");

            // PATCH-style update
            if (request.Title != null)
                existing.Title = request.Title;

            if (request.Description != null)
                existing.Description = request.Description;

            if (request.IsCompleted.HasValue)
                existing.IsCompleted = request.IsCompleted.Value;

            var updated = _repository.Update(id, existing);

            return MapToResponse(updated);
        }

        public bool DeleteTodo(int id)
        {
            var deleted = _repository.Delete(id);

            if (!deleted)
                throw new NotFoundException($"Todo with id {id} not found");

            return true;
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
