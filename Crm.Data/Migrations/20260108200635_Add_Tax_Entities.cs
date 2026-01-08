using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Crm.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tax_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("52caa0f3-e746-4f91-8be3-cfe7279768d3"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("61212e3a-7af6-43b7-a41b-1acc2ae29ecd"));

            migrationBuilder.DeleteData(
                table: "UserMemberships",
                keyColumn: "Id",
                keyValue: new Guid("9d002bed-af85-49e5-9d3a-b96f1202e823"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555551"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78b1196f-d41f-4ca0-b40d-43579c54739e", "AQAAAAIAAYagAAAAEEhwbvVf5k28g3MsgJzIYwDf0/VQjWHkZn/nXJZm+Tux7BCWim5ZEj0YY30WQOlI/Q==", "18a08c74c6a348fdbd0f168f0a15d5d3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555553"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0e99fa4-0628-45d3-8af2-4a05f37bf664", "AQAAAAIAAYagAAAAEE2/UJNSf7LDahcKJNsnhSbkzEKqj/ALGvFd5P9LPBBgyril7R0JFSeQkU7z6SHHpA==", "bb49fb2de40444858660ddeac70e5405" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eb7085aa-c0a0-4508-aee7-f6093775d91a", "AQAAAAIAAYagAAAAECPZRHf/BBv7w17N/a5U5BLdTtxSeXMdXleAZVxwgpsYEDBIUyiMtsQXTlItK/Hygg==", "ef04deadee6342b991365002f11646d9" });

            migrationBuilder.InsertData(
                table: "UserMemberships",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedByUserId", "DealerId", "DeletedAt", "DeletedByUserId", "IsDeleted", "IsPrimary", "Role", "TenantId", "UpdatedAt", "UpdatedByUserId", "UserId" },
                values: new object[,]
                {
                    { new Guid("52caa0f3-e746-4f91-8be3-cfe7279768d3"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 5, null, null, null, new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("61212e3a-7af6-43b7-a41b-1acc2ae29ecd"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("11111111-1111-1111-1111-111111111111"), null, null, false, true, 1, null, null, null, new Guid("55555555-5555-5555-5555-555555555551") },
                    { new Guid("9d002bed-af85-49e5-9d3a-b96f1202e823"), null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, false, true, 3, new Guid("22222222-2222-2222-2222-222222222222"), null, null, new Guid("55555555-5555-5555-5555-555555555553") }
                });
        }
    }
}
