using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task2
{
    // Даны две поисковые выдачи размера N, каждая из которых
    // состоит из уникальных неотрицательных идентификаторов документов.
    // Требуется посчитать количество общих документов в top-K выдачах
    // для K от 1 до N.
    // top-K выдачей называется выдача первых K результатов как из
    // первой, так и из второй поисковой выдачи.

    // Например:
    // ([0, 1, 2, 3, 4], [0, 3, 4, 2, 1]) => [1, 1, 1, 3, 5]
    // Первый элемент 1, потому что в [0] и [0] один общий документ.
    // Второй элемент 1, потому что в [0, 1] и [0, 3] один общий документ.
    // Третий элемент 1, потому что в [0, 1, 2] и [0, 3, 4] один общий документ.
    // Четвертый элемент 3, потому что в [0, 1, 2, 3] и [0, 3, 4, 2] три общих документа (0, 2 и 3).
    // Пятый элемент 5, потому что в [0, 1, 2, 3, 4] и [0, 3, 4, 2, 1] встречается каждый идентификатор документа.

    public int[] CountTopK1(int[] one, int[] other)
    {
        int counter = 0;
        int[] result = new int[one.Length];
        var set = new HashSet<int>();

        for (int i = 0; i < one.Length; i++)
        {
            if (one[i] == other[i])
            {
                ++counter;
            }
            else
            {
                counter += set.Contains(one[i]) ? 1 : 0;
                set.Add(one[i]);

                counter += set.Contains(other[i]) ? 1 : 0;
                set.Add(other[i]);
            }

            result[i] = counter;
        }

        return result;
    }

    public int[] CountTopK2(int[] one, int[] other)
    {
        var set = new HashSet<int>();
        return one.Zip(other, (x, y) => (x, y))
            .Select(tuple =>
            {
                var a = set.Add(tuple.x) ? 0 : 1;
                var b = set.Add(tuple.y) ? 0 : 1;
                return a + b;
            })
            .Scan(0, (acc, cur) => acc + cur)
            .ToArray();
    }

    [Fact]
    public void Acceptance1()
    {
        int[] first = [0, 1, 2, 3, 4];
        int[] second = [0, 3, 4, 2, 1];

        int[] result = CountTopK1(first, second);

        result.Should().BeEquivalentTo([1, 1, 1, 3, 5]);
    }

    [Fact]
    public void Acceptance2()
    {
        int[] first = [0, 1, 2, 3, 4];
        int[] second = [0, 3, 4, 2, 1];

        int[] result = CountTopK2(first, second);

        result.Should().BeEquivalentTo([1, 1, 1, 3, 5]);
    }
}