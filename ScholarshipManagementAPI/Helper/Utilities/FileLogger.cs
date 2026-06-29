using Azure.Core;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace ScholarshipManagementAPI.Helper.Utilities
{
    public class FileLogger
    {
        private static readonly string logDir = Path.Combine(Directory.GetCurrentDirectory(), "Helper/Logs");
        private static readonly object lockObj = new object();

        static FileLogger()
        {
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        private static void WriteLog(string fileName, string content)
        {
            var path = Path.Combine(logDir, fileName);

            lock (lockObj)
            {
                File.AppendAllText(path, content);
            }
        }

        public static void LogRequest(
            string requestId,
            string endpoint,
            int statusCode,
            long duration,
            string ip,
            string loginId,
            string userAgent)
        {

            var log = $@"
[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}]
RequestId : {requestId}
Type      : REQUEST
LoginId   : {loginId}
Endpoint  : {endpoint}
Status    : {statusCode}
Duration  : {duration} ms
IP        : {ip}
UserAgent : {userAgent}
------------------------------------------------
";

            WriteLog($"request-{DateTime.UtcNow:yyyy-MM-dd}.txt", log);
        
        }


        public static void LogError(string requestId, Exception ex, string endpoint)
        {

            var log = $@"
[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}]
RequestId : {requestId}
Type      : ERROR
Endpoint  : {endpoint}
Message   : {ex.Message}
StackTrace: {ex.StackTrace}
------------------------------------------------
";

            WriteLog($"error-{DateTime.UtcNow:yyyy-MM-dd}.txt", log);
        
        }

        public static void LogActivity(
            string requestId,
            long userId,
            string module,
            string action,
            string description)
        {

            var log = $@"
[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}]
RequestId : {requestId}
Type      : ACTIVITY
UserId    : {userId}
Module    : {module}
Action    : {action}
Description : {description}
------------------------------------------------
";

            WriteLog($"activity-{DateTime.UtcNow:yyyy-MM-dd}.txt", log);
       
        }
   
    
    }
}


//var requestId = HttpContext.Items["RequestId"]?.ToString() ?? "N/A";

//FileLogger.LogActivity(
//    requestId,
//    userId,
//    "Scholarship",
//    "Create",
//    $"Scholarship '{dto.Name}' created"
//);


//FileLogger.LogActivity(
//    requestId,
//    userId,
//    "Application",
//    "Approve",
//    $"Application {applicationId} approved"
//);


//FileLogger.LogActivity(
//    requestId,
//    userId,
//    "Application",
//    "Approve",
//    $"Application {applicationId} rejected"
//);