using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gameflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class Overzicht : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpelEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RondeNummer = table.Column<int>(type: "int", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectGoed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpelEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamStanden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResortType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RondeNummer = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AantalKamers = table.Column<int>(type: "int", nullable: false),
                    AantalMedewerkers = table.Column<int>(type: "int", nullable: false),
                    Bezettingsgraad = table.Column<int>(type: "int", nullable: false),
                    Kamerprijs = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Winst = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Gasttevredenheid = table.Column<int>(type: "int", nullable: false),
                    Personeelstevredenheid = table.Column<int>(type: "int", nullable: false),
                    Duurzaamheidsscore = table.Column<int>(type: "int", nullable: false),
                    Marktaandeel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamStanden", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamStanden_ResortType_RondeNummer",
                table: "TeamStanden",
                columns: new[] { "ResortType", "RondeNummer" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpelEvents");

            migrationBuilder.DropTable(
                name: "TeamStanden");
        }
    }
}
