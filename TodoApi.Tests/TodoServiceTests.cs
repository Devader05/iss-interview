using Xunit;
using Moq;
using FluentAssertions;
using TodoApi.Services;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.DTOs;
using TodoApi.Exceptions;
using Microsoft.Extensions.Logging;

namespace TodoApi.Tests
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _repoMock;
        private readonly Mock<ILogger<TodoService>> _loggerMock;
        private readonly TodoService _service;

        public TodoServiceTests()
        {
            _repoMock = new Mock<ITodoRepository>();
            _loggerMock = new Mock<ILogger<TodoService>>();
            _service = new TodoService(_repoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void CreateTodo_Should_Create_Todo_Successfully()
        {
            // Arrange
            var request = new CreateTodoRequest
            {
                Title = "Test",
                Description = "Test Desc"
            };

            _repoMock.Setup(r => r.Create(It.IsAny<Todo>()))
                .Returns((Todo t) =>
                {
                    t.Id = 1;
                    return t;
                });

            // Act
            var result = _service.CreateTodo(request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Test");
        }

        [Fact]
        public void GetTodoById_Should_Return_Todo_When_Exists()
        {
            var todo = new Todo { Id = 1, Title = "Test" };

            _repoMock.Setup(r => r.GetById(1)).Returns(todo);

            var result = _service.GetTodoById(1);

            result.Id.Should().Be(1);
        }

        [Fact]
        public void GetTodoById_Should_Throw_When_NotFound()
        {
            _repoMock.Setup(r => r.GetById(1)).Returns((Todo)null);

            Action act = () => _service.GetTodoById(1);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void UpdateTodo_Should_Update_Only_Provided_Fields()
        {
            var existing = new Todo
            {
                Id = 1,
                Title = "Old",
                Description = "Old Desc",
                IsCompleted = false
            };

            var request = new UpdateTodoRequest
            {
                Title = "New"
            };

            _repoMock.Setup(r => r.GetById(1)).Returns(existing);
            _repoMock.Setup(r => r.Update(1, It.IsAny<Todo>()))
                     .Returns((int id, Todo t) => t);

            var result = _service.UpdateTodo(1, request);

            result.Title.Should().Be("New");
            result.Description.Should().Be("Old Desc");
        }

        [Fact]
        public void UpdateTodo_Should_Throw_When_NotFound()
        {
            _repoMock.Setup(r => r.GetById(1)).Returns((Todo)null);

            var request = new UpdateTodoRequest();

            Action act = () => _service.UpdateTodo(1, request);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void DeleteTodo_Should_Return_True_When_Successful()
        {
            _repoMock.Setup(r => r.Delete(1)).Returns(true);

            var result = _service.DeleteTodo(1);

            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteTodo_Should_Throw_When_NotFound()
        {
            _repoMock.Setup(r => r.Delete(1)).Returns(false);

            Action act = () => _service.DeleteTodo(1);

            act.Should().Throw<NotFoundException>();
        }
    }
}