using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CourseSellingApp.Database.Services;
using CourseSellingApp.Models;
using Npgsql;
using Splat;

namespace CourseSellingApp.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly string _connectionString;

        public AuthorService()
        {
            var dbService = Locator.Current.GetService<DatabaseService>();
            if (dbService == null)
            {
                throw new InvalidOperationException("DatabaseService is not registered in the DI container.");
            }
            _connectionString = dbService.GetConnectionString();
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var authors = new List<Author>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("SELECT * FROM Authors", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                authors.Add(MapReaderToModel(reader));
            }

            return authors;
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("SELECT * FROM Authors WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapReaderToModel(reader);
            }

            return null;
        }

        public async Task AddAuthorAsync(Author author)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                INSERT INTO Authors (FullName, Biography, Photo)
                VALUES (@FullName, @Biography, @Photo)";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@FullName", author.FullName);
            command.Parameters.AddWithValue("@Biography", (object?)author.Biography ?? DBNull.Value);
            command.Parameters.AddWithValue("@Photo", (object?)author.Photo ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                UPDATE Authors
                SET FullName = @FullName, Biography = @Biography, Photo = @Photo
                WHERE Id = @Id";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", author.Id);
            command.Parameters.AddWithValue("@FullName", author.FullName);
            command.Parameters.AddWithValue("@Biography", (object?)author.Biography ?? DBNull.Value);
            command.Parameters.AddWithValue("@Photo", (object?)author.Photo ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = "DELETE FROM Authors WHERE Id = @Id";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private static Author MapReaderToModel(IDataRecord reader)
        {
            return new Author
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Biography = reader.IsDBNull(reader.GetOrdinal("Biography")) ? null : reader.GetString(reader.GetOrdinal("Biography")),
                Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Photo"))
            };
        }
    }
}
