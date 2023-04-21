namespace ReportService.Models;

/// <summary>
/// Структура лога.
/// </summary>
public struct Log
{
    public DateTime Date { get; set; }
    
    public string Category { get; set; }
    
    public string Text { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Log other)
        {
            return false;
        }
        return Date == other.Date && Category== other.Category && Text == other.Text;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Category, Text);
    }

    public override string ToString()
    {
        return "[" + Date + "][" + Category + "]" + Text + "\n";
    }
}