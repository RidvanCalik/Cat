using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatImages.Migrations
{
    /// <inheritdoc />
    public partial class secondaryMigraiton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "catPathName",
                table: "CatItems");

            migrationBuilder.AddColumn<string>(
                name: "catImgName",
                table: "CatItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "catImgName",
                table: "CatItems");

            migrationBuilder.AddColumn<string>(
                name: "catPathName",
                table: "CatItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
