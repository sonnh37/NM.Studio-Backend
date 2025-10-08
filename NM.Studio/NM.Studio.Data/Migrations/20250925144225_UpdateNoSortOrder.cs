using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NM.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNoSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "sub_category");

            migrationBuilder.DropColumn(
                name: "home_sort_order",
                table: "service");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "service");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "category");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "album_image");

            migrationBuilder.DropColumn(
                name: "home_sort_order",
                table: "album");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "album");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "sub_category",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "home_sort_order",
                table: "service",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "service",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "category",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "album_image",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "home_sort_order",
                table: "album",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "album",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
