using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TensorProject.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BinanceHistoricalDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpenTime = table.Column<long>(type: "bigint", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CloseTime = table.Column<long>(type: "bigint", nullable: false),
                    QuoteAssetVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfTrades = table.Column<int>(type: "int", nullable: false),
                    TakerBuyBaseAssetVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TakerBuyQuoteAssetVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unused = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinanceHistoricalDatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinanceHistoricalDatas");
        }
    }
}
