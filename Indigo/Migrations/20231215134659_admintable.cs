using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Indigo.Migrations
{
    /// <inheritdoc />
    public partial class admintable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
