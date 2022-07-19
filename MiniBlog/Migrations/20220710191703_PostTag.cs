using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingAirbnb.Migrations
{
    public partial class PostTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentary_Posts_PostId",
                table: "Commentary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commentary",
                table: "Commentary");

            migrationBuilder.RenameTable(
                name: "Commentary",
                newName: "Commentaries");

            migrationBuilder.RenameIndex(
                name: "IX_Commentary_PostId",
                table: "Commentaries",
                newName: "IX_Commentaries_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commentaries",
                table: "Commentaries",
                column: "CommentaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaries_Posts_PostId",
                table: "Commentaries",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaries_Posts_PostId",
                table: "Commentaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commentaries",
                table: "Commentaries");

            migrationBuilder.RenameTable(
                name: "Commentaries",
                newName: "Commentary");

            migrationBuilder.RenameIndex(
                name: "IX_Commentaries_PostId",
                table: "Commentary",
                newName: "IX_Commentary_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commentary",
                table: "Commentary",
                column: "CommentaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentary_Posts_PostId",
                table: "Commentary",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }
    }
}
