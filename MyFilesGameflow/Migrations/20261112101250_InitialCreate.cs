using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFilesGameflow.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beslissingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kamerprijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MarketingBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MarketingFocus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AantalMedewerkers = table.Column<int>(type: "int", nullable: false),
                    TypeInvestering = table.Column<int>(type: "int", nullable: false),
                    InvesteringKosten = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DuurzaamheidMaatregel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuurzaamheidKosten = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnderhoudsNiveau = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beslissingen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teamStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamNummer = table.Column<int>(type: "int", nullable: false),
                    RondeNummer = table.Column<int>(type: "int", nullable: false),
                    StartProfiel = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    ResortNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuidigBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AantalKamers = table.Column<int>(type: "int", nullable: false),
                    AantalMedewerkers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teamStats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beslissingen");

            migrationBuilder.DropTable(
                name: "teamStats");
        }
    }
}
