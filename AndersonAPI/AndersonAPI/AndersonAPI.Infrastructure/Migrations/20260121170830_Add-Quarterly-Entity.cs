using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndersonAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuarterlyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quarterlies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarterlies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quarterlies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartnerCount = table.Column<int>(type: "int", nullable: false),
                    Headcount = table.Column<int>(type: "int", nullable: false),
                    ClientCount = table.Column<int>(type: "int", nullable: false),
                    OfficeCount = table.Column<int>(type: "int", nullable: false),
                    LawyerCount = table.Column<int>(type: "int", nullable: false),
                    EstimatedRevenue = table.Column<double>(type: "float", nullable: false),
                    QuarterlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportLine_Quarterlies_QuarterlyId",
                        column: x => x.QuarterlyId,
                        principalTable: "Quarterlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportPartner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuaterlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPartner", x => x.Id);
                    table.CheckConstraint("report_partner_status_check", "\"Status\" IN ('Hired','Promoted','Terminated')");
                    table.ForeignKey(
                        name: "FK_ReportPartner_Quarterlies_QuaterlyId",
                        column: x => x.QuaterlyId,
                        principalTable: "Quarterlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quarterlies_CompanyId",
                table: "Quarterlies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLine_QuarterlyId",
                table: "ReportLine",
                column: "QuarterlyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPartner_QuaterlyId",
                table: "ReportPartner",
                column: "QuaterlyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportLine");

            migrationBuilder.DropTable(
                name: "ReportPartner");

            migrationBuilder.DropTable(
                name: "Quarterlies");
        }
    }
}
