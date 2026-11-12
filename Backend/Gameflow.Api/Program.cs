using Gameflow.Api.Data;
using Microsoft.EntityFrameworkCore;
using Gameflow.Api.Accounts;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ===== Services registreren (vóór Build) =====

// Swagger, testen en validation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidation();

// EF Core met SQL Server. Leest de connectionstring uit je user secrets 
builder.Services.AddDbContext<GameflowDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Gameflow")));
builder.Services.AddScoped<IAccountStore, EfAccountStore>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// ===== Middleware en routes (na Build) =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapAccounts();

app.Run();