using SOLIDExamples.Services;
using SOLIDExamples.Services.Payment;
using SOLIDExamples.Services.DocumentStorage;
using SOLIDExamples.Services.Notification;
using SOLIDExamples.Services.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register dependencies
builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<SOLIDExamples.Services.ILoggerService, LoggerService>(); // Use fully qualified name for app logger
builder.Services.AddScoped<IUserService, UserService>();

// Register payment strategies
builder.Services.AddTransient<CreditCardPaymentStrategy>();
builder.Services.AddTransient<PayPalPaymentStrategy>();
builder.Services.AddTransient<StripePaymentStrategy>();
builder.Services.AddTransient<BankTransferPaymentStrategy>();
builder.Services.AddTransient<CryptocurrencyPaymentStrategy>();
builder.Services.AddTransient<ApplePayPaymentStrategy>();

// Register factory and processor
builder.Services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();
builder.Services.AddScoped<IPaymentProcessor, PaymentProcessor>();

// Register LSP Document Storage (example: LocalFileSystemStorage)
// You can change the base path as needed, or use configuration
builder.Services.AddSingleton<DocumentStorageBase>(provider =>
    new LocalFileSystemStorage("./App_Data/Documents"));

// Register ISP Notification Services
// BAD (for demo)
builder.Services.AddSingleton<INotificationServiceBad, SmsNotificationServiceBad>();
// GOOD
builder.Services.AddSingleton<ISmsNotification, SmsNotificationService>();
builder.Services.AddSingleton<IEmailNotification, EmailNotificationService>();

// Register DIP Logger Services
// BAD (for demo)
builder.Services.AddSingleton<SOLIDExamples.Services.Logging.LoggerServiceBad>();
// GOOD
builder.Services.AddSingleton<SOLIDExamples.Services.Logging.ILoggerService, ConsoleLoggerService>();
// To use FileLoggerService instead, swap the above line:
// builder.Services.AddSingleton<SOLIDExamples.Services.Logging.ILoggerService, FileLoggerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
