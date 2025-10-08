using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "key_id",
                table: "user_token");

            migrationBuilder.DropColumn(
                name: "public_key",
                table: "user_token");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "user_token",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "expiry",
                table: "user_token",
                newName: "expiry_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "refresh_token",
                table: "user_token",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "expiry_time",
                table: "user_token",
                newName: "expiry");

            migrationBuilder.AddColumn<string>(
                name: "key_id",
                table: "user_token",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "public_key",
                table: "user_token",
                type: "text",
                nullable: true);
        }
    }
}
