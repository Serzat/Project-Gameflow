using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFilesGameflow.Migrations
{
    /// <inheritdoc />
    public partial class secondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RondeNummer",
                table: "beslissingen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamNummer",
                table: "beslissingen",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RondeNummer",
                table: "beslissingen");

            migrationBuilder.DropColumn(
                name: "TeamNummer",
                table: "beslissingen");
        }
    }
}
