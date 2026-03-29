using Microsoft.Data.Sqlite;
using TodoApi.Exceptions;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private string _connectionString = "Data Source=todos.db";

        public Todo Create(Todo todo)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt)
                    VALUES ($title, $description, $isCompleted, $createdAt);
                    SELECT last_insert_rowid();
                ";

                command.Parameters.AddWithValue("$title", todo.Title);
                command.Parameters.AddWithValue("$description", todo.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$isCompleted", todo.IsCompleted ? 1 : 0);
                command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

                var id = Convert.ToInt32(command.ExecuteScalar());

                todo.Id = id;
                todo.CreatedAt = DateTime.UtcNow;

                return todo;
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException("Failed to create todo", ex);
            }
        }

        public List<Todo> GetAll()
        {
            try
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
            catch (SqliteException ex)
            {
                throw new DatabaseException("Failed to fetch todos", ex);
            }
        }

        public Todo GetById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Todos WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                    return Map(reader);

                return null;
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException($"Failed to fetch todo with id {id}", ex);
            }
        }

        public Todo Update(int id, Todo todo)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Todos
                    SET Title = $title,
                        Description = $description,
                        IsCompleted = $isCompleted
                    WHERE Id = $id
                ";

                command.Parameters.AddWithValue("$title", todo.Title);
                command.Parameters.AddWithValue("$description", todo.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$isCompleted", todo.IsCompleted ? 1 : 0);
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();

                todo.Id = id;
                return todo;
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException($"Failed to update todo with id {id}", ex);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Todos WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                return command.ExecuteNonQuery() > 0;
            }
            catch (SqliteException ex)
            {
                throw new DatabaseException($"Failed to delete todo with id {id}", ex);
            }
        }

        private Todo Map(SqliteDataReader reader)
        {
            return new Todo
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsCompleted = reader.GetInt32(3) == 1,
                CreatedAt = DateTime.Parse(reader.GetString(4))
            };
        }
    }
}
