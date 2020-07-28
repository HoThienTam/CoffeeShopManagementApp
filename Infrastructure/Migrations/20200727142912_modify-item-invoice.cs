using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class modifyiteminvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableName",
                table: "Invoices");

            migrationBuilder.AddColumn<bool>(
                name: "IsOutOfStock",
                table: "Items",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TableId",
                table: "Invoices",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tables_TableId",
                table: "Invoices",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tables_TableId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TableId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsOutOfStock",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
