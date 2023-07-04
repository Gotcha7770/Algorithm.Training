using System.Globalization;
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
    
    // 0.1
    // 0.01111011.10011001100110011001100
    // S = 0 = 0b0
    // E = 127 - 4 = 123 = 0b01111011
    // M = (0.1 - 0.0625)/(0.125 - 0.0625) * 2^23 ~ 5033164 = 0b10011001100110011001100
    
    // 0.5
    // 0.01111110.10011001100110011001100
    // S = 0 = 0b0
    // E = 127 - 1 = 126 = 0b01111110
    // M = ?

    //float f = BitConverter.UInt32BitsToSingle(uInt32);

    [Theory]
    [InlineData(0.5, "0b0.01111110.00000000000000000000000")]
    [InlineData(0.25, "0b0.01111101.00000000000000000000000")]
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
    [InlineData("0.5", 0.5)]
    [InlineData("19.59375", 19.59375)]
    [InlineData("1234.1234", 1234.1234)]
    [InlineData("-1234.1234", -1234.1234)]
    public void Custom(string input, float output)
    {
        float result = FloatParser.Parse(input);
        result.Should()
            .Be(output);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("0.1", 0.1)]
    [InlineData("1", 1)]
    [InlineData("1.0", 1.0)]
    [InlineData("0.5", 0.5)]
    [InlineData("19.59375", 19.59375)]
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
    [InlineData("0.5", 0.5)]
    [InlineData("19.59375", 19.59375)]
    [InlineData("1234.1234", 1234.1234)]
    [InlineData("-1234.1234", -1234.1234)]
    public void ParseWithSprache(string input, float output)
    {
        var parser = 
            from sign in NumericParse.Sign
            from integer in NumericParse.Digits
            from point in Parse.Char('.').WithPosition().Optional()
            from fractional in NumericParse.DigitsOrEmpty
            let abs = IntParser.FromDigits(integer) + (point.IsDefined ? FloatParser.FromFractional(fractional) : 0)
            select abs * sign;
        
        parser.Parse(input)
            .Should().Be(output);
    }
}