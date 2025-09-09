using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task13
{
    // Есть товар, цена на который может меняться. Цена задается с помощью массива,
    // элементы которого содержат цену и дату, с которой цена вступает в действие.
    // Написать функцию, которая возвращает цену на указанную дату.

    public readonly record struct PriceWithDate(decimal Price, DateTime Date);

    // в параметре prices дубликатов цен для одной и той же даты нет и цена верная
    public (bool isPriceFound, decimal price) GetPriceByDate1(DateTime date, PriceWithDate[] prices)
    {
        prices ??= [];
        
        PriceWithDate max = default;
        
        foreach (var price in prices)
        {
            if (price.Date <= date && max.Date < price.Date)
            {
                max = price;
            }
        }
        
        return max.Date == default ? (false, default) : (true, max.Price);
    }

    public (bool isPriceFound, decimal price) GetPriceByDate2(DateTime date, PriceWithDate[] prices)
    {
        prices ??= [];

        return prices
            .Where(x => x.Date <= date)
            .OrderBy(x => x.Date)
            .Select(x => (true, x.Price))
            .LastOrDefault();
    }
    
    public (bool isPriceFound, decimal price) GetPriceByDate3(DateTime date, PriceWithDate[] prices)
    {
        var max = prices?
            .Where(x => x.Date <= date)
            .MaxByOrNull(x => x.Date);
        
        return max is { Price: var value } ? (true, value) : (false, default);
    }

    [Theory]
    [ClassData(typeof(GetPriceByDateCases))]
    public void Acceptance(DateTime date, PriceWithDate[] prices, (bool, decimal) expected)
    {
        GetPriceByDate1(date, prices)
            .Should()
            .BeEquivalentTo(expected);

        GetPriceByDate2(date, prices)
            .Should()
            .BeEquivalentTo(expected);

        GetPriceByDate3(date, prices)
            .Should()
            .BeEquivalentTo(expected);
    }

    private class GetPriceByDateCases : TheoryData<DateTime, PriceWithDate[], (bool, decimal)>
    {
        public GetPriceByDateCases()
        {
            Add(new DateTime(2000, 01, 01), null, (false, 0));
            Add(new DateTime(2000, 01, 01), [], (false, 0));
            Add(
                new DateTime(2000, 01, 01), 
                [
                    new PriceWithDate(1, new DateTime(2000, 02, 02)), 
                    new PriceWithDate(2, new DateTime(2000, 03, 03)), 
                    new PriceWithDate(4, new DateTime(2000, 04, 04))
                ],
                (false, 0));
            Add(
                new DateTime(2000, 01, 01), 
                [
                    new PriceWithDate(1, new DateTime(2000, 01, 01)), 
                    new PriceWithDate(2, new DateTime(2000, 03, 03)), 
                    new PriceWithDate(4, new DateTime(2000, 04, 04))
                ],
                (true, 1));
            Add(
                new DateTime(2000, 01, 01), 
                [
                    new PriceWithDate(1, new DateTime(2000, 02, 02)), 
                    new PriceWithDate(2, new DateTime(1999, 10, 10)), 
                    new PriceWithDate(4, new DateTime(2000, 04, 04))
                ],
                (true, 2));
            Add(
                new DateTime(2013, 02, 28), 
                [
                    new PriceWithDate(1, new DateTime(2000, 02, 02)),
                    new PriceWithDate(2, new DateTime(2000, 04, 04)),
                    new PriceWithDate(4, new DateTime(2014, 04, 04))
                ],
                (true, 2));
        }
    }
}

public static partial class AdHocExtensions
{
    public static T? MaxByOrNull<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where T : struct
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
            return null;

        T value = enumerator.Current;
        TKey key = keySelector(value);
        while (enumerator.MoveNext())
        {
            T nextValue = enumerator.Current;
            TKey nextKey = keySelector(nextValue);
            if (Comparer<TKey>.Default.Compare(nextKey, key) > 0)
            {
                key = nextKey;
                value = nextValue;
            }
        }

        return value;
    }
}