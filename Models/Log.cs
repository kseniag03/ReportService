namespace ReportService.Models;

public struct Log
{
    //public string ServerName { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Category { get; set; }
    
    public string Text { get; set; }

    public override string ToString()
    {
        return "[" + Date + "][" + Category + "]" + Text + "\n";
    }
}