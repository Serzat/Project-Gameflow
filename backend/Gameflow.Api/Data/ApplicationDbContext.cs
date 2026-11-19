using Microsoft.EntityFrameworkCore;
using MyFilesGameflow.Models;
namespace MyFilesGameflow.Data;

public class ApplicationDbContext : DbContext{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    
    public DbSet<Beslissingen> beslissingen { get; set; }
    public DbSet<TeamStats>  teamStats { get; set; }
}