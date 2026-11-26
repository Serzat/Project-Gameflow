using Gameflow.Api.Accounts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gameflow.Api.Models;

namespace Gameflow.Api.Data;

public class GameflowDb(DbContextOptions<GameflowDb> options) : IdentityDbContext<User>(options) {
    public DbSet<Beslissingen> beslissingen { get; set; }
    public DbSet<TeamStats>  teamStats { get; set; }
    
    //plaats hier al je DbSets zodat je modellen omgezet kunnen worden naar tabellen in je database :)
}