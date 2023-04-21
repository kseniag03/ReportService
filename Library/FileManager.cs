using System.Text;

namespace ReportService.Library;

public static class FileManager
{
    private const string ExtensionJson = "*.json";
    
    private const string ExtensionLog = "*.log";
    
    private const string ExtensionTxt = "*.txt";

    public static List<string> GetLogFiles(string folderPath, string pattern) 
    {
        // надо проверить путь и поменять слеши на правильные для текущей ос
        return GetFiles(folderPath.Length > 0 ? folderPath : "../../../", ExtensionLog, pattern);
    }
    
    public static List<string?> GetFileContent(string filePath) 
    {
        // надо проверить путь и поменять слеши на правильные для текущей ос
        return GetFileContent(filePath, Encoding.UTF8);
    }
    
    private static List<string> GetFiles(string folderPath, params string[] patterns)
    {
        var files = new List<string>();
        foreach (var pattern in patterns)
        {
            files.AddRange(Directory.EnumerateFiles(folderPath, pattern));
        }
        return files;
    }
    
    /// <summary>
    /// Читает построчно файл в заданной кодировке.
    /// </summary>
    /// <param name="path"> Путь к файлу </param>
    /// <param name="encoding"> Заданная кодировка </param>
    /// <returns> Список строк, содержащий данные файла </returns>
    private static List<string?> GetFileContent(string path, Encoding encoding)
    {
        List<string?> lines = new();
        try
        {
            using StreamReader sReader = new(path, encoding, false);
            while (sReader.ReadLine() is { } line)
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