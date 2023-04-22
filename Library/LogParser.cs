using System.Text.RegularExpressions;
using ReportService.Models;

namespace ReportService.Library;

/// <summary>
/// Класс для получения данных логов из строк файла.
/// </summary>
public static partial class LogParser
{
    /// <summary>
    /// Регулярное выражение для лога.
    /// </summary>
    private static readonly Regex LogRegex = DefineRegex();

    /// <summary>
    /// Парсит строку с логом и возвращает объект структуры.
    /// </summary>
    /// <param name="logString"> Строка </param>
    /// <returns> Объект лога </returns>
    /// <exception cref="ArgumentException"> Если передаётся строка не заданного формата </exception>
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
    
    /// <summary>
    /// Метод для частичного сокрытия данных пользователей
    /// </summary>
    /// <param name="text"> Текст лога </param>
    /// <returns> Обновлённый текст лога </returns>
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

    /// <summary>
    /// Схема регулярного выражения для вычленения лога из строки (формат из условия).
    /// </summary>
    /// <returns> Регулярное выражение </returns>
    [GeneratedRegex("^\\[(?<date>[^\\]]+)\\]\\[(?<category>[^\\]]+)\\] (?<text>.+)$")]
    private static partial Regex DefineRegex();
    
    /// <summary>
    /// Схема регулярного выражения для вычленения электронных почт из текста лога.
    /// </summary>
    /// <returns> Регулярное выражение </returns>
    [GeneratedRegex("[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}")]
    private static partial Regex DefineEmailRegex();
}