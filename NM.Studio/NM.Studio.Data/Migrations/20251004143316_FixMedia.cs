using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_image_image_image_id",
                table: "album_image");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "video");

            migrationBuilder.DropTable(
                name: "media_url");

            migrationBuilder.AddColumn<string>(
                name: "media_base_type",
                table: "media_base",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "media_url",
                table: "media_base",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_album_image_media_base_image_id",
                table: "album_image",
                column: "image_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_image_media_base_image_id",
                table: "album_image");

            migrationBuilder.DropColumn(
                name: "media_base_type",
                table: "media_base");

            migrationBuilder.DropColumn(
                name: "media_url",
                table: "media_base");

            migrationBuilder.CreateTable(
                name: "media_url",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    url_external = table.Column<string>(type: "text", nullable: true),
                    url_internal = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_url", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_base_id = table.Column<Guid>(type: "uuid", nullable: true),
                    media_url_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_image_media_base_media_base_id",
                        column: x => x.media_base_id,
                        principalTable: "media_base",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_image_media_url_media_url_id",
                        column: x => x.media_url_id,
                        principalTable: "media_url",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_base_id = table.Column<Guid>(type: "uuid", nullable: true),
                    media_url_id = table.Column<Guid>(type: "uuid", nullable: true),
                    category = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    resolution = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video", x => x.id);
                    table.ForeignKey(
                        name: "FK_video_media_base_media_base_id",
                        column: x => x.media_base_id,
                        principalTable: "media_base",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_video_media_url_media_url_id",
                        column: x => x.media_url_id,
                        principalTable: "media_url",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_image_media_base_id",
                table: "image",
                column: "media_base_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_image_media_url_id",
                table: "image",
                column: "media_url_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_video_media_base_id",
                table: "video",
                column: "media_base_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_video_media_url_id",
                table: "video",
                column: "media_url_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_album_image_image_image_id",
                table: "album_image",
                column: "image_id",
                principalTable: "image",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
