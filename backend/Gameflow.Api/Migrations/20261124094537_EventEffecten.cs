using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gameflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class EventEffecten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Effect",
                table: "SpelEvents");

            migrationBuilder.DropColumn(
                name: "EffectGoed",
                table: "SpelEvents");

            migrationBuilder.AddColumn<decimal>(
                name: "ImpactBeach",
                table: "SpelEvents",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ImpactCity",
                table: "SpelEvents",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ImpactCruise",
                table: "SpelEvents",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ImpactWinter",
                table: "SpelEvents",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "SpelEventEffecten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpelEventId = table.Column<int>(type: "int", nullable: false),
                    Kpi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Waarde = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpelEventEffecten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpelEventEffecten_SpelEvents_SpelEventId",
                        column: x => x.SpelEventId,
                        principalTable: "SpelEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpelEventEffecten_SpelEventId",
                table: "SpelEventEffecten",
                column: "SpelEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpelEventEffecten");

            migrationBuilder.DropColumn(
                name: "ImpactBeach",
                table: "SpelEvents");

            migrationBuilder.DropColumn(
                name: "ImpactCity",
                table: "SpelEvents");

            migrationBuilder.DropColumn(
                name: "ImpactCruise",
                table: "SpelEvents");

            migrationBuilder.DropColumn(
                name: "ImpactWinter",
                table: "SpelEvents");

            migrationBuilder.AddColumn<string>(
                name: "Effect",
                table: "SpelEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EffectGoed",
                table: "SpelEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
