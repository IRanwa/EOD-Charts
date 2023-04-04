using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230323v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EODData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    ExchangeSymbol = table.Column<int>(type: "int", nullable: false),
                    Open = table.Column<double>(type: "float", nullable: true),
                    Close = table.Column<double>(type: "float", nullable: true),
                    High = table.Column<double>(type: "float", nullable: true),
                    Low = table.Column<double>(type: "float", nullable: true),
                    AdjustedClose = table.Column<double>(type: "float", nullable: true),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EODData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EODData_ExchangeSymbols_ExchangeSymbol",
                        column: x => x.ExchangeSymbol,
                        principalTable: "ExchangeSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EODData_ExchangeSymbol",
                table: "EODData",
                column: "ExchangeSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_EODData_Timestamp",
                table: "EODData",
                column: "Timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EODData");
        }
    }
}
