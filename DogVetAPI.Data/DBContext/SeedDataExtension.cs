using DogVetAPI.Data.Models;
using DogVetAPI.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.DBContext
{
    /// <summary>
    /// Extension methods for seeding the database with comprehensive data
    /// All relationships use object references, not explicit IDs
    /// </summary>
    public static class SeedDataExtension
    {
        /// <summary>
        /// Seed all data to the database (use this when database is already created)
        /// </summary>
        public static async Task SeedAllDataAsync(this DogVetContext context)
        {
            // Clear all tables in reverse dependency order before seeding
            context.SaleNoteConcepts.RemoveRange(context.SaleNoteConcepts);
            context.SaleNotes.RemoveRange(context.SaleNotes);
            context.Prescriptions.RemoveRange(context.Prescriptions);
            context.Appointments.RemoveRange(context.Appointments);
            context.MedicalHistories.RemoveRange(context.MedicalHistories);
            context.Pets.RemoveRange(context.Pets);
            context.Veterinarians.RemoveRange(context.Veterinarians);
            context.Owners.RemoveRange(context.Owners);
            await context.SaveChangesAsync();

            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Seed in dependency order, building relationships with objects
            var owners = SeedOwners(context, seedDate);
            var veterinarians = SeedVeterinarians(context, seedDate);
            var pets = SeedPets(context, owners, seedDate);
            var medicalHistories = SeedMedicalHistories(context, pets, veterinarians, seedDate);
            var prescriptions = SeedPrescriptions(context, medicalHistories, seedDate);
            var saleNotes = SeedSaleNotes(context, medicalHistories, seedDate);
            var saleConcepts = SeedSaleNoteConcepts(context, saleNotes, seedDate);
            var appointments = SeedAppointments(context, owners, pets, seedDate);
            
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Seed owners table with diverse scenarios
        /// </summary>
        private static List<Owner> SeedOwners(DogVetContext context, DateTime seedDate)
        {
            var owners = new List<Owner>
            {
                new Owner { FirstName = "Carlos", LastName = "Gutierrez Ramirez", Email = "carlos.gutierrez@gmail.com", PhoneNumber = "33-1001-2345", Address = "Av. Patria 1230, Col. Jardines de Guadalupe", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { FirstName = "Maria", LastName = "Lopez Hernandez", Email = "maria.lopez@gmail.com", PhoneNumber = "33-1002-3456", Address = "Calle Robles 456, Col. La Calma", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { FirstName = "Jose", LastName = "Martinez Torres", Email = "jose.martinez@hotmail.com", PhoneNumber = "33-1003-4567", Address = "Blvd. Puerta de Hierro 789, Col. Puerta de Hierro", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { FirstName = "Ana", LastName = "Sanchez Flores", Email = "ana.sanchez@gmail.com", PhoneNumber = "33-1004-5678", Address = "Calle Tesistan 321, Col. Lomas de Zapopan", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { FirstName = "Luis", LastName = "Perez Mendoza", Email = "luis.perez@outlook.com", PhoneNumber = "33-1005-6789", Address = "Av. Vallarta 4850, Col. Camino Real", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Owner { FirstName = "Gabriela", LastName = "Reyes Vargas", Email = "gabriela.reyes@gmail.com", PhoneNumber = "33-1006-7890", Address = "Calle Andares 150, Col. Andares", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Owners.AddRange(owners);
            return owners;
        }

        /// <summary>
        /// Seed veterinarians table with different availability statuses
        /// </summary>
        private static List<Veterinarian> SeedVeterinarians(DogVetContext context, DateTime seedDate)
        {
            var vets = new List<Veterinarian>
            {
                new Veterinarian { FirstName = "Laura", LastName = "Castillo Medina", Email = "laura.castillo@dogvet.com", PhoneNumber = "33-2001-1111", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Roberto", LastName = "Morales Jimenez", Email = "roberto.morales@dogvet.com", PhoneNumber = "33-2002-2222", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Patricia", LastName = "Ochoa Navarro", Email = "patricia.ochoa@dogvet.com", PhoneNumber = "33-2003-3333", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Eduardo", LastName = "Ibarra Solis", Email = "eduardo.ibarra@dogvet.com", PhoneNumber = "33-2004-4444", IsActive = false, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Diana", LastName = "Cruz Aguilar", Email = "diana.cruz@dogvet.com", PhoneNumber = "33-2005-5555", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Veterinarians.AddRange(vets);
            return vets;
        }

        /// <summary>
        /// Seed pets table with owner relationships (passing Owner objects, not IDs)
        /// </summary>
        private static List<Pet> SeedPets(DogVetContext context, List<Owner> owners, DateTime seedDate)
        {
            var pets = new List<Pet>
            {
                new Pet { Name = "Canelo", Breed = "Golden Retriever", Age = 3, Weight = 30.5, Color = "Dorado", Gender = "Macho", DateOfBirth = new DateTime(2023, 3, 10, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Negra", Breed = "Labrador", Age = 2, Weight = 25.0, Color = "Negro", Gender = "Hembra", DateOfBirth = new DateTime(2024, 6, 5, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Titan", Breed = "Beagle", Age = 5, Weight = 12.3, Color = "Tricolor", Gender = "Macho", DateOfBirth = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Luna", Breed = "Poodle", Age = 1, Weight = 8.7, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2025, 2, 14, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Thor", Breed = "Pastor Aleman", Age = 4, Weight = 35.2, Color = "Negro con cafe", Gender = "Macho", DateOfBirth = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = false, Owner = owners[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Gordo", Breed = "Boxer", Age = 3, Weight = 28.0, Color = "Bayo", Gender = "Macho", DateOfBirth = new DateTime(2023, 5, 12, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Princesa", Breed = "Cocker Spaniel", Age = 2, Weight = 14.5, Color = "Dorado", Gender = "Hembra", DateOfBirth = new DateTime(2024, 4, 8, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Pets.AddRange(pets);
            return pets;
        }

        /// <summary>
        /// Seed medical histories with pet and veterinarian relationships
        /// </summary>
        private static List<MedicalHistory> SeedMedicalHistories(DogVetContext context, List<Pet> pets, List<Veterinarian> veterinarians, DateTime seedDate)
        {
            var histories = new List<MedicalHistory>
            {
                new MedicalHistory { Diagnosis = "Revision general anual", Notes = "Signos vitales normales, vacunas al dia", VisitDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[0], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Infeccion de oido – otitis externa", Notes = "Inflamacion leve, se recetaron gotas para oido", VisitDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 24, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[1], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Alergia en piel – dermatitis atopica", Notes = "Alergias estacionales, se recetaron antihistaminicos", VisitDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[2], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Vacunacion de rutina", Notes = "Vacunas antirabica y DHPP administradas", VisitDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[3], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Cojera – pata trasera izquierda", Notes = "Se solicito radiografia, posible esguince leve", VisitDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[4], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Limpieza dental", Notes = "Detartraje completado, sin caries encontradas", VisitDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[5], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Refuerzo de vacuna", Notes = "Refuerzo DHPP administrado", VisitDate = new DateTime(2026, 1, 18, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[6], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.MedicalHistories.AddRange(histories);
            return histories;
        }

        /// <summary>
        /// Seed prescriptions with medical history relationships
        /// </summary>
        private static List<Prescription> SeedPrescriptions(DogVetContext context, List<MedicalHistory> medicalHistories, DateTime seedDate)
        {
            var prescriptions = new List<Prescription>
            {
                new Prescription { MedName = "Amoxicilina", Dose = DoseFrequency.Every12Hours, DurationInDays = 10, Status = PrescriptionStatus.Administered, MedicalHistory = medicalHistories[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Enrofloxacino", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Administered, MedicalHistory = medicalHistories[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Cetirizina", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Meloxicam", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Prednisona", Dose = DoseFrequency.Every8Hours, DurationInDays = 5, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Gotas antibacterianas oticas", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Prescriptions.AddRange(prescriptions);
            return prescriptions;
        }

        /// <summary>
        /// Seed sale notes with medical history relationships
        /// </summary>
        private static List<SaleNote> SeedSaleNotes(DogVetContext context, List<MedicalHistory> medicalHistories, DateTime seedDate)
        {
            var saleNotes = new List<SaleNote>
            {
                new SaleNote { NoteDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 750.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 580.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 680.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1150.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1270.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1145.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 18, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 600.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[6], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.SaleNotes.AddRange(saleNotes);
            return saleNotes;
        }

        /// <summary>
        /// Seed sale note concepts with sale note relationships
        /// </summary>
        private static List<SaleNoteConcept> SeedSaleNoteConcepts(DogVetContext context, List<SaleNote> saleNotes, DateTime seedDate)
        {
            var concepts = new List<SaleNoteConcept>
            {
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Vacuna DHPP", Quantity = 1, UnitPrice = 350.00m, ConceptPrice = 350.00m, SaleNote = saleNotes[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Gotas para oido – Otomax", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Prueba de alergia cutanea", Quantity = 1, UnitPrice = 650.00m, ConceptPrice = 650.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Cetirizina 10mg x30", Quantity = 1, UnitPrice = 220.00m, ConceptPrice = 220.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Vacuna antirabica", Quantity = 1, UnitPrice = 280.00m, ConceptPrice = 280.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Radiografia – pata trasera", Quantity = 1, UnitPrice = 550.00m, ConceptPrice = 550.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Meloxicam 1mg x7", Quantity = 1, UnitPrice = 195.00m, ConceptPrice = 195.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Consulta dental", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Detartraje dental", Quantity = 1, UnitPrice = 750.00m, ConceptPrice = 750.00m, SaleNote = saleNotes[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Refuerzo vacuna DHPP", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.SaleNoteConcepts.AddRange(concepts);
            return concepts;
        }

        /// <summary>
        /// Seed appointments with owner and pet relationships
        /// </summary>
        private static List<Appointment> SeedAppointments(DogVetContext context, List<Owner> owners, List<Pet> pets, DateTime seedDate)
        {
            var appointments = new List<Appointment>
            {
                // Scheduled
                new Appointment { Date = new DateTime(2026, 2, 1, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 5, 14, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[1], Pet = pets[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 8, 16, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[2], Pet = pets[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 12, 11, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 15, 9, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[5], Pet = pets[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                // Completed
                new Appointment { Date = new DateTime(2026, 1, 30, 9, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[2], Pet = pets[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 28, 13, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[1], Pet = pets[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 25, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[3], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 20, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[4], Pet = pets[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 15, 11, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[0], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                // Cancelled
                new Appointment { Date = new DateTime(2026, 2, 10, 11, 15, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[3], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 7, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[1], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 3, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[4], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Appointments.AddRange(appointments);
            return appointments;
        }
    }
}
