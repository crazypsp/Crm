using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Crm.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create_With_Fixed_Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("0cd0d13b-79a9-47b6-aebb-6cdd159396e6"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("7b551db5-19bf-452c-8a64-9872d62e71f6"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("92beec81-d246-4e3c-afde-896f5cddc767"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "ConcurrencyStamp",
                value: "admin-concurrency-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "ConcurrencyStamp",
                value: "dealer-concurrency-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                column: "ConcurrencyStamp",
                value: "accountant-concurrency-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ConcurrencyStamp",
                value: "staff-concurrency-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
                column: "ConcurrencyStamp",
                value: "company-concurrency-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
                column: "ConcurrencyStamp",
                value: "subdealer-concurrency-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555551"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "admin-user-concurrency-stamp", "AQAAAAEAACcQAAAAEHRDILxLq7nLd5AL3KJQcHlL4jNQ2hTp9hK5mR8vJ1kKjHqYtZwXvB2nMjPpLk3Gzg==", "11111111-1111-1111-1111-111111111111" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555553"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "accountant-user-concurrency-stamp", "AQAAAAEAACcQAAAAEHRDILxLq7nLd5AL3KJQcHlL4jNQ2hTp9hK5mR8vJ1kKjHqYtZwXvB2nMjPpLk3Gzg==", "22222222-2222-2222-2222-222222222222" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "company-user-concurrency-stamp", "AQAAAAEAACcQAAAAEHRDILxLq7nLd5AL3KJQcHlL4jNQ2hTp9hK5mR8vJ1kKjHqYtZwXvB2nMjPpLk3Gzg==", "33333333-3333-3333-3333-333333333333" });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("11111111-1111-1111-1111-111111111111"), null, null, false, true, 1, null, null, null, new Guid("55555555-5555-5555-5555-555555555551") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 3, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("55555555-5555-5555-5555-555555555553") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 5, null, null, null, new Guid("55555555-5555-5555-5555-555555555555") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555551"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b631009f-cd58-4a88-a6d6-07fe6600763a", "AQAAAAIAAYagAAAAED0FsEBEMnfxvBxEdBR0lygSeKBQ32rwlI5h7j7JVXSHc2rSht3FUOf+my83bmYkCg==", "dbbebd80743f42d3a6c052efc062da47" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555553"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "25f324db-b3b2-4ce7-b4c1-ef2dbae5c336", "AQAAAAIAAYagAAAAEAsWQuq2jQW+H2jo7b1mWSXJwXYRclsh0xWIVUdFPKXG7xNUiPYpGAr/0nDaoiJE7A==", "3cef3189f8464dbab39e15d129bd990b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "abe23c33-eadc-4090-b0b8-bce7974f4487", "AQAAAAIAAYagAAAAEPzOD9N9oDPtHlFMGfOKfQNd4pDL90IpiuwsTSj233ZQNDxQ51Zoww+nOOvZDudJgg==", "49c1114f0bdb40249326315d0ee6c198" });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[,]
                {
                    { new Guid("0cd0d13b-79a9-47b6-aebb-6cdd159396e6"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 3, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("55555555-5555-5555-5555-555555555553") },
                    { new Guid("7b551db5-19bf-452c-8a64-9872d62e71f6"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 5, null, null, null, new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("92beec81-d246-4e3c-afde-896f5cddc767"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("11111111-1111-1111-1111-111111111111"), null, null, false, true, 1, null, null, null, new Guid("55555555-5555-5555-5555-555555555551") }
                });
        }
    }
}
