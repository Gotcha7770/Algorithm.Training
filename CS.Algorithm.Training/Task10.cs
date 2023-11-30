using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Gets the maximum number of consecutive 1s in the input array.
    /// Single 0s within the sequence of 1s are treated as part of the sequence.
    /// </summary>
    /// <param name="source">The input array of integers.</param>
    /// <returns>The maximum number of consecutive 1s considering single interspersed 0s as part of the sequence.</returns>
    private int GetMaxConsecutiveOnes(int[] source)
    {
        int max = 0;
        int current = 0;
        bool isGap = false;
        foreach (int i in source)
        {
            if (i == 1)
            {
                current++;
                isGap = false;
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

    private int GetMaxConsecutiveOnesFromAI(int[] inputArray)
    {
        int maxConsecutiveOnes = 0;
        int currentConsecutiveOnes = 0;
        int zeroCount = 0;
        int previousCount = 0;

        foreach (var element in inputArray)
        {
            if (element == 1)
            {
                currentConsecutiveOnes++;
                maxConsecutiveOnes = Math.Max(maxConsecutiveOnes, currentConsecutiveOnes + previousCount);
            }
            else // element == 0
            {
                zeroCount++;
                if (zeroCount == 2)
                {
                    zeroCount = 1;
                    previousCount = 0;
                }
                else
                {
                    previousCount = currentConsecutiveOnes;
                    currentConsecutiveOnes = 0;
                }
            }
        }
        
        return maxConsecutiveOnes;
    }

    private int GetMaxConsecutiveOnes2(IEnumerable<int> source)
    {
        int result = 0;
        using var enumerator = source.GetEnumerator();

        // 1 - сумма
        // 0 - умножение
        
        // 10
        // 01
        // 00
        
        // current > previous
        // 3 > 2

        if (!enumerator.MoveNext())
            return 0;

        int consecutiveOnesCount = enumerator.Current;
        int prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            consecutiveOnesCount += enumerator.Current;

            if (enumerator.Current + prev < 1)
            {
                result = Math.Max(result, consecutiveOnesCount);
                consecutiveOnesCount = 0;
            }
            
            prev = enumerator.Current;
        }

        return Math.Max(result, consecutiveOnesCount);
    }

    private int GetMaxConsecutiveOnes3(int[] source)
    {
        int result = 0;
        int consecutiveOnesCount = source.Length > 0 ? source[0] : 0;
        for (int i = 1; i < source.Length; i++)
        {
            consecutiveOnesCount += source[i];

            if (source[i] + source[i - 1] < 1)
            {
                result = Math.Max(result, consecutiveOnesCount);
                consecutiveOnesCount = 0;
            }
        }
        
        return Math.Max(result, consecutiveOnesCount);
    }
    
    int GetMaxConsecutiveOnes4(int[] seq)
    {
        var maxSum = 0;
        var prevSeqSize = 0;
        var curSeqSize = 0;
        var seqCount = 0;
        var sumLastTwo =0;
        for (var i = 0; i < seq.Length; i++){
            if (seq[i] == 1) ++curSeqSize;
            if (seq[i] == 0)
            {
                sumLastTwo = prevSeqSize + curSeqSize;
                if (prevSeqSize + curSeqSize > maxSum) maxSum = sumLastTwo;
                prevSeqSize = curSeqSize;
                ++seqCount;
                curSeqSize = 0;
            }
        }
        sumLastTwo = prevSeqSize + curSeqSize;
        if (sumLastTwo > maxSum) maxSum = sumLastTwo;
        if (seqCount == 0) --maxSum;
        return maxSum;
    }

    [Theory]
    [InlineData(new int[0], 0)]
    [InlineData(new[] { 0 }, 0)]
    [InlineData(new[] { 1 }, 1)]
    [InlineData(new[] { 0, 1 }, 1)]
    [InlineData(new[] { 1, 1, 0, 1 }, 3)]
    [InlineData(new[] { 1, 1, 0, 0, 1 }, 2)]
    [InlineData(new[] { 1, 1, 0, 1, 1, 0 }, 4)]
    [InlineData(new[] { 0, 0, 0, 0, 0 }, 0)]
    [InlineData(new[] { 1, 1, 1, 1, 1 }, 5)]
    [InlineData(new[] { 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1 }, 7)]
    public void Acceptance(int[] items, int expected)
    {
        GetMaxConsecutiveOnes3(items)
            .Should()
            .Be(expected);
    }
}