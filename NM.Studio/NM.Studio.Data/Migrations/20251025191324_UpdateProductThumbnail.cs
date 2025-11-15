using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_category_category_id",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_product_sub_category_sub_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_sub_category_id",
                table: "product");

            migrationBuilder.AddColumn<Guid>(
                name: "thumbnail_id",
                table: "product",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_category_id",
                table: "product",
                column: "category_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_sub_category_id",
                table: "product",
                column: "sub_category_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_thumbnail_id",
                table: "product",
                column: "thumbnail_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_product_category_category_id",
                table: "product",
                column: "category_id",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_media_base_thumbnail_id",
                table: "product",
                column: "thumbnail_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_sub_category_sub_category_id",
                table: "product",
                column: "sub_category_id",
                principalTable: "sub_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_category_category_id",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_product_media_base_thumbnail_id",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_product_sub_category_sub_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_sub_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_thumbnail_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "thumbnail_id",
                table: "product");

            migrationBuilder.CreateIndex(
                name: "IX_product_category_id",
                table: "product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_sub_category_id",
                table: "product",
                column: "sub_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_category_category_id",
                table: "product",
                column: "category_id",
                principalTable: "category",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_sub_category_sub_category_id",
                table: "product",
                column: "sub_category_id",
                principalTable: "sub_category",
                principalColumn: "id");
        }
    }
}
