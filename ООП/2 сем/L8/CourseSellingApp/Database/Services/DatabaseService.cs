using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CourseSellingApp.Database.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;
        private readonly string _databaseName;

        public DatabaseService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена.");

            var csBuilder = new NpgsqlConnectionStringBuilder(_connectionString);
            _databaseName = csBuilder.Database ?? "CourseSellingAppDb";

            csBuilder.Database = "postgres";
            _masterConnectionString = csBuilder.ToString();
        }

        public string GetConnectionString() => _connectionString;

        public async Task<int> ExecuteNonQueryAsync(string sql, Action<NpgsqlParameterCollection>? addParameters = null, CommandType commandType = CommandType.Text)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection) { CommandType = commandType };
            addParameters?.Invoke(command.Parameters);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarAsync(string sql, Action<NpgsqlParameterCollection>? addParameters = null, CommandType commandType = CommandType.Text)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection) { CommandType = commandType };
            addParameters?.Invoke(command.Parameters);

            return await command.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataRecord, T> map, Action<NpgsqlParameterCollection>? addParameters = null, CommandType commandType = CommandType.Text)
        {
            var results = new List<T>();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection) { CommandType = commandType };
            addParameters?.Invoke(command.Parameters);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }

            return results;
        }

        public async Task ExecuteInTransactionAsync(Func<NpgsqlConnection, NpgsqlTransaction, Task> action)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await action(connection, transaction);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task InitializeDatabaseAsync()
        {
            await EnsureDatabaseExistsAsync();
            await RunInitializationScriptsAsync();
        }

        private async Task EnsureDatabaseExistsAsync()
        {
            await using var masterConnection = new NpgsqlConnection(_masterConnectionString);
            await masterConnection.OpenAsync();

            bool dbExists;
            await using (var checkCmd = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @dbName", masterConnection))
            {
                checkCmd.Parameters.AddWithValue("@dbName", _databaseName);
                var result = await checkCmd.ExecuteScalarAsync();
                dbExists = result != null;
            }

            if (!dbExists)
            {
                await using var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{_databaseName}\"", masterConnection);
                await createCmd.ExecuteNonQueryAsync();

                Console.WriteLine($"База данных '{_databaseName}' успешно создана.");
            }
        }

        private async Task RunInitializationScriptsAsync()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var scriptsDir = Path.Combine(baseDir, "Database", "Scripts");

            if (!Directory.Exists(scriptsDir))
            {
                scriptsDir = Path.Combine(baseDir, "..", "..", "..", "Database", "Scripts");
            }

            if (!Directory.Exists(scriptsDir))
            {
                Console.WriteLine($"Папка со скриптами не найдена по пути: {scriptsDir}");
                return;
            }

            var scriptFiles = Directory.GetFiles(scriptsDir, "*.sql").OrderBy(f => f).ToList();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            foreach (var file in scriptFiles)
            {
                var scriptContent = await File.ReadAllTextAsync(file);
                await using var command = new NpgsqlCommand(scriptContent, connection);

                try
                {
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Скрипт {Path.GetFileName(file)} успешно выполнен.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при выполнении скрипта {Path.GetFileName(file)}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
