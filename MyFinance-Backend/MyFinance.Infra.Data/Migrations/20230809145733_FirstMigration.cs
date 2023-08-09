using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Income = table.Column<double>(type: "float(17)", precision: 17, scale: 4, nullable: false),
                    Outcome = table.Column<double>(type: "float(17)", precision: 17, scale: 4, nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ReasonToArchive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArchiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountTagBusinessUnit",
                columns: table => new
                {
                    AccountTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessUnitsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTagBusinessUnit", x => new { x.AccountTagsId, x.BusinessUnitsId });
                    table.ForeignKey(
                        name: "FK_AccountTagBusinessUnit_AccountTags_AccountTagsId",
                        column: x => x.AccountTagsId,
                        principalTable: "AccountTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTagBusinessUnit_BusinessUnits_BusinessUnitsId",
                        column: x => x.BusinessUnitsId,
                        principalTable: "BusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceYear = table.Column<int>(type: "int", nullable: false),
                    ReferenceMonth = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<double>(type: "float(17)", precision: 17, scale: 4, nullable: false),
                    Outcome = table.Column<double>(type: "float(17)", precision: 17, scale: 4, nullable: false),
                    BusinessUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyBalances_BusinessUnits_BusinessUnitId",
                        column: x => x.BusinessUnitId,
                        principalTable: "BusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<double>(type: "float(17)", precision: 17, scale: 4, nullable: false),
                    RelatedTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthlyBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_AccountTags_AccountTagId",
                        column: x => x.AccountTagId,
                        principalTable: "AccountTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_MonthlyBalances_MonthlyBalanceId",
                        column: x => x.MonthlyBalanceId,
                        principalTable: "MonthlyBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTagBusinessUnit_BusinessUnitsId",
                table: "AccountTagBusinessUnit",
                column: "BusinessUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_Name",
                table: "BusinessUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyBalances_BusinessUnitId",
                table: "MonthlyBalances",
                column: "BusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountTagId",
                table: "Transfers",
                column: "AccountTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_MonthlyBalanceId",
                table: "Transfers",
                column: "MonthlyBalanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTagBusinessUnit");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "AccountTags");

            migrationBuilder.DropTable(
                name: "MonthlyBalances");

            migrationBuilder.DropTable(
                name: "BusinessUnits");
        }
    }
}
