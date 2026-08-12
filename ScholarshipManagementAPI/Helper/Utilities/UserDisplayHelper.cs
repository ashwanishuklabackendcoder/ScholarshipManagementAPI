namespace ScholarshipManagementAPI.Helper.Utilities
{
    public static class UserDisplayHelper
    {
        public static string GetFullName(string? salutation, string? firstName, string? lastName)
        {
            return string.Join(" ", new[] { salutation, firstName, lastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }

}
