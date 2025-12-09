using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlbumImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_cover",
                table: "album_image");

            migrationBuilder.DropColumn(
                name: "is_thumbnail",
                table: "album_image");

            migrationBuilder.AddColumn<Guid>(
                name: "cover_id",
                table: "album",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "thumbnail_id",
                table: "album",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_album_cover_id",
                table: "album",
                column: "cover_id");

            migrationBuilder.CreateIndex(
                name: "IX_album_thumbnail_id",
                table: "album",
                column: "thumbnail_id");

            migrationBuilder.AddForeignKey(
                name: "FK_album_media_base_cover_id",
                table: "album",
                column: "cover_id",
                principalTable: "media_base",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_album_media_base_thumbnail_id",
                table: "album",
                column: "thumbnail_id",
                principalTable: "media_base",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_media_base_cover_id",
                table: "album");

            migrationBuilder.DropForeignKey(
                name: "FK_album_media_base_thumbnail_id",
                table: "album");

            migrationBuilder.DropIndex(
                name: "IX_album_cover_id",
                table: "album");

            migrationBuilder.DropIndex(
                name: "IX_album_thumbnail_id",
                table: "album");

            migrationBuilder.DropColumn(
                name: "cover_id",
                table: "album");

            migrationBuilder.DropColumn(
                name: "thumbnail_id",
                table: "album");

            migrationBuilder.AddColumn<bool>(
                name: "is_cover",
                table: "album_image",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_thumbnail",
                table: "album_image",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
