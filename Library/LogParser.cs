using System.Text.RegularExpressions;
using ReportService.Models;

namespace ReportService.Library;

public static partial class LogParser
{
    private static readonly Regex LogRegex = DefineRegex();

    public static Log ParseLog(string logString)
    {
        var match = LogRegex.Match(logString);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid log string.", nameof(logString));
        }

        return new Log
        {
            Date = DateTime.Parse(match.Groups["date"].Value),
            Category = match.Groups["category"].Value,
            Text = EmailAnonymization(match.Groups["text"].Value)
        };
    }
    
    private static string EmailAnonymization(string text)
    {
        var matches = DefineEmailRegex().Matches(text);
        
        foreach (Match match in matches)
        {
            var email = match.Value;
            var maskedEmail = $"{email[0]}*{email.Substring(2, email.IndexOf('@') - 3)}*{email.Substring(email.IndexOf('@') - 1)}";
            text = text.Replace(email, maskedEmail);
        }
        
        return text;
    }

    [GeneratedRegex("^\\[(?<date>[^\\]]+)\\]\\[(?<category>[^\\]]+)\\] (?<text>.+)$")]
    private static partial Regex DefineRegex();
    
    [GeneratedRegex("[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}")]
    private static partial Regex DefineEmailRegex();
}