using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gameflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class RandomEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RondeNummer",
                table: "SpelEvents");

            migrationBuilder.AddColumn<decimal>(
                name: "KansGlobaal",
                table: "Spelinstellingen",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "KansIndividueel",
                table: "Spelinstellingen",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Titel",
                table: "SpelEvents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Soort",
                table: "SpelEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RondeTrekkingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RondeNummer = table.Column<int>(type: "int", nullable: false),
                    Doel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpelEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RondeTrekkingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RondeTrekkingen_SpelEvents_SpelEventId",
                        column: x => x.SpelEventId,
                        principalTable: "SpelEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpelEvents_Titel",
                table: "SpelEvents",
                column: "Titel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RondeTrekkingen_RondeNummer_Doel",
                table: "RondeTrekkingen",
                columns: new[] { "RondeNummer", "Doel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RondeTrekkingen_SpelEventId",
                table: "RondeTrekkingen",
                column: "SpelEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RondeTrekkingen");

            migrationBuilder.DropIndex(
                name: "IX_SpelEvents_Titel",
                table: "SpelEvents");

            migrationBuilder.DropColumn(
                name: "KansGlobaal",
                table: "Spelinstellingen");

            migrationBuilder.DropColumn(
                name: "KansIndividueel",
                table: "Spelinstellingen");

            migrationBuilder.DropColumn(
                name: "Soort",
                table: "SpelEvents");

            migrationBuilder.AlterColumn<string>(
                name: "Titel",
                table: "SpelEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "RondeNummer",
                table: "SpelEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
