namespace ReportService.Library;

public static class FileSettings
{
    public const string ExtensionJson = "*.json";
    public const string ExtensionLog = "*.log";
    public const string ExtensionTxt = "*.txt";
    
    public static List<string> GetLogFiles(string folderPath) {
        
        DirectoryInfo dirInfo = new(folderPath.Length > 0 ? folderPath : "../../../");
        GetFiles(dirInfo, pattern);
        
        var files = Directory.GetFiles(folderPath, FileSettings.ExtensionLog);
        return files.ToList();
    }
    
    /// <summary>
    /// Получает список файлов директории по заданной маске, записывая его в выходной файл.
    /// </summary>
    /// <param name="root"> Корневая папка </param>
    /// <param name="pattern"> Маска, по которой выбираются файлы </param>
    private static void GetFiles(DirectoryInfo root, string pattern)
    {
        FileInfo[] files = null;
        try
        {
            files = root.GetFiles(pattern);
        }
        catch (UnauthorizedAccessException)
        {
            s_output += $"Нет доступа к {root}" + '\n';
        }
        catch (DirectoryNotFoundException)
        {
            s_output += $"Директория {root} не найдена" + '\n';
        }
        catch (Exception exception)
        {
            s_output += $"Ошибка 500: {exception.Message}" + '\n';
        }
        foreach (var file in files)
        {
            s_output += file.Name + '\n';
        }
    }
}