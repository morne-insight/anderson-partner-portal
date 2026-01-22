using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndersonAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuarterlyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Quarter",
                table: "Quarterlies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SubmittedDate",
                table: "Quarterlies",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Quarterlies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "quarterly_quarter_check",
                table: "Quarterlies",
                sql: "\"Quarter\" IN ('Q1','Q2','Q3','Q4')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "quarterly_quarter_check",
                table: "Quarterlies");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "Quarterlies");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Quarterlies");

            migrationBuilder.AlterColumn<int>(
                name: "Quarter",
                table: "Quarterlies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
