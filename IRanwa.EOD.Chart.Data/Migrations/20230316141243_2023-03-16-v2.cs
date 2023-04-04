using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230316v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DataSyncCompleted",
                table: "ExchangeSymbols",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "ExchangeSymbols",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSyncCompleted",
                table: "ExchangeSymbols");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "ExchangeSymbols");
        }
    }
}
