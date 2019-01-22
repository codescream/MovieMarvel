using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieMarvel.Migrations
{
    public partial class blessGod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Item",
                newName: "Costs");
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
