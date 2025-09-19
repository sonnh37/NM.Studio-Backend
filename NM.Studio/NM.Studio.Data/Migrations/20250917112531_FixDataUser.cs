using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDataUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "avatar",
                table: "user",
                newName: "display_name");

            migrationBuilder.AddColumn<Guid>(
                name: "avatar_id",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user_otp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_otp", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_otp_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_avatar_id",
                table: "user",
                column: "avatar_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_otp_user_id",
                table: "user_otp",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_media_base_avatar_id",
                table: "user",
                column: "avatar_id",
                principalTable: "media_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_media_base_avatar_id",
                table: "user");

            migrationBuilder.DropTable(
                name: "user_otp");

            migrationBuilder.DropIndex(
                name: "IX_user_avatar_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "avatar_id",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "display_name",
                table: "user",
                newName: "avatar");
        }
    }
}
