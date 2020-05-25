using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class edit_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Invoices_InvoiceId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_InvoiceId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Tables");

            migrationBuilder.AddColumn<bool>(
                name: "IsBeingUsed",
                table: "Tables",
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
                name: "IsBeingUsed",
                table: "Tables");

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId",
                table: "Tables",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tables_InvoiceId",
                table: "Tables",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Invoices_InvoiceId",
                table: "Tables",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
