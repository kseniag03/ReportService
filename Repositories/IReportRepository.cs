using System;
using System.Collections.Generic;
using ReportService.Models;

namespace ReportService.Repositories;

/// <summary>
/// Интерфейс для взаимодействия с хранилищем данных отчётов.
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Создаёт новый отчёт и добавляет его в список.
    /// </summary>
    /// <param name="serviceName"> Имя сервиса </param>
    /// <param name="folderPath"> Путь к директории с файлами логов </param>
    /// <returns> Новый отчёт </returns>
    Report CreateReport(string serviceName, string folderPath);

    /// <summary>
    /// Возвращает список всех отчётов.
    /// </summary>
    /// <param name="serviceName"> Имя сервиса </param>
    /// <param name="folderPath"> Путь к директории с файлами логов </param>
    /// <returns> Список отчётов </returns>
    IReadOnlyList<Report> GetAllReportsByServiceName(string serviceName, string folderPath);
    
    /// <summary>
    /// Возвращает список всех отчётов.
    /// </summary>
    /// <returns> Список отчётов </returns>
    IReadOnlyList<Report> GetAllReports();

    /// <summary>
    /// Читает json-файл и инициализирует список.                           /// а надо ли?..
    /// </summary>
    void ReadJsonFile();

    /// <summary>
    /// Сортирует список и записывает его текущее состояние в json-файл.    /// !!!!!!!!!!! need to separate
    /// </summary>
    void SortList();

    /// <summary>
    /// Чистит список.
    /// </summary>
    void ClearList();
}