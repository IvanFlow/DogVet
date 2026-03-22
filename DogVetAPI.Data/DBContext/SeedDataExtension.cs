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
        /// Supporting multiple pets (2-3) per owner with rich medical histories
        /// </summary>
        private static List<Owner> SeedOwners(DogVetContext context, DateTime seedDate)
        {
            var owners = new List<Owner>
            {
                // Owner 1: Has 3 pets
                new Owner { FirstName = "Carlos", LastName = "Gutierrez Ramirez", Email = "carlos.gutierrez@gmail.com", PhoneNumber = "3312345678", Address = "Av. Patria 1230, Col. Jardines de Guadalupe", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 2: Has 2 pets
                new Owner { FirstName = "Maria", LastName = "Lopez Hernandez", Email = "maria.lopez@gmail.com", PhoneNumber = "3323456789", Address = "Calle Robles 456, Col. La Calma", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 3: Has 3 pets
                new Owner { FirstName = "Jose", LastName = "Martinez Torres", Email = "jose.martinez@hotmail.com", PhoneNumber = "3334567890", Address = "Blvd. Puerta de Hierro 789, Col. Puerta de Hierro", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 4: Has 1 pet
                new Owner { FirstName = "Ana", LastName = "Sanchez Flores", Email = "ana.sanchez@gmail.com", PhoneNumber = "3345678901", Address = "Calle Tesistan 321, Col. Lomas de Zapopan", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 5: Has 2 pets
                new Owner { FirstName = "Luis", LastName = "Perez Mendoza", Email = "luis.perez@outlook.com", PhoneNumber = "3356789012", Address = "Av. Vallarta 4850, Col. Camino Real", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 6: Has 3 pets
                new Owner { FirstName = "Gabriela", LastName = "Reyes Vargas", Email = "gabriela.reyes@gmail.com", PhoneNumber = "3367890123", Address = "Calle Andares 150, Col. Andares", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 7: Has 2 pets
                new Owner { FirstName = "Francisco", LastName = "Mendez Corona", Email = "francisco.mendez@gmail.com", PhoneNumber = "3378901234", Address = "Av. Mexico 2050, Col. Monumental", City = "Guadalajara", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 8: Has 1 pet
                new Owner { FirstName = "Patricia", LastName = "Ruiz Molina", Email = "patricia.ruiz@gmail.com", PhoneNumber = "3389012345", Address = "Calle 8 de Julio 456, Col. Centro", City = "Guadalajara", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 9: Has 3 pets
                new Owner { FirstName = "Antonio", LastName = "Campos Navarro", Email = "antonio.campos@hotmail.com", PhoneNumber = "3390123456", Address = "Paseo Montepios 1500, Col. Santa Paula", City = "Tlaquepaque", CreatedAt = seedDate, UpdatedAt = seedDate },
                // Owner 10: Has 2 pets
                new Owner { FirstName = "Sofia", LastName = "Valencia Gutierrez", Email = "sofia.valencia@gmail.com", PhoneNumber = "3301234567", Address = "Av. Tepeyac 890, Col. El Colli", City = "Zapopan", CreatedAt = seedDate, UpdatedAt = seedDate }
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
                new Veterinarian { FirstName = "Laura", LastName = "Castillo Medina", Email = "laura.castillo@dogvet.com", PhoneNumber = "3322221111", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Roberto", LastName = "Morales Jimenez", Email = "roberto.morales@dogvet.com", PhoneNumber = "3322222222", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Patricia", LastName = "Ochoa Navarro", Email = "patricia.ochoa@dogvet.com", PhoneNumber = "3322233333", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Eduardo", LastName = "Ibarra Solis", Email = "eduardo.ibarra@dogvet.com", PhoneNumber = "3322244444", IsActive = false, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Veterinarian { FirstName = "Diana", LastName = "Cruz Aguilar", Email = "diana.cruz@dogvet.com", PhoneNumber = "3322255555", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Veterinarians.AddRange(vets);
            return vets;
        }

        /// <summary>
        /// Seed pets table with owner relationships
        /// Each owner has 2-3 pets with diverse breeds and ages
        /// </summary>
        private static List<Pet> SeedPets(DogVetContext context, List<Owner> owners, DateTime seedDate)
        {
            var pets = new List<Pet>
            {
                // Carlos Gutierrez (owner[0]) - 3 pets
                new Pet { Name = "Canelo", Breed = "Golden Retriever", Age = 3, Weight = 30.5, Color = "Dorado", Gender = "Macho", DateOfBirth = new DateTime(2023, 3, 10, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Bella", Breed = "Labrador", Age = 2, Weight = 28.0, Color = "Chocolate", Gender = "Hembra", DateOfBirth = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Rocky", Breed = "German Shepherd", Age = 4, Weight = 35.2, Color = "Negro y cafe", Gender = "Macho", DateOfBirth = new DateTime(2022, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Maria Lopez (owner[1]) - 2 pets
                new Pet { Name = "Negra", Breed = "Poodle Negro", Age = 2, Weight = 12.3, Color = "Negro", Gender = "Hembra", DateOfBirth = new DateTime(2024, 5, 5, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Max", Breed = "Boxer", Age = 5, Weight = 32.0, Color = "Bayo", Gender = "Macho", DateOfBirth = new DateTime(2021, 2, 10, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Jose Martinez (owner[2]) - 3 pets
                new Pet { Name = "Titan", Breed = "Beagle", Age = 5, Weight = 12.3, Color = "Tricolor", Gender = "Macho", DateOfBirth = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Luna", Breed = "Cocker Spaniel", Age = 3, Weight = 14.5, Color = "Dorado", Gender = "Hembra", DateOfBirth = new DateTime(2023, 4, 8, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Charlie", Breed = "Schnauzer", Age = 1, Weight = 8.5, Color = "Gris", Gender = "Macho", DateOfBirth = new DateTime(2025, 3, 12, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Ana Sanchez (owner[3]) - 1 pet
                new Pet { Name = "Princesa", Breed = "Shih Tzu", Age = 1, Weight = 8.7, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2025, 2, 14, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Luis Perez (owner[4]) - 2 pets
                new Pet { Name = "Thor", Breed = "Pastor Aleman", Age = 4, Weight = 35.2, Color = "Negro con cafe", Gender = "Macho", DateOfBirth = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = false, Owner = owners[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Nala", Breed = "Husky", Age = 3, Weight = 25.0, Color = "Blanco y gris", Gender = "Hembra", DateOfBirth = new DateTime(2023, 8, 25, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Gabriela Reyes (owner[5]) - 3 pets
                new Pet { Name = "Gordo", Breed = "Bulldog Ingles", Age = 3, Weight = 28.0, Color = "Bayo", Gender = "Macho", DateOfBirth = new DateTime(2023, 5, 12, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Mimi", Breed = "Maltesa", Age = 2, Weight = 4.5, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2024, 7, 22, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Rex", Breed = "Doberman", Age = 4, Weight = 32.0, Color = "Negro y fuego", Gender = "Macho", DateOfBirth = new DateTime(2022, 4, 5, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Francisco Mendez (owner[6]) - 2 pets
                new Pet { Name = "Coco", Breed = "Pug", Age = 5, Weight = 8.0, Color = "Negro", Gender = "Macho", DateOfBirth = new DateTime(2021, 11, 30, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Laila", Breed = "Dachshund", Age = 3, Weight = 6.5, Color = "Cafe oscuro", Gender = "Hembra", DateOfBirth = new DateTime(2023, 9, 14, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Patricia Ruiz (owner[7]) - 1 pet
                new Pet { Name = "Toby", Breed = "Jack Russell Terrier", Age = 2, Weight = 6.2, Color = "Blanco con cafe", Gender = "Macho", DateOfBirth = new DateTime(2024, 10, 8, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Antonio Campos (owner[8]) - 3 pets
                new Pet { Name = "Fido", Breed = "Rottweiler", Age = 4, Weight = 40.0, Color = "Negro y fuego", Gender = "Macho", DateOfBirth = new DateTime(2022, 6, 18, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Daisy", Breed = "Pinscher", Age = 2, Weight = 5.5, Color = "Rojo", Gender = "Hembra", DateOfBirth = new DateTime(2024, 8, 3, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Duke", Breed = "Mastiff", Age = 6, Weight = 55.0, Color = "Cafe claro", Gender = "Macho", DateOfBirth = new DateTime(2020, 5, 27, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sofia Valencia (owner[9]) - 2 pets
                new Pet { Name = "Pelusa", Breed = "Poodle Blanco", Age = 1, Weight = 10.0, Color = "Blanco", Gender = "Hembra", DateOfBirth = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Pet { Name = "Simba", Breed = "Cocker Spaniel", Age = 4, Weight = 16.0, Color = "Cafe y blanco", Gender = "Macho", DateOfBirth = new DateTime(2022, 12, 10, 0, 0, 0, DateTimeKind.Utc), IsActive = true, Owner = owners[9], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Pets.AddRange(pets);
            return pets;
        }

        /// <summary>
        /// Seed medical histories with diverse diagnoses, statuses, and follow-ups
        /// Each pet has 2-4 medical records with varied conditions and treatments
        /// </summary>
        private static List<MedicalHistory> SeedMedicalHistories(DogVetContext context, List<Pet> pets, List<Veterinarian> veterinarians, DateTime seedDate)
        {
            var histories = new List<MedicalHistory>
            {
                // Canelo (pets[0]) - Golden Retriever
                new MedicalHistory { Diagnosis = "Revisión general anual con vacunación", Notes = "Signos vitales normales, vacunas DHPP y antirrábica al día. Peso adecuado.", VisitDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[0], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Limpieza dental preventiva", Notes = "Acumulación leve de sarro, detartraje realizado sin complicaciones", VisitDate = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[0], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Otitis externa bilateral", Notes = "Infecciones de oído relacionadas con alergias. Gotas y medicamento oral prescritos.", VisitDate = new DateTime(2025, 12, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 17, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[0], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Bella (pets[1]) - Labrador
                new MedicalHistory { Diagnosis = "Vacunación de rutina para cachorra", Notes = "Primera dosis de DHPP aplicada. Revisar en 3-4 semanas para refuerzo.", VisitDate = new DateTime(2024, 8, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2024, 9, 5, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[1], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Refuerzo de vacuna DHPP", Notes = "Segunda dosis de DHPP aplicada correctamente. Será revacunada en 1 año.", VisitDate = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[1], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Alergia alimentaria con dermatitis", Notes = "Cambio de dieta recomendado. Medicamento antihistamínico prescrito.", VisitDate = new DateTime(2025, 10, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 11, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[1], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión de seguimiento nutrición", Notes = "Se evalúa respuesta al nuevo alimento. Mejora notoria en condición de piel.", VisitDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[1], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Rocky (pets[2]) - German Shepherd
                new MedicalHistory { Diagnosis = "Displasia de cadera - evaluación radiológica", Notes = "Radiografías realizadas. Grado moderado de displasia. Fisioterapia recomendada.", VisitDate = new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[2], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Control de peso y condición muscular", Notes = "Reducción de actividad debido a displasia. Dieta especial iniciada.", VisitDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[2], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión anual con pruebas de sangre", Notes = "Análisis completo realizado. Resultados dentro de parámetros normales.", VisitDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[2], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Negra (pets[3]) - Poodle Negro
                new MedicalHistory { Diagnosis = "Otitis externa con infección bacteriana", Notes = "Cultivo realizado. Gotas antibacterianas prescritas. Control en 2 semanas.", VisitDate = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 22, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[3], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Limpieza de oidos y revisión dental", Notes = "Higiene correcta. Se recomienda limpieza semanal de oídos.", VisitDate = new DateTime(2025, 12, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[3], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión pre-grooming y vacunación", Notes = "Estado general excelente. Vacuna antirrábica aplicada.", VisitDate = new DateTime(2025, 10, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[3], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Max (pets[4]) - Boxer
                new MedicalHistory { Diagnosis = "Displasia de cadera leve", Notes = "Radiografías muestran displasia leve. Seguimiento clínico recomendado.", VisitDate = new DateTime(2025, 8, 12, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 12, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[4], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Control articular y evaluación de dolor", Notes = "Sin evidencia de dolor. Peso adecuado. Suplemento articular iniciado.", VisitDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[4], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Vacuna antirrábica de refuerzo", Notes = "Refuerzo aplicado sin complicaciones.", VisitDate = new DateTime(2025, 11, 25, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[4], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Titan (pets[5]) - Beagle
                new MedicalHistory { Diagnosis = "Otitis media recurrente", Notes = "Historial de infecciones de oído. Evaluación auditiva recomendada.", VisitDate = new DateTime(2025, 12, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[5], Veterinarian = veterinarians[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Limpieza dental - eliminación de cálculo", Notes = "Detartraje completo realizado. Higiene oral deficiente. Cepillado diario recomendado.", VisitDate = new DateTime(2025, 10, 8, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 4, 8, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[5], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión geriátrica completa", Notes = "Perro adulto mayor evaluado. Signos vitales estables. Seguimiento preventivo recomendado.", VisitDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 6, 12, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[5], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Luna (pets[6]) - Cocker Spaniel
                new MedicalHistory { Diagnosis = "Queratitis ulcerativa en ojo derecho", Notes = "Úlcera corneal superficial. Lubricantes oftálmicos y antibióticos prescritos.", VisitDate = new DateTime(2025, 11, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 12, 4, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[6], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión oftalmológica post-tratamiento", Notes = "Úlcera cicatrizada completamente. Visión recuperada.", VisitDate = new DateTime(2025, 12, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[6], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Vacunación de rutina", Notes = "Vacunas DHPP y antirrábica aplicadas. Presencia de ectoparásitos detectada.", VisitDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 24, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[6], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Charlie (pets[7]) - Schnauzer
                new MedicalHistory { Diagnosis = "Desparasitación cachorro - interna y externa", Notes = "Parásitos internos detectados en análisis fecal. Tratamiento completo iniciado.", VisitDate = new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[7], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Refuerzo de desparasitación", Notes = "Análisis fecal repetido. Control negativo. Cachorro en buena salud.", VisitDate = new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[7], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Vacunación primaria - DHPP", Notes = "Primera dosis de vacuna polivalente aplicada correctamente.", VisitDate = new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 6, 20, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[7], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Princesa (pets[8]) - Shih Tzu
                new MedicalHistory { Diagnosis = "Conjuntivitis alérgica bilateral", Notes = "Inflamación leve de conjuntivas. Gotas oftálmicas y antihistamínico prescrito.", VisitDate = new DateTime(2025, 12, 28, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[8], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Limpieza de oídos preventiva", Notes = "Orejas largas propensas a infecciones. Limpieza completa realizada.", VisitDate = new DateTime(2025, 11, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[8], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Vacunación de cachorra", Notes = "DHPP y antirrábica iniciadas. Cachorrita pequeña pero saludable.", VisitDate = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[8], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Thor (pets[9]) - Pastor Aleman (Inactivo)
                new MedicalHistory { Diagnosis = "Enfermedad articular degenerativa avanzada", Notes = "Displasia severa progresiva. Medicamentos para dolor prescritos. Pronóstico limitado.", VisitDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[9], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Evaluación gerátrica integral", Notes = "Perro adulto mayor con movilidad reducida. Calidad de vida evaluada.", VisitDate = new DateTime(2025, 8, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[9], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Nala (pets[10]) - Husky
                new MedicalHistory { Diagnosis = "Alopecia estacional seasonal", Notes = "Caída excesiva de pelo típica de la raza. Suplemento de ácidos grasos prescrito.", VisitDate = new DateTime(2025, 12, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[10], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión post-muda", Notes = "Muda completada. Pelaje en excelente condición. Recomendación de cepillado frecuente mantenida.", VisitDate = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[10], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Gordo (pets[11]) - Bulldog Ingles
                new MedicalHistory { Diagnosis = "Síndrome braquicefálico - dificultad respiratoria", Notes = "Respiración ruidosa y limitación en ejercicio. Cirugía correctiva evaluada.", VisitDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[11], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Evaluación pre-quirurgica", Notes = "Análisis de sangre y radiografías completadas. Apto para cirugía.", VisitDate = new DateTime(2025, 10, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[11], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Control de peso y dieta", Notes = "Sobrepeso leve detectado. Dieta baja en calorías recomendada.", VisitDate = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 4, 3, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[11], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Mimi (pets[12]) - Maltesa
                new MedicalHistory { Diagnosis = "Calculus dental y enfermedad periodontal", Notes = "Acumulación severa de sarro. Limpieza dental urgente recomendada.", VisitDate = new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 6, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[12], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión rutinaria con vacunación", Notes = "Signos vitales normales. DHPP y antirrábica aplicadas.", VisitDate = new DateTime(2025, 10, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[12], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Rex (pets[13]) - Doberman
                new MedicalHistory { Diagnosis = "Cardiomiopatía dilatada - evaluación ecocardiográfica", Notes = "Función cardíaca reducida. Medicamentos para corazón prescritos. Restricción de ejercicio.", VisitDate = new DateTime(2025, 9, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 2, 20, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[13], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Control cardíaco - evaluación de medicamentos", Notes = "Frecuencia cardíaca mejorando con tratamiento. Continuación de fármacos indicada.", VisitDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[13], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión anual de rutina", Notes = "Dieta cardíaca y medicamentos continuados. Estado general estable.", VisitDate = new DateTime(2025, 12, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[13], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Coco (pets[14]) - Pug
                new MedicalHistory { Diagnosis = "Obesidad - plan de pérdida de peso", Notes = "Sobrepeso severo. Dieta restrictiva calórica iniciada. Ejercicio progresivo recomendado.", VisitDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[14], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión oftalmológica preventiva", Notes = "Ojos prominentes evaluados. Sin úlceras detectadas. Limpieza de arrugas realizada.", VisitDate = new DateTime(2025, 11, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[14], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Laila (pets[15]) - Dachshund
                new MedicalHistory { Diagnosis = "Parálisis lumbar - disco intervertebral herniad", Notes = "Pérdida parcial de movilidad trasera. Medicamentos y reposo prescrito.", VisitDate = new DateTime(2025, 8, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[15], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Evaluación de recuperación neuromuscular", Notes = "Mejora leve en movilidad. Fisioterapia continuada recomendada.", VisitDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[15], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Toby (pets[16]) - Jack Russell Terrier
                new MedicalHistory { Diagnosis = "Luxación patelar unilateral grado II", Notes = "Rótula dislocada ocasionalmente. Seguimiento clínico recomendado. Cirugía evaluada.", VisitDate = new DateTime(2025, 10, 25, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 4, 25, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[16], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión post-control - comportamiento y movilidad", Notes = "Perro activo. Sin síntomas agudos. Observación continuada recomendada.", VisitDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[16], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Fido (pets[17]) - Rottweiler
                new MedicalHistory { Diagnosis = "Gastroenteritis aguda - vómitos y diarrea", Notes = "Probable intoxicación alimentaria. Fluidoterapia y medicamentos antiemético prescritos.", VisitDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 19, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[17], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Revisión anual de rutina", Notes = "Signos vitales normales. Vacunaciones al día. Peso adecuado.", VisitDate = new DateTime(2025, 12, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[17], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Daisy (pets[18]) - Miniature Pinscher
                new MedicalHistory { Diagnosis = "Vacunación de cachorra - serie primaria", Notes = "Primera dosis DHPP y antirrábica aplicadas sin reacción.", VisitDate = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[18], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Refuerzo de vacunación", Notes = "Segunda dosis DHPP aplicada. Tercera dosis en 3-4 semanas.", VisitDate = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[18], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Caída de diente de cachorro - erupción normal", Notes = "Cambio adeacuado de dentición. Sin retención de dientes deciduos.", VisitDate = new DateTime(2025, 8, 10, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[18], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Duke (pets[19]) - Mastiff
                new MedicalHistory { Diagnosis = "Artritis degenerativa - etapa avanzada", Notes = "Movilidad limitada. Medicamentos para dolor y condroprotectores prescritos.", VisitDate = new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), Status = "Seguimiento", Pet = pets[19], Veterinarian = veterinarians[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Evaluación de calidad de vida - perro geriátrico", Notes = "Confort y medicación evaluados. Cama ortopédica recomendada.", VisitDate = new DateTime(2026, 1, 18, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[19], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Pelusa (pets[20]) - Poodle Blanco
                new MedicalHistory { Diagnosis = "Control de cachorra - revisión integral", Notes = "Cachorra sana. Crecimiento adecuado. Vacunación iniciada.", VisitDate = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[20], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Serie de vacunación primaria completada", Notes = "Tercera dosis DHPP aplicada. Protección completa alcanzada.", VisitDate = new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[20], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Simba (pets[21]) - Cocker Spaniel
                new MedicalHistory { Diagnosis = "Infección de oídos bilateral - otitis externa", Notes = "Inflamación moderada. Gotas antibacterianas y anti-inflamatorias prescritas.", VisitDate = new DateTime(2025, 12, 12, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2025, 12, 26, 0, 0, 0, DateTimeKind.Utc), Status = "Pendiente", Pet = pets[21], Veterinarian = veterinarians[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Limpieza dental y revisión oral", Notes = "Sarro presente. Detartraje recomendado. Higiene dental pobre.", VisitDate = new DateTime(2025, 11, 8, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc), Status = "Completado", Pet = pets[21], Veterinarian = veterinarians[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new MedicalHistory { Diagnosis = "Vacunación anual de refuerzo", Notes = "DHPP y antirrábica aplicadas. Historial de vacuna al día.", VisitDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc), FollowUpDate = null, Status = "Completado", Pet = pets[21], Veterinarian = veterinarians[0], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.MedicalHistories.AddRange(histories);
            return histories;
        }

        /// <summary>
        /// Seed prescriptions with medical history relationships
        /// Multiple prescriptions per condition with realistic dosages and durations
        /// </summary>
        private static List<Prescription> SeedPrescriptions(DogVetContext context, List<MedicalHistory> medicalHistories, DateTime seedDate)
        {
            var prescriptions = new List<Prescription>
            {
                // Otitis externa (medicalHistories[2]) - Canelo
                new Prescription { MedName = "Gotas antibacterianas otitis - Otomax", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Cetirizina 10mg - antihistamínico", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Alergia alimentaria (medicalHistories[5]) - Bella
                new Prescription { MedName = "Cetirizina 10mg - antihistamínico", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Prednisona 5mg - corticosteroide", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Displasia de cadera (medicalHistories[6]) - Rocky
                new Prescription { MedName = "Meloxicam 15mg - antiinflamatorio", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Glucosamina + Condroitina - protector articular", Dose = DoseFrequency.Daily, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Otitis externa (medicalHistories[9]) - Negra
                new Prescription { MedName = "Enrofloxacino gotas - antibiótico", Dose = DoseFrequency.Every12Hours, DurationInDays = 10, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Limpieza de oídos - solución otológica", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Queratitis ulcerativa (medicalHistories[15]) - Luna
                new Prescription { MedName = "Ciprofloxacino gotas - antibiótico oftálmico", Dose = DoseFrequency.Every4Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[15], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Lubricante oftálmico - protector corneal", Dose = DoseFrequency.Every6Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[15], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Desparasitación (medicalHistories[17]) - Charlie
                new Prescription { MedName = "Albendazol - antiparasitario interno", Dose = DoseFrequency.Daily, DurationInDays = 5, Status = PrescriptionStatus.Administered, MedicalHistory = medicalHistories[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Fipronil + Methoprene spray - antiparasitario externo", Dose = DoseFrequency.Weekly, DurationInDays = 21, Status = PrescriptionStatus.Administered, MedicalHistory = medicalHistories[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Conjuntivitis alérgica (medicalHistories[20]) - Princesa
                new Prescription { MedName = "Gotas oftálmicas antihistamínicas", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Cetirizina 10mg - antihistamínico sistémico", Dose = DoseFrequency.Daily, DurationInDays = 21, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Enfermedad articular (medicalHistories[23]) - Thor
                new Prescription { MedName = "Tramadol 50mg - analgésico", Dose = DoseFrequency.Every8Hours, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[23], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Meloxicam 15mg - antiinflamatorio", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[23], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Alopecia estacional (medicalHistories[25]) - Nala
                new Prescription { MedName = "Omega-3 + Omega-6 - suplemento ácidos grasos", Dose = DoseFrequency.Daily, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[25], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Síndrome braquicefálico (medicalHistories[27]) - Gordo
                new Prescription { MedName = "Furosemida 40mg - diurético", Dose = DoseFrequency.Daily, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[27], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Calculus dental (medicalHistories[30]) - Mimi
                new Prescription { MedName = "Clorhexidina enjuague - desinfectante oral", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[30], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Cardiomiopatía (medicalHistories[32]) - Rex
                new Prescription { MedName = "Enalapril 5mg - inhibidor ACE", Dose = DoseFrequency.Every12Hours, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[32], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Furosemida 40mg - diurético", Dose = DoseFrequency.Daily, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[32], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Pimobendan 5mg - inotrópico", Dose = DoseFrequency.Every12Hours, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[32], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Obesidad (medicalHistories[35]) - Coco
                new Prescription { MedName = "Orlistat 120mg - inhibidor lipasa", Dose = DoseFrequency.Every8Hours, DurationInDays = 90, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[35], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Parálisis lumbar (medicalHistories[38]) - Laila
                new Prescription { MedName = "Metilprednisolona 4mg - corticosteroide", Dose = DoseFrequency.Daily, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[38], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Gabapentina 100mg - analgésico neuropático", Dose = DoseFrequency.Every8Hours, DurationInDays = 30, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[38], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Gastroenteritis (medicalHistories[42]) - Fido
                new Prescription { MedName = "Metoclopramida 10mg - antiemético", Dose = DoseFrequency.Every8Hours, DurationInDays = 5, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Famotidina 20mg - protector gástrico", Dose = DoseFrequency.Every12Hours, DurationInDays = 7, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Probiótico - flora intestinal", Dose = DoseFrequency.Daily, DurationInDays = 10, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Infección de oídos (medicalHistories[46]) - Simba
                new Prescription { MedName = "Gotas otitis - Otomax", Dose = DoseFrequency.Every12Hours, DurationInDays = 14, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[46], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Prescription { MedName = "Enrofloxacino 100mg - antibiótico sistémico", Dose = DoseFrequency.Daily, DurationInDays = 10, Status = PrescriptionStatus.Prescribed, MedicalHistory = medicalHistories[46], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Prescriptions.AddRange(prescriptions);
            return prescriptions;
        }

        /// <summary>
        /// Seed sale notes with medical history relationships
        /// Payment tracking for various medical treatments
        /// </summary>
        private static List<SaleNote> SeedSaleNotes(DogVetContext context, List<MedicalHistory> medicalHistories, DateTime seedDate)
        {
            var saleNotes = new List<SaleNote>
            {
                // Paid notes
                new SaleNote { NoteDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 750.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 580.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1150.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2025, 12, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 420.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 680.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 950.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 520.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2025, 11, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 780.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[15], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 350.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 640.00m, PaymentStatus = PaymentStatus.Paid, MedicalHistory = medicalHistories[29], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Pending notes
                new SaleNote { NoteDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1270.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 1145.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 850.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2025, 12, 28, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 520.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNote { NoteDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 715.00m, PaymentStatus = PaymentStatus.Pending, MedicalHistory = medicalHistories[42], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.SaleNotes.AddRange(saleNotes);
            return saleNotes;
        }

        /// <summary>
        /// Seed sale note concepts with sale note relationships
        /// Line items for veterinary services and products
        /// </summary>
        private static List<SaleNoteConcept> SeedSaleNoteConcepts(DogVetContext context, List<SaleNote> saleNotes, DateTime seedDate)
        {
            var concepts = new List<SaleNoteConcept>
            {
                // Sale note 0
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Vacuna DHPP", Quantity = 1, UnitPrice = 350.00m, ConceptPrice = 350.00m, SaleNote = saleNotes[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 1
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Limpieza dental", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 2
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Gotas antibacterianas otitis", Quantity = 1, UnitPrice = 250.00m, ConceptPrice = 250.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Medicament oral", Quantity = 1, UnitPrice = 100.00m, ConceptPrice = 100.00m, SaleNote = saleNotes[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 3
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Vacuna antirrábica", Quantity = 1, UnitPrice = 120.00m, ConceptPrice = 120.00m, SaleNote = saleNotes[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 4
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Radiografía", Quantity = 1, UnitPrice = 550.00m, ConceptPrice = 550.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Suplemento articular", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 5
                new SaleNoteConcept { Description = "Revisión general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Prueba alergia", Quantity = 1, UnitPrice = 650.00m, ConceptPrice = 650.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Antihistamínico oral", Quantity = 1, UnitPrice = 220.00m, ConceptPrice = 220.00m, SaleNote = saleNotes[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 6
                new SaleNoteConcept { Description = "Consulta", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Gotas oftálmicas antibióticas", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Lubricante ocular", Quantity = 1, UnitPrice = 120.00m, ConceptPrice = 120.00m, SaleNote = saleNotes[6], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 7
                new SaleNoteConcept { Description = "Consulta general", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Detartraje dental", Quantity = 1, UnitPrice = 750.00m, ConceptPrice = 750.00m, SaleNote = saleNotes[7], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 8
                new SaleNoteConcept { Description = "Desparasitante oral", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Antiparasitario externo", Quantity = 1, UnitPrice = 170.00m, ConceptPrice = 170.00m, SaleNote = saleNotes[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 9
                new SaleNoteConcept { Description = "Revisión nutricional", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Suplemento dietético", Quantity = 1, UnitPrice = 240.00m, ConceptPrice = 240.00m, SaleNote = saleNotes[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 10
                new SaleNoteConcept { Description = "Evaluación alergia", Quantity = 1, UnitPrice = 600.00m, ConceptPrice = 600.00m, SaleNote = saleNotes[10], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Medicament antihistamínico", Quantity = 1, UnitPrice = 320.00m, ConceptPrice = 320.00m, SaleNote = saleNotes[10], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Corticosteroide", Quantity = 1, UnitPrice = 350.00m, ConceptPrice = 350.00m, SaleNote = saleNotes[10], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 11
                new SaleNoteConcept { Description = "Radiografía articular", Quantity = 1, UnitPrice = 700.00m, ConceptPrice = 700.00m, SaleNote = saleNotes[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Antiinflamatorio oral", Quantity = 1, UnitPrice = 200.00m, ConceptPrice = 200.00m, SaleNote = saleNotes[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Suplemento articular premium", Quantity = 1, UnitPrice = 245.00m, ConceptPrice = 245.00m, SaleNote = saleNotes[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 12
                new SaleNoteConcept { Description = "Revisión oftalmológica", Quantity = 1, UnitPrice = 450.00m, ConceptPrice = 450.00m, SaleNote = saleNotes[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Lubricante ocular artificial", Quantity = 1, UnitPrice = 220.00m, ConceptPrice = 220.00m, SaleNote = saleNotes[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Gotas antibacterianas", Quantity = 1, UnitPrice = 180.00m, ConceptPrice = 180.00m, SaleNote = saleNotes[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 13
                new SaleNoteConcept { Description = "Consulta alergia conjuntival", Quantity = 1, UnitPrice = 400.00m, ConceptPrice = 400.00m, SaleNote = saleNotes[13], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Gotas oftálmicas antihistamínicas", Quantity = 1, UnitPrice = 120.00m, ConceptPrice = 120.00m, SaleNote = saleNotes[13], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Sale note 14
                new SaleNoteConcept { Description = "Evaluación GI urgente", Quantity = 1, UnitPrice = 500.00m, ConceptPrice = 500.00m, SaleNote = saleNotes[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Antiemético IV", Quantity = 1, UnitPrice = 150.00m, ConceptPrice = 150.00m, SaleNote = saleNotes[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new SaleNoteConcept { Description = "Protector gástrico oral", Quantity = 1, UnitPrice = 65.00m, ConceptPrice = 65.00m, SaleNote = saleNotes[14], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.SaleNoteConcepts.AddRange(concepts);
            return concepts;
        }

        /// <summary>
        /// Seed appointments with owner and pet relationships
        /// Multiple appointments per owner/pet with various statuses
        /// </summary>
        private static List<Appointment> SeedAppointments(DogVetContext context, List<Owner> owners, List<Pet> pets, DateTime seedDate)
        {
            var appointments = new List<Appointment>
            {
                // Scheduled appointments
                new Appointment { Date = new DateTime(2026, 2, 1, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 2, 11, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[1], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 3, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[0], Pet = pets[2], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 5, 9, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[1], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 5, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[1], Pet = pets[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 8, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[2], Pet = pets[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 10, 13, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[3], Pet = pets[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 12, 11, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[4], Pet = pets[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 15, 9, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[5], Pet = pets[11], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 18, 14, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[6], Pet = pets[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 20, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[7], Pet = pets[16], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 22, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[8], Pet = pets[17], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 25, 11, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Scheduled, Owner = owners[9], Pet = pets[20], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Completed appointments
                new Appointment { Date = new DateTime(2026, 1, 30, 9, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[2], Pet = pets[5], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 28, 13, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[1], Pet = pets[4], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 25, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[3], Pet = pets[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 20, 15, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[4], Pet = pets[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 15, 11, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[0], Pet = pets[0], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 12, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[5], Pet = pets[12], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 10, 10, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[6], Pet = pets[14], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 8, 16, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[7], Pet = pets[16], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 5, 13, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[8], Pet = pets[18], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2025, 12, 28, 11, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Completed, Owner = owners[9], Pet = pets[21], CreatedAt = seedDate, UpdatedAt = seedDate },
                
                // Cancelled appointments
                new Appointment { Date = new DateTime(2026, 2, 10, 11, 15, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[3], Pet = pets[8], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 7, 14, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[1], Pet = pets[3], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 2, 3, 10, 0, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[4], Pet = pets[9], CreatedAt = seedDate, UpdatedAt = seedDate },
                new Appointment { Date = new DateTime(2026, 1, 31, 15, 30, 0, DateTimeKind.Utc), Status = AppointmentStatus.Cancelled, Owner = owners[2], Pet = pets[7], CreatedAt = seedDate, UpdatedAt = seedDate }
            };
            context.Appointments.AddRange(appointments);
            return appointments;
        }
    }
}
