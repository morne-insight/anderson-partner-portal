using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndersonAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyEmbedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmbeddingDim",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbeddingModel",
                table: "Companies",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EmbeddingUpdated",
                table: "Companies",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbeddingDim",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EmbeddingModel",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EmbeddingUpdated",
                table: "Companies");
        }
    }
}
