using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230327v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ExchangeSymbols",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeSymbols_AnnualSyncCompleted",
                table: "ExchangeSymbols",
                column: "AnnualSyncCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeSymbols_DataSyncCompleted",
                table: "ExchangeSymbols",
                column: "DataSyncCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeSymbols_QuarterlySyncCompleted",
                table: "ExchangeSymbols",
                column: "QuarterlySyncCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeSymbols_Type",
                table: "ExchangeSymbols",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeSymbols_AnnualSyncCompleted",
                table: "ExchangeSymbols");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeSymbols_DataSyncCompleted",
                table: "ExchangeSymbols");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeSymbols_QuarterlySyncCompleted",
                table: "ExchangeSymbols");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeSymbols_Type",
                table: "ExchangeSymbols");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ExchangeSymbols",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
