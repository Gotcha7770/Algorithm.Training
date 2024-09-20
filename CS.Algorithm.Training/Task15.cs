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

    IEnumerable<DateOnly> EnumerateMonths(DateOnly from, DateOnly to)
    {
        from = new DateOnly(from.Year, from.Month, 1);
        while (from <= to)
        {
            from = from.AddMonths(1);
            yield return from;
        }
    }

    // Назовем "коэффициентом удачности" календарного года число "удачных" месяцев в этом году.
    int GetSuccessRate(int year) => EnumerateMonths(new DateOnly(year, 1, 1), new DateOnly(year, 12, 1)).Count(x => x.DayOfWeek == DayOfWeek.Sunday);

    [Fact]
    public void Acceptance()
    {
        // Вывести на печать все "удачные месяцы" по одному на строку в формате MM.yyyy (например, 01.2022),
        // начиная с января 2010 года и по декабрь 2020 включительно,
        var formated = EnumerateMonths(new DateOnly(2010, 1, 1), new DateOnly(2020, 12, 1))
            .Where(IsSuccessfulMonth)
            .Select(x => x.ToString("MM.yyyy"))
            .ToArray();
        
        // отсортировать список полученный в пункте №1
        // * сначала по "коэффициенту удачности" соответствующего года по убыванию,
        // * затем по числу дней в этом месяце по убыванию,
        // * затем по номеру месяца в году по возрастанию,
        // * затем по году по возрастанию// для сортировки использовать Linq
        var sorted = EnumerateMonths(new DateOnly(2010, 1, 1), new DateOnly(2020, 12, 1))
            .Where(IsSuccessfulMonth)
            .OrderByDescending(x => GetSuccessRate(x.Year))
            .ThenByDescending(x => x.DaysInMonth())
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Year)
            .ToArray();
    }
}