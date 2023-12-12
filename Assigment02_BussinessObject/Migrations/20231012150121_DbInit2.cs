using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assigment02_BussinessObject.Migrations
{
    public partial class DbInit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publisher_pub_id",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "pub_id",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publisher_pub_id",
                table: "Books",
                column: "pub_id",
                principalTable: "Publisher",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publisher_pub_id",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "pub_id",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publisher_pub_id",
                table: "Books",
                column: "pub_id",
                principalTable: "Publisher",
                principalColumn: "pub_id");
        }
    }
}
