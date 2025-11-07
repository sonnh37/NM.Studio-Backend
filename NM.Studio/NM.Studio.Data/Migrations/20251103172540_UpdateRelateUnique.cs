using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelateUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_avatar_id",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_service_background_cover_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_service_thumbnail_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_product_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_sub_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_thumbnail_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog");

            migrationBuilder.CreateIndex(
                name: "IX_user_avatar_id",
                table: "user",
                column: "avatar_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_background_cover_id",
                table: "service",
                column: "background_cover_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_thumbnail_id",
                table: "service",
                column: "thumbnail_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_category_id",
                table: "product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_sub_category_id",
                table: "product",
                column: "sub_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_thumbnail_id",
                table: "product",
                column: "thumbnail_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog",
                column: "thumbnail_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_avatar_id",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_service_background_cover_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_service_thumbnail_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_product_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_sub_category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_thumbnail_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog");

            migrationBuilder.CreateIndex(
                name: "IX_user_avatar_id",
                table: "user",
                column: "avatar_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_background_cover_id",
                table: "service",
                column: "background_cover_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_thumbnail_id",
                table: "service",
                column: "thumbnail_id",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog",
                column: "thumbnail_id",
                unique: true);
        }
    }
}
