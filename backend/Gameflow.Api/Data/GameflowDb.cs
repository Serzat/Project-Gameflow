using Gameflow.Api.Accounts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFilesGameflow.Models;

namespace Gameflow.Api.Data;

public class GameflowDb(DbContextOptions<GameflowDb> options) : IdentityDbContext<User>(options) {
    public DbSet<Beslissingen> beslissingen { get; set; }
    public DbSet<TeamStats>  teamStats { get; set; }
}