namespace DogVetAPI.Data.Entities.Enums
{
    public enum Species
    {
        Dog = 0,
        Cat = 1,
        Bird = 2,
        Rabbit = 3,
        Hamster = 4,
        Other = 5
    }

    /// <summary>
    /// Extension methods for Species enum
    /// </summary>
    public static class SpeciesExtensions
    {
        /// <summary>
        /// Converts a string to Species enum
        /// </summary>
        public static Species? StringToEnum(string? speciesStr)
        {
            if (string.IsNullOrEmpty(speciesStr))
                return null;
            
            return Enum.TryParse<Species>(speciesStr, out var result) ? result : null;
        }

        /// <summary>
        /// Validates if the provided species string is a valid Species enum value
        /// </summary>
        public static bool IsValidSpecies(string species)
        {
            return StringToEnum(species) != null;
        }
    }
}

