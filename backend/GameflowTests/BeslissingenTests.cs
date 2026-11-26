using Gameflow.Api.Data;
using Microsoft.EntityFrameworkCore;
namespace GameflowTests;

public class BeslissingenTests {
    private GameflowDb TestDbContext()
    {
        var options = new DbContextOptionsBuilder<GameflowDb>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new GameflowDb(options);
    }
    
    [Fact]
    public async Task PostBeslissingTest() {
        
    }
}