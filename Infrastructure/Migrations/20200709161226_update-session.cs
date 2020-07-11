using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updatesession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Sessions");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Sessions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sessions");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Sessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
