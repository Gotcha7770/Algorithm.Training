using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Algorithm.Training;

public static partial class AdHocExtensions
{
    public static int DaysInMonth(this DateOnly date) => DateTime.DaysInMonth(date.Year, date.Month);
}

public class Task15
{
    IEnumerable<DateOnly> EnumerateDays(DateOnly date)
    {
        for (int day = 1; day <= date.DaysInMonth(); day++)
        {
            yield return new DateOnly(date.Year, date.Month, day);
        }
    }

    // Назовем месяц "удачным" если в нем 5 воскресений.
    bool IsSuccessfulMonth(DateOnly date) => EnumerateDays(date).Count(x => x.DayOfWeek == DayOfWeek.Sunday) == 5;

    IEnumerable<DateOnly> EnumerateMonths(int year)
    {
        for (int month = 1; month <= 12; month++)
        {
            yield return new DateOnly(year, month, 1);
        }
    }

    // Назовем "коэффициентом удачности" календарного года число "удачных" месяцев в этом году.
    int GetSuccessRate(int year) => EnumerateMonths(year).Count(x => x.DayOfWeek == DayOfWeek.Sunday);

    [Fact]
    public void Acceptance()
    {
        // Вывести на печать все "удачные месяцы" по одному на строку в формате MM.yyyy (например, 01.2022),
        // начиная с января 2010 года и по декабрь 2020 включительно
        var formated = Enumerable.Range(2010, 11)
            .SelectMany(EnumerateMonths)
            .Where(IsSuccessfulMonth)
            .Select(x => x.ToString("MM.yyyy"))
            .ToArray();
        
        // отсортировать список полученный в пункте №1
        // * сначала по "коэффициенту удачности" соответствующего года по убыванию,
        // * затем по числу дней в этом месяце по убыванию,
        // * затем по номеру месяца в году по возрастанию,
        // * затем по году по возрастанию// для сортировки использовать Linq
        var sorted = Enumerable.Range(2010, 2020)
            .SelectMany(EnumerateMonths)
            .Where(IsSuccessfulMonth)
            .Select(x => new
            {
                Date = x,
                SuccessRate = GetSuccessRate(x.Year),
                DaysInMonth = x.DaysInMonth()
            })
            .OrderByDescending(x => x.SuccessRate)
            .ThenByDescending(x => x.DaysInMonth)
            .ThenBy(x => x.Date.Month)
            .ThenBy(x => x.Date.Year)
            .ToArray();
    }
}