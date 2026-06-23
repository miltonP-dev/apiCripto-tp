using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace apiCripto_tp.Migrations
{
    /// <inheritdoc />
    public partial class apiCripto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cryptos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Simbolo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CryptoCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CryptoAmount = table.Column<decimal>(type: "decimal(28,12)", precision: 28, scale: 12, nullable: false),
                    Money = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: false),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Usuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cryptos",
                columns: new[] { "Id", "Codigo", "Nombre", "Simbolo" },
                values: new object[,]
                {
                    { 1, "btc", "Bitcoin", "BTC" },
                    { 2, "eth", "Ethereum", "ETH" },
                    { 3, "usdc", "USD Coin", "USDC" },
                    { 4, "usdt", "Tether", "USDT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Nombre_Usuario",
                table: "Usuarios",
                column: "Nombre_Usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cryptos");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
