using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CourseSellingApp.Database.Entities;
using Npgsql;
using NpgsqlTypes;

namespace CourseSellingApp.Database.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly string _connectionString;

        public CourseRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task AddAsync(Course course)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string sql = @"
                    INSERT INTO Courses (
                        Name, FullName, Description, Price, Quantity, Discount, Rating,
                        CategoryId, AuthorId, CoverImagePath, ImagePaths, RelatedCoursesIds
                    ) VALUES (
                        @Name, @FullName, @Description, @Price, @Quantity, @Discount, @Rating,
                        @CategoryId, @AuthorId, @CoverImagePath, @ImagePaths, @RelatedCoursesIds
                    )";

                await using var command = new NpgsqlCommand(sql, connection, transaction);
                AddCourseParameters(command, course);

                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch (NpgsqlException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("delete_course_proc", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("p_course_id", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<Course?> GetAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = "SELECT * FROM Courses WHERE Id = @Id";
            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapReaderToCourse(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        {
            var courses = new List<Course>();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("get_all_courses_with_details", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                courses.Add(MapReaderToCourse(reader));
            }
            return courses;
        }

        public async Task UpdateAsync(Course course)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string sql = @"
                    UPDATE Courses SET
                        Name = @Name,
                        FullName = @FullName,
                        Description = @Description,
                        Price = @Price,
                        Quantity = @Quantity,
                        Discount = @Discount,
                        Rating = @Rating,
                        CategoryId = @CategoryId,
                        AuthorId = @AuthorId,
                        CoverImagePath = @CoverImagePath,
                        ImagePaths = @ImagePaths,
                        RelatedCoursesIds = @RelatedCoursesIds
                    WHERE Id = @Id";

                await using var command = new NpgsqlCommand(sql, connection, transaction);
                command.Parameters.AddWithValue("@Id", course.Id);
                AddCourseParameters(command, course);

                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch (NpgsqlException)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private Course MapReaderToCourse(IDataRecord record)
        {
            return new Course
            {
                Id = record.GetInt32(record.GetOrdinal("Id")),
                Name = record.GetString(record.GetOrdinal("Name")),
                FullName = record.IsDBNull(record.GetOrdinal("FullName")) ? null : record.GetString(record.GetOrdinal("FullName")),
                Description = record.IsDBNull(record.GetOrdinal("Description")) ? null : record.GetString(record.GetOrdinal("Description")),
                Price = record.GetDecimal(record.GetOrdinal("Price")),
                Quantity = record.GetInt32(record.GetOrdinal("Quantity")),
                Discount = record.GetDouble(record.GetOrdinal("Discount")),
                IsAvailable = record.GetBoolean(record.GetOrdinal("IsAvailable")),
                PurchasesCount = record.GetInt32(record.GetOrdinal("PurchasesCount")),
                Rating = record.GetDouble(record.GetOrdinal("Rating")),
                CoverImagePath = record.IsDBNull(record.GetOrdinal("CoverImagePath")) ? null : record.GetString(record.GetOrdinal("CoverImagePath")),
                ImagePaths = record.IsDBNull(record.GetOrdinal("ImagePaths")) ? null : record.GetString(record.GetOrdinal("ImagePaths")),
                RelatedCoursesIds = record.IsDBNull(record.GetOrdinal("RelatedCoursesIds")) ? null : record.GetString(record.GetOrdinal("RelatedCoursesIds")),
                CategoryId = record.GetInt32(record.GetOrdinal("CategoryId")),
                AuthorId = record.IsDBNull(record.GetOrdinal("AuthorId")) ? null : record.GetInt32(record.GetOrdinal("AuthorId")),
                CreatedAt = record.GetDateTime(record.GetOrdinal("CreatedAt")),
                CoverImage = record.IsDBNull(record.GetOrdinal("CoverImage")) ? null : (byte[])record.GetValue(record.GetOrdinal("CoverImage"))
            };
        }

        private void AddCourseParameters(NpgsqlCommand command, Course course)
        {
            command.Parameters.AddWithValue("@Name", course.Name);
            command.Parameters.AddWithValue("@FullName", (object)course.FullName ?? DBNull.Value);
            command.Parameters.AddWithValue("@Description", (object)course.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@Price", course.Price);
            command.Parameters.AddWithValue("@Quantity", course.Quantity);
            command.Parameters.AddWithValue("@Discount", course.Discount);
            command.Parameters.AddWithValue("@Rating", course.Rating);
            command.Parameters.AddWithValue("@CategoryId", course.CategoryId);
            command.Parameters.AddWithValue("@AuthorId", (object)course.AuthorId ?? DBNull.Value);
            command.Parameters.AddWithValue("@CoverImagePath", (object)course.CoverImagePath ?? DBNull.Value);
            command.Parameters.AddWithValue("@ImagePaths", (object)course.ImagePaths ?? DBNull.Value);
            command.Parameters.AddWithValue("@RelatedCoursesIds", (object)course.RelatedCoursesIds ?? DBNull.Value);
        }
    }
}
