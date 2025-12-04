using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMediaBaseFieldSourceAndFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mime_type",
                table: "media_base",
                newName: "resource_type");

            migrationBuilder.RenameColumn(
                name: "media_base_type",
                table: "media_base",
                newName: "format");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resource_type",
                table: "media_base",
                newName: "mime_type");

            migrationBuilder.RenameColumn(
                name: "format",
                table: "media_base",
                newName: "media_base_type");
        }
    }
}
