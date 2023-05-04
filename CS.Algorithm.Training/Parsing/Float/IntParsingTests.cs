using System.Linq;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training.Parsing.Float;

public class IntParsingTests
{
    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("10", 10)]
    [InlineData("1234", 1234)]
    [InlineData("-1234", -1234)]
    public void Custom(string input, int output)
    {
        int result = ParseInt(input);
        result.Should().Be(output);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("10", 10)]
    [InlineData("1234", 1234)]
    [InlineData("-1234", -1234)]
    public void TryParse(string input, int output)
    {
        int result = int.Parse(input);
        result.Should().Be(output);
    }

    private static int ParseInt(string input)
    {
        var result = 0;
        foreach (char character in input)
        {
            result *= 10;
            result += CharToInt(character);
        }

        return result;
    }

    private static int ParseIntWithoutSign(string input)
    {
        return input.Aggregate(0, (acc, current) => acc * 10 + current);
    }
    
    private static int CharToInt(char x) => x - '0';
}