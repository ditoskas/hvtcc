using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hvt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SettingKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BinanceSymbol = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PolygonTicker = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LeverageToTrade = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    EquityPercentageToBet = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    ProfitTargetPercentage = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    StopLossTargetPercentage = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false),
                    TradeType = table.Column<int>(type: "integer", nullable: false),
                    OpenPrice = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "numeric(18,8)", nullable: true),
                    ProfitAndLoss = table.Column<decimal>(type: "numeric(18,8)", nullable: true),
                    Volume = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    InvestedAmount = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    Leverage = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    OpenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CloseAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trades_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trades_SymbolId",
                table: "Trades",
                column: "SymbolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Symbols");
        }
    }
}
