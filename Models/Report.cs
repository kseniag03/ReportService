namespace ReportService.Models;

public struct Report
{
    public string ServiceName { get; set; }

    public DateTime FirstReportDate { get; set; }

    public DateTime LastReportDate { get; set; }

    public Dictionary<string, int> NumberOfReports { get; set; }

    public int NumberOfRotations { get; set; }

    public override bool Equals(object? obj)
    {
        if (!(obj is Report))
        {
            return false;
        }
        var other = (Report)obj;

        return ServiceName == other.ServiceName
               && FirstReportDate == other.FirstReportDate
               && LastReportDate == other.LastReportDate
               && NumberOfRotations == other.NumberOfRotations
               && NumberOfReports.OrderBy(kvp => kvp.Key).SequenceEqual(other.NumberOfReports.OrderBy(kvp => kvp.Key));
    }

    public override string ToString()
    {
        return ServiceName + " " + FirstReportDate + " " + LastReportDate + " " + NumberOfReports + " " + NumberOfRotations + Environment.NewLine;
    }
}

/*
Структура отчёта:

    Имя сервиса
    Дата и время самой ранней записи в логах
    Дата и время самой последней записи в логах
    Количество записей в каждой категории
    Количество ротаций.
 */