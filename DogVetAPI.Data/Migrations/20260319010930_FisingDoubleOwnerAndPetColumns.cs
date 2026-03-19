using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogVetAPI.Migrations
{
    /// <inheritdoc />
    public partial class FisingDoubleOwnerAndPetColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Owners_OwnerId1",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Pets_PetId1",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OwnerId1",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PetId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PetId1",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId1",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId1",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OwnerId1", "PetId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "OwnerId1", "PetId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "OwnerId1", "PetId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "OwnerId1", "PetId1" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "OwnerId1", "PetId1" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OwnerId1",
                table: "Appointments",
                column: "OwnerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PetId1",
                table: "Appointments",
                column: "PetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Owners_OwnerId1",
                table: "Appointments",
                column: "OwnerId1",
                principalTable: "Owners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Pets_PetId1",
                table: "Appointments",
                column: "PetId1",
                principalTable: "Pets",
                principalColumn: "Id");
        }
    }
}
