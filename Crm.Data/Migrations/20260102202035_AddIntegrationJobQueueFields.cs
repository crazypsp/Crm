using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIntegrationJobQueueFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "IntegrationJobs");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "IntegrationJobs",
                newName: "LockedUntil");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "IntegrationJobs",
                newName: "LastError");

            migrationBuilder.AlterColumn<string>(
                name: "ResultJson",
                table: "IntegrationJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayloadJson",
                table: "IntegrationJobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "CommandType",
                table: "IntegrationJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "IntegrationJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LockedBy",
                table: "IntegrationJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "IntegrationJobs");

            migrationBuilder.DropColumn(
                name: "LockedBy",
                table: "IntegrationJobs");

            migrationBuilder.RenameColumn(
                name: "LockedUntil",
                table: "IntegrationJobs",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "LastError",
                table: "IntegrationJobs",
                newName: "ErrorMessage");

            migrationBuilder.AlterColumn<string>(
                name: "ResultJson",
                table: "IntegrationJobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PayloadJson",
                table: "IntegrationJobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CommandType",
                table: "IntegrationJobs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedAt",
                table: "IntegrationJobs",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
