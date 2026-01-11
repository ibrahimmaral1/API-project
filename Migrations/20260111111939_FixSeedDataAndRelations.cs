using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net9ApiOdev.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 11, 11, 16, 4, 994, DateTimeKind.Utc).AddTicks(9227), new DateTime(2026, 1, 11, 11, 16, 4, 994, DateTimeKind.Utc).AddTicks(9421) });
        }
    }
}
