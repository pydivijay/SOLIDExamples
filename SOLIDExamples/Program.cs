using SOLIDExamples.Services;
using SOLIDExamples.Services.Payment;

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
builder.Services.AddScoped<ILoggerService, LoggerService>();
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
