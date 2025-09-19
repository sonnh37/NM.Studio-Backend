using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelateInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_image_image_image_id",
                table: "album_image");

            migrationBuilder.DropForeignKey(
                name: "FK_blog_media_base_background_cover_id",
                table: "blog");

            migrationBuilder.DropForeignKey(
                name: "FK_blog_media_base_thumbnail_id",
                table: "blog");

            migrationBuilder.DropForeignKey(
                name: "FK_product_image_image_image_id",
                table: "product_image");

            migrationBuilder.DropForeignKey(
                name: "FK_service_media_base_background_cover_id",
                table: "service");

            migrationBuilder.DropForeignKey(
                name: "FK_service_media_base_thumbnail_id",
                table: "service");

            migrationBuilder.DropForeignKey(
                name: "FK_service_booking_service_service_id",
                table: "service_booking");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropIndex(
                name: "IX_service_background_cover_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_service_thumbnail_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_blog_background_cover_id",
                table: "blog");

            migrationBuilder.DropIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog");

            migrationBuilder.DropColumn(
                name: "cache",
                table: "user");

            migrationBuilder.DropColumn(
                name: "failed_login_attempts",
                table: "user");

            migrationBuilder.DropColumn(
                name: "last_login_date",
                table: "user");

            migrationBuilder.DropColumn(
                name: "last_login_ip",
                table: "user");

            migrationBuilder.DropColumn(
                name: "lockout_end",
                table: "user");

            migrationBuilder.DropColumn(
                name: "otp",
                table: "user");

            migrationBuilder.DropColumn(
                name: "otp_expiration",
                table: "user");

            migrationBuilder.DropColumn(
                name: "password_changed_date",
                table: "user");

            migrationBuilder.DropColumn(
                name: "password_reset_expiration",
                table: "user");

            migrationBuilder.DropColumn(
                name: "password_reset_token",
                table: "user");

            migrationBuilder.CreateTable(
                name: "user_session",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    login_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    login_ip = table.Column<string>(type: "text", nullable: true),
                    device_info = table.Column<string>(type: "text", nullable: true),
                    session_token = table.Column<string>(type: "text", nullable: true),
                    last_activity = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    failed_login_attempts = table.Column<int>(type: "integer", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_session", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_session_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_setting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    key = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_setting", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_setting_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true),
                    key_id = table.Column<string>(type: "text", nullable: true),
                    public_key = table.Column<string>(type: "text", nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: true),
                    ip_address = table.Column<string>(type: "text", nullable: true),
                    expiry = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_blog_background_cover_id",
                table: "blog",
                column: "background_cover_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog",
                column: "thumbnail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_session_user_id",
                table: "user_session",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_setting_user_id",
                table: "user_setting",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_token_user_id",
                table: "user_token",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_album_image_image_image_id",
                table: "album_image",
                column: "image_id",
                principalTable: "image",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blog_media_base_background_cover_id",
                table: "blog",
                column: "background_cover_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blog_media_base_thumbnail_id",
                table: "blog",
                column: "thumbnail_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_image_image_image_id",
                table: "product_image",
                column: "image_id",
                principalTable: "image",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_media_base_background_cover_id",
                table: "service",
                column: "background_cover_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_media_base_thumbnail_id",
                table: "service",
                column: "thumbnail_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_booking_service_service_id",
                table: "service_booking",
                column: "service_id",
                principalTable: "service",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_image_image_image_id",
                table: "album_image");

            migrationBuilder.DropForeignKey(
                name: "FK_blog_media_base_background_cover_id",
                table: "blog");

            migrationBuilder.DropForeignKey(
                name: "FK_blog_media_base_thumbnail_id",
                table: "blog");

            migrationBuilder.DropForeignKey(
                name: "FK_product_image_image_image_id",
                table: "product_image");

            migrationBuilder.DropForeignKey(
                name: "FK_service_media_base_background_cover_id",
                table: "service");

            migrationBuilder.DropForeignKey(
                name: "FK_service_media_base_thumbnail_id",
                table: "service");

            migrationBuilder.DropForeignKey(
                name: "FK_service_booking_service_service_id",
                table: "service_booking");

            migrationBuilder.DropTable(
                name: "user_session");

            migrationBuilder.DropTable(
                name: "user_setting");

            migrationBuilder.DropTable(
                name: "user_token");

            migrationBuilder.DropIndex(
                name: "IX_service_background_cover_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_service_thumbnail_id",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_blog_background_cover_id",
                table: "blog");

            migrationBuilder.DropIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog");

            migrationBuilder.AddColumn<string>(
                name: "cache",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "failed_login_attempts",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "last_login_date",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_login_ip",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lockout_end",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otp",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "otp_expiration",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_changed_date",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_reset_expiration",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password_reset_token",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    expiry = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ip_address = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    key_id = table.Column<string>(type: "text", nullable: true),
                    public_key = table.Column<string>(type: "text", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_refresh_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_service_background_cover_id",
                table: "service",
                column: "background_cover_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_thumbnail_id",
                table: "service",
                column: "thumbnail_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_background_cover_id",
                table: "blog",
                column: "background_cover_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_thumbnail_id",
                table: "blog",
                column: "thumbnail_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_user_id",
                table: "refresh_token",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_album_image_image_image_id",
                table: "album_image",
                column: "image_id",
                principalTable: "image",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_blog_media_base_background_cover_id",
                table: "blog",
                column: "background_cover_id",
                principalTable: "media_base",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_blog_media_base_thumbnail_id",
                table: "blog",
                column: "thumbnail_id",
                principalTable: "media_base",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_image_image_image_id",
                table: "product_image",
                column: "image_id",
                principalTable: "image",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_media_base_background_cover_id",
                table: "service",
                column: "background_cover_id",
                principalTable: "media_base",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_media_base_thumbnail_id",
                table: "service",
                column: "thumbnail_id",
                principalTable: "media_base",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_booking_service_service_id",
                table: "service_booking",
                column: "service_id",
                principalTable: "service",
                principalColumn: "id");
        }
    }
}
