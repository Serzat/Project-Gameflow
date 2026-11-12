using Gameflow.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Services registreren (vóór Build) =====

// Swagger, zodat je endpoints kunt testen (les 5)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core met SQL Server. Leest de connectionstring uit je user secrets (les 6)
builder.Services.AddDbContext<GameflowDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Gameflow")));

var app = builder.Build();

// ===== Middleware en routes (na Build) =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();