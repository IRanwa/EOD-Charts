using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IRanwa.EOD.Chart.Data.Migrations
{
    public partial class _20230317v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnnualSyncCompleted",
                table: "ExchangeSymbols",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QuarterlySyncCompleted",
                table: "ExchangeSymbols",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SyncException",
                table: "ExchangeSymbols",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualSyncCompleted",
                table: "ExchangeSymbols");

            migrationBuilder.DropColumn(
                name: "QuarterlySyncCompleted",
                table: "ExchangeSymbols");

            migrationBuilder.DropColumn(
                name: "SyncException",
                table: "ExchangeSymbols");
        }
    }
}
