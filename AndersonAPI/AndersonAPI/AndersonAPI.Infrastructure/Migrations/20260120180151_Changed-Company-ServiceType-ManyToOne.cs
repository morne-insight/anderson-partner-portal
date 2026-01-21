using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndersonAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCompanyServiceTypeManyToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyServiceTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("E34A3164-0967-4BD6-466F-08DE54E6A79C"));

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ServiceTypeId",
                table: "Companies",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_ServiceTypes_ServiceTypeId",
                table: "Companies",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_ServiceTypes_ServiceTypeId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ServiceTypeId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Companies");

            migrationBuilder.CreateTable(
                name: "CompanyServiceTypes",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceTypes", x => new { x.CompanyId, x.ServiceTypesId });
                    table.ForeignKey(
                        name: "FK_CompanyServiceTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyServiceTypes_ServiceTypes_ServiceTypesId",
                        column: x => x.ServiceTypesId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceTypes_ServiceTypesId",
                table: "CompanyServiceTypes",
                column: "ServiceTypesId");
        }
    }
}
