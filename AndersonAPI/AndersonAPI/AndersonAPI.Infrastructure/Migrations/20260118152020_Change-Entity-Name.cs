using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndersonAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Oppertunities_OppertunityId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "OppertunityCapabilities");

            migrationBuilder.DropTable(
                name: "OppertunityCompanies");

            migrationBuilder.DropTable(
                name: "OppertunityIndustries");

            migrationBuilder.DropTable(
                name: "OppertunityServiceTypes");

            migrationBuilder.DropTable(
                name: "Oppertunities");

            migrationBuilder.DropTable(
                name: "OppertunityTypes");

            migrationBuilder.RenameColumn(
                name: "OppertunityId",
                table: "Message",
                newName: "OpportunityId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_OppertunityId",
                table: "Message",
                newName: "IX_Message_OpportunityId");

            migrationBuilder.CreateTable(
                name: "OpportunityTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Opportunities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpportunityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunities", x => x.Id);
                    table.CheckConstraint("opportunity_status_check", "\"Status\" IN ('Open','Closed')");
                    table.ForeignKey(
                        name: "FK_Opportunities_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Opportunities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Opportunities_OpportunityTypes_OpportunityTypeId",
                        column: x => x.OpportunityTypeId,
                        principalTable: "OpportunityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpportunityCapabilities",
                columns: table => new
                {
                    CapabilitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpportunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityCapabilities", x => new { x.CapabilitiesId, x.OpportunitiesId });
                    table.ForeignKey(
                        name: "FK_OpportunityCapabilities_Capabilities_CapabilitiesId",
                        column: x => x.CapabilitiesId,
                        principalTable: "Capabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpportunityCapabilities_Opportunities_OpportunitiesId",
                        column: x => x.OpportunitiesId,
                        principalTable: "Opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpportunityCompanies",
                columns: table => new
                {
                    InterestedPartnersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SavedOpportunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityCompanies", x => new { x.InterestedPartnersId, x.SavedOpportunitiesId });
                    table.ForeignKey(
                        name: "FK_OpportunityCompanies_Companies_InterestedPartnersId",
                        column: x => x.InterestedPartnersId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpportunityCompanies_Opportunities_SavedOpportunitiesId",
                        column: x => x.SavedOpportunitiesId,
                        principalTable: "Opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpportunityIndustries",
                columns: table => new
                {
                    IndustriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpportunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityIndustries", x => new { x.IndustriesId, x.OpportunitiesId });
                    table.ForeignKey(
                        name: "FK_OpportunityIndustries_Industries_IndustriesId",
                        column: x => x.IndustriesId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpportunityIndustries_Opportunities_OpportunitiesId",
                        column: x => x.OpportunitiesId,
                        principalTable: "Opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpportunityServiceTypes",
                columns: table => new
                {
                    OpportunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityServiceTypes", x => new { x.OpportunitiesId, x.ServiceTypesId });
                    table.ForeignKey(
                        name: "FK_OpportunityServiceTypes_Opportunities_OpportunitiesId",
                        column: x => x.OpportunitiesId,
                        principalTable: "Opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpportunityServiceTypes_ServiceTypes_ServiceTypesId",
                        column: x => x.ServiceTypesId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_CompanyId",
                table: "Opportunities",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_CountryId",
                table: "Opportunities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_OpportunityTypeId",
                table: "Opportunities",
                column: "OpportunityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityCapabilities_OpportunitiesId",
                table: "OpportunityCapabilities",
                column: "OpportunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityCompanies_SavedOpportunitiesId",
                table: "OpportunityCompanies",
                column: "SavedOpportunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityIndustries_OpportunitiesId",
                table: "OpportunityIndustries",
                column: "OpportunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityServiceTypes_ServiceTypesId",
                table: "OpportunityServiceTypes",
                column: "ServiceTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Opportunities_OpportunityId",
                table: "Message",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Opportunities_OpportunityId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "OpportunityCapabilities");

            migrationBuilder.DropTable(
                name: "OpportunityCompanies");

            migrationBuilder.DropTable(
                name: "OpportunityIndustries");

            migrationBuilder.DropTable(
                name: "OpportunityServiceTypes");

            migrationBuilder.DropTable(
                name: "Opportunities");

            migrationBuilder.DropTable(
                name: "OpportunityTypes");

            migrationBuilder.RenameColumn(
                name: "OpportunityId",
                table: "Message",
                newName: "OppertunityId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_OpportunityId",
                table: "Message",
                newName: "IX_Message_OppertunityId");

            migrationBuilder.CreateTable(
                name: "OppertunityTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OppertunityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oppertunities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OppertunityTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oppertunities", x => x.Id);
                    table.CheckConstraint("oppertunity_status_check", "\"Status\" IN ('Open','Closed')");
                    table.ForeignKey(
                        name: "FK_Oppertunities_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Oppertunities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Oppertunities_OppertunityTypes_OppertunityTypeId",
                        column: x => x.OppertunityTypeId,
                        principalTable: "OppertunityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OppertunityCapabilities",
                columns: table => new
                {
                    CapabilitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OppertunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OppertunityCapabilities", x => new { x.CapabilitiesId, x.OppertunitiesId });
                    table.ForeignKey(
                        name: "FK_OppertunityCapabilities_Capabilities_CapabilitiesId",
                        column: x => x.CapabilitiesId,
                        principalTable: "Capabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OppertunityCapabilities_Oppertunities_OppertunitiesId",
                        column: x => x.OppertunitiesId,
                        principalTable: "Oppertunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OppertunityCompanies",
                columns: table => new
                {
                    InterestedPartnersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SavedOppertunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OppertunityCompanies", x => new { x.InterestedPartnersId, x.SavedOppertunitiesId });
                    table.ForeignKey(
                        name: "FK_OppertunityCompanies_Companies_InterestedPartnersId",
                        column: x => x.InterestedPartnersId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OppertunityCompanies_Oppertunities_SavedOppertunitiesId",
                        column: x => x.SavedOppertunitiesId,
                        principalTable: "Oppertunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OppertunityIndustries",
                columns: table => new
                {
                    IndustriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OppertunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OppertunityIndustries", x => new { x.IndustriesId, x.OppertunitiesId });
                    table.ForeignKey(
                        name: "FK_OppertunityIndustries_Industries_IndustriesId",
                        column: x => x.IndustriesId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OppertunityIndustries_Oppertunities_OppertunitiesId",
                        column: x => x.OppertunitiesId,
                        principalTable: "Oppertunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OppertunityServiceTypes",
                columns: table => new
                {
                    OppertunitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OppertunityServiceTypes", x => new { x.OppertunitiesId, x.ServiceTypesId });
                    table.ForeignKey(
                        name: "FK_OppertunityServiceTypes_Oppertunities_OppertunitiesId",
                        column: x => x.OppertunitiesId,
                        principalTable: "Oppertunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OppertunityServiceTypes_ServiceTypes_ServiceTypesId",
                        column: x => x.ServiceTypesId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oppertunities_CompanyId",
                table: "Oppertunities",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Oppertunities_CountryId",
                table: "Oppertunities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Oppertunities_OppertunityTypeId",
                table: "Oppertunities",
                column: "OppertunityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OppertunityCapabilities_OppertunitiesId",
                table: "OppertunityCapabilities",
                column: "OppertunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_OppertunityCompanies_SavedOppertunitiesId",
                table: "OppertunityCompanies",
                column: "SavedOppertunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_OppertunityIndustries_OppertunitiesId",
                table: "OppertunityIndustries",
                column: "OppertunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_OppertunityServiceTypes_ServiceTypesId",
                table: "OppertunityServiceTypes",
                column: "ServiceTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Oppertunities_OppertunityId",
                table: "Message",
                column: "OppertunityId",
                principalTable: "Oppertunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
