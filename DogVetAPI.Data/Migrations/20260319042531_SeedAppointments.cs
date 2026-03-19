using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DogVetAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Status" },
                values: new object[] { new DateTime(2026, 2, 8, 16, 0, 0, 0, DateTimeKind.Utc), "Scheduled" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "OwnerId", "PetId", "Status" },
                values: new object[] { new DateTime(2026, 2, 12, 11, 0, 0, 0, DateTimeKind.Utc), 1, 2, "Scheduled" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "OwnerId", "PetId", "Status" },
                values: new object[] { new DateTime(2026, 1, 30, 9, 0, 0, 0, DateTimeKind.Utc), 3, 3, "Completed" });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedAt", "Date", "OwnerId", "PetId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 28, 13, 30, 0, 0, DateTimeKind.Utc), 2, 2, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 25, 10, 30, 0, 0, DateTimeKind.Utc), 4, 4, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 20, 15, 0, 0, 0, DateTimeKind.Utc), 5, 5, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 11, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 11, 15, 0, 0, DateTimeKind.Utc), 4, 4, "Cancelled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 14, 0, 0, 0, DateTimeKind.Utc), 2, 4, "Cancelled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc), 5, 1, "Cancelled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Status" },
                values: new object[] { new DateTime(2026, 1, 30, 9, 0, 0, 0, DateTimeKind.Utc), "Completed" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "OwnerId", "PetId", "Status" },
                values: new object[] { new DateTime(2026, 2, 10, 11, 15, 0, 0, DateTimeKind.Utc), 4, 4, "Cancelled" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "OwnerId", "PetId", "Status" },
                values: new object[] { new DateTime(2026, 2, 3, 15, 45, 0, 0, DateTimeKind.Utc), 5, 5, "Scheduled" });
        }
    }
}
