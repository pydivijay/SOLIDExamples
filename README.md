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


## Open/Closed Principle (OCP) in ASP.NET Core API

### What is Open/Closed Principle?
The Open/Closed Principle states that **software entities (classes, modules, functions) should be open for extension but closed for modification**. You should be able to extend a class's behavior without modifying its existing code.

### Real-Time Example: Payment Processing API
This project demonstrates OCP with a Payment Processing API supporting multiple payment methods (Credit Card, PayPal, Stripe, Bank Transfer, Cryptocurrency, Apple Pay, etc.).

#### ❌ BAD Example - Violating OCP
A single class with a switch statement for each payment method. Adding new methods requires modifying the class, risking bugs and violating OCP.

#### ✅ GOOD Example - Following OCP
- **Strategy Pattern**: Each payment method is implemented as a separate strategy class.
- **Factory Pattern**: A factory resolves the correct strategy based on the payment method.
- **Dependency Injection**: New payment methods are registered in DI without modifying existing code.
- **Controller**: The API controller uses abstractions and is closed for modification.

#### Benefits
- **Easy Extension**: Add new payment methods by creating new strategy classes.
- **Reduced Risk**: Existing code remains untouched when adding features.
- **Better Testing**: Each payment strategy can be tested independently.
- **Improved Maintainability**: Changes are isolated to specific strategies.
- **Scalable Architecture**: System can grow without breaking existing features.

#### Key Takeaways
- **Open for Extension**: New functionality via new classes.
- **Closed for Modification**: No changes to existing code for new features.
- **Polymorphism & DI**: Use interfaces and DI for extensibility.

---

### Project Structure
- `Models/Models.cs`: Payment models, enums, and DTOs.
- `Services/Payment/`: Payment strategies, factory, and processor.
- `Controllers/PaymentsController.cs`: API endpoints for payment processing.
- `Program.cs`: DI setup for payment strategies and services.

---

### How to Add a New Payment Method
1. Create a new class implementing `IPaymentStrategy`.
2. Register the new strategy in `Program.cs` and the factory.
3. No changes required to controller or processor.

---

#### This approach makes the ASP.NET Core API extensible, maintainable, and SOLID-compliant.
![Description](assets/ocp_diagram_svg.svg)


## Liskov Substitution Principle (LSP) in ASP.NET Core API

### What is Liskov Substitution Principle?
The Liskov Substitution Principle (LSP) states that **objects of a superclass should be replaceable with objects of its subclasses without breaking the application**. Derived classes must be substitutable for their base classes without altering the correctness of the program.

### Real-Time Example: Document Storage API
This project demonstrates LSP with a Document Storage API supporting multiple storage providers (Local File System, Azure Blob Storage, AWS S3, etc.).

### LSP-Compliant Example
- All storage providers inherit from `DocumentStorageBase` and implement the same contract.
- Methods return the expected types and throw the expected exceptions.
- Client code can use any storage provider interchangeably.

### LSP Violation Example
- A `BadDocumentStorage` class is provided that returns null, throws generic exceptions, or returns incorrect values, violating the contract.
- A `BadDocumentStorageController` demonstrates the issues that arise from LSP violations.

### Steps to Integrate LSP Principle

1. **Models and Exception Types**
   - Add `DocumentInfo`, `UploadResult`, `DocumentStorageException`, `DocumentNotFoundException`, and `StorageProvider` to your models.

2. **Abstract Base Class**
   - Create `DocumentStorageBase` with abstract methods for upload, download, delete, info, existence, and listing.

3. **LSP-Compliant Implementation**
   - Implement `LocalFileSystemStorage` inheriting from `DocumentStorageBase`.
   - Ensure all methods return the correct types and throw the correct exceptions.

4. **LSP-Violating Implementation**
   - Implement `BadDocumentStorage` that returns null, throws generic exceptions, or returns incorrect values.

5. **Controllers**
   - Add `DocumentStorageController` for the LSP-compliant implementation.
   - Add `BadDocumentStorageController` for the violation example.

6. **Dependency Injection**
   - Register `LocalFileSystemStorage` as `DocumentStorageBase` in `Program.cs`.
   - Optionally register `BadDocumentStorage` for demonstration.

### LSP Principle vs Violation

| Aspect                | LSP-Compliant (`LocalFileSystemStorage`) | LSP-Violating (`BadDocumentStorage`)      |
|-----------------------|------------------------------------------|-------------------------------------------|
| Return Types          | Always as specified                      | Sometimes null or wrong type              |
| Exception Handling    | Throws expected custom exceptions        | Throws generic or unexpected exceptions   |
| Method Semantics      | Consistent with base class               | Inconsistent, breaks client expectations  |
| Substitutability      | Safe                                     | Unsafe, can break client code             |

### Conclusion
- **LSP-compliant code** ensures all subclasses can be used interchangeably, making your system robust and extensible.
- **LSP-violating code** leads to bugs, runtime errors, and fragile code that is hard to maintain or extend.
![Description](assets/lsp_diagram_svg.svg)