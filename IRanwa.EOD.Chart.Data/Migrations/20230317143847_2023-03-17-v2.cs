using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230317v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockAnnual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangeId = table.Column<int>(type: "int", nullable: false),
                    ExchangeSymbol = table.Column<int>(type: "int", nullable: false),
                    MarketCap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PERate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PBRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceToSale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvEBITDA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvSales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvOpeningCashFlow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarningYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitToAssets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayoutRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuickRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossProfitMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DividendYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceToCashFlow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceToFreeCashFlow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreeCashFlowYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitToEquity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitToEBITDA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingProfitMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsTurnOverRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnOnCapitalEmployed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EBITDAMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetIncomeMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EBITDAInterestExpense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAnnual", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockQuarterly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangeId = table.Column<int>(type: "int", nullable: false),
                    ExchangeSymbol = table.Column<int>(type: "int", nullable: false),
                    MarketCap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PERate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PBRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceToSale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvEBITDA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvSales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvOpeningCashFlow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarningYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitToAssets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayoutRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuickRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossProfitMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DividendYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceToCashFlow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceToFreeCashFlow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreeCashFlowYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitToEquity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitToEBITDA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingProfitMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsTurnOverRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnOnCapitalEmployed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EBITDAMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetIncomeMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EBITDAInterestExpense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuarterly", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockAnnual");

            migrationBuilder.DropTable(
                name: "StockQuarterly");
        }
    }
}
