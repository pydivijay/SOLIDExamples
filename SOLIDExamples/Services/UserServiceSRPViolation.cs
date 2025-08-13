using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services
{
    // This class violates SRP - it has multiple responsibilities
    public class UserServiceSRPViolation
    {
        private readonly string _connectionString;

        public UserServiceSRPViolation(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> RegisterUser(UserRegistrationDto userDto)
        {
            try
            {
                // Responsibility 1: Validation
                if (string.IsNullOrEmpty(userDto.Email) || !userDto.Email.Contains("@"))
                    throw new ArgumentException("Invalid email format");

                if (string.IsNullOrEmpty(userDto.Password) || userDto.Password.Length < 6)
                    throw new ArgumentException("Password must be at least 6 characters");

                // Responsibility 2: Business Logic
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = userDto.Email,
                    Name = userDto.Name,
                    PasswordHash = HashPassword(userDto.Password),
                    CreatedAt = DateTime.UtcNow
                };

                // Responsibility 3: Data Access
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

                // Responsibility 4: Email Sending
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("user@gmail.com", "password"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@company.com"),
                    Subject = "Welcome to Our Platform",
                    Body = $"Hello {user.Name}, welcome to our platform!",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(user.Email);

                await smtpClient.SendMailAsync(mailMessage);

                // Responsibility 5: Logging
                Console.WriteLine($"User {user.Email} registered successfully at {DateTime.UtcNow}");

                return true;
            }
            catch (Exception ex)
            {
                // Responsibility 6: Error Handling & Logging
                Console.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Simple hash for demonstration
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
        }
    }
}
// Add this using directive at the top of the file to fix CS1069

