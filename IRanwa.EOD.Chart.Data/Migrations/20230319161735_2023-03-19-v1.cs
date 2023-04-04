using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230319v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ExchangeSymbols",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ExchangeCodes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockQuarterly_ExchangeId",
                table: "StockQuarterly",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuarterly_ExchangeSymbol",
                table: "StockQuarterly",
                column: "ExchangeSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_StockAnnual_ExchangeId",
                table: "StockAnnual",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAnnual_ExchangeSymbol",
                table: "StockAnnual",
                column: "ExchangeSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeSymbols_Code",
                table: "ExchangeSymbols",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeCodes_Code",
                table: "ExchangeCodes",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockQuarterly_ExchangeId",
                table: "StockQuarterly");

            migrationBuilder.DropIndex(
                name: "IX_StockQuarterly_ExchangeSymbol",
                table: "StockQuarterly");

            migrationBuilder.DropIndex(
                name: "IX_StockAnnual_ExchangeId",
                table: "StockAnnual");

            migrationBuilder.DropIndex(
                name: "IX_StockAnnual_ExchangeSymbol",
                table: "StockAnnual");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeSymbols_Code",
                table: "ExchangeSymbols");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeCodes_Code",
                table: "ExchangeCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ExchangeSymbols",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ExchangeCodes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
