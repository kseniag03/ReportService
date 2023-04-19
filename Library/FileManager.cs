namespace ReportService.Library;

public static class FileManager
{
    private const string ExtensionJson = "*.json";
    
    private const string ExtensionLog = "*.log";
    
    private const string ExtensionTxt = "*.txt";

    public static List<string> GetLogFiles(string folderPath, string pattern) {
        return GetFiles(folderPath.Length > 0 ? folderPath : "../../../", ExtensionLog, pattern);
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
}