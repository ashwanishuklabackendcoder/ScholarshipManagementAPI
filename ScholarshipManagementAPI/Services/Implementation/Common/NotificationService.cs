using Microsoft.EntityFrameworkCore;
using ScholarshipManagementAPI.DTOs.SuperAdmin.AdminEmailTemplate;
using ScholarshipManagementAPI.Helper;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;
using System.Buffers.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IAdminEmailTemplateService _adminEmailTemplateService;

        public NotificationService(IEmailService emailService, IConfiguration config , IAdminEmailTemplateService adminEmailTemplateService)
        {
            _emailService = emailService;
            _config = config;
            _adminEmailTemplateService = adminEmailTemplateService;
        }


        public async Task SendNewUserAccountAsync(
            string toEmail, string loginName, string fullName,
            string organizationName, string tempPassword)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new CustomException("Email address is required.");

            var result = await _adminEmailTemplateService.GetByFilterAsync(
                new AdminEmailTemplateFilterDto
                {
                    SearchText = "New User Account",
                    IsActive = true
                });

            var template = result?.Items?.FirstOrDefault();

            if (template == null)
                throw new InvalidOperationException("Email template 'New User Account' not found.");


            string subject = template.Subject ?? "Your Account Has Been Created";
            string body = template.Template ?? string.Empty;

            // Read config safely
            string baseUrl = _config["AlertSettings:BaseUrl"]
                ?? throw new InvalidOperationException("AlertSettings:BaseUrl is not configured.");

            string logoPath = _config["AlertSettings:LogoPath"]
                ?? throw new InvalidOperationException("AlertSettings:LogoPath is not configured.");

            // Ensure proper URL formatting
            baseUrl = baseUrl.TrimEnd('/') + "/";
            logoPath = logoPath.TrimStart('/');

            string logoUrl = baseUrl + logoPath;

            body = body
                .Replace("###Name###", fullName ?? "")
                .Replace("###username###", loginName ?? "")
                .Replace("###tempPassword###", tempPassword ?? "")
                .Replace("###campusname###", organizationName ?? "")
                .Replace("###siteurl###", baseUrl)
                .Replace("###logoUrl###", logoUrl);

            await _emailService.SendHtmlAsync(toEmail, subject, body);
        }




        public async Task SendForgotUsernameAsync(
            string toEmail, string loginName , 
            string fullName, string organizationName)
        {
            //var subject = "Your Username";

            //var body = BuildForgotUsernameBody(username);


            // Send email
            // await _emailService.SendAsync(toEmail, subject, body);

            if (string.IsNullOrWhiteSpace(toEmail))
                throw new CustomException("Email address is required.");

            // Get template from DB
            var result = await _adminEmailTemplateService.GetByFilterAsync(
                new AdminEmailTemplateFilterDto
                {
                    SearchText = "Forgot Username",
                    IsActive = true
                }
            );

            var template = result?.Items?.FirstOrDefault();

            if (template == null)
                throw new InvalidOperationException("Email template 'Forgot Username' not found.");

            // Safe subject/body
            string subject = template.Subject ?? "Your Username";
            string body = template.Template ?? string.Empty;

            // Read config safely
            string baseUrl = _config["AlertSettings:BaseUrl"]
                ?? throw new InvalidOperationException("AlertSettings:BaseUrl is not configured.");

            string logoPath = _config["AlertSettings:LogoPath"]
                ?? throw new InvalidOperationException("AlertSettings:LogoPath is not configured.");

            // Ensure proper URL formatting
            baseUrl = baseUrl.TrimEnd('/') + "/";
            logoPath = logoPath.TrimStart('/');

            string logoUrl = baseUrl + logoPath;


            // Replace placeholders
            body = body
                .Replace("###Name###", fullName ?? string.Empty)
                .Replace("###username###", loginName ?? string.Empty)
                .Replace("###campusname###", organizationName ?? string.Empty)
                .Replace("###siteurl###", baseUrl ?? string.Empty)
                .Replace("###logoUrl###", logoUrl ?? string.Empty);

            // Send email
            await _emailService.SendHtmlAsync(toEmail, subject, body);
        }


        public async Task SendForgotPasswordAsync(
            string toEmail, string loginName,
            string fullName, string organizationName, string resetLink)
        {
            //var subject = "Reset Your Password";

            //var body = BuildForgotPasswordBody(resetLink);

            //await _emailService.SendAsync(toEmail, subject, body);


            if (string.IsNullOrWhiteSpace(toEmail))
                throw new CustomException("Email address is required.");

            // Get template from DB
            var result = await _adminEmailTemplateService.GetByFilterAsync(
                new AdminEmailTemplateFilterDto
                {
                    SearchText = "Forgot Password",
                    IsActive = true
                }
            );

            var template = result?.Items?.FirstOrDefault();

            if (template == null)
                throw new InvalidOperationException("Email template 'Forgot Password' not found.");

            // Safe subject/body
            string subject = template.Subject ?? "Forgot Password";
            string body = template.Template ?? string.Empty;

            // Read config safely
            string baseUrl = _config["AlertSettings:BaseUrl"]
                ?? throw new InvalidOperationException("AlertSettings:BaseUrl is not configured.");

            string logoPath = _config["AlertSettings:LogoPath"]
                ?? throw new InvalidOperationException("AlertSettings:LogoPath is not configured.");

            // Ensure proper URL formatting
            baseUrl = baseUrl.TrimEnd('/') + "/";
            logoPath = logoPath.TrimStart('/');

            string logoUrl = baseUrl + logoPath;


            // Replace placeholders
            body = body
                .Replace("###Name###", fullName ?? string.Empty)
                .Replace("###username###", loginName ?? string.Empty)
                .Replace("###resetlink###", resetLink ?? string.Empty)
                .Replace("###expirytime###", "10" ?? string.Empty)
                .Replace("###campusname###", organizationName ?? string.Empty)
                .Replace("###siteurl###", baseUrl ?? string.Empty)
                .Replace("###logoUrl###", logoUrl ?? string.Empty);

            // Send email
            await _emailService.SendHtmlAsync(toEmail, subject, body);

        }


        public async Task SendLoginCodeAsync(
            string toEmail, string loginName,
            string fullName, string organizationName,
            string code , string expiryMin)
        {
            //var subject = "Your Login Verification Code";
            //var body = BuildLoginWithCodeBody(code);

            //await _emailService.SendAsync(toEmail, subject, body);

            if (string.IsNullOrWhiteSpace(toEmail))
                throw new CustomException("Email address is required.");

            // Get template from DB
            var result = await _adminEmailTemplateService.GetByFilterAsync(
                new AdminEmailTemplateFilterDto
                {
                    SearchText = "Login With Code",
                    IsActive = true
                }
            );

            var template = result?.Items?.FirstOrDefault();

            if (template == null)
                throw new InvalidOperationException("Email template 'Login With Code' not found.");

            // Safe subject/body
            string subject = template.Subject ?? "Login With Code";
            string body = template.Template ?? string.Empty;

            // Read config safely
            string baseUrl = _config["AlertSettings:BaseUrl"]
                ?? throw new InvalidOperationException("AlertSettings:BaseUrl is not configured.");

            string logoPath = _config["AlertSettings:LogoPath"]
                ?? throw new InvalidOperationException("AlertSettings:LogoPath is not configured.");

            // Ensure proper URL formatting
            baseUrl = baseUrl.TrimEnd('/') + "/";
            logoPath = logoPath.TrimStart('/');

            string logoUrl = baseUrl + logoPath;


            // Replace placeholders
            body = body
                .Replace("###Name###", fullName ?? string.Empty)
                .Replace("###username###", loginName ?? string.Empty)
                .Replace("###loginCode###", code ?? string.Empty)
                .Replace("###expiryMinutes###", expiryMin ?? "10")
                .Replace("###campusname###", organizationName ?? string.Empty)
                .Replace("###siteurl###", baseUrl ?? string.Empty)
                .Replace("###logoUrl###", logoUrl ?? string.Empty);

            // Send email
            await _emailService.SendHtmlAsync(toEmail, subject, body);

        }




        public async Task SendExceptionAlertAsync(
            Exception ex, string source)
        {
            var adminEmail = _config["AlertSettings:AdminEmail"];

            if (string.IsNullOrWhiteSpace(adminEmail))
            {
                // Fail fast – configuration error
                throw new InvalidOperationException(
                    "AlertSettings:AdminEmail is not configured.");
            }


            //var subject = $"Application Error ({source})";
            //var body = BuildExceptionBody(ex, source);

            //await _emailService.SendAsync(adminEmail, subject, body);


            // Get template from DB
            var result = await _adminEmailTemplateService.GetByFilterAsync(
                new AdminEmailTemplateFilterDto
                {
                    SearchText = "Exception Alert",
                    IsActive = true
                }
            );

            var template = result?.Items?.FirstOrDefault();

            if (template == null)
                throw new InvalidOperationException("Email template 'Exception Alert' not found.");

            string subject = $"{template.Subject} ({source})";
            string body = template.Template ?? string.Empty;

            string baseUrl = _config["AlertSettings:BaseUrl"] ?? "";
            string logoPath = _config["AlertSettings:LogoPath"] ?? "";

            baseUrl = baseUrl.TrimEnd('/') + "/";
            logoPath = logoPath.TrimStart('/');

            string logoUrl = baseUrl + logoPath;

            body = body
                .Replace("###source###", source ?? "")
                .Replace("###message###", ex.Message ?? "")
                .Replace("###exceptionType###", ex.GetType().FullName ?? "")
                .Replace("###occurredOn###", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("###stackTrace###", ex.StackTrace ?? "")
                .Replace("###siteurl###", baseUrl)
                .Replace("###logoUrl###", logoUrl);

            await _emailService.SendHtmlAsync(adminEmail, subject, body);

        }







        // helper method for forgot username email
        private string BuildForgotUsernameBody(string username)
        {
            return $@"
Hello,

Your username is: {username}

If you did not request this, please contact support immediately.

Thanks,
Support Team
";
        }


        // helper method for forgot password email
        private string BuildForgotPasswordBody(string resetLink)
        {
            return $@"
Hello,

We received a request to reset your password.

To create a new password, click the link below:

{resetLink}

This link will expire in 10 minutes for security reasons.

If you did not request a password reset, you can safely ignore this email.
Your account will remain secure.

Best regards,
Scholarship Management Support Team
";
        }



        // helper method for login code email
        private string BuildLoginWithCodeBody(string code)
        {
            return $@"
Hello,

Your login verification code is: {code}

This code will expire in 10 minutes.
Please enter this code in the application to complete your login.

If you did not request this, please contact support immediately.

Thanks,
Support Team
";
        }


        // helper methods for exception email
        private string BuildExceptionBody(Exception ex, string source)
        {
            return $@"
An exception occurred in the application.

Source:
{source}

Message:
{ex.Message}

Stack Trace:
{ex.StackTrace}

Occurred At (UTC):
{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
";
        }



    }
}
