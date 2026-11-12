using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFilesGameflow.Service;
using MyFilesGameflow.Data;
using Microsoft.EntityFrameworkCore;

class Program {
    static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        
        builder.Services.AddCors(options => {
            options.AddPolicy("OpenCorsPolicy", policy => {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<TeamStatsService>();
        
        
        var app = builder.Build();
        
        app.UseCors("OpenCorsPolicy");
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.Urls.Add("http://localhost:5050");
        Console.WriteLine("http://localhost:5050/api/teamstats");
        Console.WriteLine("http://localhost:5050/swagger");
        
        app.MapControllers();
        app.UseHttpsRedirection();
        app.Run();
    }
}