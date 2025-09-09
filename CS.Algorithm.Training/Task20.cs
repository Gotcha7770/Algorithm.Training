using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

/// <summary>
/// Отсортированное декартово произведение
/// </summary>
public class Task20
{
    [Theory]
    [ClassData(typeof(SortedProductCases))]
    public void Acceptance1(
        IEnumerable<IEnumerable<Tariff>> input,
        IEnumerable<IEnumerable<Tariff>> expected)
    {
        var result = input.ProductNImperative().Select(x => x.ToArray());
        result
            .Should()
            .BeEquivalentTo(expected);
    }

    [Theory]
    [ClassData(typeof(SortedProductCases))]
    public void Acceptance2(
        IEnumerable<ImmutableSortedSet<Tariff>> input,
        IEnumerable<IEnumerable<Tariff>> expected)
    {
        input.SortedProductN()
            .Should()
            .BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
}

public static partial class AdHocExtensions
{
    public static IEnumerable<IEnumerable<T>> ProductNImperative<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return EnumerableEx.Create(() =>
        {
            var enumerators = new List<IEnumerator<T>>();
            foreach (var item in source)
            {
                if (item is null)
                    throw new ArgumentNullException(nameof(source), "One of the sequences is null.");

                var enumerator = item.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    enumerators.Add(enumerator);
                }
                else
                {
                    enumerator.Dispose();
                    enumerators.ForEach(x => x.Dispose());
                    throw new ArgumentNullException(nameof(source), "One of the sequences is empty.");
                }
            }

            return Iterator(enumerators);
        });
    }

    private static IEnumerator<IEnumerable<T>> Iterator<T>(this List<IEnumerator<T>> enumerators)
    {
        try
        {
            int index = enumerators.Count - 1;
            while (true)
            {
                // возвращаем текущий набор
                yield return enumerators.Select(x => x.Current);

                // на каждом шагу продвигаем enumerator на 1 элемент
                if (enumerators[index].MoveNext() is false)
                {
                    // если это не возможно, ищем следующий enumerator, который можно продвинуть на 1 элемент
                    do { --index; } while ( index >= 0 && enumerators[index].MoveNext() is false);
                    
                    // если такого нет, выходим
                    if(index == -1)
                        break;

                    // если есть, сбрасываем все предыдущие
                    enumerators[(index+1)..].ForEach(x =>
                    {
                        x.Reset();
                        x.MoveNext();
                    });
                    
                    index = enumerators.Count - 1;
                }
            }
        }
        finally
        {
            enumerators.ForEach(x => x.Dispose());
        }
    }

    public static IEnumerable<IEnumerable<T>> SortedProductN<T>(
        this IEnumerable<IEnumerable<T>> source)
    {
        return source.Aggregate<IEnumerable<T>, IEnumerable<IEnumerable<T>>>(
            [[]],
            (acc, cur) =>
                from prevProductItem in acc
                from item in cur
                select prevProductItem.Append(item));
    }
}

public record Tariff(string FareCode, int BaseAmount) : IComparable<Tariff>
{
    public int CompareTo(Tariff other)
    {
        return BaseAmount.CompareTo(other.BaseAmount);
    }
}

public class SortedProductCases : TheoryData<IEnumerable<ImmutableSortedSet<Tariff>>, IEnumerable<IEnumerable<Tariff>>>
{
    public SortedProductCases()
    {
        Add(
            [
                [
                    new Tariff("T1", 100),
                    new Tariff("T2", 170)
                ],
                [
                    new Tariff("T3", 200),
                    new Tariff("T4", 150)
                ]
            ],
            [
                [new Tariff("T1", 100), new Tariff("T4", 150)], //250
                [new Tariff("T1", 100), new Tariff("T3", 200)], //300
                [new Tariff("T2", 170), new Tariff("T4", 150)], //320
                [new Tariff("T2", 170), new Tariff("T3", 200)] //370
            ]);

        Add(
            [
                [
                    new Tariff("T1", 100),
                    new Tariff("T2", 160)
                ],
                [
                    new Tariff("T3", 210),
                    new Tariff("T4", 140)
                ]
            ],
            [
                [new Tariff("T1", 100), new Tariff("T4", 140)], //240
                [new Tariff("T2", 160), new Tariff("T4", 140)], //300
                [new Tariff("T1", 100), new Tariff("T3", 210)], //310
                [new Tariff("T2", 160), new Tariff("T3", 210)] //370
            ]);

        Add(
            [
                [
                    new Tariff("T1", 100),
                    new Tariff("T2", 200),
                    new Tariff("T3", 300)
                ],
                [
                    new Tariff("T4", 50),
                    new Tariff("T5", 150)
                ],
                [
                    new Tariff("T6", 80),
                    new Tariff("T7", 120)
                ]
            ],
            [
                [new Tariff("T1", 100), new Tariff("T4", 50), new Tariff("T6", 80)], // 230
                [new Tariff("T1", 100), new Tariff("T4", 50), new Tariff("T7", 120)], // 270
                [new Tariff("T1", 100), new Tariff("T5", 150), new Tariff("T6", 80)], // 330
                [new Tariff("T2", 200), new Tariff("T4", 50), new Tariff("T6", 80)], // 330
                [new Tariff("T1", 100), new Tariff("T5", 150), new Tariff("T7", 120)], // 370
                [new Tariff("T2", 200), new Tariff("T4", 50), new Tariff("T7", 120)], // 370
                [new Tariff("T2", 200), new Tariff("T5", 150), new Tariff("T6", 80)], // 430
                [new Tariff("T3", 300), new Tariff("T4", 50), new Tariff("T6", 80)], // 430
                [new Tariff("T2", 200), new Tariff("T5", 150), new Tariff("T7", 120)], // 470
                [new Tariff("T3", 300), new Tariff("T4", 50), new Tariff("T7", 120)], // 470
                [new Tariff("T3", 300), new Tariff("T5", 150), new Tariff("T6", 80)], // 530
                [new Tariff("T3", 300), new Tariff("T5", 150), new Tariff("T7", 120)] // 570
            ]
        );
    }
}