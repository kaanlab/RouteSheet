using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RouteSheet.Data.Migrations
{
    public partial class UpdateOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cadets_Classrooms_ClassroomId",
                table: "Cadets");

            migrationBuilder.AddForeignKey(
                name: "FK_Cadets_Classrooms_ClassroomId",
                table: "Cadets",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cadets_Classrooms_ClassroomId",
                table: "Cadets");

            migrationBuilder.AddForeignKey(
                name: "FK_Cadets_Classrooms_ClassroomId",
                table: "Cadets",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id");
        }
    }
}
