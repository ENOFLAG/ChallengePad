using Microsoft.EntityFrameworkCore.Migrations;

namespace ChallengePad.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objectives_OperationId",
                table: "Objectives");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_OperationId_Name",
                table: "Objectives",
                columns: new[] { "OperationId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objectives_OperationId_Name",
                table: "Objectives");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_OperationId",
                table: "Objectives",
                column: "OperationId");
        }
    }
}
