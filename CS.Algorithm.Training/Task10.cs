using System;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

// Дан массив из нулей и единиц. Нужно определить, какой максимальный по длине подынтервал единиц можно получить,
// удалив ровно один элемент массива.
// Удалять один элемент из массива обязательно.
    
// assert(maxOnes(new int[]{1, 1, 0, 1})) == 3
// assert(maxOnes(new int[]{1, 1, 0, 0, 1})) == 2

public class Task10
{
    private int GetMaxOnes(int[] source)
    {
        int max = 0;
        int current = 0;
        bool isGap = false;
        foreach (int i in source)
        {
            if (i == 1)
            {
                current++;
            }
            else if (!isGap)
            {
                isGap = true;
            }
            else
            {
                max = Math.Max(max, current);
                current = 0;
            }
        }

        max = Math.Max(max, current);
        return max;
    }

    [Theory]
    [InlineData(new int[0], 0)]
    [InlineData(new[] { 0 }, 0)]
    [InlineData(new[] { 0, 1 }, 1)]
    [InlineData(new[] { 1, 1, 0, 1 }, 3)]
    [InlineData(new[] { 1, 1, 0, 0, 1 }, 2)]
    public void Acceptance(int[] items, int expected)
    {
        GetMaxOnes(items)
            .Should()
            .Be(expected);
    }
}