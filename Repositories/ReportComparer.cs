using ReportService.Models;

namespace ReportService.Repositories;

/// <summary>
/// Класс объекта для сравнения отчётов.
/// </summary>
public class ReportComparer : IComparer<Report>
{
    /// <summary>
    /// Сравнение отчётов для сортировки имён сервисов в алфавитном порядке
    /// </summary>
    /// <param name="first"> Первый отчёт </param>
    /// <param name="second"> Второй отчёт </param>
    /// <returns></returns>
    public int Compare(Report first, Report second)
    {
        var compare = String.CompareOrdinal(first.ServiceName, second.ServiceName);
        if (compare == 0)
        {
            compare = first.FirstReportDate < second.FirstReportDate ? -1 : first.FirstReportDate > second.FirstReportDate ? 1 : 0;
        }
        return compare;
    }
}
