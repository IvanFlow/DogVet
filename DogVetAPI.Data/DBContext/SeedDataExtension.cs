using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;
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

            var today = DateTime.UtcNow;
            var seedDate = today;
            
            // Seed in dependency order, building relationships with objects
            var owners = SeedOwners(context, seedDate);
            var veterinarians = SeedVeterinarians(context, seedDate);
            var pets = SeedPets(context, owners, seedDate);
            var medicalHistories = SeedMedicalHistories(context, pets, veterinarians, seedDate, today);
            var prescriptions = SeedPrescriptions(context, medicalHistories, seedDate);
            var saleNotes = SeedSaleNotes(context, medicalHistories, seedDate, today);
            var saleConcepts = SeedSaleNoteConcepts(context, saleNotes, seedDate);
            var appointments = SeedAppointments(context, owners, pets, seedDate);
            
            await context.SaveChangesAsync();
            SeedMedicalHistoryFollowUps(context);
        }

        /// <summary>
        /// Seed owners table with diverse scenarios
        /// Supporting multiple pets (2-3) per owner with rich medical histories
        /// </summary>
        private static List<OwnerEntity> SeedOwners(DogVetContext context, DateTime seedDate)
        {
            var owners = new List<OwnerEntity>
            {
                // Owner 1: Has 3 pets
                new OwnerEntity { FirstName = "Carlos", LastName = "Gutierrez Ramirez", Email = "carlos.gutierrez@gmail.com", PhoneNumber = "3312345678", Address = "Av. Patria 1230, Col. Jardines de Guadalupe", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 2: Has 2 pets
                new OwnerEntity { FirstName = "Maria", LastName = "Lopez Hernandez", Email = "maria.lopez@gmail.com", PhoneNumber = "3323456789", Address = "Calle Robles 456, Col. La Calma", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 3: Has 3 pets
                new OwnerEntity { FirstName = "Jose", LastName = "Martinez Torres", Email = "jose.martinez@hotmail.com", PhoneNumber = "3334567890", Address = "Blvd. Puerta de Hierro 789, Col. Puerta de Hierro", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 4: Has 1 pet
                new OwnerEntity { FirstName = "Ana", LastName = "Sanchez Flores", Email = "ana.sanchez@gmail.com", PhoneNumber = "3345678901", Address = "Calle Tesistan 321, Col. Lomas de Zapopan", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 5: Has 2 pets
                new OwnerEntity { FirstName = "Luis", LastName = "Perez Mendoza", Email = "luis.perez@outlook.com", PhoneNumber = "3356789012", Address = "Av. Vallarta 4850, Col. Camino Real", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 6: Has 3 pets
                new OwnerEntity { FirstName = "Gabriela", LastName = "Reyes Vargas", Email = "gabriela.reyes@gmail.com", PhoneNumber = "3367890123", Address = "Calle Andares 150, Col. Andares", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 7: Has 2 pets
                new OwnerEntity { FirstName = "Francisco", LastName = "Mendez Corona", Email = "francisco.mendez@gmail.com", PhoneNumber = "3378901234", Address = "Av. Mexico 2050, Col. Monumental", City = "Guadalajara", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 8: Has 1 pet
                new OwnerEntity { FirstName = "Patricia", LastName = "Ruiz Molina", Email = "patricia.ruiz@gmail.com", PhoneNumber = "3389012345", Address = "Calle 8 de Julio 456, Col. Centro", City = "Guadalajara", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 9: Has 3 pets
                new OwnerEntity { FirstName = "Antonio", LastName = "Campos Navarro", Email = "antonio.campos@hotmail.com", PhoneNumber = "3390123456", Address = "Paseo Montepios 1500, Col. Santa Paula", City = "Tlaquepaque", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 10: Has 2 pets
                new OwnerEntity { FirstName = "Sofia", LastName = "Valencia Gutierrez", Email = "sofia.valencia@gmail.com", PhoneNumber = "3301234567", Address = "Av. Tepeyac 890, Col. El Colli", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Owners.AddRange(owners);
            return owners;
        }

        /// <summary>
        /// Seed veterinarians table with different availability statuses
        /// </summary>
        private static List<VeterinarianEntity> SeedVeterinarians(DogVetContext context, DateTime seedDate)
        {
            var vets = new List<VeterinarianEntity>
            {
                new VeterinarianEntity { FirstName = "Laura", LastName = "Castillo Medina", Email = "laura.castillo@dogvet.com", PhoneNumber = "3322221111", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new VeterinarianEntity { FirstName = "Roberto", LastName = "Morales Jimenez", Email = "roberto.morales@dogvet.com", PhoneNumber = "3322222222", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new VeterinarianEntity { FirstName = "Patricia", LastName = "Ochoa Navarro", Email = "patricia.ochoa@dogvet.com", PhoneNumber = "3322233333", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new VeterinarianEntity { FirstName = "Eduardo", LastName = "Ibarra Solis", Email = "eduardo.ibarra@dogvet.com", PhoneNumber = "3322244444", IsActive = false, CreatedAt = seedDate, UpdatedAt = seedDate },
                new VeterinarianEntity { FirstName = "Diana", LastName = "Cruz Aguilar", Email = "diana.cruz@dogvet.com", PhoneNumber = "3322255555", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Veterinarians.AddRange(vets);
            return vets;
        }

        /// <summary>
        /// Seed pets table with owner relationships
        /// Each owner has 2-3 pets with diverse breeds and ages
        /// </summary>
        private static List<PetEntity> SeedPets(DogVetContext context, List<OwnerEntity> owners, DateTime seedDate)
        {
            var pets = new List<PetEntity>
            {
                // Carlos Gutierrez (owner[0]) - 3 pets
                new PetEntity { Name = "Canelo", Breed = "Golden Retriever", Weight = 30.5, Color = "Dorado", Gender = "Macho", DateOfBirth = new DateTime(2023, 3, 10, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Bella", Breed = "Labrador", Weight = 28.0, Color = "Chocolate", Gender = "Hembra", DateOfBirth = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Rocky", Breed = "German Shepherd", Weight = 35.2, Color = "Negro y caf�", Gender = "Macho", DateOfBirth = new DateTime(2022, 1, 20, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Maria Lopez (owner[1]) - 2 pets
                new PetEntity { Name = "Negra", Breed = "Poodle Negro", Weight = 12.3, Color = "Negro", Gender = "Hembra", DateOfBirth = new DateTime(2024, 5, 5, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Max", Breed = "Boxer", Weight = 32.0, Color = "Bayo", Gender = "Macho", DateOfBirth = new DateTime(2021, 2, 10, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Jose Martinez (owner[2]) - 3 pets
                new PetEntity { Name = "Titan", Breed = "Beagle", Weight = 12.3, Color = "Tricolor", Gender = "Macho", DateOfBirth = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Luna", Breed = "Cocker Spaniel", Weight = 14.5, Color = "Dorado", Gender = "Hembra", DateOfBirth = new DateTime(2023, 4, 8, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Charlie", Breed = "Schnauzer", Weight = 8.5, Color = "Gris", Gender = "Macho", DateOfBirth = new DateTime(2025, 3, 12, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Ana Sanchez (owner[3]) - 1 pet
                new PetEntity { Name = "Princesa", Breed = "Shih Tzu", Weight = 8.7, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2025, 2, 14, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Luis Perez (owner[4]) - 2 pets
                new PetEntity { Name = "Thor", Breed = "Pastor Alem�n", Weight = 35.2, Color = "Negro con caf�", Gender = "Macho", DateOfBirth = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = false, Owner = owners[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Nala", Breed = "Husky", Weight = 25.0, Color = "Blanco y gris", Gender = "Hembra", DateOfBirth = new DateTime(2023, 8, 25, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Gabriela Reyes (owner[5]) - 3 pets
                new PetEntity { Name = "Gordo", Breed = "Bulldog Ingl�s", Weight = 28.0, Color = "Bayo", Gender = "Macho", DateOfBirth = new DateTime(2023, 5, 12, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Mimi", Breed = "Maltesa", Weight = 4.5, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2024, 7, 22, 0, 0, 0, DateTimeKind.Utc), Species = Species.Cat.ToString(), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Rex", Breed = "Doberman", Weight = 32.0, Color = "Negro y fuego", Gender = "Macho", DateOfBirth = new DateTime(2022, 4, 5, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Francisco Mendez (owner[6]) - 2 pets
                new PetEntity { Name = "Coco", Breed = "Pug", Weight = 8.0, Color = "Negro", Gender = "Macho", DateOfBirth = new DateTime(2021, 11, 30, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Laila", Breed = "Dachshund", Weight = 6.5, Color = "Caf� oscuro", Gender = "Hembra", DateOfBirth = new DateTime(2023, 9, 14, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Patricia Ruiz (owner[7]) - 1 pet
                new PetEntity { Name = "Toby", Breed = "Jack Russell Terrier", Weight = 6.2, Color = "Blanco con caf�", Gender = "Macho", DateOfBirth = new DateTime(2024, 10, 8, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Antonio Campos (owner[8]) - 3 pets
                new PetEntity { Name = "Fido", Breed = "Rottweiler", Weight = 40.0, Color = "Negro y fuego", Gender = "Macho", DateOfBirth = new DateTime(2022, 6, 18, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Daisy", Breed = "Pinscher", Weight = 5.5, Color = "Rojo", Gender = "Hembra", DateOfBirth = new DateTime(2024, 8, 3, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Duke", Breed = "Mastiff", Weight = 55.0, Color = "Caf� claro", Gender = "Macho", DateOfBirth = new DateTime(2020, 5, 27, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sofia Valencia (owner[9]) - 2 pets
                new PetEntity { Name = "Pelusa", Breed = "Poodle Blanco", Weight = 10.0, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PetEntity { Name = "Simba", Breed = "Cocker Spaniel", Weight = 16.0, Color = "Caf� y blanco", Gender = "Macho", DateOfBirth = new DateTime(2022, 12, 10, 0, 0, 0, DateTimeKind.Utc), Species = Species.Dog.ToString(), IsActive = true, Owner = owners[9], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Pets.AddRange(pets);
            return pets;
        }

        /// <summary>
        /// Seed medical histories with diverse diagnoses, statuses, and follow-ups
        /// Each pet has 2-4 medical records with varied conditions and treatments
        /// </summary>
        private static List<MedicalHistoryEntity> SeedMedicalHistories(DogVetContext context, List<PetEntity> pets, List<VeterinarianEntity> veterinarians, DateTime seedDate, DateTime today)
        {
            // Calculate dynamic dates relative to today
            // Past visit dates (simulating historical records)
            var visitPast120 = today.AddDays(-120);
            var visitPast105 = today.AddDays(-105);
            var visitPast100 = today.AddDays(-100);
            var visitPast95 = today.AddDays(-95);
            var visitPast90 = today.AddDays(-90);
            var visitPast85 = today.AddDays(-85);
            var visitPast75 = today.AddDays(-75);
            var visitPast70 = today.AddDays(-70);
            var visitPast65 = today.AddDays(-65);
            var visitPast60 = today.AddDays(-60);
            var visitPast55 = today.AddDays(-55);
            var visitPast50 = today.AddDays(-50);
            var visitPast45 = today.AddDays(-45);
            var visitPast42 = today.AddDays(-42);
            var visitPast40 = today.AddDays(-40);
            var visitPast35 = today.AddDays(-35);
            var visitPast30 = today.AddDays(-30);
            var visitPast28 = today.AddDays(-28);
            var visitPast25 = today.AddDays(-25);
            var visitPast20 = today.AddDays(-20);
            var visitPast18 = today.AddDays(-18);
            var visitPast15 = today.AddDays(-15);
            var visitPast12 = today.AddDays(-12);
            var visitPast10 = today.AddDays(-10);
            var visitPast8 = today.AddDays(-8);
            var visitPast5 = today.AddDays(-5);
            
            // Overdue follow-ups (j� pasaron)
            var followUpOverdue30 = today.AddDays(-30);
            var followUpOverdue20 = today.AddDays(-20);
            var followUpOverdue15 = today.AddDays(-15);
            var followUpOverdue10 = today.AddDays(-10);
            
            // Follow-ups pr�ximos 30 d�as
            var followUpNext5 = today.AddDays(5);
            var followUpNext10 = today.AddDays(10);
            var followUpNext12 = today.AddDays(12);
            var followUpNext15 = today.AddDays(15);
            var followUpNext18 = today.AddDays(18);
            var followUpNext20 = today.AddDays(20);
            var followUpNext25 = today.AddDays(25);
            
            // Follow-ups futuros (>30 d�as)
            var followUpFuture60 = today.AddDays(60);
            var followUpFuture75 = today.AddDays(75);
            var followUpFuture90 = today.AddDays(90);
            var followUpFuture120 = today.AddDays(120);
            
            var histories = new List<MedicalHistoryEntity>
            {
                // Canelo (pets[0]) - Golden Retriever
                new MedicalHistoryEntity { Diagnosis = "Revisi�n general anual con vacunaci�n", Notes = "Signos vitales normales, vacunas DHPP y antirr�bica al d�a. Peso adecuado.", VisitDate = visitPast10, FollowUpDate = null, Status = "Completed", Pet = pets[0], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Limpieza dental preventiva", Notes = "Acumulaci�n leve de sarro, detartraje realizado sin complicaciones", VisitDate = visitPast30, FollowUpDate = followUpFuture120, Status = "Completed", Pet = pets[0], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Otitis externa bilateral", Notes = "Infecciones de o�do relacionadas con alergias. Gotas y medicamento oral prescritos.", VisitDate = visitPast20, FollowUpDate = followUpOverdue20, Status = "Follow-up", Pet = pets[0], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n de seguimiento - otitis", Notes = "infecci�n mejorando con tratamiento. Continuar gotas por una semana m�s.", VisitDate = visitPast5, FollowUpDate = null, Status = "Completed", Pet = pets[0], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Bella (pets[1]) - Labrador
                new MedicalHistoryEntity { Diagnosis = "vacunaci�n de rutina para cachorra", Notes = "Primera dosis de DHPP aplicada. Revisar en 3-4 semanas para refuerzo.", VisitDate = visitPast120, FollowUpDate = visitPast90, Status = "Completed", Pet = pets[1], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Refuerzo de vacuna DHPP", Notes = "Segunda dosis de DHPP aplicada correctamente. Ser� revacunada en 1 a�o.", VisitDate = visitPast90, FollowUpDate = null, Status = "Completed", Pet = pets[1], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Alergia alimentaria con dermatitis", Notes = "Cambio de dieta recomendado. Medicamento antihistam�nico prescrito.", VisitDate = visitPast45, FollowUpDate = followUpNext25, Status = "Follow-up", Pet = pets[1], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n de seguimiento nutrici�n", Notes = "Se eval�a respuesta al nuevo alimento. Mejora notoria en condici�n de piel.", VisitDate = visitPast15, FollowUpDate = null, Status = "Completed", Pet = pets[1], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Rocky (pets[2]) - German Shepherd
                new MedicalHistoryEntity { Diagnosis = "Displasia de cadera - evaluaci�n radiol�gica", Notes = "Radiograf�as realizadas. Grado moderado de displasia. Fisioterapia recomendada.", VisitDate = visitPast60, FollowUpDate = followUpFuture75, Status = "Follow-up", Pet = pets[2], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Control de peso y condici�n muscular", Notes = "Reducci�n de actividad debido a displasia. Dieta especial iniciada.", VisitDate = visitPast10, FollowUpDate = null, Status = "Completed", Pet = pets[2], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n anual con pruebas de sangre", Notes = "An�lisis completo realizado. Resultados dentro de par�metros normales.", VisitDate = visitPast5, FollowUpDate = null, Status = "Completed", Pet = pets[2], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Negra (pets[3]) - Poodle Negro
                new MedicalHistoryEntity { Diagnosis = "Otitis externa con infecci�n bacteriana", Notes = "Cultivo realizado. Gotas antibacterianas prescritas. Control en 2 semanas.", VisitDate = visitPast15, FollowUpDate = followUpNext20, Status = "Follow-up", Pet = pets[3], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n de seguimiento - otitis control", Notes = "infecci�n controlada con gotas. Continuar tratamiento 7 d�as m�s.", VisitDate = visitPast10, FollowUpDate = null, Status = "Completed", Pet = pets[3], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Limpieza de oidos y Revisi�n dental", Notes = "Higiene correcta. Se recomienda limpieza semanal de o�dos.", VisitDate = visitPast25, FollowUpDate = null, Status = "Completed", Pet = pets[3], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n pre-grooming y vacunaci�n", Notes = "Estado general excelente. Vacuna antirr�bica aplicada.", VisitDate = visitPast45, FollowUpDate = null, Status = "Completed", Pet = pets[3], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Max (pets[4]) - Boxer
                new MedicalHistoryEntity { Diagnosis = "Displasia de cadera leve", Notes = "Radiograf�as muestran displasia leve. Seguimiento cl�nico recomendado.", VisitDate = visitPast60, FollowUpDate = followUpNext20, Status = "Follow-up", Pet = pets[4], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Control articular y evaluaci�n de dolor", Notes = "Sin evidencia de dolor. Peso adecuado. Suplemento articular iniciado.", VisitDate = visitPast10, FollowUpDate = null, Status = "Completed", Pet = pets[4], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Vacuna antirr�bica de refuerzo", Notes = "Refuerzo aplicado sin complicaciones.", VisitDate = visitPast30, FollowUpDate = null, Status = "Completed", Pet = pets[4], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Titan (pets[5]) - Beagle
                new MedicalHistoryEntity { Diagnosis = "Otitis media recurrente", Notes = "Historial de infecciones de o�do. evaluaci�n auditiva recomendada.", VisitDate = visitPast25, FollowUpDate = followUpNext15, Status = "Follow-up", Pet = pets[5], Veterinarian = veterinarians[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Limpieza dental - eliminaci�n de c�lculo", Notes = "Detartraje completo realizado. Higiene oral deficiente. Cepillado diario recomendado.", VisitDate = visitPast45, FollowUpDate = followUpFuture90, Status = "Completed", Pet = pets[5], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n geri�trica completa", Notes = "Perro adulto mayor evaluado. Signos vitales estables. Seguimiento preventivo recomendado.", VisitDate = visitPast15, FollowUpDate = followUpFuture120, Status = "Completed", Pet = pets[5], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Luna (pets[6]) - Cocker Spaniel
                new MedicalHistoryEntity { Diagnosis = "Queratitis ulcerativa en ojo derecho", Notes = "�lcera corneal superficial. Lubricantes oft�lmicos y antibi�ticos prescritos.", VisitDate = visitPast30, FollowUpDate = visitPast20, Status = "Completed", Pet = pets[6], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n oftalmol�gica post-tratamiento", Notes = "�lcera cicatrizada completamente. Visi�n recuperada.", VisitDate = visitPast25, FollowUpDate = null, Status = "Completed", Pet = pets[6], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "vacunaci�n de rutina", Notes = "Vacunas DHPP y antirr�bica aplicadas. Presencia de ectopar�sitos detectada.", VisitDate = visitPast10, FollowUpDate = followUpNext10, Status = "Follow-up", Pet = pets[6], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Charlie (pets[7]) - Schnauzer
                new MedicalHistoryEntity { Diagnosis = "desparasitaci�n cachorro - interna y externa", Notes = "Par�sitos internos detectados en An�lisis fecal. Tratamiento completo iniciado.", VisitDate = visitPast90, FollowUpDate = visitPast75, Status = "Completed", Pet = pets[7], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Refuerzo de desparasitaci�n", Notes = "An�lisis fecal repetido. Control negativo. Cachorro en buena salud.", VisitDate = visitPast75, FollowUpDate = null, Status = "Completed", Pet = pets[7], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "vacunaci�n primaria - DHPP", Notes = "Primera dosis de vacuna polivalente aplicada correctamente.", VisitDate = visitPast85, FollowUpDate = visitPast65, Status = "Completed", Pet = pets[7], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Princesa (pets[8]) - Shih Tzu
                new MedicalHistoryEntity { Diagnosis = "Conjuntivitis al�rgica bilateral", Notes = "Inflamaci�n leve de conjuntivas. Gotas oft�lmicas y antihistam�nico prescrito.", VisitDate = visitPast20, FollowUpDate = followUpOverdue15, Status = "Follow-up", Pet = pets[8], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Limpieza de o�dos preventiva", Notes = "Orejas largas propensas a infecciones. Limpieza completa realizada.", VisitDate = visitPast40, FollowUpDate = followUpFuture75, Status = "Completed", Pet = pets[8], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "vacunaci�n de cachorra", Notes = "DHPP y antirr�bica iniciadas. Cachorrita peque�a pero saludable.", VisitDate = visitPast70, FollowUpDate = visitPast50, Status = "Completed", Pet = pets[8], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Thor (pets[9]) - Pastor Aleman (Inactivo)
                new MedicalHistoryEntity { Diagnosis = "Enfermedad articular degenerativa avanzada", Notes = "Displasia severa progresiva. Medicamentos para dolor prescritos. Pron�stico limitado.", VisitDate = visitPast60, FollowUpDate = followUpNext10, Status = "Follow-up", Pet = pets[9], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "evaluaci�n geri�trica integral", Notes = "Perro adulto mayor con movilidad reducida. Calidad de vida evaluada.", VisitDate = visitPast90, FollowUpDate = null, Status = "Completed", Pet = pets[9], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Nala (pets[10]) - Husky
                new MedicalHistoryEntity { Diagnosis = "Alopecia estacional seasonal", Notes = "Ca�da excesiva de pelo t�pica de la raza. Suplemento de �cidos grasos prescrito.", VisitDate = visitPast25, FollowUpDate = followUpFuture120, Status = "Completed", Pet = pets[10], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n post-muda", Notes = "Muda completada. Pelaje en excelente condici�n. Recomendaci�n de cepillado frecuente mantenida.", VisitDate = visitPast10, FollowUpDate = null, Status = "Completed", Pet = pets[10], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Gordo (pets[11]) - Bulldog Ingles
                new MedicalHistoryEntity { Diagnosis = "S�ndrome braquicef�lico - dificultad respiratoria", Notes = "Respiraci�n ruidosa y limitaci�n en ejercicio. Cirug�a correctiva evaluada.", VisitDate = visitPast40, FollowUpDate = followUpNext5, Status = "Follow-up", Pet = pets[11], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "evaluaci�n pre-quirurgica", Notes = "An�lisis de sangre y Radiograf�as completadas. Apto para Cirug�a.", VisitDate = visitPast50, FollowUpDate = null, Status = "Completed", Pet = pets[11], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Control de peso y dieta", Notes = "Sobrepeso leve detectado. Dieta baja en calor�as recomendada.", VisitDate = visitPast8, FollowUpDate = followUpFuture90, Status = "Completed", Pet = pets[11], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Mimi (pets[12]) - Maltesa
                new MedicalHistoryEntity { Diagnosis = "Calculus dental y enfermedad periodontal", Notes = "Acumulaci�n severa de sarro. Limpieza dental urgente recomendada.", VisitDate = visitPast12, FollowUpDate = followUpOverdue10, Status = "Follow-up", Pet = pets[12], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n rutinaria con vacunaci�n", Notes = "Signos vitales normales. DHPP y antirr�bica aplicadas.", VisitDate = visitPast50, FollowUpDate = null, Status = "Completed", Pet = pets[12], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Rex (pets[13]) - Doberman
                new MedicalHistoryEntity { Diagnosis = "Cardiomiopat�a dilatada - evaluaci�n ecocardiogr�fica", Notes = "Funci�n card�aca reducida. Medicamentos para coraz�n prescritos. Restricci�n de ejercicio.", VisitDate = visitPast65, FollowUpDate = followUpFuture60, Status = "Follow-up", Pet = pets[13], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Control card�aco - evaluaci�n de medicamentos", Notes = "Frecuencia card�aca mejorando con tratamiento. Continuaci�n de f�rmacos indicada.", VisitDate = visitPast8, FollowUpDate = null, Status = "Completed", Pet = pets[13], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n anual de rutina", Notes = "Dieta card�aca y medicamentos continuados. Estado general estable.", VisitDate = visitPast30, FollowUpDate = null, Status = "Completed", Pet = pets[13], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Coco (pets[14]) - Pug
                new MedicalHistoryEntity { Diagnosis = "Obesidad - plan de P�rdida de peso", Notes = "Sobrepeso severo. Dieta restrictiva cal�rica iniciada. Ejercicio progresivo recomendado.", VisitDate = visitPast28, FollowUpDate = followUpNext12, Status = "Follow-up", Pet = pets[14], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n oftalmol�gica preventiva", Notes = "Ojos prominentes evaluados. Sin �lceras detectadas. Limpieza de arrugas realizada.", VisitDate = visitPast35, FollowUpDate = null, Status = "Completed", Pet = pets[14], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Laila (pets[15]) - Dachshund
                new MedicalHistoryEntity { Diagnosis = "Par�lisis lumbar - disco intervertebral herniad", Notes = "P�rdida parcial de movilidad trasera. Medicamentos y reposo prescrito.", VisitDate = visitPast75, FollowUpDate = followUpOverdue30, Status = "Follow-up", Pet = pets[15], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "evaluaci�n de recuperaci�n neuromuscular", Notes = "Mejora leve en movilidad. Fisioterapia continuada recomendada.", VisitDate = visitPast5, FollowUpDate = null, Status = "Completed", Pet = pets[15], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Toby (pets[16]) - Jack Russell Terrier
                new MedicalHistoryEntity { Diagnosis = "Luxaci�n patelar unilateral grado II", Notes = "R�tula dislocada ocasionalmente. Seguimiento cl�nico recomendado. Cirug�a evaluada.", VisitDate = visitPast55, FollowUpDate = followUpFuture90, Status = "Completed", Pet = pets[16], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n post-control - comportamiento y movilidad", Notes = "Perro activo. Sin s�ntomas agudos. Observaci�n continuada recomendada.", VisitDate = visitPast8, FollowUpDate = null, Status = "Completed", Pet = pets[16], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Fido (pets[17]) - Rottweiler
                new MedicalHistoryEntity { Diagnosis = "Gastroenteritis aguda - v�mitos y diarrea", Notes = "Probable intoxicaci�n alimentaria. Fluidoterapia y medicamentos antiem�tico prescritos.", VisitDate = visitPast15, FollowUpDate = followUpNext18, Status = "Follow-up", Pet = pets[17], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n post-gastroenteritis", Notes = "recuperaci�n completa. Digesti�n normalizada. Dieta blanda continuada 3 d�as m�s.", VisitDate = visitPast10, FollowUpDate = null, Status = "Completed", Pet = pets[17], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Revisi�n anual de rutina", Notes = "Signos vitales normales. Vacunaciones al d�a. Peso adecuado.", VisitDate = visitPast35, FollowUpDate = null, Status = "Completed", Pet = pets[17], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Daisy (pets[18]) - Miniature Pinscher
                new MedicalHistoryEntity { Diagnosis = "vacunaci�n de cachorra - serie primaria", Notes = "Primera dosis DHPP y antirr�bica aplicadas sin reacci�n.", VisitDate = visitPast120, FollowUpDate = visitPast105, Status = "Completed", Pet = pets[18], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Refuerzo de vacunaci�n", Notes = "Segunda dosis DHPP aplicada. Tercera dosis en 3-4 semanas.", VisitDate = visitPast105, FollowUpDate = visitPast90, Status = "Completed", Pet = pets[18], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Ca�da de diente de cachorro - erupci�n normal", Notes = "Cambio adeacuado de dentici�n. Sin retenci�n de dientes deciduos.", VisitDate = visitPast65, FollowUpDate = null, Status = "Completed", Pet = pets[18], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Duke (pets[19]) - Mastiff
                new MedicalHistoryEntity { Diagnosis = "Artritis degenerativa - etapa avanzada", Notes = "Movilidad limitada. Medicamentos para dolor y condroprotectores prescritos.", VisitDate = visitPast100, FollowUpDate = followUpNext5, Status = "Follow-up", Pet = pets[19], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "evaluaci�n de calidad de vida - perro geri�trico", Notes = "Confort y medicaci�n evaluados. Cama ortop�dica recomendada.", VisitDate = visitPast5, FollowUpDate = null, Status = "Completed", Pet = pets[19], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Pelusa (pets[20]) - Poodle Blanco
                new MedicalHistoryEntity { Diagnosis = "Control de cachorra - Revisi�n integral", Notes = "Cachorra sana. Crecimiento adecuado. vacunaci�n iniciada.", VisitDate = visitPast95, FollowUpDate = visitPast85, Status = "Completed", Pet = pets[20], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Serie de vacunaci�n primaria completada", Notes = "Tercera dosis DHPP aplicada. Protecci�n completa alcanzada.", VisitDate = visitPast75, FollowUpDate = null, Status = "Completed", Pet = pets[20], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Simba (pets[21]) - Cocker Spaniel
                new MedicalHistoryEntity { Diagnosis = "infecci�n de o�dos bilateral - otitis externa", Notes = "Inflamaci�n moderada. Gotas antibacterianas y anti-inflamatorias prescritas.", VisitDate = visitPast18, FollowUpDate = followUpOverdue20, Status = "Follow-up", Pet = pets[21], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "Limpieza dental y Revisi�n oral", Notes = "Sarro presente. Detartraje recomendado. Higiene dental pobre.", VisitDate = visitPast42, FollowUpDate = followUpFuture90, Status = "Completed", Pet = pets[21], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistoryEntity { Diagnosis = "vacunaci�n anual de refuerzo", Notes = "DHPP y antirr�bica aplicadas. Historial de vacuna al d�a.", VisitDate = visitPast60, FollowUpDate = null, Status = "Completed", Pet = pets[21], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.MedicalHistories.AddRange(histories);
            return histories;
        }

        /// <summary>
        /// Establish follow-up relationships between medical records after they are created and have IDs assigned
        /// </summary>
        private static void SeedMedicalHistoryFollowUps(DogVetContext context)
        {
            // Canelo: Otitis revision is a follow-up of the original otitis
            var caneloOtitis = context.MedicalHistories.FirstOrDefault(m => m.Diagnosis == "Otitis externa bilateral");
            var caneloOtitisFollowUp = context.MedicalHistories.FirstOrDefault(m => m.Diagnosis == "Revisi�n de seguimiento - otitis");
            if (caneloOtitis != null && caneloOtitisFollowUp != null)
            {
                caneloOtitisFollowUp.FollowUpOf = caneloOtitis.Id;
                caneloOtitis.Status = "Completed"; // Mark original otitis as completed since it has a follow-up
            }

            // Bella: Vaccation refuerzo is follow-up of initial vaccination
            var bellaVaccinationInitial = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "vacunaci�n de rutina para cachorra" && 
                m.VisitDate == new DateTime(2024, 8, 10, 0, 0, 0, DateTimeKind.Utc));
            var bellaVaccinationRefuerzo = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Refuerzo de vacuna DHPP");
            if (bellaVaccinationInitial != null && bellaVaccinationRefuerzo != null)
            {
                bellaVaccinationRefuerzo.FollowUpOf = bellaVaccinationInitial.Id;
                bellaVaccinationInitial.Status = "Completed"; // Mark initial vaccination as completed since it has a follow-up
            }

            // Bella: nutrici�n follow-up is follow-up of alergia alimentaria
            var bellaAllergy = context.MedicalHistories.FirstOrDefault(m => m.Diagnosis == "Alergia alimentaria con dermatitis");
            var bellaNutrition = context.MedicalHistories.FirstOrDefault(m => m.Diagnosis == "Revisi�n de seguimiento nutrici�n");
            if (bellaAllergy != null && bellaNutrition != null)
            {
                bellaNutrition.FollowUpOf = bellaAllergy.Id;
                bellaAllergy.Status = "Completed"; // Mark allergy consultation as completed since it has a follow-up

            }

            // Negra: Otitis control is follow-up of original otitis
            var negraOtitis = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Otitis externa con infecci�n bacteriana");
            var negraOtitisControl = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Revisi�n de seguimiento - otitis control");
            if (negraOtitis != null && negraOtitisControl != null)
            {
                negraOtitisControl.FollowUpOf = negraOtitis.Id;
                negraOtitis.Status = "Completed"; // Mark original otitis as completed since it has a follow-up
            }

            // Max: Articular control is follow-up of hip dysplasia diagnosis
            var maxDysplasia = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Displasia de cadera leve");
            var maxArticularControl = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Control articular y evaluaci�n de dolor");
            if (maxDysplasia != null && maxArticularControl != null)
            {
                maxArticularControl.FollowUpOf = maxDysplasia.Id;
                maxDysplasia.Status = "Completed"; // Mark hip dysplasia diagnosis as completed since it has a follow-up
            }

            // Charlie: Deworming refuerzo is follow-up of initial deworming
            var charlieDeworming = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "desparasitaci�n cachorro - interna y externa");
            var charlieDeformingRefuerzo = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Refuerzo de desparasitaci�n");
            if (charlieDeworming != null && charlieDeformingRefuerzo != null)
            {
                charlieDeformingRefuerzo.FollowUpOf = charlieDeworming.Id;
                charlieDeworming.Status = "Completed"; // Mark initial deworming as completed since it has a follow-up
            }

            // Luna: Oftalmology post-treatment is follow-up of keratitis
            var lunaKeratitis = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Queratitis ulcerativa en ojo derecho");
            var lunaOftalmologyPostTreatment = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Revisi�n oftalmol�gica post-tratamiento");
            if (lunaKeratitis != null && lunaOftalmologyPostTreatment != null)
            {
                lunaOftalmologyPostTreatment.FollowUpOf = lunaKeratitis.Id;
                lunaKeratitis.Status = "Completed"; // Mark keratitis consultation as completed since it has a follow-up
            }

            // Nala: Post-shedding review is follow-up of alopecia consultation
            var nalaAlopecia = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Alopecia estacional seasonal");
            var nalaPostShedding = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Revisi�n post-muda");
            if (nalaAlopecia != null && nalaPostShedding != null)
            {
                nalaPostShedding.FollowUpOf = nalaAlopecia.Id;
                nalaAlopecia.Status = "Completed"; // Mark alopecia consultation as completed since it has a follow-up
            }

            // Rex: Cardiac control is follow-up of cardiomyopathy diagnosis
            var rexCardiomyopathy = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Cardiomiopat�a dilatada - evaluaci�n ecocardiogr�fica");
            var rexCardiacControl = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Control card�aco - evaluaci�n de medicamentos");
            if (rexCardiomyopathy != null && rexCardiacControl != null)
            {
                rexCardiacControl.FollowUpOf = rexCardiomyopathy.Id;
                rexCardiomyopathy.Status = "Completed"; // Mark cardiomyopathy diagnosis as completed since it has a follow-up
            }

            // Laila: Recovery evaluation is follow-up of lumbar paralysis
            var lailaParalysis = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Par�lisis lumbar - disco intervertebral herniad");
            var lailaRecovery = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "evaluaci�n de recuperaci�n neuromuscular");
            if (lailaParalysis != null && lailaRecovery != null)
            {
                lailaRecovery.FollowUpOf = lailaParalysis.Id;
                lailaParalysis.Status = "Completed"; // Mark lumbar paralysis diagnosis as completed since it has a follow-up
            }

            // Fido: Post-gastroenteritis review is follow-up of acute gastroenteritis
            var fidoGastroenteritis = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Gastroenteritis aguda - v�mitos y diarrea");
            var fidoPostGastroenteritis = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Revisi�n post-gastroenteritis");
            if (fidoGastroenteritis != null && fidoPostGastroenteritis != null)
            {
                fidoPostGastroenteritis.FollowUpOf = fidoGastroenteritis.Id;
                fidoGastroenteritis.Status = "Completed"; // Mark acute gastroenteritis diagnosis as completed since it has a follow-up
            }

            // Daisy: Vaccination refuerzo is follow-up of initial vaccination series
            var daisyVaccinationInitial = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "vacunaci�n de cachorra - serie primaria");
            var daisyVaccinationRefuerzo = context.MedicalHistories.FirstOrDefault(m => 
                m.Diagnosis == "Refuerzo de vacunaci�n" && 
                m.Pet != null && m.Pet.Name == "Daisy");
            if (daisyVaccinationInitial != null && daisyVaccinationRefuerzo != null)
            {
                daisyVaccinationRefuerzo.FollowUpOf = daisyVaccinationInitial.Id;
                daisyVaccinationInitial.Status = "Completed"; // Mark initial vaccination as completed since it has a follow-up
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Seed prescriptions with medical history relationships
        /// Multiple prescriptions per condition with realistic dosages and durations
        /// </summary>
        private static List<PrescriptionEntity> SeedPrescriptions(DogVetContext context, List<MedicalHistoryEntity> medicalHistories, DateTime seedDate)
        {
            var prescriptions = new List<PrescriptionEntity>
            {
                // Otitis externa (medicalHistories[2]) - Canelo
                new PrescriptionEntity { MedName = "Gotas antibacterianas otitis - Otomax", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Cetirizina 10mg - antihistam�nico", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Alergia alimentaria (medicalHistories[5]) - Bella
                new PrescriptionEntity { MedName = "Cetirizina 10mg - antihistam�nico", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Prednisona 5mg - corticosteroide", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Displasia de cadera (medicalHistories[6]) - Rocky
                new PrescriptionEntity { MedName = "Meloxicam 15mg - antiinflamatorio", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Glucosamina + Condroitina - protector articular", Dose = DoseFrequency.Daily, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Otitis externa (medicalHistories[9]) - Negra
                new PrescriptionEntity { MedName = "Enrofloxacino gotas - antibi�tico", Dose = DoseFrequency.Every12Hours, DurationInDays = 10, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Limpieza de o�dos - soluci�n otol�gica", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Queratitis ulcerativa (medicalHistories[15]) - Luna
                new PrescriptionEntity { MedName = "Ciprofloxacino gotas - antibi�tico oft�lmico", Dose = DoseFrequency.Every4Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[15], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Lubricante oft�lmico - protector corneal", Dose = DoseFrequency.Every6Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[15], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // desparasitaci�n (medicalHistories[17]) - Charlie
                new PrescriptionEntity { MedName = "Albendazol - antiparasitario interno", Dose = DoseFrequency.Daily, DurationInDays = 5, Status = PrescriptionStatus.Administered, MedicalHistory = medicalHistories[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Fipronil + Methoprene spray - antiparasitario externo", Dose = DoseFrequency.Weekly, DurationInDays = 21, Status = PrescriptionStatus.Administered, MedicalHistory = medicalHistories[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Conjuntivitis al�rgica (medicalHistories[20]) - Princesa
                new PrescriptionEntity { MedName = "Gotas oft�lmicas antihistam�nicas", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Cetirizina 10mg - antihistam�nico sist�mico", Dose = DoseFrequency.Daily, DurationInDays = 21, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Enfermedad articular (medicalHistories[23]) - Thor
                new PrescriptionEntity { MedName = "Tramadol 50mg - analg�sico", Dose = DoseFrequency.Every8Hours, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[23], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Meloxicam 15mg - antiinflamatorio", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[23], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Alopecia estacional (medicalHistories[25]) - Nala
                new PrescriptionEntity { MedName = "Omega-3 + Omega-6 - suplemento �cidos grasos", Dose = DoseFrequency.Daily, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[25], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // S�ndrome braquicef�lico (medicalHistories[27]) - Gordo
                new PrescriptionEntity { MedName = "Furosemida 40mg - diur�tico", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[27], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Calculus dental (medicalHistories[30]) - Mimi
                new PrescriptionEntity { MedName = "Clorhexidina enjuague - desinfectante oral", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[30], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Cardiomiopat�a (medicalHistories[32]) - Rex
                new PrescriptionEntity { MedName = "Enalapril 5mg - inhibidor ACE", Dose = DoseFrequency.Every12Hours, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[32], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Furosemida 40mg - diur�tico", Dose = DoseFrequency.Daily, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[32], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Pimobendan 5mg - inotr�pico", Dose = DoseFrequency.Every12Hours, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[32], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Obesidad (medicalHistories[35]) - Coco
                new PrescriptionEntity { MedName = "Orlistat 120mg - inhibidor lipasa", Dose = DoseFrequency.Every8Hours, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[35], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Par�lisis lumbar (medicalHistories[38]) - Laila
                new PrescriptionEntity { MedName = "Metilprednisolona 4mg - corticosteroide", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[38], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Gabapentina 100mg - analg�sico neurop�tico", Dose = DoseFrequency.Every8Hours, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[38], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Gastroenteritis (medicalHistories[42]) - Fido
                new PrescriptionEntity { MedName = "Metoclopramida 10mg - antiem�tico", Dose = DoseFrequency.Every8Hours, DurationInDays = 5, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Famotidina 20mg - protector g�strico", Dose = DoseFrequency.Every12Hours, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Probi�tico - flora intestinal", Dose = DoseFrequency.Daily, DurationInDays = 10, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // infecci�n de o�dos (medicalHistories[46]) - Simba
                new PrescriptionEntity { MedName = "Gotas otitis - Otomax", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[46], CreatedAt = seedDate, UpdatedAt = seedDate },
                new PrescriptionEntity { MedName = "Enrofloxacino 100mg - antibi�tico sist�mico", Dose = DoseFrequency.Daily, DurationInDays = 10, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[46], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Prescriptions.AddRange(prescriptions);
            return prescriptions;
        }

        /// <summary>
        /// Seed sale notes with medical history relationships
        /// Payment tracking for various medical treatments
        /// </summary>
        private static List<SaleNoteEntity> SeedSaleNotes(DogVetContext context, List<MedicalHistoryEntity> medicalHistories, DateTime seedDate, DateTime today)
        {
            // Calculate dynamic dates relative to today
            var notePast280 = today.AddDays(-280);  // May 2025
            var notePast130 = today.AddDays(-130);  // November 2025
            var notePast125 = today.AddDays(-125);  // November 2025
            var notePast110 = today.AddDays(-110);  // December 2025
            var notePast105 = today.AddDays(-105);  // December 2025
            var notePast85 = today.AddDays(-85);    // January 2026
            var notePast80 = today.AddDays(-80);    // January 2026
            var notePast75 = today.AddDays(-75);    // January 2026
            var notePast70 = today.AddDays(-70);    // January 2026
            var notePast65 = today.AddDays(-65);    // January 2026
            var notePast60 = today.AddDays(-60);    // January 2026
            var notePast55 = today.AddDays(-55);    // January 2026
            var notePast50 = today.AddDays(-50);    // January 2026
            var notePast45 = today.AddDays(-45);    // January 2026
            var notePast40 = today.AddDays(-40);    // January 2026
            var notePast35 = today.AddDays(-35);    // February 2026
            var notePast30 = today.AddDays(-30);    // February 2026
            var notePast25 = today.AddDays(-25);    // February 2026
            var notePast20 = today.AddDays(-20);    // February 2026
            var notePast15 = today.AddDays(-15);    // February 2026
            var notePast5 = today.AddDays(-5);      // April 2026
            
            var saleNotes = new List<SaleNoteEntity>
            {
                // Paid notes
                new SaleNoteEntity { NoteDate = notePast75, TotalAmount = 750.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[0], CreatedAt = notePast75, UpdatedAt = notePast75 },
                new SaleNoteEntity { NoteDate = notePast70, TotalAmount = 580.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[1], CreatedAt = notePast70, UpdatedAt = notePast70 },
                new SaleNoteEntity { NoteDate = notePast65, TotalAmount = 1150.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[2], CreatedAt = notePast65, UpdatedAt = notePast65 },
                new SaleNoteEntity { NoteDate = notePast110, TotalAmount = 420.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[3], CreatedAt = notePast110, UpdatedAt = notePast110 },
                new SaleNoteEntity { NoteDate = notePast130, TotalAmount = 680.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[4], CreatedAt = notePast130, UpdatedAt = notePast130 },
                new SaleNoteEntity { NoteDate = notePast280, TotalAmount = 950.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[6], CreatedAt = notePast280, UpdatedAt = notePast280 },
                new SaleNoteEntity { NoteDate = notePast80, TotalAmount = 520.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[9], CreatedAt = notePast80, UpdatedAt = notePast80 },
                new SaleNoteEntity { NoteDate = notePast125, TotalAmount = 780.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[15], CreatedAt = notePast125, UpdatedAt = notePast125 },
                new SaleNoteEntity { NoteDate = notePast280.AddDays(224), TotalAmount = 350.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[17], CreatedAt = notePast280.AddDays(224), UpdatedAt = notePast280.AddDays(224) },
                new SaleNoteEntity { NoteDate = notePast60, TotalAmount = 640.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[29], CreatedAt = notePast60, UpdatedAt = notePast60 },
                
                // Pending notes
                new SaleNoteEntity { NoteDate = notePast30, TotalAmount = 1270.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[5], CreatedAt = notePast30, UpdatedAt = notePast30 },
                new SaleNoteEntity { NoteDate = notePast25, TotalAmount = 1145.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[8], CreatedAt = notePast25, UpdatedAt = notePast25 },
                new SaleNoteEntity { NoteDate = notePast20, TotalAmount = 850.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[7], CreatedAt = notePast20, UpdatedAt = notePast20 },
                new SaleNoteEntity { NoteDate = notePast15, TotalAmount = 520.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[20], CreatedAt = notePast15, UpdatedAt = notePast15 },
                new SaleNoteEntity { NoteDate = notePast5, TotalAmount = 715.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[42], CreatedAt = notePast5, UpdatedAt = notePast5 }
            };
            context.SaleNotes.AddRange(saleNotes);
            return saleNotes;
        }

        /// <summary>
        /// Seed sale note concepts with sale note relationships
        /// Line items for veterinary services and products
        /// </summary>
        private static List<SaleNoteConceptEntity> SeedSaleNoteConcepts(DogVetContext context, List<SaleNoteEntity> saleNotes, DateTime seedDate)
        {
            var concepts = new List<SaleNoteConceptEntity>
            {
                // Sale note 0
                new SaleNoteConceptEntity { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Vacuna DHPP", Quantity = 1, UnitPrice = 350.00m, ConceptPrice = 350.00m, SaleNote = saleNotes[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 1
                new SaleNoteConceptEntity { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Limpieza dental", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 2
                new SaleNoteConceptEntity { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Gotas antibacterianas otitis", Quantity = 1, UnitPrice = 250.00m, ConceptPrice = 250.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Medicament oral", Quantity = 1, UnitPrice = 100.00m, ConceptPrice = 100.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 3
                new SaleNoteConceptEntity { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Vacuna antirrábica", Quantity = 1, UnitPrice = 120.00m, ConceptPrice = 120.00m, SaleNote = saleNotes[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 4
                new SaleNoteConceptEntity { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Radiografía", Quantity = 1, UnitPrice = 550.00m, ConceptPrice = 550.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Suplemento articular", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 5
                new SaleNoteConceptEntity { Description = "Revisión general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Prueba alergia", Quantity = 1, UnitPrice = 650.00m, ConceptPrice = 650.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "antihistamínico oral", Quantity = 1, UnitPrice = 220.00m, ConceptPrice = 220.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 6
                new SaleNoteConceptEntity { Description = "Consulta", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Gotas oftálmicas antibióticas", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Lubricante ocular", Quantity = 1, UnitPrice = 120.00m, ConceptPrice = 120.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 7
                new SaleNoteConceptEntity { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Detartraje dental", Quantity = 1, UnitPrice = 750.00m, ConceptPrice = 750.00m, SaleNote = saleNotes[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 8
                new SaleNoteConceptEntity { Description = "Desparasitante oral", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Antiparasitario externo", Quantity = 1, UnitPrice = 170.00m, ConceptPrice = 170.00m, SaleNote = saleNotes[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 9
                new SaleNoteConceptEntity { Description = "Revisión nutricional", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Suplemento dietético", Quantity = 1, UnitPrice = 240.00m, ConceptPrice = 240.00m, SaleNote = saleNotes[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 10
                new SaleNoteConceptEntity { Description = "evaluación alergia", Quantity = 1, UnitPrice = 600.00m, ConceptPrice = 600.00m, SaleNote = saleNotes[10], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Medicament antihistamínico", Quantity = 1, UnitPrice = 320.00m, ConceptPrice = 320.00m, SaleNote = saleNotes[10], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Corticosteroide", Quantity = 1, UnitPrice = 350.00m, ConceptPrice = 350.00m, SaleNote = saleNotes[10], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 11
                new SaleNoteConceptEntity { Description = "Radiografía articular", Quantity = 1, UnitPrice = 700.00m, ConceptPrice = 700.00m, SaleNote = saleNotes[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Antiinflamatorio oral", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Suplemento articular premium", Quantity = 1, UnitPrice = 245.00m, ConceptPrice = 245.00m, SaleNote = saleNotes[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 12
                new SaleNoteConceptEntity { Description = "Revisión oftalmológica", Quantity = 1, UnitPrice = 450.00m, ConceptPrice = 450.00m, SaleNote = saleNotes[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Lubricante ocular artificial", Quantity = 1, UnitPrice = 220.00m, ConceptPrice = 220.00m, SaleNote = saleNotes[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Gotas antibacterianas", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 13
                new SaleNoteConceptEntity { Description = "Consulta alergia conjuntival", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[13], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Gotas oftálmicas antihistamínicas", Quantity = 1, UnitPrice = 120.00m, ConceptPrice = 120.00m, SaleNote = saleNotes[13], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 14
                new SaleNoteConceptEntity { Description = "evaluación GI urgente", Quantity = 1, UnitPrice = 500.00m, ConceptPrice = 500.00m, SaleNote = saleNotes[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "antiemético IV", Quantity = 1, UnitPrice = 150.00m, ConceptPrice = 150.00m, SaleNote = saleNotes[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConceptEntity { Description = "Protector gástrico oral", Quantity = 1, UnitPrice = 65.00m, ConceptPrice = 65.00m, SaleNote = saleNotes[14], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.SaleNoteConcepts.AddRange(concepts);
            return concepts;
        }

        /// <summary>
        /// Seed appointments with owner and pet relationships
        /// Multiple appointments per owner/pet with various statuses
        /// </summary>
        private static List<AppointmentEntity> SeedAppointments(DogVetContext context, List<OwnerEntity> owners, List<PetEntity> pets, DateTime seedDate)
        {
            var appointments = new List<AppointmentEntity>
            {
                // Scheduled appointments
                new AppointmentEntity { Date = new DateTime(2026, 2, 1, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 2, 11, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 3, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 5, 9, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[1], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 5, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[1], Pet = pets[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 8, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[2], Pet = pets[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 10, 13, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[3], Pet = pets[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 12, 11, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[4], Pet = pets[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 15, 9, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[5], Pet = pets[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 18, 14, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[6], Pet = pets[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 20, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[7], Pet = pets[16], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 22, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[8], Pet = pets[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 25, 11, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[9], Pet = pets[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Completed appointments
                new AppointmentEntity { Date = new DateTime(2026, 1, 30, 9, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[2], Pet = pets[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 28, 13, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[1], Pet = pets[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 25, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[3], Pet = pets[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 20, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[4], Pet = pets[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 15, 11, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[0], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 12, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[5], Pet = pets[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 10, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[6], Pet = pets[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 8, 16, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[7], Pet = pets[16], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 5, 13, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[8], Pet = pets[18], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2025, 12, 28, 11, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[9], Pet = pets[21], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Cancelled appointments
                new AppointmentEntity { Date = new DateTime(2026, 2, 10, 11, 15, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[3], Pet = pets[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 7, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[1], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 2, 3, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[4], Pet = pets[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new AppointmentEntity { Date = new DateTime(2026, 1, 31, 15, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[2], Pet = pets[7], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Appointments.AddRange(appointments);
            return appointments;
        }
    }
}

