using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gameflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class Doelen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doelen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResortType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Volgorde = table.Column<int>(type: "int", nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kpi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doelwaarde = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doelen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BehaaldeDoelen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResortType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoelId = table.Column<int>(type: "int", nullable: false),
                    RondeNummer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaaldeDoelen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehaaldeDoelen_Doelen_DoelId",
                        column: x => x.DoelId,
                        principalTable: "Doelen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BehaaldeDoelen_DoelId",
                table: "BehaaldeDoelen",
                column: "DoelId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaaldeDoelen_ResortType_DoelId",
                table: "BehaaldeDoelen",
                columns: new[] { "ResortType", "DoelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doelen_Modus_ResortType_Level_Volgorde",
                table: "Doelen",
                columns: new[] { "Modus", "ResortType", "Level", "Volgorde" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BehaaldeDoelen");

            migrationBuilder.DropTable(
                name: "Doelen");
        }
    }
}
