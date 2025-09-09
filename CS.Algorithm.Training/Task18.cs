using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task18
{
    [Theory]
    [ClassData(typeof(ProductCases))]
    public void Acceptance(
        IEnumerable<IEnumerable<int>> input, 
        Func<int, int, bool> canBeCombined, 
        IEnumerable<IEnumerable<int>> expected)
    {
        input.ProductN(canBeCombined)
            .Should()
            .BeEquivalentTo(expected);
    }
}

public static partial class AdHocExtensions
{
    public static IEnumerable<IEnumerable<T>> ProductN<T>(
        this IEnumerable<IEnumerable<T>> source,
        Func<T, T, bool> canBeCombined)
    {
        return source.Aggregate<IEnumerable<T>, IEnumerable<LinkedList<T>>>(
            [[]],
            (acc, cur) =>
                from prevProductItem in acc
                from item in cur
                where prevProductItem.Last is null || canBeCombined(prevProductItem.Last.Value, item)
                select prevProductItem.With(item));
    }

    private static LinkedList<T> With<T>(this LinkedList<T> list, T item)
    {
        return new LinkedList<T>(list.Append(item));
    }
}

public class ProductCases : TheoryData<IEnumerable<IEnumerable<int>>, Func<int, int, bool>, IEnumerable<IEnumerable<int>>>
{
    public ProductCases()
    {
        Add(
            [
                [1, 2],
                [3, 4]
            ],
            (x, y) => true,
            [
                [1, 3],
                [1, 4],
                [2, 3],
                [2, 4]
            ]);
        
        Add(
            [
                [1, 2, 3],
                [3, 4]
            ],
            (x, y) => (x, y) switch
            {
                (3, _) => y is 3,
                (_, 3) => x is 3,
                _ => true
            },
            [
                [1, 4],
                [2, 4],
                [3, 3]
            ]);
        
        Add(
            [
                [1, 2, 3],
                [3, 4, 5],
                [5, 6, 7]
            ],
            (x, y) => (x, y) switch
            {
                (3, _) => y is 3 or 6 or 7,
                (_, 3) => x is 3,
                (5, _) => y is 5,
                (_, 5) => x is 5 or 1 or 2,
                _ => true
            },
            [
                [1, 4, 6],
                [1, 4, 7],
                [1, 5, 5],
                [2, 4, 6],
                [2, 4, 7],
                [2, 5, 5],
                [3,  3, 6],
                [3, 3, 7]
            ]);
    }
}