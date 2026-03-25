using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CourseSellingApp.Database.Services;
using CourseSellingApp.Models;
using Npgsql;
using Splat;

namespace CourseSellingApp.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly DatabaseService _dbService;

        public AuthorService()
        {
            _dbService = Locator.Current.GetService<DatabaseService>() 
                ?? throw new InvalidOperationException("DatabaseService is not registered in the DI container.");
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            const string sql = "SELECT * FROM Authors";
            return await _dbService.QueryAsync(sql, MapReaderToModel);
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Authors WHERE Id = @Id";
            var result = await _dbService.QueryAsync(sql, MapReaderToModel, p => p.AddWithValue("@Id", id));
            return result.FirstOrDefault();
        }

        public async Task AddAuthorAsync(Author author)
        {
            const string sql = @"
                INSERT INTO Authors (FullName, Biography, Photo)
                VALUES (@FullName, @Biography, @Photo)";

            await _dbService.ExecuteNonQueryAsync(sql, p =>
            {
                p.AddWithValue("@FullName", author.FullName);
                p.AddWithValue("@Biography", (object?)author.Biography ?? DBNull.Value);
                p.AddWithValue("@Photo", (object?)author.Photo ?? DBNull.Value);
            });
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            const string sql = @"
                UPDATE Authors
                SET FullName = @FullName, Biography = @Biography, Photo = @Photo
                WHERE Id = @Id";

            await _dbService.ExecuteNonQueryAsync(sql, p =>
            {
                p.AddWithValue("@Id", author.Id);
                p.AddWithValue("@FullName", author.FullName);
                p.AddWithValue("@Biography", (object?)author.Biography ?? DBNull.Value);
                p.AddWithValue("@Photo", (object?)author.Photo ?? DBNull.Value);
            });
        }

        public async Task DeleteAuthorAsync(int id)
        {
            const string sql = "DELETE FROM Authors WHERE Id = @Id";
            await _dbService.ExecuteNonQueryAsync(sql, p => p.AddWithValue("@Id", id));
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
