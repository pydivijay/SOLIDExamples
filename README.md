# SOLIDExamples (.NET 8)

## Overview
This project demonstrates the Single Responsibility Principle (SRP) in an ASP.NET Core API using .NET 8. It includes both a well-structured, SRP-compliant user management system and an example of a class that violates SRP for comparison.

## What is SRP?
**Single Responsibility Principle** states that a class should have only one reason to change, meaning it should have only one job or responsibility. Following SRP leads to code that is easier to maintain, test, and extend.

## Project Structure
```
SOLIDExamples/
?
??? Controllers/
?   ??? UsersController.cs         # Handles HTTP requests for user registration
?
??? Models/
?   ??? Models.cs                 # Contains User, UserRegistrationDto, ValidationResult, ApiResponse<T>
?
??? Services/
?   ??? UserValidator.cs          # Validates user registration data
?   ??? PasswordService.cs        # Handles password hashing and verification
?   ??? UserRepository.cs         # Handles data access for users (SQL Server)
?   ??? EmailService.cs           # Sends emails (SMTP)
?   ??? LoggerService.cs          # Logging abstraction
?   ??? UserService.cs            # Coordinates user registration (SRP-compliant)
?   ??? UserServiceSRPViolation.cs# Example of a class that violates SRP
?
??? Program.cs                    # Configures services and starts the app
??? ...
```

## How to Run
1. Ensure you have .NET 8 SDK installed.
2. Set up your database and update the connection string in `appsettings.json` under `DefaultConnection`.
3. Configure email settings in `appsettings.json` under the `Email` section.
4. Restore dependencies:
   ```
   dotnet restore
   ```
5. Build and run the project:
   ```
   dotnet run --project SOLIDExamples/SOLIDExamples.csproj
   ```
6. Use Swagger (enabled in development) or Postman to test the registration endpoint:
   - POST `/api/users/register`

## Comparing SRP Approaches
- **SRP-compliant:** See `Services/UserService.cs` and related service files. Each class has a single responsibility.
- **SRP-violating:** See `Services/UserServiceSRPViolation.cs`. This class mixes validation, business logic, data access, email, and logging.

## Benefits of SRP
- Easier testing
- Better maintainability
- Improved reusability
- Clear separation of concerns
- Easier to extend and modify

---

**Feel free to explore and modify the code to see the impact of following or violating SRP!**
![Description](assets/srp_diagram_svg.svg)