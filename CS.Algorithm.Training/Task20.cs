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
/// k-way merge
/// best-first search.
/// </summary>
public class Task20
{
    [Theory]
    [ClassData(typeof(SortedProductCases))]
    public void Acceptance(
        IEnumerable<ImmutableSortedSet<Tariff>> input,
        IEnumerable<IEnumerable<Tariff>> expected)
    {
        input.SortedProduct()
            .Should()
            .BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
}

public static partial class AdHocExtensions
{
    public static IEnumerable<IEnumerable<T>> SortedProduct<T>(this IEnumerable<IEnumerable<T>> source)
    {
        return source.Aggregate<IEnumerable<T>, IEnumerable<IEnumerable<T>>>(
            [[]],
            SortedProduct);
    }

    public static IEnumerable<IEnumerable<T>> SortedProduct<T>(this IEnumerable<IEnumerable<T>> one,
        IEnumerable<T> other)
    {
        using var multiplicand = one.GetEnumerator();
        if (multiplicand.MoveNext() is false)
            yield break;

        using var multiplier = other.GetEnumerator();
        if (multiplier.MoveNext() is false)
            yield break;

        var (a, b) = (multiplicand.Current, multiplier.Current);
        yield return a.Append(b); // the first values are minimal

        var queue = new UniquePriorityQueue<T[]>();

        if (multiplicand.MoveNext())
        {
            queue.Enqueue([.. multiplicand.Current, b]);
        }
        
        if (multiplier.MoveNext())
        {
            queue.Enqueue([.. a, multiplier.Current]);
        }

        while (queue.TryDequeue(out var element))
        {
            yield return element;
        }
    }
}

public record Tariff(string FareCode, int BaseAmount) : IComparable<Tariff>
{
    public int CompareTo(Tariff other)
    {
        return BaseAmount.CompareTo(other.BaseAmount);
    }
}

public class UniquePriorityQueue<T> : IEnumerable<T>
{
    private readonly SortedSet<T> _set;

    public UniquePriorityQueue(IComparer<T> comparer = null)
    {
        _set = new SortedSet<T>(comparer);
    }

    public UniquePriorityQueue(IEnumerable<T> items, IComparer<T> comparer = null)
    {
        _set = new SortedSet<T>(items, comparer);
    }

    public void Enqueue(T item) => _set.Add(item);

    public bool TryDequeue(out T item)
    {
        if (_set.Count == 0)
        {
            item = default;
            return false;
        }

        item = _set.Min;
        _set.Remove(item);
        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (TryDequeue(out T item))
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
                []
            ],
            []);

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