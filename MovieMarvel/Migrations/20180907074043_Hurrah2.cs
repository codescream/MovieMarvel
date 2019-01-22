using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieMarvel.Migrations
{
    public partial class Hurrah2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Costs",
                table: "Item",
                newName: "Cost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Item",
                newName: "Costs");
        }
    }
}
