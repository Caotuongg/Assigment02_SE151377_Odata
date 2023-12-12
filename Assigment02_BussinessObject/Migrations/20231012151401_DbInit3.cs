using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assigment02_BussinessObject.Migrations
{
    public partial class DbInit3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publisher_pub_id",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_pub_id",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Publisherpub_id",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_Publisherpub_id",
                table: "Books",
                column: "Publisherpub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publisher_Publisherpub_id",
                table: "Books",
                column: "Publisherpub_id",
                principalTable: "Publisher",
                principalColumn: "pub_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publisher_Publisherpub_id",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Publisherpub_id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Publisherpub_id",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_pub_id",
                table: "Books",
                column: "pub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publisher_pub_id",
                table: "Books",
                column: "pub_id",
                principalTable: "Publisher",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
