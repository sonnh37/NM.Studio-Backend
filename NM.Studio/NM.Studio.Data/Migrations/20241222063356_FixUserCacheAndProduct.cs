using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserCacheAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumXPhoto_Album_AlbumId",
                table: "AlbumXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumXPhoto_Photo_PhotoId",
                table: "AlbumXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Color_ColorId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Size_SizeId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_SubCategory_SubCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXPhoto_Photo_PhotoId",
                table: "ProductXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXPhoto_Product_ProductId",
                table: "ProductXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory");

            migrationBuilder.DropIndex(
                name: "IX_Product_ColorId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SizeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "User",
                newName: "Preferences");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductXColor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductXColor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductXColor_Color_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Color",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductXColor_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductXSize",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductXSize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductXSize_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductXSize_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductXColor_ColorId",
                table: "ProductXColor",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductXColor_ProductId",
                table: "ProductXColor",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductXSize_ProductId",
                table: "ProductXSize",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductXSize_SizeId",
                table: "ProductXSize",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumXPhoto_Album_AlbumId",
                table: "AlbumXPhoto",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumXPhoto_Photo_PhotoId",
                table: "AlbumXPhoto",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SubCategory_SubCategoryId",
                table: "Product",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXPhoto_Photo_PhotoId",
                table: "ProductXPhoto",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXPhoto_Product_ProductId",
                table: "ProductXPhoto",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumXPhoto_Album_AlbumId",
                table: "AlbumXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumXPhoto_Photo_PhotoId",
                table: "AlbumXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_SubCategory_SubCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXPhoto_Photo_PhotoId",
                table: "ProductXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductXPhoto_Product_ProductId",
                table: "ProductXPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory");

            migrationBuilder.DropTable(
                name: "ProductXColor");

            migrationBuilder.DropTable(
                name: "ProductXSize");

            migrationBuilder.RenameColumn(
                name: "Preferences",
                table: "User",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ColorId",
                table: "Product",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SizeId",
                table: "Product",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumXPhoto_Album_AlbumId",
                table: "AlbumXPhoto",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumXPhoto_Photo_PhotoId",
                table: "AlbumXPhoto",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Color_ColorId",
                table: "Product",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Size_SizeId",
                table: "Product",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SubCategory_SubCategoryId",
                table: "Product",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXPhoto_Photo_PhotoId",
                table: "ProductXPhoto",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductXPhoto_Product_ProductId",
                table: "ProductXPhoto",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
