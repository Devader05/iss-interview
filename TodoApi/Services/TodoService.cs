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
        private readonly ILogger<TodoService> _logger;

        public TodoService(ITodoRepository repository, ILogger<TodoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public TodoResponse CreateTodo(CreateTodoRequest createTodoRequest)
        {
            _logger.LogInformation("Creating new todo with title: {Title}", createTodoRequest.Title);

            var todo = new Todo
            {
                Title = createTodoRequest.Title,
                Description = createTodoRequest.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var created = _repository.Create(todo);

            _logger.LogInformation("Todo created with id: {Id}", created.Id);

            return MapToResponse(created);
        }

        public List<TodoResponse> GetAllTodos()
        {
            _logger.LogInformation("Fetching all todos");
            
            var todos = _repository.GetAll();

            return todos.Select(MapToResponse).ToList();
        }

        public TodoResponse GetTodoById(int id)
        {
            _logger.LogInformation("Fetching todo with id: {Id}", id);

            var todo = _repository.GetById(id);

            if (todo == null)
            {
                _logger.LogWarning("Todo not found with id: {Id}", id);
                throw new NotFoundException($"Todo with id {id} not found");
            }

            return MapToResponse(todo);
        }

        public TodoResponse UpdateTodo(int id, UpdateTodoRequest updateTodoRequest)
        {
            _logger.LogInformation("Updating todo with id: {Id}", id);

            var existing = _repository.GetById(id);

            if (existing == null)
            {
                _logger.LogWarning("Update failed. Todo not found with id: {Id}", id);
                throw new NotFoundException($"Todo with id {id} not found");
            }

            if (updateTodoRequest.Title != null)
                existing.Title = updateTodoRequest.Title;

            if (updateTodoRequest.Description != null)
                existing.Description = updateTodoRequest.Description;

            if (updateTodoRequest.IsCompleted.HasValue)
                existing.IsCompleted = updateTodoRequest.IsCompleted.Value;

            var updated = _repository.Update(id, existing);

            _logger.LogInformation("Todo updated successfully with id: {Id}", id);

            return MapToResponse(updated);
        }

        public bool DeleteTodo(int id)
        {
            _logger.LogInformation("Deleting todo with id: {Id}", id);

            var deleted = _repository.Delete(id);

            if (!deleted)
            {
                _logger.LogWarning("Delete failed. Todo not found with id: {Id}", id);
                throw new NotFoundException($"Todo with id {id} not found");
            }

            _logger.LogInformation("Todo deleted successfully with id: {Id}", id);

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
