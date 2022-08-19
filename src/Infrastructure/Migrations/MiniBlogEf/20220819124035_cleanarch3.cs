using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBlog.Infrastructure.Migrations.MiniBlogEf
{
    public partial class cleanarch3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tags_TagsTagId",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_TagsTagId",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagsTagId",
                table: "PostTag");

            migrationBuilder.RenameColumn(
                name: "CommentaryId",
                table: "Commentaries",
                newName: "Id");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Tags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "TagsId",
                table: "PostTag",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                columns: new[] { "PostsId", "TagsId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tags_TagsId",
                table: "PostTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tags_TagsId",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "PostTag");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Commentaries",
                newName: "CommentaryId");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TagsTagId",
                table: "PostTag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                columns: new[] { "PostsId", "TagsTagId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsTagId",
                table: "PostTag",
                column: "TagsTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tags_TagsTagId",
                table: "PostTag",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
