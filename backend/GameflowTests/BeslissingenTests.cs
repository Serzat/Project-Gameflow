using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Gameflow.Api.Services;
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
        //Arrange
        var database = TestDbContext();
        var context = new BeslissingenService(database);
        
        var dto = new BeslissingInputDto {
            TeamNummer = 1,
            RondeNummer = 1, 
            Kamerprijs = 50m,
            MarketingBudget = 50m, 
            MarketingFocus = "Alle Doelgroepen",
            AantalMedewerkers = 35,
            TypeInvestering = "Spa & Welness",
            InvesteringKosten = 50m,
            DuurzaamheidMaatregel = "Zonnepanelen installeren",
            DuurzaamheidKosten = 50m,
            OnderhoudsNiveau = "Minimaal"
            
        };

        var newBeslissingen = new Beslissingen{
            Id = 1,
            TeamNummer = dto.TeamNummer,
            RondeNummer = dto.RondeNummer,
            Kamerprijs = dto.Kamerprijs,
            MarketingBudget = dto.MarketingBudget,
            MarketingFocus = dto.MarketingFocus,
            AantalMedewerkers = dto.AantalMedewerkers,
            TypeInvestering = dto.TypeInvestering,
            InvesteringKosten = dto.InvesteringKosten,
            DuurzaamheidMaatregel = dto.DuurzaamheidMaatregel,
            DuurzaamheidKosten =  dto.DuurzaamheidKosten,
            OnderhoudsNiveau = dto.OnderhoudsNiveau
        };

        var beslissingen = database.beslissingen.Add(newBeslissingen);
        await database.SaveChangesAsync();
        
        //Act
        var beslissingenFromDb = database.beslissingen.FirstOrDefault(
            t => t.TeamNummer == newBeslissingen.TeamNummer && t.RondeNummer == newBeslissingen.RondeNummer);
        
        //Assert
        Assert.NotNull(beslissingenFromDb);
        Assert.Equal(newBeslissingen.Id, beslissingenFromDb.Id);
        Assert.Equal(newBeslissingen.TeamNummer, beslissingenFromDb.TeamNummer);
        Assert.Equal(newBeslissingen.RondeNummer, beslissingenFromDb.RondeNummer);
    }
    
    [Fact]
    public async Task UpdateBestaandeBeslissingenTest() {
        //Arrange
        var database = TestDbContext();
        var context = new BeslissingenService(database);
        
        //Act (Hier haal ik de beslissingen op die ik in de test methode hierboven heb aangemaakt.)
        
        
        //Assert
    }
}