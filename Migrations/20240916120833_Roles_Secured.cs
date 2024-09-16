using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BowlingAlley.Migrations
{
    /// <inheritdoc />
    public partial class Roles_Secured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Roles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "1478523690");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Roles");
        }
    }
}
