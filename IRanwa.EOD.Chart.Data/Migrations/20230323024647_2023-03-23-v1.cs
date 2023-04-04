using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230323v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockQuarterly_ExchangeId",
                table: "StockQuarterly");

            migrationBuilder.DropIndex(
                name: "IX_StockAnnual_ExchangeId",
                table: "StockAnnual");

            migrationBuilder.DropColumn(
                name: "ExchangeId",
                table: "StockQuarterly");

            migrationBuilder.DropColumn(
                name: "ExchangeId",
                table: "StockAnnual");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAnnual_ExchangeSymbols_ExchangeSymbol",
                table: "StockAnnual",
                column: "ExchangeSymbol",
                principalTable: "ExchangeSymbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockQuarterly_ExchangeSymbols_ExchangeSymbol",
                table: "StockQuarterly",
                column: "ExchangeSymbol",
                principalTable: "ExchangeSymbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAnnual_ExchangeSymbols_ExchangeSymbol",
                table: "StockAnnual");

            migrationBuilder.DropForeignKey(
                name: "FK_StockQuarterly_ExchangeSymbols_ExchangeSymbol",
                table: "StockQuarterly");

            migrationBuilder.AddColumn<int>(
                name: "ExchangeId",
                table: "StockQuarterly",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeId",
                table: "StockAnnual",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockQuarterly_ExchangeId",
                table: "StockQuarterly",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAnnual_ExchangeId",
                table: "StockAnnual",
                column: "ExchangeId");
        }
    }
}
