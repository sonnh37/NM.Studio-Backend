using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixProductMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_image");

            migrationBuilder.CreateTable(
                name: "product_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_base_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_media_media_base_media_base_id",
                        column: x => x.media_base_id,
                        principalTable: "media_base",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_media_product_variant_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_media_media_base_id",
                table: "product_media",
                column: "media_base_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_media_product_variant_id",
                table: "product_media",
                column: "product_variant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_media");

            migrationBuilder.CreateTable(
                name: "product_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_image_image_image_id",
                        column: x => x.image_id,
                        principalTable: "image",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_image_product_variant_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_image_image_id",
                table: "product_image",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_image_product_variant_id",
                table: "product_image",
                column: "product_variant_id");
        }
    }
}
