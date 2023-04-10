using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task1
{
    // Дан упорядоченный массив натуральных чисел, повторяющихся элементов в списке нет.
    // Нужно преобразовать в строку с перечислением интервалов через запятую.
    // Пример: [2, 3, 5, 6, 7, 8, 9, 11, 20, 21, 22] -> “2-3,5-9,11,20-22”

    public IEnumerable<(int start, int end)> GetIntervals(IEnumerable<int> input)
    {
        using (var enumerator = input.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                yield break;

            var start = enumerator.Current;
            var prev = enumerator.Current;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current - prev > 1)
                {
                    yield return (start, prev);
                    start = enumerator.Current;
                }

                prev = enumerator.Current;
            }
                
            yield return (start, prev);
        }
    }
        
    public string Transform(IEnumerable<int> input)
    {
        return string.Join(',', GetIntervals(input)
            .Select(x => x.start == x.end ? x.start.ToString() : $"{x.start}-{x.end}"));
    }

    [Fact]
    public void Acceptance()
    {
        var input = new [] {2, 3, 5, 6, 7, 8, 9, 11, 20, 21, 22};

        var result = Transform(input);
            
        result.Should().Be("2-3,5-9,11,20-22");
    }
}