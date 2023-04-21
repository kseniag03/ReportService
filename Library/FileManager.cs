using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ReportService.Library;

/// <summary>
/// Класс для получения файлов логов.
/// </summary>
public static class FileManager
{
    private const string ExtensionJson = "*.json";
    
    private const string ExtensionLog = "*.log";
    
    private const string ExtensionTxt = "*.txt";

    /// <summary>
    /// Получает файлы логов, удовлетворяющие маске.
    /// </summary>
    /// <param name="folderPath"> Путь к директории </param>
    /// <param name="pattern"> Маска (имя сервера) </param>
    /// <returns></returns>
    public static List<string> GetLogFiles(string folderPath, string pattern) 
    {
        return GetFiles(folderPath.Length > 0 ? folderPath : GetDefaultFolderPath(), pattern);
    }
    
    /// <summary>
    /// Получает содержимое заданного файла.
    /// </summary>
    /// <param name="filePath"> Путь к файлу </param>
    /// <returns> Список строк из файла </returns>
    public static List<string?> GetFileContent(string filePath) 
    {
        return GetFileContent(filePath, Encoding.UTF8);
    }

    /// <summary>
    /// Получает папку проекта (путь к директории по умолчанию).
    /// </summary>
    /// <returns> Путь к директории по умолчанию </returns>
    /// <exception cref="InvalidOperationException"> Если невозможно определить папку проекта </exception>
    private static string GetDefaultFolderPath()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        for (var i = 0; i < 3 && path != null; ++i)
        {
            path = Directory.GetParent(path)?.FullName;
        }
        if (path == null)
        {
            throw new InvalidOperationException("Unable to determine default folder path.");
        }
        return path;
    }
/*
    /// <summary>
    /// Возвращает список путей к файлам из директории, удовлетворяющих маске.
    /// </summary>
    /// <param name="folderPath"> Путь к директории </param>
    /// <param name="patterns"> Маска (имя сервера) </param>
    /// <returns></returns>
    private static List<string> GetFiles(string folderPath, params string[] patterns)
    {
        var files = new List<string>();
        foreach (var pattern in patterns)
        {
            files.AddRange(Directory.EnumerateFiles(folderPath, pattern));
        }
        return files;
    }*/
    
    /// <summary>
    /// Возвращает список путей к файлам из директории, удовлетворяющих маске.
    /// </summary>
    /// <param name="folderPath"> Путь к директории </param>
    /// <param name="pattern"> Маска (имя сервера) </param>
    /// <returns> Список путей к подходящим по параметрам поиска файлам </returns>
    private static List<string> GetFiles(string folderPath, string pattern)
    {
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        var files = new List<string>();
        files.AddRange(Directory.EnumerateFiles(folderPath, ExtensionLog)
            .Where(filePath => regex.IsMatch(Path.GetFileNameWithoutExtension(filePath))));
        return files;
    }

    /// <summary>
    /// Читает построчно файл в заданной кодировке и возвращает содержимое в виде списка строк.
    /// </summary>
    /// <param name="filePath"> Путь к файлу </param>
    /// <param name="encoding"> Заданная кодировка </param>
    /// <returns> Список строк, содержащий данные файла </returns>
    private static List<string?> GetFileContent(string filePath, Encoding encoding)
    {
        List<string?> lines = new();
        try
        {
            using StreamReader sr = new(filePath, encoding, false);
            while (sr.ReadLine() is { } line)
            {
                lines.Add(line);
            }
        }
        catch (Exception)
        {
            lines.Clear();
        }
        return lines;
    }
}