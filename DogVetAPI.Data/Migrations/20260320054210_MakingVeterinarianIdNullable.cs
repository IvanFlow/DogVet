using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DogVetAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakingVeterinarianIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5);

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

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SaleNoteConcepts",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SaleNotes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SaleNotes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SaleNotes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SaleNotes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SaleNotes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MedicalHistories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalHistories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicalHistories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MedicalHistories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MedicalHistories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Veterinarians",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<int>(
                name: "VeterinarianId",
                table: "MedicalHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VeterinarianId",
                table: "MedicalHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "Email", "FirstName", "LastName", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "123 Maple St", "Austin", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "james.harrington@email.com", "James", "Harrington", "555-1001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "456 Oak Ave", "Dallas", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sophia.caldwell@email.com", "Sophia", "Caldwell", "555-1002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "789 Pine Rd", "Houston", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "marcus.nguyen@email.com", "Marcus", "Nguyen", "555-1003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "321 Birch Blvd", "San Antonio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "elena.rossi@email.com", "Elena", "Rossi", "555-1004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "654 Cedar Ln", "Austin", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "daniel.fitzgerald@email.com", "Daniel", "Fitzgerald", "555-1005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Veterinarians",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "laura.bennett@dogvet.com", "Laura", true, "Bennett", "555-2001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "carlos.morales@dogvet.com", "Carlos", true, "Morales", "555-2002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "natalie.thornton@dogvet.com", "Natalie", true, "Thornton", "555-2003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "kevin.patel@dogvet.com", "Kevin", false, "Patel", "555-2004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "diana.cruz@dogvet.com", "Diana", true, "Cruz", "555-2005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Age", "Breed", "Color", "CreatedAt", "DateOfBirth", "Gender", "IsActive", "Name", "OwnerId", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { 1, 3, "Golden Retriever", "Golden", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Male", true, "Buddy", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30.5 },
                    { 2, 2, "Labrador", "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Female", true, "Bella", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25.0 },
                    { 3, 5, "Beagle", "Tricolor", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Male", true, "Max", 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12.300000000000001 },
                    { 4, 1, "Poodle", "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Female", true, "Luna", 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8.6999999999999993 },
                    { 5, 4, "German Shepherd", "Black & Tan", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Male", false, "Charlie", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 35.200000000000003 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedAt", "Date", "OwnerId", "PetId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Scheduled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 14, 30, 0, 0, DateTimeKind.Utc), 2, 2, "Scheduled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 8, 16, 0, 0, 0, DateTimeKind.Utc), 3, 3, "Scheduled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 12, 11, 0, 0, 0, DateTimeKind.Utc), 1, 2, "Scheduled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 30, 9, 0, 0, 0, DateTimeKind.Utc), 3, 3, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 28, 13, 30, 0, 0, DateTimeKind.Utc), 2, 2, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 25, 10, 30, 0, 0, DateTimeKind.Utc), 4, 4, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 20, 15, 0, 0, 0, DateTimeKind.Utc), 5, 5, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 11, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 11, 15, 0, 0, DateTimeKind.Utc), 4, 4, "Cancelled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 7, 14, 0, 0, 0, DateTimeKind.Utc), 2, 4, "Cancelled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc), 5, 1, "Cancelled", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "MedicalHistories",
                columns: new[] { "Id", "CreatedAt", "Diagnosis", "FollowUpDate", "Notes", "PetId", "Status", "UpdatedAt", "VeterinarianId", "VisitDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Annual wellness check", null, "All vitals normal, vaccines up to date", 1, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ear infection – otitis externa", new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Mild inflammation, prescribed ear drops", 2, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Skin allergy – atopic dermatitis", new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Seasonal allergies, antihistamines prescribed", 3, "Pending", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Routine vaccination", null, "Rabies and DHPP vaccines administered", 4, "Completed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Limping – left hind leg", new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "X-ray ordered, suspected minor sprain", 5, "Pending", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Prescriptions",
                columns: new[] { "Id", "CreatedAt", "Dose", "DurationInDays", "MedName", "MedicalHistoryId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Every12Hours", 10, "Amoxicillin", 2, "Administered", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Daily", 30, "Cetirizine", 3, "Prescribed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Daily", 7, "Meloxicam", 5, "Prescribed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Every12Hours", 14, "Enrofloxacin", 1, "Administered", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Every8Hours", 5, "Prednisone", 3, "Prescribed", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SaleNotes",
                columns: new[] { "Id", "CreatedAt", "MedicalHistoryId", "NoteDate", "PaymentStatus", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Paid", 120.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Paid", 85.50m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", 210.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Paid", 95.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Pending", 175.75m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SaleNoteConcepts",
                columns: new[] { "Id", "ConceptPrice", "CreatedAt", "Description", "Quantity", "SaleNoteId", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consultation fee", 1, 1, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vaccine – DHPP", 1, 1, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consultation fee", 1, 2, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 25.50m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ear drops – Otomax", 1, 2, 25.50m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consultation fee", 1, 3, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, 90.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allergy skin test", 1, 3, 90.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cetirizine 10mg x30", 1, 3, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consultation fee", 1, 4, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 35.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rabies vaccine", 1, 4, 35.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consultation fee", 1, 5, 60.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, 85.75m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "X-ray – hind leg", 1, 5, 85.75m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, 30.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Meloxicam 1mg x7", 1, 5, 30.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
