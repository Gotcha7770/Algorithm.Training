using System;
using Algorithm.Training.Parsing.Parsers;
using FluentAssertions;
using Sprache;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Algorithm.Training.Parsing.Integer;

public class IntParsingTests
{
    [Fact]
    public void Custom_Null_Throws()
    {
        Invoking(() => IntParser.Parse(null))
            .Should()
            .Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void Custom_EmptyString_Throws()
    {
        Invoking(() => IntParser.Parse(string.Empty))
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
        Invoking(() => IntParser.Parse(input))
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
        int result = IntParser.Parse(input);
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
            from sign in NumericParse.Sign
            from abs in NumericParse.Digits.End().Aggregate(0, (acc, current) => acc * 10 + current)
            select abs * sign;

        parser.Parse(input)
            .Should().Be(output);
    }
}