using DogVetAPI.Data.Models;
using DogVetAPI.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.DBContext
{
    /// <summary>
    /// Extension methods for seeding the database
    /// </summary>
    public static class SeedDataExtension
    {
        /// <summary>
        /// Add all seed data to the model builder
        /// </summary>
        public static ModelBuilder SeedAllData(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.SeedOwners(seedDate);
            modelBuilder.SeedVeterinarians(seedDate);
            modelBuilder.SeedPets(seedDate);
            modelBuilder.SeedMedicalHistories(seedDate);
            modelBuilder.SeedPrescriptions(seedDate);
            modelBuilder.SeedSaleNotes(seedDate);
            modelBuilder.SeedSaleNoteConcepts(seedDate);
            modelBuilder.SeedAppointments(seedDate);

            return modelBuilder;
        }

        /// <summary>
        /// Seed owners table
        /// </summary>
        private static ModelBuilder SeedOwners(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<Owner>().HasData(
                new Owner { Id = 1, FirstName = "James",   LastName = "Harrington", Email = "james.harrington@email.com",  PhoneNumber = "555-1001", Address = "123 Maple St",    City = "Austin",    CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 2, FirstName = "Sophia",  LastName = "Caldwell",   Email = "sophia.caldwell@email.com",   PhoneNumber = "555-1002", Address = "456 Oak Ave",     City = "Dallas",    CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 3, FirstName = "Marcus",  LastName = "Nguyen",     Email = "marcus.nguyen@email.com",     PhoneNumber = "555-1003", Address = "789 Pine Rd",     City = "Houston",   CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 4, FirstName = "Elena",   LastName = "Rossi",      Email = "elena.rossi@email.com",       PhoneNumber = "555-1004", Address = "321 Birch Blvd",  City = "San Antonio", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 5, FirstName = "Daniel",  LastName = "Fitzgerald", Email = "daniel.fitzgerald@email.com", PhoneNumber = "555-1005", Address = "654 Cedar Ln",    City = "Austin",    CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed veterinarians table
        /// </summary>
        private static ModelBuilder SeedVeterinarians(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<Veterinarian>().HasData(
                new Veterinarian { Id = 1, FirstName = "Laura",   LastName = "Bennett",  Email = "laura.bennett@dogvet.com",  PhoneNumber = "555-2001", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 2, FirstName = "Carlos",  LastName = "Morales",  Email = "carlos.morales@dogvet.com", PhoneNumber = "555-2002", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 3, FirstName = "Natalie", LastName = "Thornton", Email = "natalie.thornton@dogvet.com", PhoneNumber = "555-2003", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 4, FirstName = "Kevin",   LastName = "Patel",    Email = "kevin.patel@dogvet.com",    PhoneNumber = "555-2004", IsActive = false, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 5, FirstName = "Diana",   LastName = "Cruz",     Email = "diana.cruz@dogvet.com",     PhoneNumber = "555-2005", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed pets table
        /// </summary>
        private static ModelBuilder SeedPets(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<Pet>().HasData(
                new Pet { Id = 1, Name = "Buddy",   Breed = "Golden Retriever", Age = 3, Weight = 30.5, Color = "Golden",      Gender = "Male",   DateOfBirth = new DateTime(2023, 3, 10, 0, 0, 0, DateTimeKind.Utc), IsActive = true,  OwnerId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 2, Name = "Bella",   Breed = "Labrador",         Age = 2, Weight = 25.0, Color = "Black",       Gender = "Female", DateOfBirth = new DateTime(2024, 6, 5, 0, 0, 0, DateTimeKind.Utc),  IsActive = true,  OwnerId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 3, Name = "Max",     Breed = "Beagle",           Age = 5, Weight = 12.3, Color = "Tricolor",    Gender = "Male",   DateOfBirth = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true,  OwnerId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 4, Name = "Luna",    Breed = "Poodle",           Age = 1, Weight = 8.7,  Color = "White",       Gender = "Female", DateOfBirth = new DateTime(2025, 2, 14, 0, 0, 0, DateTimeKind.Utc), IsActive = true,  OwnerId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 5, Name = "Charlie", Breed = "German Shepherd",  Age = 4, Weight = 35.2, Color = "Black & Tan", Gender = "Male",   DateOfBirth = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc),  IsActive = false, OwnerId = 5, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed medical histories table
        /// </summary>
        private static ModelBuilder SeedMedicalHistories(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<MedicalHistory>().HasData(
                new MedicalHistory { Id = 1, Diagnosis = "Annual wellness check",         Notes = "All vitals normal, vaccines up to date",         VisitDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc),  FollowUpDate = null,                                                    Status = "Completed", PetId = 1, VeterinarianId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 2, Diagnosis = "Ear infection – otitis externa", Notes = "Mild inflammation, prescribed ear drops",        VisitDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 24, 0, 0, 0, DateTimeKind.Utc), Status = "Completed", PetId = 2, VeterinarianId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 3, Diagnosis = "Skin allergy – atopic dermatitis", Notes = "Seasonal allergies, antihistamines prescribed", VisitDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Pending",   PetId = 3, VeterinarianId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 4, Diagnosis = "Routine vaccination",            Notes = "Rabies and DHPP vaccines administered",          VisitDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null,                                                    Status = "Completed", PetId = 4, VeterinarianId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 5, Diagnosis = "Limping – left hind leg",       Notes = "X-ray ordered, suspected minor sprain",          VisitDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc),  Status = "Pending",   PetId = 5, VeterinarianId = 1, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed prescriptions table
        /// </summary>
        private static ModelBuilder SeedPrescriptions(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<Prescription>().HasData(
                new Prescription { Id = 1, MedName = "Amoxicillin",    Dose = DoseFrequency.Every12Hours, DurationInDays = 10, Status = PrescriptionStatus.Administered, MedicalHistoryId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 2, MedName = "Cetirizine",     Dose = DoseFrequency.Daily,        DurationInDays = 30, Status = PrescriptionStatus.Prescribed,   MedicalHistoryId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 3, MedName = "Meloxicam",      Dose = DoseFrequency.Daily,        DurationInDays = 7,  Status = PrescriptionStatus.Prescribed,   MedicalHistoryId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 4, MedName = "Enrofloxacin",   Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Administered, MedicalHistoryId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 5, MedName = "Prednisone",     Dose = DoseFrequency.Every8Hours,  DurationInDays = 5,  Status = PrescriptionStatus.Prescribed,   MedicalHistoryId = 3, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed sale notes table
        /// </summary>
        private static ModelBuilder SeedSaleNotes(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<SaleNote>().HasData(
                new SaleNote { Id = 1, NoteDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc),  TotalAmount = 120.00m, PaymentStatus = PaymentStatus.Paid,            MedicalHistoryId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 2, NoteDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 85.50m,  PaymentStatus = PaymentStatus.Paid,            MedicalHistoryId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 3, NoteDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 210.00m, PaymentStatus = PaymentStatus.Pending,         MedicalHistoryId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 4, NoteDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 95.00m,  PaymentStatus = PaymentStatus.Paid,            MedicalHistoryId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 5, NoteDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 175.75m, PaymentStatus = PaymentStatus.Pending,         MedicalHistoryId = 5, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed sale note concepts table
        /// </summary>
        private static ModelBuilder SeedSaleNoteConcepts(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<SaleNoteConcept>().HasData(
                new SaleNoteConcept { Id = 1,  Description = "Consultation fee",      Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 2,  Description = "Vaccine – DHPP",        Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 3,  Description = "Consultation fee",      Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 4,  Description = "Ear drops – Otomax",   Quantity = 1, UnitPrice = 25.50m,  ConceptPrice = 25.50m,  SaleNoteId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 5,  Description = "Consultation fee",      Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 6,  Description = "Allergy skin test",     Quantity = 1, UnitPrice = 90.00m,  ConceptPrice = 90.00m,  SaleNoteId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 7,  Description = "Cetirizine 10mg x30",  Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 8,  Description = "Consultation fee",      Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 9,  Description = "Rabies vaccine",        Quantity = 1, UnitPrice = 35.00m,  ConceptPrice = 35.00m,  SaleNoteId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 10, Description = "Consultation fee",      Quantity = 1, UnitPrice = 60.00m,  ConceptPrice = 60.00m,  SaleNoteId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 11, Description = "X-ray – hind leg",      Quantity = 1, UnitPrice = 85.75m,  ConceptPrice = 85.75m,  SaleNoteId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Id = 12, Description = "Meloxicam 1mg x7",      Quantity = 1, UnitPrice = 30.00m,  ConceptPrice = 30.00m,  SaleNoteId = 5, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }

        /// <summary>
        /// Seed appointments table with different statuses and scenarios
        /// </summary>
        private static ModelBuilder SeedAppointments(this ModelBuilder modelBuilder, DateTime seedDate)
        {
            modelBuilder.Entity<Appointment>().HasData(
                // Scheduled appointments
                new Appointment { Id = 1, Date = new DateTime(2026, 2, 1, 10, 0, 0, DateTimeKind.Utc),   Status = AppointmentStatus.Scheduled, OwnerId = 1, PetId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 2, Date = new DateTime(2026, 2, 5, 14, 30, 0, DateTimeKind.Utc),  Status = AppointmentStatus.Scheduled, OwnerId = 2, PetId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 3, Date = new DateTime(2026, 2, 8, 16, 0, 0, DateTimeKind.Utc),   Status = AppointmentStatus.Scheduled, OwnerId = 3, PetId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 4, Date = new DateTime(2026, 2, 12, 11, 0, 0, DateTimeKind.Utc),  Status = AppointmentStatus.Scheduled, OwnerId = 1, PetId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Completed appointments
                new Appointment { Id = 5, Date = new DateTime(2026, 1, 30, 9, 0, 0, DateTimeKind.Utc),   Status = AppointmentStatus.Completed, OwnerId = 3, PetId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 6, Date = new DateTime(2026, 1, 28, 13, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, OwnerId = 2, PetId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 7, Date = new DateTime(2026, 1, 25, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, OwnerId = 4, PetId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 8, Date = new DateTime(2026, 1, 20, 15, 0, 0, DateTimeKind.Utc),  Status = AppointmentStatus.Completed, OwnerId = 5, PetId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 9, Date = new DateTime(2026, 1, 15, 11, 0, 0, DateTimeKind.Utc),  Status = AppointmentStatus.Completed, OwnerId = 1, PetId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Cancelled appointments
                new Appointment { Id = 10, Date = new DateTime(2026, 2, 10, 11, 15, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, OwnerId = 4, PetId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 11, Date = new DateTime(2026, 2, 7, 14, 0, 0, DateTimeKind.Utc),  Status = AppointmentStatus.Cancelled, OwnerId = 2, PetId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Id = 12, Date = new DateTime(2026, 2, 3, 10, 0, 0, DateTimeKind.Utc),  Status = AppointmentStatus.Cancelled, OwnerId = 5, PetId = 1, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
            return modelBuilder;
        }
    }
}
