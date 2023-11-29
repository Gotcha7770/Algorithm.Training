using System.Linq;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

public class Task5
{
    [Theory]
    [InlineData(2, 3, 1)]
    [InlineData(3, 3, 1)]
    [InlineData(4, 3, 2)]
    [InlineData(5, 3, 2)]
    [InlineData(6, 3, 2)]
    [InlineData(7, 3, 3)]
    public void CountPagesTest(int total, int pageSize, int expected)
    {
        CountPages(total, pageSize)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData(2, 3, 2)]
    [InlineData(3, 3, 0)]
    [InlineData(4, 3, 1)]
    [InlineData(5, 3, 2)]
    [InlineData(6, 3, 0)]
    [InlineData(7, 3, 1)]
    public void LeftOverTest(int total, int pageSize, int expected)
    {
        GetLeftOver(total, pageSize)
            .Should()
            .Be(expected);
    }

    public static int CountPages(int total, int pageSize) => (total + pageSize - 1) / pageSize;

    public static int GetLeftOver(int total, int pageSize) => total % pageSize;
}