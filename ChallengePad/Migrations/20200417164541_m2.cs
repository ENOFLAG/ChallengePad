using Microsoft.EntityFrameworkCore.Migrations;

namespace ChallengePad.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Operations",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Objectives",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Objectives");
        }
    }
}
