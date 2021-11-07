using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jorteck.Toolbox.Migrations
{
    public partial class AddEncounterModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EncounterPresets",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterPresets", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "EncounterSpawn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocalOffsetX = table.Column<float>(type: "REAL", nullable: false),
                    LocalOffsetY = table.Column<float>(type: "REAL", nullable: false),
                    LocalOffsetZ = table.Column<float>(type: "REAL", nullable: false),
                    RotationX = table.Column<float>(type: "REAL", nullable: false),
                    RotationY = table.Column<float>(type: "REAL", nullable: false),
                    RotationZ = table.Column<float>(type: "REAL", nullable: false),
                    CreatureData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    EncounterName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterSpawn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncounterSpawn_EncounterPresets_EncounterName",
                        column: x => x.EncounterName,
                        principalTable: "EncounterPresets",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EncounterSpawn_EncounterName",
                table: "EncounterSpawn",
                column: "EncounterName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncounterSpawn");

            migrationBuilder.DropTable(
                name: "EncounterPresets");
        }
    }
}
