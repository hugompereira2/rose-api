using Microsoft.OpenApi.Models;
using rose_api.ExternalServices.Services;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

string server = Environment.GetEnvironmentVariable("SERVER");
string port = Environment.GetEnvironmentVariable("PORT");
string database = Environment.GetEnvironmentVariable("DATABASE");
string saUser = Environment.GetEnvironmentVariable("SA_USER");
string saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");

string connectionString = $"Server={server},{port};Database={database};User Id={saUser};Password={saPassword};TrustServerCertificate=True";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rose API", Version = "v1" });
});

builder.Services.AddScoped(provider => new DapperRepository(connectionString));
builder.Services.AddScoped<BrasilApi>();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rose API");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
