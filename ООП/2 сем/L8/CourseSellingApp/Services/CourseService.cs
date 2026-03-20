#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CourseSellingApp.Database.Services;
using Newtonsoft.Json;
using Npgsql;
using Splat;

namespace CourseSellingApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly string _connectionString;

        public CourseService()
        {
            var dbService = Locator.Current.GetService<DatabaseService>();
            if (dbService == null)
            {
                throw new InvalidOperationException("DatabaseService is not registered in the DI container.");
            }
            _connectionString = dbService.GetConnectionString();
        }

        public async Task<IEnumerable<Models.Course>> GetCoursesAsync()
        {
            var courses = new List<Models.Course>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("SELECT * FROM get_all_courses_with_details()", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                courses.Add(MapReaderToModel(reader));
            }

            return courses;
        }

        public async Task AddCourseAsync(Models.Course course)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var categoryId = await GetOrCreateEntityAsync(connection, transaction, "Categories", "Name", course.Category, "Uncategorized");
                var authorId = await GetOrCreateEntityAsync(connection, transaction, "Authors", "FullName", course.Author);

                const string sql = @"
                    INSERT INTO Courses (
                        Name, FullName, Description, Price, Quantity, Discount, Rating,
                        CategoryId, AuthorId, CoverImagePath, ImagePaths, RelatedCoursesIds
                    ) VALUES (
                        @Name, @FullName, @Description, @Price, @Quantity, @Discount, @Rating,
                        @CategoryId, @AuthorId, @CoverImagePath, @ImagePaths, @RelatedCoursesIds
                    )";

                await using var command = new NpgsqlCommand(sql, connection, transaction);
                AddParameters(command, course, categoryId.Value, authorId);
                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateCourseAsync(Models.Course course)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var categoryId = await GetOrCreateEntityAsync(connection, transaction, "Categories", "Name", course.Category, "Uncategorized");
                var authorId = await GetOrCreateEntityAsync(connection, transaction, "Authors", "FullName", course.Author);

                const string sql = @"
                    UPDATE Courses SET
                        Name = @Name, FullName = @FullName, Description = @Description, Price = @Price,
                        Quantity = @Quantity, Discount = @Discount, Rating = @Rating, CategoryId = @CategoryId,
                        AuthorId = @AuthorId, CoverImagePath = @CoverImagePath, ImagePaths = @ImagePaths,
                        RelatedCoursesIds = @RelatedCoursesIds
                    WHERE Id = @Id";

                await using var command = new NpgsqlCommand(sql, connection, transaction);
                command.Parameters.AddWithValue("@Id", course.Id);
                AddParameters(command, course, categoryId.Value, authorId);

                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("delete_course_proc", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("p_course_id", courseId);

            await command.ExecuteNonQueryAsync();
        }


        private static Models.Course MapReaderToModel(IDataRecord reader)
        {
            return new Models.Course
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? string.Empty : reader.GetString(reader.GetOrdinal("FullName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                Discount = reader.GetDouble(reader.GetOrdinal("Discount")),
                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                PurchasesCount = reader.GetInt32(reader.GetOrdinal("PurchasesCount")),
                Rating = reader.GetDouble(reader.GetOrdinal("Rating")),
                CoverImagePath = reader.IsDBNull(reader.GetOrdinal("CoverImagePath")) ? null : reader.GetString(reader.GetOrdinal("CoverImagePath")),
                ImagePaths = reader.IsDBNull(reader.GetOrdinal("ImagePaths")) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(reader.GetString(reader.GetOrdinal("ImagePaths"))) ?? new List<string>(),
                RelatedCoursesIds = reader.IsDBNull(reader.GetOrdinal("RelatedCoursesIds")) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(reader.GetString(reader.GetOrdinal("RelatedCoursesIds"))) ?? new List<int>(),
                Category = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                Author = reader.IsDBNull(reader.GetOrdinal("AuthorName")) ? string.Empty : reader.GetString(reader.GetOrdinal("AuthorName"))
            };
        }

        private static void AddParameters(NpgsqlCommand command, Models.Course course, int categoryId, int? authorId)
        {
            command.Parameters.AddWithValue("@Name", course.Name);
            command.Parameters.AddWithValue("@FullName", (object)course.FullName ?? DBNull.Value);
            command.Parameters.AddWithValue("@Description", (object)course.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", course.Price);
            command.Parameters.AddWithValue("@Quantity", course.Quantity);
            command.Parameters.AddWithValue("@Discount", course.Discount);
            command.Parameters.AddWithValue("@Rating", course.Rating);
            command.Parameters.AddWithValue("@CategoryId", categoryId);
            command.Parameters.AddWithValue("@AuthorId", (object)authorId ?? DBNull.Value);
            command.Parameters.AddWithValue("@CoverImagePath", (object)course.CoverImagePath ?? DBNull.Value);
            command.Parameters.AddWithValue("@ImagePaths", JsonConvert.SerializeObject(course.ImagePaths));
            command.Parameters.AddWithValue("@RelatedCoursesIds", JsonConvert.SerializeObject(course.RelatedCoursesIds));
        }

        private static async Task<int?> GetOrCreateEntityAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, string tableName, string columnName, string? value, string? defaultValue = null)
        {
            var currentValue = string.IsNullOrWhiteSpace(value) ? defaultValue : value;
            if (currentValue == null)
            {
                return null;
            }

            await using var findCmd = new NpgsqlCommand($"SELECT Id FROM {tableName} WHERE {columnName} = @Value", connection, transaction);
            findCmd.Parameters.AddWithValue("@Value", currentValue);
            var existingId = await findCmd.ExecuteScalarAsync();

            if (existingId != null)
            {
                return (int)existingId;
            }

            await using var insertCmd = new NpgsqlCommand($"INSERT INTO {tableName} ({columnName}) VALUES (@Value) RETURNING Id", connection, transaction);
            insertCmd.Parameters.AddWithValue("@Value", currentValue);
            var newId = await insertCmd.ExecuteScalarAsync();

            return (int?)newId;
        }

    }
}
