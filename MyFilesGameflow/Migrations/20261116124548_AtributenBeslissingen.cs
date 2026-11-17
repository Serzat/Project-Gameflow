using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFilesGameflow.Migrations
{
    /// <inheritdoc />
    public partial class AtributenBeslissingen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoelGroep",
                table: "beslissingen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Korting",
                table: "beslissingen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingKanaal",
                table: "beslissingen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersoneelTraining",
                table: "beslissingen",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoelGroep",
                table: "beslissingen");

            migrationBuilder.DropColumn(
                name: "Korting",
                table: "beslissingen");

            migrationBuilder.DropColumn(
                name: "MarketingKanaal",
                table: "beslissingen");

            migrationBuilder.DropColumn(
                name: "PersoneelTraining",
                table: "beslissingen");
        }
    }
}
