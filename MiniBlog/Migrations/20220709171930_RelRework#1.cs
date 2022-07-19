using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampingAirbnb.Migrations
{
    public partial class RelRework1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentary_Posts_PostId",
                table: "Commentary");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Commentary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentary_Posts_PostId",
                table: "Commentary",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentary_Posts_PostId",
                table: "Commentary");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Commentary",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentary_Posts_PostId",
                table: "Commentary",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
