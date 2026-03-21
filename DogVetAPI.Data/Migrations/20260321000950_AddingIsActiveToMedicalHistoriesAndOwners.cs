using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogVetAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsActiveToMedicalHistoriesAndOwners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Owners",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_IsActive",
                table: "Pets",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_IsActive",
                table: "Owners",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_IsActive",
                table: "MedicalHistories",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pets_IsActive",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Owners_IsActive",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_IsActive",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MedicalHistories");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Pets",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
