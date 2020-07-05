using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fix_item_discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceItemId",
                table: "ItemDiscounts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "InvoiceItems",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "InvoiceItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceItemId",
                table: "ItemDiscounts");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "InvoiceItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
