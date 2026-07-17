namespace ScholarshipManagementAPI.Helper.Enums
{
    public enum StudentHistoryTypeEnum
    {
        // Application
        ApplicationDraftCreated = 1,
        ApplicationDraftCancelled = 2,
        ApplicationSubmitted = 3,

        // Documents
        DocumentUploaded = 4,
        DocumentDeleted = 5,

        // Future Workflow
        ApplicationRejected = 6,
        ApplicationSponsored = 7,
        ApplicationAwarded = 8,
        StudentRegistered = 9,
        StudentFailed = 10,
        StudentDismissed = 11,
        StudentGraduated = 12,

        // Administration
        ApplicationUpdated = 13,
        RemarksUpdated = 14
    }
}
