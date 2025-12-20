using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChameRozAP.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ChameRozToday",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChameID = table.Column<int>(type: "INTEGER", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChameRozToday", x => x.id);
                    table.ForeignKey(
                        name: "FK_ChameRozToday_ChameRoz_ChameID",
                        column: x => x.ChameID,
                        principalTable: "ChameRoz",
                        principalColumn: "ChameID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChameRozToday_ChameID",
                table: "ChameRozToday",
                column: "ChameID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChameRozToday");

            migrationBuilder.DropTable(
                name: "ChameRoz");
        }
    }
}
