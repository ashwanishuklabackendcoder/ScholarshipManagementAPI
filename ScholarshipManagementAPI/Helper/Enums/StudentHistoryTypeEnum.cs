namespace ScholarshipManagementAPI.Helper.Enums
{
    public enum StudentHistoryTypeEnum
    {
        // Application
        ApplicationDraftCreated = 1,
        ApplicationDraftCancelled = 2,
        ApplicationSubmittedForReview = 3,
        // application acceptance in process

        // Documents
        DocumentUploaded = 4,
        DocumentDeleted = 5,

        // University - Acceptance
        ApplicationAccepted = 6,
        ApplicationAcceptanceRejected = 7,

        // University - Awarding
        ApplicationAwardingInProcess = 8,
        ApplicationAwarded = 9,
        ApplicationAwardingRejected = 10,

        // Direct Aid Committee - Sponsoring
        ApplicationSponsoringInProcess = 11,
        ApplicationSponsored = 12,
        ApplicationSponsoringRejected = 13,

        // Student Lifecycle
        StudentRegistered = 14,
        StudentFailed = 15,
        StudentDismissed = 16,
        StudentGraduated = 17,

        // Administration
        ApplicationUpdated = 18,
        RemarksUpdated = 19
    }
}
