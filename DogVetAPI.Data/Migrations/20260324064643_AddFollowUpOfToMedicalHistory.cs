using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogVetAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowUpOfToMedicalHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FollowUpOf",
                table: "MedicalHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_FollowUpOf",
                table: "MedicalHistories",
                column: "FollowUpOf",
                unique: true,
                filter: "[FollowUpOf] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_MedicalHistories_FollowUpOf",
                table: "MedicalHistories",
                column: "FollowUpOf",
                principalTable: "MedicalHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_MedicalHistories_FollowUpOf",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_FollowUpOf",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "FollowUpOf",
                table: "MedicalHistories");
        }
    }
}
