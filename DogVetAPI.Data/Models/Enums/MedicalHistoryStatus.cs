namespace DogVetAPI.Data.Models.Enums
{
    public enum MedicalHistoryStatus
    {
        Completed = 1,
        FollowUp = 2
    }

    public static class MedicalHistoryStatusStrings
    {
        public const string Completed = "Completed";
        public const string FollowUp  = "Follow-up";

        public static string FromEnum(MedicalHistoryStatus status) => status switch
        {
            MedicalHistoryStatus.Completed => Completed,
            MedicalHistoryStatus.FollowUp  => FollowUp,
            _                              => FollowUp
        };
    }
}
