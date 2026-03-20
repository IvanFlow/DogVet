using DogVetAPI.Data.Models;
using DogVetAPI.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.DBContext
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
        public DbSet<Appointment> Appointments { get; set; }

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
                // SetNull: deleting a veterinarian nullifies VeterinarianId in their records
                entity.HasMany(e => e.MedicalHistories)
                    .WithOne(m => m.Veterinarian)
                    .HasForeignKey(m => m.VeterinarianId)
                    .OnDelete(DeleteBehavior.SetNull);
                
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
                
                // Foreign key configuration - VeterinarianId is optional
                entity.Property(e => e.VeterinarianId).IsRequired(false);
                
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

            // Appointment entity configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);

                // One-to-many relationships
                // Cascade: deleting an owner removes their appointments at DB level
                entity.HasOne(e => e.Owner)
                    .WithMany(o => o.Appointments)
                    .HasForeignKey(e => e.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ClientCascade: EF handles deletion before DB to avoid multiple cascade paths
                // (Owner→Pets→Appointments AND Owner→Appointments would violate SQL Server restriction)
                entity.HasOne(e => e.Pet)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(e => e.PetId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                // Indexes
                entity.HasIndex(e => e.Date);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.OwnerId);
                entity.HasIndex(e => e.PetId);
            });
        }
    }
}
