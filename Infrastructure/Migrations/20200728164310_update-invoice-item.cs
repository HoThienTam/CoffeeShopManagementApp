using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updateinvoiceitem : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "TableId",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AlterColumn<Guid>(
                name: "TableId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
