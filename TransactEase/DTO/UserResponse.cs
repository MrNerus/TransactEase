using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransactEase.Enums;

namespace TransactEase.DTO
{
    public class UserResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.INFO;
        public T? Data { get; set; } = default;
    }

    public class UserResponse : UserResponse<dynamic>
    {
    }

    public static class UserResponseExtension
    {
        public static void SaveLog<T>(this UserResponse<T> userResponse)
        {
            string logPath = $"Logs/UserException-{DateTime.Now:yyyy.MM.dd}.log";
            string logMessage = $"{DateTime.Now:HH:mm:ss} ~~ {Enum.GetName(userResponse.Status)} ~~ {JsonConvert.SerializeObject(userResponse.Data)}";

            File.AppendAllText(logPath, logMessage + Environment.NewLine);
            // Who made API call, What was Payload?
        }
        public static void SaveLog(this UserResponse userResponse)
        {
            string logPath = $"Logs/UserException-{DateTime.Now:yyyy.MM.dd}.log";
            string logMessage = $"{DateTime.Now:HH:mm:ss} ~~ {Enum.GetName(userResponse.Status)} ~~ {JsonConvert.SerializeObject(userResponse.Data)}";

            File.AppendAllText(logPath, logMessage + Environment.NewLine);
            // Who made API call, What was Payload?
        }
    }
}