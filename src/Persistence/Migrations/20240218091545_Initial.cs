using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SimulationStep = table.Column<long>(type: "INTEGER", nullable: false),
                    Price = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPrices", x => x.Id);
                    table.UniqueConstraint("AK_StockPrices_StockId_SimulationStep", x => new { x.StockId, x.SimulationStep });
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ticker = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Volatility = table.Column<float>(type: "REAL", nullable: false),
                    Drift = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockPrices_SimulationStep",
                table: "StockPrices",
                column: "SimulationStep");

            migrationBuilder.CreateIndex(
                name: "IX_StockPrices_StockId",
                table: "StockPrices",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_StockId",
                table: "Stocks",
                column: "StockId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Ticker",
                table: "Stocks",
                column: "Ticker",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPrices");

            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
