using DogVetAPI.Data.Models;
using DogVetAPI.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data
{
    /// <summary>
    /// Database context for the DogVet application
    /// </summary>
    public class DogVetContext : DbContext
    {
        public DogVetContext(DbContextOptions<DogVetContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<SaleNote> SaleNotes { get; set; }
        public DbSet<SaleNoteConcept> SaleNoteConcepts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Owner entity configuration
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(300);
                entity.Property(e => e.City).HasMaxLength(100);
                
                // One-to-many relationship with Pet
                entity.HasMany(e => e.Pets)
                    .WithOne(p => p.Owner)
                    .HasForeignKey(p => p.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Indexes
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Pet entity configuration
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Breed).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Color).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(20);
                
                // One-to-many relationship with MedicalHistory
                entity.HasMany(e => e.MedicalHistories)
                    .WithOne(m => m.Pet)
                    .HasForeignKey(m => m.PetId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Indexes
                entity.HasIndex(e => e.OwnerId);
            });

            // Veterinarian entity configuration
            modelBuilder.Entity<Veterinarian>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                
                // One-to-many relationship with MedicalHistory
                entity.HasMany(e => e.MedicalHistories)
                    .WithOne(m => m.Veterinarian)
                    .HasForeignKey(m => m.VeterinarianId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Indexes
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // MedicalHistory entity configuration
            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Diagnosis).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Status).HasMaxLength(50);
                
                // One-to-many relationship with Prescription
                entity.HasMany(e => e.Prescriptions)
                    .WithOne(p => p.MedicalHistory)
                    .HasForeignKey(p => p.MedicalHistoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-many relationship with SaleNote
                entity.HasMany(e => e.SaleNotes)
                    .WithOne(s => s.MedicalHistory)
                    .HasForeignKey(s => s.MedicalHistoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.PetId);
                entity.HasIndex(e => e.VeterinarianId);
                entity.HasIndex(e => e.VisitDate);
            });

            // Prescription entity configuration
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MedName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Dose).HasConversion<string>().HasMaxLength(50);
                entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);

                // Indexes
                entity.HasIndex(e => e.MedicalHistoryId);
            });

            // SaleNote entity configuration
            modelBuilder.Entity<SaleNote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentStatus).HasConversion<string>().HasMaxLength(50);

                // One-to-many relationship with SaleNoteConcept
                entity.HasMany(e => e.Concepts)
                    .WithOne(c => c.SaleNote)
                    .HasForeignKey(c => c.SaleNoteId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.MedicalHistoryId);
                entity.HasIndex(e => e.NoteDate);
            });

            // SaleNoteConcept entity configuration
            modelBuilder.Entity<SaleNoteConcept>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(300);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ConceptPrice).HasColumnType("decimal(18,2)");

                // Indexes
                entity.HasIndex(e => e.SaleNoteId);
            });

            // Seed data
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Owner>().HasData(
                new Owner { Id = 1, FirstName = "James",   LastName = "Harrington", Email = "james.harrington@email.com",  PhoneNumber = "555-1001", Address = "123 Maple St",    City = "Austin",    CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 2, FirstName = "Sophia",  LastName = "Caldwell",   Email = "sophia.caldwell@email.com",   PhoneNumber = "555-1002", Address = "456 Oak Ave",     City = "Dallas",    CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 3, FirstName = "Marcus",  LastName = "Nguyen",     Email = "marcus.nguyen@email.com",     PhoneNumber = "555-1003", Address = "789 Pine Rd",     City = "Houston",   CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 4, FirstName = "Elena",   LastName = "Rossi",      Email = "elena.rossi@email.com",       PhoneNumber = "555-1004", Address = "321 Birch Blvd",  City = "San Antonio", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { Id = 5, FirstName = "Daniel",  LastName = "Fitzgerald", Email = "daniel.fitzgerald@email.com", PhoneNumber = "555-1005", Address = "654 Cedar Ln",    City = "Austin",    CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<Veterinarian>().HasData(
                new Veterinarian { Id = 1, FirstName = "Laura",   LastName = "Bennett",  Email = "laura.bennett@dogvet.com",  PhoneNumber = "555-2001", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 2, FirstName = "Carlos",  LastName = "Morales",  Email = "carlos.morales@dogvet.com", PhoneNumber = "555-2002", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 3, FirstName = "Natalie", LastName = "Thornton", Email = "natalie.thornton@dogvet.com", PhoneNumber = "555-2003", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 4, FirstName = "Kevin",   LastName = "Patel",    Email = "kevin.patel@dogvet.com",    PhoneNumber = "555-2004", IsActive = false, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { Id = 5, FirstName = "Diana",   LastName = "Cruz",     Email = "diana.cruz@dogvet.com",     PhoneNumber = "555-2005", IsActive = true,  CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<Pet>().HasData(
                new Pet { Id = 1, Name = "Buddy",   Breed = "Golden Retriever", Age = 3, Weight = 30.5, Color = "Golden",      Gender = "Male",   DateOfBirth = new DateTime(2023, 3, 10, 0, 0, 0, DateTimeKind.Utc), IsActive = true,  OwnerId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 2, Name = "Bella",   Breed = "Labrador",         Age = 2, Weight = 25.0, Color = "Black",       Gender = "Female", DateOfBirth = new DateTime(2024, 6, 5, 0, 0, 0, DateTimeKind.Utc),  IsActive = true,  OwnerId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 3, Name = "Max",     Breed = "Beagle",           Age = 5, Weight = 12.3, Color = "Tricolor",    Gender = "Male",   DateOfBirth = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true,  OwnerId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 4, Name = "Luna",    Breed = "Poodle",           Age = 1, Weight = 8.7,  Color = "White",       Gender = "Female", DateOfBirth = new DateTime(2025, 2, 14, 0, 0, 0, DateTimeKind.Utc), IsActive = true,  OwnerId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Id = 5, Name = "Charlie", Breed = "German Shepherd",  Age = 4, Weight = 35.2, Color = "Black & Tan", Gender = "Male",   DateOfBirth = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc),  IsActive = false, OwnerId = 5, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<MedicalHistory>().HasData(
                new MedicalHistory { Id = 1, Diagnosis = "Annual wellness check",         Notes = "All vitals normal, vaccines up to date",         VisitDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc),  FollowUpDate = null,                                                    Status = "Completed", PetId = 1, VeterinarianId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 2, Diagnosis = "Ear infection – otitis externa", Notes = "Mild inflammation, prescribed ear drops",        VisitDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 24, 0, 0, 0, DateTimeKind.Utc), Status = "Completed", PetId = 2, VeterinarianId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 3, Diagnosis = "Skin allergy – atopic dermatitis", Notes = "Seasonal allergies, antihistamines prescribed", VisitDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Pending",   PetId = 3, VeterinarianId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 4, Diagnosis = "Routine vaccination",            Notes = "Rabies and DHPP vaccines administered",          VisitDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null,                                                    Status = "Completed", PetId = 4, VeterinarianId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Id = 5, Diagnosis = "Limping – left hind leg",       Notes = "X-ray ordered, suspected minor sprain",          VisitDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc),  Status = "Pending",   PetId = 5, VeterinarianId = 1, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<Prescription>().HasData(
                new Prescription { Id = 1, MedName = "Amoxicillin",    Dose = DoseFrequency.Every12Hours, DurationInDays = 10, Status = PrescriptionStatus.Administered, MedicalHistoryId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 2, MedName = "Cetirizine",     Dose = DoseFrequency.Daily,        DurationInDays = 30, Status = PrescriptionStatus.Prescribed,   MedicalHistoryId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 3, MedName = "Meloxicam",      Dose = DoseFrequency.Daily,        DurationInDays = 7,  Status = PrescriptionStatus.Prescribed,   MedicalHistoryId = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 4, MedName = "Enrofloxacin",   Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Administered, MedicalHistoryId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { Id = 5, MedName = "Prednisone",     Dose = DoseFrequency.Every8Hours,  DurationInDays = 5,  Status = PrescriptionStatus.Prescribed,   MedicalHistoryId = 3, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            modelBuilder.Entity<SaleNote>().HasData(
                new SaleNote { Id = 1, NoteDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc),  TotalAmount = 120.00m, PaymentStatus = PaymentStatus.Paid,            MedicalHistoryId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 2, NoteDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 85.50m,  PaymentStatus = PaymentStatus.Paid,            MedicalHistoryId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 3, NoteDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 210.00m, PaymentStatus = PaymentStatus.Pending,         MedicalHistoryId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 4, NoteDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 95.00m,  PaymentStatus = PaymentStatus.Paid,            MedicalHistoryId = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { Id = 5, NoteDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 175.75m, PaymentStatus = PaymentStatus.Pending,         MedicalHistoryId = 5, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

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
        }
    }
}
