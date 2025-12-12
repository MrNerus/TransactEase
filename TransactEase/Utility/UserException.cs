using System;
using System.IO;
using Newtonsoft.Json;
using TransactEase.Enums;

namespace TransactEase.Utility;

public class UserException: Exception
{
    public UserException() {}
    public UserException(dynamic message, bool saveLog = true, StatusEnum status = StatusEnum.INFO) : base((string)(message?.message ?? "No news is good news. There is no news.")) {
        if (saveLog) {
            string logPath = $"Logs/UserException-{DateTime.Now:yyyy.MM.dd}.log";
            string logMessage = $"{DateTime.Now:HH:mm:ss} ~~ {Enum.GetName(status)} ~~ {JsonConvert.SerializeObject(message)}";

            File.AppendAllText(logPath, logMessage + Environment.NewLine);
        }
    }

}
