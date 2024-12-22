using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductXColor_Color_ColorId",
                table: "ProductXColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXColor_Product_ProductId",
                table: "ProductXColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXSize_Product_ProductId",
                table: "ProductXSize");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXSize_Size_SizeId",
                table: "ProductXSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductXSize",
                table: "ProductXSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductXColor",
                table: "ProductXColor");

            migrationBuilder.RenameTable(
                name: "ProductXSize",
                newName: "ProductXSizes");

            migrationBuilder.RenameTable(
                name: "ProductXColor",
                newName: "ProductXColors");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXSize_SizeId",
                table: "ProductXSizes",
                newName: "IX_ProductXSizes_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXSize_ProductId",
                table: "ProductXSizes",
                newName: "IX_ProductXSizes_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXColor_ProductId",
                table: "ProductXColors",
                newName: "IX_ProductXColors_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXColor_ColorId",
                table: "ProductXColors",
                newName: "IX_ProductXColors_ColorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductXSizes",
                table: "ProductXSizes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductXColors",
                table: "ProductXColors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXColors_Color_ColorId",
                table: "ProductXColors",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXColors_Product_ProductId",
                table: "ProductXColors",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXSizes_Product_ProductId",
                table: "ProductXSizes",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXSizes_Size_SizeId",
                table: "ProductXSizes",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductXColors_Color_ColorId",
                table: "ProductXColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXColors_Product_ProductId",
                table: "ProductXColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXSizes_Product_ProductId",
                table: "ProductXSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXSizes_Size_SizeId",
                table: "ProductXSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductXSizes",
                table: "ProductXSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductXColors",
                table: "ProductXColors");

            migrationBuilder.RenameTable(
                name: "ProductXSizes",
                newName: "ProductXSize");

            migrationBuilder.RenameTable(
                name: "ProductXColors",
                newName: "ProductXColor");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXSizes_SizeId",
                table: "ProductXSize",
                newName: "IX_ProductXSize_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXSizes_ProductId",
                table: "ProductXSize",
                newName: "IX_ProductXSize_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXColors_ProductId",
                table: "ProductXColor",
                newName: "IX_ProductXColor_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductXColors_ColorId",
                table: "ProductXColor",
                newName: "IX_ProductXColor_ColorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductXSize",
                table: "ProductXSize",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductXColor",
                table: "ProductXColor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXColor_Color_ColorId",
                table: "ProductXColor",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXColor_Product_ProductId",
                table: "ProductXColor",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXSize_Product_ProductId",
                table: "ProductXSize",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXSize_Size_SizeId",
                table: "ProductXSize",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id");
        }
    }
}
