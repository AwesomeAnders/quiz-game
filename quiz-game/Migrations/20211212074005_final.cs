using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiz_game.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Quizzes",
                newName: "Titel");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "QuestionOptions",
                newName: "Text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titel",
                table: "Quizzes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "QuestionOptions",
                newName: "Name");
        }
    }
}
