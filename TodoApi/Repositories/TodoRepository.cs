using Microsoft.Data.Sqlite;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private string _connectionString = "Data Source=todos.db";

        public Todo Create(Todo todo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt)
                VALUES ('{todo.Title}', '{todo.Description}', {(todo.IsCompleted ? 1 : 0)}, '{DateTime.UtcNow:o}');
                SELECT last_insert_rowid();
            ";

            var id = Convert.ToInt32(command.ExecuteScalar());
            todo.Id = id;
            todo.CreatedAt = DateTime.UtcNow;

            return todo;
        }

        public List<Todo> GetAll()
        {
            var todos = new List<Todo>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Todos";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                todos.Add(Map(reader));
            }

            return todos;
        }

        public Todo GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Todos WHERE Id = {id}";

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return Map(reader);
            }

            return null;
        }

        public Todo Update(int id, Todo todo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                UPDATE Todos
                SET Title = '{todo.Title}', Description = '{todo.Description}', IsCompleted = {(todo.IsCompleted ? 1 : 0)}
                WHERE Id = {id}
            ";

            command.ExecuteNonQuery();

            todo.Id = id;
            return todo;
        }

        public bool Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Todos WHERE Id = {id}";

            return command.ExecuteNonQuery() > 0;
        }

        private Todo Map(SqliteDataReader reader)
        {
            return new Todo
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                IsCompleted = reader.GetInt32(3) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(4))
            };
        }
    }
}
