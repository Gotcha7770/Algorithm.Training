using System;
using System.Buffers.Binary;
using System.Globalization;
using System.Linq;
using Algorithm.Training.Parsing.Parsers;
using FluentAssertions;
using Sprache;
using Xunit;

namespace Algorithm.Training.Parsing.Float;

public class FloatParsingTests
{
    // https://habr.com/ru/articles/337260/
    // http://jonskeet.uk/csharp/DoubleConverter.cs

    // 19.59375
    // 0.10000011.00111001100000000000000
    // S = 0 = 0b0
    // E = 127 + 4 = 131 = 0b10000011
    // M = (19.59375 - 16)/(32 - 16) * 2^23 = 1884160 = 0b00111001100000000000000

    // 263.3
    // 0.10000111.00000111010011001100110
    // S = 0 = 0b0
    // E = 127 + 8 = 135 = 0b10000111
    // M = (263.3 - 256)/(512 - 256) * 2^23 ~ 239206 = 0b00000111010011001100110

    // 12.12
    // 0.10000010.10000011110101110000101
    // S = 0 = 0b0
    // E = 127 + 3 = 130 = 0b10000010
    // M = (12.12 - 8)/(16 - 8) * 2^23 ~ 4320133 = 0b10000011110101110000101

    //float f = BitConverter.UInt32BitsToSingle(uInt32);

    [Theory]
    [InlineData(19.59375, "0b0.10000011.00111001100000000000000")]
    [InlineData(263.3, "0b0.10000111.00000111010011001100110")]
    [InlineData(12.12, "0b0.10000010.10000011110101110000101")]
    public void GetBitRepresentation(float input, string output)
    {
        new FloatToStringConverterV2().ConvertToString(input)
            .Should()
            .Be(output);
    }

    [Theory]
    [InlineData(0.5, 0b1000_0000_0000_0000_0000_0000)]
    [InlineData(0.375, 0b0110_0000_0000_0000_0000_0000)]
    [InlineData(0.59375, 0b1001_1000_0000_0000_0000_0000)]
    [InlineData(0.12, 0b0001_1110_1011_1000_0101_0001)]
    public void DecimalFractionToBinaryTest(float input, uint output)
    {
        FloatParser.DecimalFractionToBinary(input, 24)
            .Should()
            .Be(output);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("0.1", 0.1)]
    [InlineData("1", 1)]
    [InlineData("1.0", 1.0)]
    [InlineData("1234.1234", 1234.1234)]
    [InlineData("-1234.1234", -1234.1234)]
    public void Custom(string input, float output)
    {
        float result = ParseFloat(input);
        result.Should()
            .Be(output);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("0.1", 0.1)]
    [InlineData("1", 1)]
    [InlineData("1.0", 1.0)]
    [InlineData("1234.1234", 1234.1234)]
    [InlineData("-1234.1234", -1234.1234)]
    public void SystemParse(string input, float output)
    {
        float result = float.Parse(input, NumberFormatInfo.InvariantInfo);
        result.Should()
            .Be(output);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("0.1", 0.1)]
    [InlineData("1", 1)]
    [InlineData("1.0", 1.0)]
    [InlineData("1234.1234", 1234.1234)]
    [InlineData("-1234.1234", -1234.1234)]
    public void ParseWithSprache(string input, float output)
    {
        throw new NotImplementedException();
        // var parser = 
        //     from sign in NumericParse.Sign
        //     from integer in NumericParse.Digits
        //     from point in Parse.Char('.').Optional()
        //     from fractional in NumericParse.Digits.End()
        //     select (integer + fractional) * sign;
    }

    private static float ParseFloat(string input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length == 0)
            throw new FormatException("string length is 0");

        return input[0] == '-' ? -1 * ParseFloatWithoutSign(input.AsSpan(1)) : ParseFloatWithoutSign(input.AsSpan());
    }

    private static float ParseFloatWithoutSign(ReadOnlySpan<char> input)
    {
        throw new NotImplementedException();
        int pointIndex = input.IndexOf('.');
        int integer = IntParser.ParseWithoutSign(input[..pointIndex]);

        // 12.12 -> 12 + . + 12 -> 12 * 0.1^fractional.Length
        var t = Math.Log2(12);
        var t2 = Math.ILogB(12);
    }
}