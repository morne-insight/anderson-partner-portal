using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndersonAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceSubType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceSubTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSubTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceSubTypes_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServiceSubTypes",
                columns: table => new
                {
                    CompaniesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceSubTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServiceSubTypes", x => new { x.CompaniesId, x.ServiceSubTypesId });
                    table.ForeignKey(
                        name: "FK_CompanyServiceSubTypes_Companies_CompaniesId",
                        column: x => x.CompaniesId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyServiceSubTypes_ServiceSubTypes_ServiceSubTypesId",
                        column: x => x.ServiceSubTypesId,
                        principalTable: "ServiceSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServiceSubTypes_ServiceSubTypesId",
                table: "CompanyServiceSubTypes",
                column: "ServiceSubTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSubTypes_ServiceTypeId",
                table: "ServiceSubTypes",
                column: "ServiceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyServiceSubTypes");

            migrationBuilder.DropTable(
                name: "ServiceSubTypes");
        }
    }
}
