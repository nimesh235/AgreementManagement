using Microsoft.EntityFrameworkCore.Migrations;

namespace AgreementManagement.Data.Migrations
{
    public partial class MigrationV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreement_ProductGroup_ProductGroupId",
                table: "Agreement");

            migrationBuilder.DropIndex(
                name: "IX_Agreement_ProductGroupId",
                table: "Agreement");

            migrationBuilder.DropColumn(
                name: "ProductGroupId",
                table: "Agreement");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Agreement",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductGroupId",
                table: "Product",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_UserId",
                table: "Agreement",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreement_AspNetUsers_UserId",
                table: "Agreement",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductGroup_ProductGroupId",
                table: "Product",
                column: "ProductGroupId",
                principalTable: "ProductGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreement_AspNetUsers_UserId",
                table: "Agreement");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductGroup_ProductGroupId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductGroupId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Agreement_UserId",
                table: "Agreement");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Agreement",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "ProductGroupId",
                table: "Agreement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_ProductGroupId",
                table: "Agreement",
                column: "ProductGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreement_ProductGroup_ProductGroupId",
                table: "Agreement",
                column: "ProductGroupId",
                principalTable: "ProductGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
