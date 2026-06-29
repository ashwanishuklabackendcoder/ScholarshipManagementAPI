namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface INotificationService
    {
        Task SendNewUserAccountAsync(string toEmail, string loginName,
            string fullName, string organisationName, string tempPassword);

        Task SendForgotUsernameAsync(string toEmail, string loginName,
            string fullName, string organisationName);
        
        Task SendForgotPasswordAsync(string toEmail, string loginName,
            string fullName, string organisationName, string resetLink);


        Task SendLoginCodeAsync(string toEmail, string loginName,
            string fullName, string organisationName,
            string code, string expiryMin);



        Task SendExceptionAlertAsync(Exception ex, string source);

    }
}
