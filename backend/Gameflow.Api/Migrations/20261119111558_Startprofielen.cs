using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gameflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class Startprofielen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResortStartprofielen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResortType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Startbudget = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    AantalKamers = table.Column<int>(type: "int", nullable: false),
                    AantalMedewerkers = table.Column<int>(type: "int", nullable: false),
                    Bezettingsgraad = table.Column<int>(type: "int", nullable: false),
                    Kamerprijs = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Gasttevredenheid = table.Column<int>(type: "int", nullable: false),
                    Personeelstevredenheid = table.Column<int>(type: "int", nullable: false),
                    Duurzaamheidsscore = table.Column<int>(type: "int", nullable: false),
                    Marktaandeel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResortStartprofielen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spelinstellingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GekozenModus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelinstellingen", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResortStartprofielen_Modus_ResortType",
                table: "ResortStartprofielen",
                columns: new[] { "Modus", "ResortType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResortStartprofielen");

            migrationBuilder.DropTable(
                name: "Spelinstellingen");
        }
    }
}
