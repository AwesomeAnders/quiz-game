using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiz_game.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "Questions");

            migrationBuilder.AlterColumn<long>(
                name: "QuizId",
                table: "Questions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QuizId1",
                table: "Questions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId1",
                table: "Questions",
                column: "QuizId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId1",
                table: "Questions",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }
    }
}
