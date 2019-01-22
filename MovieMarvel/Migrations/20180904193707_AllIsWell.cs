using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieMarvel.Migrations
{
    public partial class AllIsWell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoviePoster",
                table: "Item",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MovieTitle",
                table: "Item",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoviePoster",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "MovieTitle",
                table: "Item");
        }
    }
}
