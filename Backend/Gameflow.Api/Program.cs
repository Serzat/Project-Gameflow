using Gameflow.Api.Accounts;
using Gameflow.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Services registreren (vóór Build) =====

// Swagger en validatie (les 5 en 6)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidation();

// EF Core met SQL Server (les 6)
builder.Services.AddDbContext<GameflowDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Gameflow")));

// Identity met rollen (les 7)
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GameflowDb>();

var app = builder.Build();

// ===== Middleware en routes (na Build) =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Standaardroutes van Identity: /api/auth/login, /api/auth/register enz.
app.MapGroup("/api/auth").MapIdentityApi<User>();

// Onze eigen routes: /api/auth/signup, /api/auth/me, /api/auth/logout
app.MapAccounts();

app.Run();