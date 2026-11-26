
using Gameflow.Api.Accounts;
using Gameflow.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gameflow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== Services registreren =====

// Controllers voor Competities, Teams en Startprofielen
builder.Services.AddControllers();

// Swagger en validatie
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidation();

// Strikte verwerking van getallen in JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling =
        System.Text.Json.Serialization.JsonNumberHandling.Strict;
});

// EF Core met SQL Server
builder.Services.AddDbContext<GameflowDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Gameflow")));

builder.Services.AddScoped<IStartprofielService, StartprofielService>();

builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddScoped<IOverzichtService, OverzichtService>();

builder.Services.AddScoped<IDoelService, DoelService>();

builder.Services.AddScoped<TeamStatsService>();
builder.Services.AddScoped<BeslissingenService>();
builder.Services.AddScoped<ExtrabeslissingenService>();

// Identity met rollen
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

// Rollen en het spelbegeleider-account aanmaken
await DbSeeder.SeedAsync(app.Services);

// ===== Middleware =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ===== API-routes =====

// Standaardroutes van Identity
app.MapGroup("/api/auth").MapIdentityApi<User>();

// Eigen routes voor accounts
app.MapAccounts();

// Controllers voor het Resort Manager-spel
app.MapControllers();

app.Run();
