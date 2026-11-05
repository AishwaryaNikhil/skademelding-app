using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TelenorSkademeldingApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DamageCauses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageCauses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    DamageTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DamageCauseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    ReportedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_DamageCauses_DamageCauseId",
                        column: x => x.DamageCauseId,
                        principalTable: "DamageCauses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_DamageTypes_DamageTypeId",
                        column: x => x.DamageTypeId,
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 260, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    StoragePath = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimAttachments_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DamageCauses",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "ACCIDENT", "Skade forårsaket av en ulykke, f.eks. kollisjon", true, "Ulykke" },
                    { 2, "NATURE", "Skade forårsaket av vind, storm, lyn eller annet vær", true, "Natur/vær" },
                    { 3, "FIRE", "Skade forårsaket av brann eller røyk", true, "Brann" },
                    { 4, "THEFT", "Skade forårsaket av tyveri eller hærverk", true, "Tyveri/hærverk" },
                    { 5, "WATER", "Skade forårsaket av vannlekkasje eller flom", true, "Vann/flom" }
                });

            migrationBuilder.InsertData(
                table: "DamageTypes",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "CAR", "Skade på bil eller annet kjøretøy", true, "Skade på bil" },
                    { 2, "PROPERTY", "Skade på bygning, hus eller annen eiendom", true, "Skade på bygning/eiendom" },
                    { 3, "WIND_AREA", "Uteområde rammet av vind eller storm", true, "Område rammet av vind" },
                    { 4, "OTHER", "Annen type skade", true, "Annet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimAttachments_ClaimId",
                table: "ClaimAttachments",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CreatedAtUtc",
                table: "Claims",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_DamageCauseId",
                table: "Claims",
                column: "DamageCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_DamageTypeId",
                table: "Claims",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Reference",
                table: "Claims",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ReportedBy",
                table: "Claims",
                column: "ReportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_Status",
                table: "Claims",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DamageCauses_Code",
                table: "DamageCauses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DamageTypes_Code",
                table: "DamageTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimAttachments");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "DamageCauses");

            migrationBuilder.DropTable(
                name: "DamageTypes");
        }
    }
}
