using System.Linq;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task19
{
    // расчет сочетания с повторениями n (типов) по k

    [Theory]
    [InlineData(2, 2, 3)]
    [InlineData(5, 2, 15)]
    [InlineData(10, 2, 55)]
    [InlineData(15, 2, 120)]
    [InlineData(20, 2, 210)]
    [InlineData(20, 3, 1540)]
    public void Acceptance(int n, int k, long expected)
    {
        PermutationsCount(n, k).Should().Be(expected);
    }
    
    private static long PermutationsCount(int n, int k)
    {
        long dividend = EnumerableEx.Generate(k + n - 1, x => x > n - 1, x => x - 1, x => (long)x).Aggregate((acc, cur) => acc * cur);
        long divider = Factorial(k);

        return dividend / divider;
    }

    private static long Factorial(int n) => Enumerable.Range(0, n+1)
        .Select(x => (long)x)
        .Aggregate((acc, cur) => acc == 0 ? 1 : acc * cur);
}