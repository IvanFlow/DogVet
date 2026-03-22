namespace DogVetAPI.Data.Models.Enums
{
    public enum MedicalHistoryStatus
    {
        Completed = 1,
        FollowUp = 2,
        Pending = 3
    }

    public static class MedicalHistoryStatusStrings
    {
        public const string Completed = "Completed";
        public const string FollowUp  = "Follow-up";
        public const string Pending   = "Pending";

        public static string FromEnum(MedicalHistoryStatus status) => status switch
        {
            MedicalHistoryStatus.Completed => Completed,
            MedicalHistoryStatus.FollowUp  => FollowUp,
            MedicalHistoryStatus.Pending   => Pending,
            _                              => Pending
        };
    }
}
