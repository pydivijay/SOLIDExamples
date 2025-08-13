using SOLIDExamples.Models;
using System.Data.SqlClient;

namespace SOLIDExamples.Services
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User> CreateAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "INSERT INTO Users (Id, Email, Name, PasswordHash, CreatedAt) VALUES (@Id, @Email, @Name, @PasswordHash, @CreatedAt)",
                connection);

            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

            await command.ExecuteNonQueryAsync();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id, Email, Name, PasswordHash, CreatedAt FROM Users WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetGuid(0),
                    Email = reader.GetString(1),
                    Name = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }
            return null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            return user != null;
        }
    }
}
