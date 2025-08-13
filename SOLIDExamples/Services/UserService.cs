using SOLIDExamples.Models;

namespace SOLIDExamples.Services
{
    public interface IUserService
    {
        Task<ApiResponse<User>> RegisterUserAsync(UserRegistrationDto userDto);
    }

    public class UserService : IUserService
    {
        private readonly IUserValidator _validator;
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILoggerService _logger;

        public UserService(
            IUserValidator validator,
            IPasswordService passwordService,
            IUserRepository userRepository,
            IEmailService emailService,
            ILoggerService logger)
        {
            _validator = validator;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ApiResponse<User>> RegisterUserAsync(UserRegistrationDto userDto)
        {
            try
            {
                var validationResult = _validator.ValidateRegistration(userDto);
                if (!validationResult.IsValid)
                {
                    return ApiResponse<User>.CreateFailure(validationResult.Errors);
                }

                if (await _userRepository.EmailExistsAsync(userDto.Email))
                {
                    return ApiResponse<User>.CreateFailure(new[] { "Email already exists" });
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = userDto.Email,
                    Name = userDto.Name,
                    PasswordHash = _passwordService.HashPassword(userDto.Password),
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendWelcomeEmailAsync(user.Email, user.Name);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to send welcome email to {user.Email}", ex);
                    }
                });

                _logger.LogInfo($"User {user.Email} registered successfully");

                return ApiResponse<User>.CreateSuccess(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering user: {ex.Message}", ex);
                return ApiResponse<User>.CreateFailure(new[] { "An error occurred while registering user" });
            }
        }
    }
}
