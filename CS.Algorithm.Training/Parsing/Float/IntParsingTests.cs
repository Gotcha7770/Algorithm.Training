using System;
using System.Linq;
using FluentAssertions;
using Sprache;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Algorithm.Training.Parsing.Float;

public class IntParsingTests
{
    [Fact]
    public void Custom_Null_Throws()
    {
        Invoking(() => ParseInt(null))
            .Should()
            .Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void Custom_EmptyString_Throws()
    {
        Invoking(() => ParseInt(string.Empty))
            .Should()
            .Throw<FormatException>()
            .WithMessage("string length is 0");
    }
    
    [Theory]
    [InlineData("a", "invalid character at 0 index")]
    [InlineData("12a4", "invalid character at 2 index")]
    [InlineData("-12a4", "invalid character at 2 index")]
    public void Custom_InvalidCharacters_Throws(string input, string message)
    {
        Invoking(() => ParseInt(input))
            .Should()
            .Throw<FormatException>()
            .WithMessage(message);
    }

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
    public void SystemParse(string input, int output)
    {
        int result = int.Parse(input);
        result.Should().Be(output);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("10", 10)]
    [InlineData("1234", 1234)]
    [InlineData("-1234", -1234)]
    public void ParseWithSprache(string input, int output)
    {
        var parser = 
            from op in Parse.Char('-').Token().Optional()
            let sign = op.IsDefined ? -1 : 1
            from digits in Parse.Digit.Select(CharToInt).Many().End()
            let abs = digits.Aggregate(0, (acc, current) => acc * 10 + current)
            select abs * sign;

        parser.Parse(input)
            .Should().Be(output);
    }

    private static int ParseInt(string input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length == 0)
            throw new FormatException("string length is 0");

        return input[0] == '-' 
            ? -1 * ParseIntWithoutSign(input.AsSpan(1))
            : ParseIntWithoutSign(input.AsSpan());
            
    }

    private static int ParseIntWithoutSign(ReadOnlySpan<char> input)
    {
        int result = 0;
        for (int i = 0; i < input.Length; i++)
        {
            char character = input[i];
            if (char.IsDigit(character))
                throw new FormatException($"invalid character at {i} index");
            result = result * 10 + CharToInt(character);
        }

        return result;
    }

    private static int CharToInt(char x) => x - '0';
}